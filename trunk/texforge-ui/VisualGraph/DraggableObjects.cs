using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using texforge_definitions.Settings;

namespace texforge
{
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
            foreach (Graph.Graph.Transition connector in graph.Graph.Transitions)
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
                    graph.Graph.DisconnectNodes(transition.Value.from, transition.Value.to);
                    break;
                case DropResult.Invalid:
                    break;
                case DropResult.ReconnectTo:
                    graph.Graph.DisconnectNodes(transition.Value.from, transition.Value.to);
                    graph.Graph.ConnectNodes(target, transition.Value.to);
                    break;
                case DropResult.ReconnectFrom:
                    graph.Graph.DisconnectNodes(transition.Value.from, transition.Value.to);
                    graph.Graph.ConnectNodes(transition.Value.from, target);
                    break;
                case DropResult.ConnectTo:
                    graph.Graph.ConnectNodes(socket, target);
                    break;
                case DropResult.ConnectFrom:
                    graph.Graph.ConnectNodes(target, socket);
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

}
