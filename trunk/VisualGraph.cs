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

        List<Node> nodes = new List<Node>();

        public void ImportFromGraph(Graph.Graph graph)
        {
            foreach (Graph.Node node in graph.Nodes)
            {
                nodes.Add(new RenderNode(this, node.Data.header.point));
            }
        }

        public VisualGraph()
        {
         
            UnitTest_Graph unitTest = new UnitTest_Graph();
            ImportFromGraph(unitTest.graph);
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
            foreach (Node node in nodes)
            {
                node.Render(graphics, clip);
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
            nodes.Add(new RenderNode(this, TransformFromScreen(position, currentClip)));
        }

        public void AddBlendNode(Point position, Rectangle currentClip)
        {
            nodes.Add(new BlendNode(this, TransformFromScreen(position, currentClip)));
        }

        public object GetDraggableObject(Point position, Rectangle currentClip)
        {
            Point world = TransformFromScreen(position, currentClip);
            foreach (Node node in nodes)
            {
                object drag = node.GetDraggableObject(world, out dragOffset);
                if (drag != null)
                    return drag;
            }
            return null;
        }

        public void DropDraggedObject(object what, Point position, Rectangle currentClip)
        {
            Point world = TransformFromScreen(position, currentClip);
            Node node = (Node)what;
            world.X -= dragOffset.X;
            world.Y -= dragOffset.Y;
            node.MoveTo(world);
            dragging = null;
        }

        public void DraggingObject(object what, Point position, Rectangle currentClip)
        {
            dragging = what;
            dragPosition = TransformFromScreen(position, currentClip);
        }

        public bool IsDragged(object what)
        {
            return dragging == what;
        }

        public Point GetDraggingPosition()
        {
            return new Point(dragPosition.X - dragOffset.X, dragPosition.Y - dragOffset.Y);
        }

        abstract private class Node
        {
            const int connectorSize = 8;
            const int connectorOffset = 3;

            protected List<Node> inputs = new List<Node>();
            protected List<Node> outputs = new List<Node>();

            protected VisualGraph owner;
            protected Point position;
            protected Size size;

            protected Brush color = Brushes.White;

            public Node(VisualGraph owner, Point position)
            {
                this.owner = owner;
                this.position = position;
                size = new Size(120, 80);
            }

            public void MoveTo(Point target)
            {
                position = target;
            }

            public virtual void Render(Graphics graphics, Rectangle clip)
            {
                Point origin = position;

                // Actual node
                Brush outline = Brushes.Black;
                if (owner.IsDragged(this))
                {
                    outline = Brushes.White;
                    origin = owner.GetDraggingPosition();
                }
                Point end = owner.TransformToScreen(new Point(origin.X + size.Width, origin.Y + size.Height), clip);
                origin = owner.TransformToScreen(origin, clip);
                Rectangle node = new Rectangle(origin, new Size(end.X - origin.X, end.Y - origin.Y));
                graphics.FillRectangle(color, node);
                graphics.DrawRectangle(new Pen(outline), node);

                // Connector sockets
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
            }

            public object GetDraggableObject(Point atPosition, out Point offset)
            {
                if (atPosition.X >= position.X && atPosition.Y >= position.Y &&
                    atPosition.X < position.X + size.Width && atPosition.Y < position.Y + size.Height)
                {
                    offset = new Point(atPosition.X - position.X, atPosition.Y - position.Y);
                    return this;
                }
                offset = new Point();
                return null;
            }

        }

        private class RenderNode : Node
        {
            public RenderNode(VisualGraph owner, Point position)
                : base(owner, position)
            {
                color = Brushes.LimeGreen;
                outputs.Add(null);
            }
        }

        private class BlendNode : Node
        {
            public BlendNode(VisualGraph owner, Point position)
                : base(owner, position)
            {
                color = Brushes.Orange;
                inputs.Add(null);
                inputs.Add(null);
                outputs.Add(null);
            }
        }
	}
}
