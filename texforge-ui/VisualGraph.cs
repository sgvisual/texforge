using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using texforge.Graph;
using texforge_definitions.Settings;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace texforge
{
	public class VisualGraph
	{
        Point offset = new Point();
        float zoom = 100.0f;
        const int originWidth = 3;
        const int gridSize = 100;
        const int subGridDivisions = 10;
        const int connectorSizeDefault = 16;
        const int transitionSizeDefault = 8;
        int connectorSize = connectorSizeDefault;
        int transitionSize = transitionSizeDefault;
        Thread processing = null;
        bool graphPossiblyDirty = false;
        Graph.Graph.SharedThreadProperties sharedThreadProperties = new Graph.Graph.SharedThreadProperties();
        float time = 0.0f;

        Control drawnSurface = null;

        DraggableObject dragging = null;
        DraggableObject active = null;
        public DraggableObject ActiveObject
        {
            set { active = value; }
            get { return active; }
        }

        Graph.Graph graph;
        public Graph.Graph Graph
        {
            get { return graph; }
        }

        Preview currentPreview = null;
        public Preview CurrentPreview
        {
            set { currentPreview = value; }
            get { return currentPreview; }
        }

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

        public Image Output
        {
            get
            {
                if (graph.Final != null && graph.Final.DisplayAtom != null && graph.Final.DisplayAtom.Result != null)
                {
                    return graph.Final.DisplayAtom.Result;
                }
                return null;
            }
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
            public virtual LinkedList<SettingBase> GetSettings() { return new LinkedList<SettingBase>(); }
            public virtual bool Dirty
            {
                set { }
            }
            public virtual void Delete()
            {
            }
            public virtual void DisconnectAll()
            {
            }
            public virtual void SetAsFinalOutput()
            {
            }
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
                return node.Name;
            }
            public override Bitmap GetPreview()
            {
                if (node.DisplayAtom == null)
                    return null;
                return node.DisplayAtom.Result;
            }
            public override LinkedList<SettingBase> GetSettings()
            {
                return node.Settings;
            }
            public override bool Dirty
            {
                set { node.Dirty = value; }
            }
            public override void Delete()
            {
                node.Remove();
            }
            public override void DisconnectAll()
            {
                node.DisconnectAll();
            }
            public override void SetAsFinalOutput()
            {
                node.SetAsFinalOutput();
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

            public enum DropResult
            {
                Invalid,
                DeleteTransition,
                ReconnectTo,
                ReconnectFrom,
                ConnectTo,
                ConnectFrom,
            }

            private bool FindNodeInParentTree(Graph.Node current, Graph.Node find)
            {
                if (current == find)
                    return true;
                foreach (Graph.Node.Socket parentSocket in current.InputSockets)
                {
                    foreach (Graph.Node connected in parentSocket.Connections)
                    {
                        if (FindNodeInParentTree(connected, find))
                            return true;
                    }
                }
                return false;
            }

            public DropResult CanDropInSocket(VisualGraph graph, Point position, out Graph.Node.Socket target, out Graph.Graph.Transition? transition)
            {
                target = null;
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
                transition = null;
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
                if (target == null)
                {
                    // Delete transition
                    if (transition != null)
                    {
                        return DropResult.DeleteTransition;
                    }
                    return DropResult.Invalid;
                }

                // Find the socket types
                bool fromOutput = false;
                bool toOutput = false;
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
                        return DropResult.Invalid;
                    }
                    if (fromOutput)
                    {
                        // Loop guard
                        if (FindNodeInParentTree(target.owner, socket.connection.owner))
                            return DropResult.Invalid;
                        return DropResult.ReconnectTo;
                    }
                    else
                    {
                        // Already one there, can't reconnect
                        if (target.Connections.Count > 0)
                        {
                            return DropResult.Invalid;
                        }
                        // Loop guard
                        if (FindNodeInParentTree(socket.connection.owner, target.owner))
                            return DropResult.Invalid;
                        return DropResult.ReconnectFrom;
                    }
                }

                // Validate output to input to add a new transition
                if ((fromOutput && toOutput) || (!fromOutput && !toOutput))
                {
                    return DropResult.Invalid;
                }

                // Loop guard
                if (fromOutput)
                {
                    if (FindNodeInParentTree(socket.owner, target.owner))
                        return DropResult.Invalid;
                }
                else
                {
                    if (FindNodeInParentTree(target.owner, socket.owner))
                        return DropResult.Invalid;
                }

                // Final valid possibilities
                if (fromOutput)
                {
                    // Already one there, can't reconnect
                    if (target.Connections.Count > 0)
                    {
                        return DropResult.Invalid;
                    }
                    return DropResult.ConnectTo;
                }
                else
                {
                    return DropResult.ConnectFrom;
                }
            }

            public override void Drop(VisualGraph graph, Point position, Rectangle clip)
            {
                base.Drop(graph, position, clip);

                Graph.Graph.Transition? transition = null;
                Graph.Node.Socket target = null;

                switch (CanDropInSocket(graph, position, out target, out transition))
                {
                    case DropResult.DeleteTransition:
                        graph.graph.DisconnectNodes(transition.Value.from, transition.Value.to);
                        break;
                    case DropResult.Invalid:
                        break;
                    case DropResult.ReconnectTo:
                        graph.graph.DisconnectNodes(transition.Value.from, transition.Value.to);
                        graph.graph.ConnectNodes(target, transition.Value.to);
                        break;
                    case DropResult.ReconnectFrom:
                        graph.graph.DisconnectNodes(transition.Value.from, transition.Value.to);
                        graph.graph.ConnectNodes(transition.Value.from, target);
                        break;
                    case DropResult.ConnectTo:
                        graph.graph.ConnectNodes(socket, target);
                        break;
                    case DropResult.ConnectFrom:
                        graph.graph.ConnectNodes(target, socket);
                        break;
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
            graph = new Graph.Graph(new GraphSettings());
            associatedFile = "";
            modified = false;
            offset = new Point();
            zoom = 100.0f;
            dragging = null;
            active = null;
            connectorSize = connectorSizeDefault;
            transitionSize = transitionSizeDefault;
        }

        public void Zoom(float amount)
        {
            zoom += amount;
            if (zoom < 30.0f)
                zoom = 30.0f;
            if (zoom > 300.0f)
                zoom = 300.0f;
            connectorSize = (int)((float)connectorSizeDefault / 100.0f * zoom);
            transitionSize = (int)((float)transitionSizeDefault / 100.0f * zoom);
            if (transitionSize < 1)
                transitionSize = 1;
            if (connectorSize <= transitionSize)
                connectorSize = transitionSize + 1;
        }

        public void Pan(Point delta)
        {
            offset.X += (int)((float)delta.X / zoom * 100.0f);
            offset.Y += (int)((float)delta.Y / zoom * 100.0f);
        }

        public void Invalidate()
        {
            drawnSurface.Invalidate();
        }

        void Process()
        {
            while (true)
            {
                if(graphPossiblyDirty)
                {
                    graphPossiblyDirty = false;
                    graph.Process(sharedThreadProperties);
                }
                if (sharedThreadProperties.invalidate)
                {
                    sharedThreadProperties.invalidate = false;
                    Invalidate();
                }
                Thread.Sleep(1);
            }
        }

        public void Render(Graphics graphics, Rectangle clip, Control surface)
        {
            time += 1.0f / 60.0f;
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
            cachedSocketRender.Clear();
            graphPossiblyDirty = true;
            if (currentPreview != null)
                currentPreview.Invalidate();
            foreach (Graph.Node node in graph.Nodes)
            {
                RenderNode(node, graphics, clip);
            }
            // Render transitions
            bool foundDragged = false;
            foreach (Graph.Graph.Transition transition in graph.Transitions)
            {
                Rectangle fromConnector = cachedSocketRender[transition.from];
                Rectangle toConnector = cachedSocketRender[transition.to];
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
                    foreach( Graph.Node.Socket socket in node.InputSockets)
                    {
                        if (dragging.Is(socket))
                        {
                            target = socket;
                            break;
                        }
                    }
                    foreach( Graph.Node.Socket socket in node.OutputSockets)
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
                    Point from = new Point(cachedSocketRender[target].X + connectorSize / 2, cachedSocketRender[target].Y + connectorSize / 2);
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
            if (active != null && active.Is(node))
            {
                outline = Brushes.LightGray;
            }
            if (dragging != null && dragging.Is(node))
            {
                outline = Brushes.White;
                origin = dragging.Position;
            }
            if (graph.Final == node)
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
                cachedSocketRender[connector] = connectorShape;
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
                cachedSocketRender[connector] = connectorShape;
            }
            
            //graphics.DrawString(clip.X.ToString() + "," + clip.Y + "," + clip.Width + "," + clip.Height, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Red, new PointF((float)(0), (float)(0)));
            //graphics.DrawString(drawnSurface.Left.ToString() + "," + drawnSurface.Top + "," + drawnSurface.Width + "," + drawnSurface.Height, new Font(FontFamily.GenericSansSerif, (float)labelDefaultHeight / 250.0f * zoom), Brushes.Red, new PointF((float)(0), (float)(10)));
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

        public List<string> NodeTypes
        {
            get { return NodeFactory.Get().NodeTypes; }
        }

        public DraggableObject AddNode(string type, Point position, Rectangle currentClip)
        {
            Graph.Node a = graph.CreateNode(type, "");
            a.Name = type + graph.Nodes.Count;
            NodeData aData = new NodeData();
            a.Data = aData;
            a.Data.header.point = TransformFromScreen(position, currentClip);
            modified = true;
            return new DraggableNode(a, new Point());
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
            const int connectionsHeight = 20;
            int extraConnections = Math.Max(node.InputSockets.Count, node.OutputSockets.Count) - 2;
            int nodeHeight = minimumNodeHeight;
            if (extraConnections > 0)
            {
                nodeHeight += connectionsHeight * extraConnections;
            }
            return new Size(minimumNodeWidth, nodeHeight);
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
            Clear();
            associatedFile = filename;
            graph.Load(filename);
        }

	}
}
