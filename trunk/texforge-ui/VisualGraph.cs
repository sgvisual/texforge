using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using texforge.Graph;

namespace texforge
{
	public class VisualGraph
	{
        Point offset = new Point();
        float zoom = 100.0f;
        const int originWidth = 3;
        const int gridSize = 100;
        const int subGridDivisions = 10;

        object dragging = null;
        Point dragPosition;
        Point dragOffset;

        Graph.Graph graph;

        public VisualGraph()
        {
            graph = new UnitTest_Graph().graph;
        }

        public void Zoom(float amount)
        {
            zoom += amount;
            if (zoom < 30.0f)
                zoom = 30.0f;
            if (zoom > 300.0f)
                zoom = 300.0f;
        }

        public void Pan(Point delta)
        {
            offset.X += (int)((float)delta.X / zoom * 100.0f);
            offset.Y += (int)((float)delta.Y / zoom * 100.0f);
        }

        public void Render(Graphics graphics, Rectangle clip)
        {
            graphics.FillRectangle(Brushes.CornflowerBlue, clip);
            Point center = GetCenter(clip);
            // Horizon and Zenith;
            Rectangle horizon = new Rectangle(clip.X, center.Y - originWidth / 2, clip.X + clip.Width, originWidth);
            Rectangle zenith = new Rectangle(center.X - originWidth / 2, clip.Y, originWidth, clip.Y + clip.Height);
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
            foreach (Graph.Node node in graph.Nodes)
            {
                RenderNode(node, graphics, clip);
            } 
        }

        void RenderNode(Graph.Node node, Graphics graphics, Rectangle clip)
        {
            //const int connectorSize = 8;
            //const int connectorOffset = 3;

            Point origin = node.Data.header.point;

            // Actual node
            Brush outline = Brushes.Black;
            if (dragging == node)
            {
                outline = Brushes.White;
                origin = GetDraggingPosition();
            }
            Size size = NodeGetSize(node);
            Point end = TransformToScreen(new Point(origin.X + size.Width, origin.Y + size.Height), clip);
            origin = TransformToScreen(origin, clip);
            Rectangle nodeRect = new Rectangle(origin, new Size(end.X - origin.X, end.Y - origin.Y));
            graphics.FillRectangle(Brushes.LimeGreen, nodeRect);
            graphics.DrawRectangle(new Pen(outline), nodeRect);

            // Connector sockets
            /*
            Pen black = new Pen(Brushes.Black);
            int delta = (end.Y - origin.Y + connectorSize / 2) / (inputs.Count + 1);
            int height = origin.Y;
            foreach (Node connector in inputs)
            {
                height += delta;
                Rectangle connectorShape = new Rectangle(origin.X + connectorOffset, height - connectorSize / 2, connectorSize, connectorSize);
                graphics.FillEllipse(Brushes.White, connectorShape);
                graphics.DrawEllipse(black, connectorShape);
            }
            delta = (end.Y - origin.Y + connectorSize / 2) / (outputs.Count + 1);
            height = origin.Y;
            foreach (Node connector in outputs)
            {
                height += delta;
                Rectangle connectorShape = new Rectangle(end.X - connectorSize - connectorOffset, height - connectorSize / 2, connectorSize, connectorSize);
                graphics.FillEllipse(Brushes.White, connectorShape);
                graphics.DrawEllipse(black, connectorShape);
            }
             */

        }

        Point GetCenter(Rectangle clip)
        {
            return new Point((clip.Width - clip.X) / 2 + (int)((float)offset.X / 100.0f * zoom), (clip.Height - clip.Y) / 2 + (int)((float)offset.Y / 100.0f * zoom));
        }

        Point TransformFromScreen(Point position, Rectangle clip)
        {
            Point center = GetCenter(clip);
            position.X -= center.X;
            position.X = (int)((float)position.X / (float)zoom * 100.0f);
            position.Y -= center.Y;
            position.Y = (int)((float)position.Y / (float)zoom * 100.0f);
            return position;
        }

        Point TransformToScreen(Point position, Rectangle clip)
        {
            Point center = GetCenter(clip);
            position.X = (int)((float)position.X / 100.0f * (float)zoom);
            position.X += center.X;
            position.Y = (int)((float)position.Y / 100.0f * (float)zoom);
            position.Y += center.Y;
            return position;
        }

        public void AddRenderNode(Point position, Rectangle currentClip)
        {
            Graph.Node a = graph.CreateNode("ExampleNode");
            NodeData aData = new NodeData();
            aData.header.title = "RenderNode" + graph.Nodes.Count;
            a.Data = aData;
            a.Data.header.point = TransformFromScreen(position, currentClip);
        }

        public void AddBlendNode(Point position, Rectangle currentClip)
        {
            Graph.Node a = graph.CreateNode("ExampleNode");
            NodeData aData = new NodeData();
            aData.header.title = "BlendNode" + graph.Nodes.Count;
            a.Data = aData;
            a.Data.header.point = TransformFromScreen(position, currentClip);
        }

        public object GetDraggableObject(Point position, Rectangle currentClip)
        {
            Point world = TransformFromScreen(position, currentClip);
            foreach (Graph.Node node in graph.Nodes)
            {
                object drag = NodeGetDraggableObject(node, world, out dragOffset);
                if (drag != null)
                    return drag;
            }
            return null;
        }

        object NodeGetDraggableObject(Graph.Node node, Point atPosition, out Point offset)
        {
            Point position = node.Data.header.point;
            Size size = NodeGetSize(node);
            if (atPosition.X >= position.X && atPosition.Y >= position.Y &&
                atPosition.X < position.X + size.Width && atPosition.Y < position.Y + size.Height)
            {
                offset = new Point(atPosition.X - position.X, atPosition.Y - position.Y);
                return node;
            }
            offset = new Point();
            return null;
        }

        Size NodeGetSize(Graph.Node node)
        {
            const int minimumNodeWidth = 100;
            const int minimumNodeHeight = 75;
            return new Size(minimumNodeWidth, minimumNodeHeight);
        }

        public void DropDraggedObject(object what, Point position, Rectangle currentClip)
        {
            Point world = TransformFromScreen(position, currentClip);
            Graph.Node node = (Graph.Node)what;
            world.X -= dragOffset.X;
            world.Y -= dragOffset.Y;
            node.Data.header.point = world;
            dragging = null;
        }

        public void DraggingObject(object what, Point position, Rectangle currentClip)
        {
            dragging = what;
            dragPosition = TransformFromScreen(position, currentClip);
        }

        public Point GetDraggingPosition()
        {
            return new Point(dragPosition.X - dragOffset.X, dragPosition.Y - dragOffset.Y);
        }
	}
}
