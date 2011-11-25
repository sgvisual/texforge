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

        DraggableObject dragging = null;
        DraggableObject active = null;
        public DraggableObject ActiveObject
        {
            set { active = value; }
            get { return active; }
            }

        Graph.Graph graph;

        bool debug = false;

        string associatedFile = "";
        public string AssociatedFile
        {
            get { return associatedFile; }
        }

        bool modified = false;
        public bool Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        // Cached data
        Dictionary<Graph.Node.Socket, Rectangle> cachedSocketRender = new Dictionary<Node.Socket, Rectangle>();
        public Dictionary<Graph.Node.Socket, Rectangle> CachedSocketRender
        {
            get { return cachedSocketRender; }
        }

        // Draggable objects interface from within the visual graph view
        public abstract class DraggableObject
        {
            Point position = new Point();
            public virtual Point Position
            {
                set { position = value; }
                get { return position; }
            }   
            public abstract bool Is(object compare);
            public virtual void Drop(VisualGraph graph, Point position, Rectangle clip) { graph.Modified = true; }
            public abstract string GetName();
            public abstract Bitmap GetPreview();
        }

        class DraggableNode : DraggableObject
        {
            Graph.Node node;
            Point offset;
            public DraggableNode(Graph.Node node, Point offset)
            {
                this.node = node;
                this.offset = offset;
            }
            public override bool Is(object compare)
            {
                return node == compare;
            }
            public override void Drop(VisualGraph graph, Point position, Rectangle clip)
            {
                base.Drop(graph, position, clip);
                Point world = graph.TransformFromScreen(position, clip);
                world.X -= offset.X;
                world.Y -= offset.Y;
                node.Data.header.point = world;
            }
            public override Point Position
            {
                get
                {
                    Point position = base.Position;
                    return new Point(position.X - offset.X, position.Y - offset.Y);
                }
            }
            public override string GetName()
            {
                return node.Data.header.title;
            }
            public override Bitmap GetPreview()
            {
                if (node.Data.atom == null)
                    return null;
                return node.Data.atom.Result;
            }
        }

        class DraggableSocket : DraggableObject
        {
            Graph.Node.Socket socket;
            public DraggableSocket(Graph.Node.Socket socket)
            {
                this.socket = socket;
            }
            public override bool Is(object compare)
            {
                return socket == compare;
            }
            public override void Drop(VisualGraph graph, Point position, Rectangle clip)
            {
                base.Drop(graph, position, clip);
                Graph.Node.Socket target = null;
                Graph.Graph.Transition? transition = null;
                bool fromOutput = false;
                bool toOutput = false;
                // Find the target socket
                foreach (KeyValuePair<Graph.Node.Socket, Rectangle> socket in graph.CachedSocketRender)
                {
                    if (position.X >= socket.Value.X && position.X < socket.Value.X + socket.Value.Width &&
                        position.Y >= socket.Value.Y && position.Y < socket.Value.Y + socket.Value.Height)
                    {
                        target = socket.Key;
                        break;
                    }
                }
                // Find if its in any transitions
                foreach (Graph.Graph.Transition connector in graph.graph.Transitions)
                {
                    if (connector.from == socket)
                    {
                        transition = connector;
                        break;
                    }
                    if (connector.to == socket)
                    {
                        transition = connector;
                        break;
                    }
                }
                // No target node
                if (target == null )
                {
                    // Delete transition
                    if( transition != null )
                    {
                        graph.graph.Transitions.Remove(transition.Value);
                    }
                    return;
                }
                // Find the socket types
                foreach (Graph.Node.Socket nodeSocket in socket.owner.OutputSockets)
                {
                    if (nodeSocket == socket)
                    {
                        fromOutput = true;
                        break;
                    }
                }
                foreach (Graph.Node.Socket nodeSocket in target.owner.OutputSockets)
                {
                    if (nodeSocket == target)
                    {
                        toOutput = true;
                        break;
                    }
                }
                // Move existing transition
                if (transition != null)
                {
                    // Validate same type to same type
                    if ((fromOutput && !toOutput) || (!fromOutput && toOutput))
                    {
                        return;
                    }
                    graph.graph.Transitions.Remove(transition.Value);
                    if (fromOutput)
                    {
                        graph.graph.ConnectNodes(target, transition.Value.to);
                    }
                    else
                    {
                        graph.graph.ConnectNodes(transition.Value.from, target);
                    }
                    return;
                }
                // Validate output to input to add a new transition
                if ((fromOutput && toOutput) || (!fromOutput && !toOutput))
                {
                    return;
                }
                if (fromOutput)
                {
                    graph.graph.ConnectNodes(socket, target);
                }
                else
                {
                    graph.graph.ConnectNodes(target, socket);
                }
            }
            public override string GetName()
            {
                return socket.name;
            }
            public override Bitmap GetPreview()
            {
                return null;
            }
        }

        public VisualGraph()
        {
            graph = new UnitTest_Graph().graph;
        }

        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        public void Clear()
        {
            graph = new Graph.Graph();
            associatedFile = "";
            modified = false;
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
            cachedSocketRender.Clear();
            foreach (Graph.Node node in graph.Nodes)
            {
                RenderNode(node, graphics, clip);
            }
            // Render transitions
            foreach (Graph.Graph.Transition transition in graph.Transitions)
            {
                Rectangle fromConnector = cachedSocketRender[transition.from];
                Rectangle toConnector = cachedSocketRender[transition.to];
                graphics.DrawLine(new Pen(Brushes.Yellow, 8), new Point(fromConnector.X + fromConnector.Width / 2, fromConnector.Y + fromConnector.Height / 2), new Point(toConnector.X + toConnector.Width / 2, toConnector.Y + toConnector.Height / 2));
            }
        }

        void RenderNode(Graph.Node node, Graphics graphics, Rectangle clip)
        {
            const int connectorSize = 8;
            const int connectorOffset = 3;
            const int labelDefaultHeight = 15;
            int labelHeight = (int)((float)labelDefaultHeight / 100.0f * zoom);

            Point origin = node.Data.header.point;

            // Actual node
            Brush outline = Brushes.Black;
            if (active != null && active.Is(node))
            {
                outline = Brushes.LightGray;
            }
            if (dragging != null && dragging.Is(node))
            {
                outline = Brushes.White;
                origin = dragging.Position;
            }
            Size size = NodeGetSize(node);
            Point end = TransformToScreen(new Point(origin.X + size.Width, origin.Y + size.Height), clip);
            origin = TransformToScreen(origin, clip);
            Rectangle nodeRect = new Rectangle(origin, new Size(end.X - origin.X, end.Y - origin.Y));
            graphics.FillRectangle(Brushes.LimeGreen, nodeRect);
            graphics.DrawRectangle(new Pen(outline), nodeRect);

            // Debug info
            if (debug)
            {
                graphics.DrawString("(" + node.Data.header.point.X + ", " + node.Data.header.point.Y + ")", new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Red, new PointF((float)(origin.X), (float)(origin.Y - 10)));
            }

            // Label
            Rectangle label = new Rectangle(nodeRect.X + 2, nodeRect.Y + 2, nodeRect.Width - 3, labelHeight);
            graphics.FillRectangle(Brushes.ForestGreen, label);
            graphics.DrawString(node.Data.header.title, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 120.0f * zoom), Brushes.Black, new PointF((float)(label.X), (float)(label.Y - 3)));

            // Render preview
            if (node.Data.atom != null && node.Data.atom.Result != null)
            {
                int renderSize = (nodeRect.Height - labelHeight) * 3 / 4;
                Rectangle render = new Rectangle(new Point(nodeRect.X + nodeRect.Width / 4, nodeRect.Y + (nodeRect.Height - renderSize - labelHeight) / 2 + labelHeight), new Size(renderSize, renderSize));
                graphics.DrawRectangle(new Pen(Brushes.Black), new Rectangle(new Point(render.X - 1, render.Y - 1), new Size(render.Width + 1, render.Height + 1)));
                graphics.DrawImage(node.Data.atom.Result, render);
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
                graphics.FillEllipse(Brushes.White, connectorShape);
                graphics.DrawEllipse(black, connectorShape);
                graphics.DrawString(connector.name, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Black, new PointF((float)(connectorShape.X + connectorShape.Width + 1), (float)(connectorShape.Y + 1)));
                cachedSocketRender[connector] = connectorShape;
            }
            List<Graph.Node.Socket> outputs = node.OutputSockets;
            height = origin.Y + labelHeight;
            delta = (end.Y - height + connectorSize / 2) / (outputs.Count + 1);
            foreach (Graph.Node.Socket connector in outputs)
            {
                height += delta;
                Rectangle connectorShape = new Rectangle(end.X - connectorSize - connectorOffset, height - connectorSize / 2, connectorSize, connectorSize);
                graphics.FillEllipse(Brushes.White, connectorShape);
                graphics.DrawEllipse(black, connectorShape);
                graphics.DrawString(connector.name, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Black, new PointF((float)(connectorShape.X - label.Width / 4), (float)(connectorShape.Y + 1)));
                cachedSocketRender[connector] = connectorShape;
            }
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
            Graph.Node a = graph.CreateNode("ExampleNode", "");
            NodeData aData = new NodeData();
            aData.header.title = "Render" + graph.Nodes.Count;
            a.Data = aData;
            a.Data.header.point = TransformFromScreen(position, currentClip);
            modified = true;
        }

        public void AddBlendNode(Point position, Rectangle currentClip)
        {
            Graph.Node a = graph.CreateNode("ExampleNode", "");
            NodeData aData = new NodeData();
            aData.header.title = "Blend" + graph.Nodes.Count;
            a.Data = aData;
            a.Data.header.point = TransformFromScreen(position, currentClip);
            modified = true;
        }

        public DraggableObject GetDraggableObject(Point position, Rectangle currentClip)
        {
            // Check for sockets
            foreach (KeyValuePair<Node.Socket, Rectangle> socket in cachedSocketRender)
            {
                if (position.X >= socket.Value.X && position.X < socket.Value.X + socket.Value.Width &&
                    position.Y >= socket.Value.Y && position.Y < socket.Value.Y + socket.Value.Height)
                {
                    return new DraggableSocket(socket.Key);
                }
            }
            // Check for nodes
            Point world = TransformFromScreen(position, currentClip);
            foreach (Graph.Node node in graph.Nodes)
            {
                DraggableObject drag = NodeGetDraggableObject(node, world);
                if (drag != null)
                    return drag;
            }
            return null;
        }

        DraggableObject NodeGetDraggableObject(Graph.Node node, Point atPosition)
        {
            Point position = node.Data.header.point;
            Size size = NodeGetSize(node);
            if (atPosition.X >= position.X && atPosition.Y >= position.Y &&
                atPosition.X < position.X + size.Width && atPosition.Y < position.Y + size.Height)
            {
                return new DraggableNode(node, new Point(atPosition.X - position.X, atPosition.Y - position.Y));
            }
            return null;
        }

        Size NodeGetSize(Graph.Node node)
        {
            const int minimumNodeWidth = 100;
            const int minimumNodeHeight = 75;
            return new Size(minimumNodeWidth, minimumNodeHeight);
        }

        public void DropDraggedObject(DraggableObject what, Point position, Rectangle currentClip)
        {
            dragging.Drop(this, position, currentClip);
            dragging = null;
        }

        public void DraggingObject(DraggableObject what, Point position, Rectangle currentClip)
        {
            dragging = what;
            dragging.Position = TransformFromScreen(position, currentClip);
        }

        public void Save()
        {
            Save(associatedFile);
        }

        public void Save(string filename)
        {
            associatedFile = filename;
            graph.Save(filename);
            modified = false;
        }

        public void Load(string filename)
        {
            associatedFile = filename;
            graph.Load(filename);
            modified = false;
        }

	}
}
