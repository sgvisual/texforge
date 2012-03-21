using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace texforge
{
    public class VisualGraphRender : VisualGraph
    {
        const int gridSize = 100;
        const int originWidth = 3;
        const int subGridDivisions = 10;

        const int connectorSizeDefault = 16;
        const int transitionSizeDefault = 8;
        int connectorSize = connectorSizeDefault;
        int transitionSize = transitionSizeDefault;

        Control drawnSurface = null;

        public override void AbortThread()
        {
            if (processing != null)
            {
                processing.Abort();
                processing = null;
            }
        }

        public void Render(Graphics graphics, Rectangle clip, Control surface)
        {
            dirtyNodes = 0;
            sharedThreadProperties.rendering = true;
            while (sharedThreadProperties.preventRendering)
                System.Threading.Thread.Sleep(1);
            if (processing == null)
            {
                processing = new Thread(new ThreadStart(Process));
                processing.Start();
            }
            drawnSurface = surface;
            graphics.FillRectangle(Brushes.CornflowerBlue, clip);
            Point center = GetCenter(new Rectangle(0, 0, surface.Width, surface.Height));
            // Horizon and Zenith;
            Rectangle horizon = new Rectangle(clip.X, center.Y - originWidth / 2, clip.X + surface.Width, originWidth);
            Rectangle zenith = new Rectangle(center.X - originWidth / 2, clip.Y, originWidth, clip.Y + surface.Height);
            // Subgrid
            Brush color = Brushes.DarkGray;
            Rectangle yaxis = zenith;
            yaxis.Width = 1;
            Rectangle xaxis = horizon;
            xaxis.Height = 1;
            for (int x = center.X; x < clip.Width + clip.X; x += gridSize / subGridDivisions)
            {
                yaxis.X = x;
                graphics.FillRectangle(color, yaxis);
            }
            for (int x = center.X; x >= clip.X; x -= gridSize / subGridDivisions)
            {
                yaxis.X = x;
                graphics.FillRectangle(color, yaxis);
            }
            for (int y = center.Y; y < clip.Height + clip.Y; y += gridSize / subGridDivisions)
            {
                xaxis.Y = y;
                graphics.FillRectangle(color, xaxis);
            }
            for (int y = center.Y; y >= clip.Y; y -= gridSize / subGridDivisions)
            {
                xaxis.Y = y;
                graphics.FillRectangle(color, xaxis);
            }
            // Grid
            color = Brushes.LightGray;
            for (int x = center.X; x < clip.Width + clip.X; x += gridSize)
            {
                yaxis.X = x;
                graphics.FillRectangle(color, yaxis);
            }
            for (int x = center.X; x >= clip.X; x -= gridSize)
            {
                yaxis.X = x;
                graphics.FillRectangle(color, yaxis);
            }
            for (int y = center.Y; y < clip.Height + clip.Y; y += gridSize)
            {
                xaxis.Y = y;
                graphics.FillRectangle(color, xaxis);
            }
            for (int y = center.Y; y >= clip.Y; y -= gridSize)
            {
                xaxis.Y = y;
                graphics.FillRectangle(color, xaxis);
            }
            // Origin cross
            horizon.Intersect(clip);
            zenith.Intersect(clip);
            graphics.FillRectangle(Brushes.Black, horizon);
            graphics.FillRectangle(Brushes.Black, zenith);
            // Render nodes
            CachedSocketRender.Clear();
            graphPossiblyDirty = true;
            if (CurrentPreview != null)
                CurrentPreview.Invalidate();
            foreach (Graph.Node node in graph.Nodes)
            {
                if (node.Dirty)
                    ++dirtyNodes;
                RenderNode(node, graphics, clip);
            }
            // Render transitions
            bool foundDragged = false;
            foreach (Graph.Graph.Transition transition in graph.Transitions)
            {
                Rectangle fromConnector = CachedSocketRender[transition.from];
                Rectangle toConnector = CachedSocketRender[transition.to];
                Point from = new Point(fromConnector.X + fromConnector.Width / 2, fromConnector.Y + fromConnector.Height / 2);
                Point to = new Point(toConnector.X + toConnector.Width / 2, toConnector.Y + toConnector.Height / 2);
                Point? dragPos = null;
                if (dragging != null && dragging.Is(transition.from))
                {
                    foundDragged = true;
                    dragPos = from = TransformToScreen(dragging.Position, clip);
                }
                else if (dragging != null && dragging.Is(transition.to))
                {
                    foundDragged = true;
                    dragPos = to = TransformToScreen(dragging.Position, clip);
                }
                RenderTransitionCurve(graphics, from, to);
                if (dragPos.HasValue)
                {
                    RenderDraggingSocket(graphics, (DraggableSocket)dragging, dragPos.Value);
                }
            }
            if (!foundDragged && dragging != null)
            {
                Graph.Node.Socket target = null;
                foreach (Graph.Node node in Graph.Nodes)
                {
                    foreach (Graph.Node.Socket socket in node.InputSockets)
                    {
                        if (dragging.Is(socket))
                        {
                            target = socket;
                            break;
                        }
                    }
                    foreach (Graph.Node.Socket socket in node.OutputSockets)
                    {
                        if (dragging.Is(socket))
                        {
                            target = socket;
                            break;
                        }
                    }
                    if (target != null)
                        break;
                }
                if (target != null)
                {
                    Point from = new Point(CachedSocketRender[target].X + connectorSize / 2, CachedSocketRender[target].Y + connectorSize / 2);
                    Point to = TransformToScreen(dragging.Position, clip);
                    RenderTransitionCurve(graphics, from, to);
                    RenderDraggingSocket(graphics, (DraggableSocket)dragging, to);
                }
            }
            sharedThreadProperties.rendering = false;
        }

        void RenderDraggingSocket(Graphics graphics, DraggableSocket dragSocket, Point dragPos)
        {
            Graph.Node.Socket target = null;
            Graph.Graph.Transition? trans = null;
            Brush targetSocket = Brushes.LawnGreen;
            switch (dragSocket.CanDropInSocket(this, dragPos, out target, out trans))
            {
                case DraggableSocket.DropResult.Invalid:
                    targetSocket = Brushes.Gray;
                    break;

                case DraggableSocket.DropResult.DeleteTransition:
                    targetSocket = Brushes.Red;
                    break;
            }
            Rectangle fakeSocket = new Rectangle(dragPos.X - connectorSize / 2, dragPos.Y - connectorSize / 2, connectorSize, connectorSize);
            graphics.FillEllipse(targetSocket, fakeSocket);
            graphics.DrawEllipse(new Pen(Brushes.Black), fakeSocket);
        }

        void RenderTransitionCurve(Graphics graphics, Point from, Point to)
        {
            Point[] curve = new Point[4];
            curve[0] = from;
            curve[3] = to;
            curve[1] = new Point(curve[0].X + (curve[3].X - curve[0].X) * 1 / 4, curve[0].Y + (curve[3].Y - curve[0].Y) * 1 / 8);
            curve[2] = new Point(curve[0].X + (curve[3].X - curve[0].X) * 3 / 4, curve[0].Y + (curve[3].Y - curve[0].Y) * 7 / 8);
            graphics.DrawCurve(new Pen(Brushes.Black, transitionSize + 1), curve);
            graphics.DrawCurve(new Pen(Brushes.LightSteelBlue, transitionSize - 1), curve);
        }

        void RenderNode(Graph.Node node, Graphics graphics, Rectangle clip)
        {
            const int connectorOffset = 3;
            const int labelDefaultHeight = 15;
            int labelHeight = (int)((float)labelDefaultHeight / 100.0f * zoom);

            Point origin = node.Data.header.point;

            // Actual node
            Brush outline = Brushes.Black;
            Brush color = Brushes.LimeGreen;
            if (ActiveObject != null && ActiveObject.Is(node))
            {
                outline = Brushes.LightGray;
            }
            if (dragging != null && dragging.Is(node))
            {
                outline = Brushes.White;
                origin = dragging.Position;
            }
            if (graph.FinalOutput.Contains(node) || ( graph.FinalOutput.Count == 0 && graph.ProceduralFinal == node))
            {
                color = Brushes.LawnGreen;
            }
            Size size = NodeGetSize(node);
            Rectangle actualClip = new Rectangle(0, 0, drawnSurface.Width, drawnSurface.Height);
            Point end = TransformToScreen(new Point(origin.X + size.Width, origin.Y + size.Height), actualClip);
            origin = TransformToScreen(origin, actualClip);
            Rectangle nodeRect = new Rectangle(origin, new Size(end.X - origin.X, end.Y - origin.Y));
            graphics.FillRectangle(color, nodeRect);
            graphics.DrawRectangle(new Pen(outline), nodeRect);

            // Debug info
            if (debug)
            {
                graphics.DrawString("(" + node.Data.header.point.X + ", " + node.Data.header.point.Y + ")", new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Red, new PointF((float)(origin.X), (float)(origin.Y - 10)));
            }

            // Label
            Rectangle label = new Rectangle(nodeRect.X + 2, nodeRect.Y + 2, nodeRect.Width - 3, labelHeight);
            graphics.FillRectangle(Brushes.ForestGreen, label);
            graphics.DrawString(node.Name, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 120.0f * zoom), Brushes.Black, new PointF((float)(label.X), (float)(label.Y - 3)));

            // Render preview
            int renderSize = (nodeRect.Height - labelHeight) * 3 / 4;
            Rectangle render = new Rectangle(new Point(nodeRect.X + nodeRect.Width / 4, nodeRect.Y + (nodeRect.Height - renderSize - labelHeight) / 2 + labelHeight), new Size(renderSize, renderSize));
            if (node.DisplayAtom != null && node.DisplayAtom.Result != null)
            {
                graphics.DrawRectangle(new Pen(Brushes.Black), new Rectangle(new Point(render.X - 1, render.Y - 1), new Size(render.Width + 1, render.Height + 1)));
                graphics.DrawImage(node.DisplayAtom.Result, render);
            }

            // Currently waiting to be or being processed
            if (node.Dirty)
            {
                const int invalidSize = 16;
                Point renderCenter = new Point(render.X + render.Width / 2, render.Y + render.Height / 2);
                graphics.FillEllipse(Brushes.Red, renderCenter.X - invalidSize / 2, renderCenter.Y - invalidSize / 2, invalidSize, invalidSize);
            }

            // Connector sockets
            Pen black = new Pen(Brushes.Black);
            List<Graph.Node.Socket> inputs = node.InputSockets;
            int height = origin.Y + labelHeight;
            int delta = (end.Y - height + connectorSize / 2) / (inputs.Count + 1);
            foreach (Graph.Node.Socket connector in inputs)
            {
                height += delta;
                Rectangle connectorShape = new Rectangle(origin.X + connectorOffset, height - connectorSize / 2, connectorSize, connectorSize);
                Brush connectorColor = Brushes.White;
                if (connector.Connections.Count > 0)
                    connectorColor = Brushes.LightSteelBlue;
                graphics.FillEllipse(connectorColor, connectorShape);
                graphics.DrawEllipse(black, connectorShape);
                graphics.DrawString(connector.name, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Black, new PointF((float)(connectorShape.X + connectorShape.Width + 1), (float)(connectorShape.Y + 1)));
                CachedSocketRender[connector] = connectorShape;
            }
            List<Graph.Node.Socket> outputs = node.OutputSockets;
            height = origin.Y + labelHeight;
            delta = (end.Y - height + connectorSize / 2) / (outputs.Count + 1);
            foreach (Graph.Node.Socket connector in outputs)
            {
                height += delta;
                Rectangle connectorShape = new Rectangle(end.X - connectorSize - connectorOffset, height - connectorSize / 2, connectorSize, connectorSize);
                Brush connectorColor = Brushes.White;
                if (connector.Connections.Count > 0)
                    connectorColor = Brushes.LightSteelBlue;
                graphics.FillEllipse(connectorColor, connectorShape);
                graphics.DrawEllipse(black, connectorShape);
                graphics.DrawString(connector.name, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Black, new PointF((float)(connectorShape.X - label.Width / 4), (float)(connectorShape.Y + 1)));
                CachedSocketRender[connector] = connectorShape;
            }

            //graphics.DrawString(clip.X.ToString() + "," + clip.Y + "," + clip.Width + "," + clip.Height, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Red, new PointF((float)(0), (float)(0)));
            //graphics.DrawString(drawnSurface.Left.ToString() + "," + drawnSurface.Top + "," + drawnSurface.Width + "," + drawnSurface.Height, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Red, new PointF((float)(0), (float)(10)));
        }

        public override void Clear()
        {
            base.Clear();
            connectorSize = connectorSizeDefault;
            transitionSize = transitionSizeDefault;
        }

        public override void Zoom(float amount)
        {
            base.Zoom(amount);
            connectorSize = (int)((float)connectorSizeDefault / 100.0f * zoom);
            transitionSize = (int)((float)transitionSizeDefault / 100.0f * zoom);
            if (transitionSize < 1)
                transitionSize = 1;
            if (connectorSize <= transitionSize)
                connectorSize = transitionSize + 1;
        }

        public override void Invalidate()
        {
            base.Invalidate();
            drawnSurface.Invalidate();
        }
    }
}
