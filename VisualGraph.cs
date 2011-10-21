﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace texforge
{
	public class VisualGraph
	{
        Point offset;
        float zoom;
        const int originWidth = 3;
        const int gridSize = 100;
        const int subGridDivisions = 10;

        List<Node> nodes = new List<Node>();

        public VisualGraph()
        {
            offset = new Point();
            zoom = 100.0f;
        }

        public void Zoom(float amount)
        {
            zoom += amount;
            if (zoom < 10.0f)
                zoom = 10.0f;
            if (zoom > 1000.0f)
                zoom = 1000.0f;
        }

        public void Pan(Point delta)
        {
            offset.X += (int)((float)delta.X / zoom * 100.0f);
            offset.Y += (int)((float)delta.Y / zoom * 100.0f);
        }

        public void Render(Graphics graphics, Rectangle clip)
        {
            graphics.FillRectangle(Brushes.CornflowerBlue, clip);
            Point center = new Point((clip.Width - clip.X) / 2 + (int)((float)offset.X / 100.0f * zoom), (clip.Height - clip.Y) / 2 + (int)((float)offset.Y / 100.0f * zoom));
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

        // Coordinates are broken as they are relative to the top corner of the graph instead of the origin cross
        // TODO: Fix the screen transforms
        Point TransformFromScreen(Point position)
        {
            position.X = (int)((float)position.X * (float)zoom / 100.0f);
            position.X -= offset.X;
            position.Y = (int)((float)position.Y * (float)zoom / 100.0f);
            position.Y -= offset.Y;
            return position;
        }

        // Coordinates are broken as they are relative to the top corner of the graph instead of the origin cross
        // TODO: Fix the screen transforms
        Point TransformToScreen(Point position)
        {
            position.X += offset.X;
            position.X = (int)((float)position.X / 100.0f * (float)zoom);
            position.Y += offset.Y;
            position.Y = (int)((float)position.Y / 100.0f * (float)zoom);
            return position;
        }

        public void AddRenderNode(Point position)
        {
            nodes.Add(new RenderNode(this, TransformFromScreen(position)));
        }

        abstract private class Node
        {
            protected VisualGraph owner;
            protected Point position;

            public Node(VisualGraph owner, Point position)
            {
                this.owner = owner;
                this.position = position;
            }

            abstract public void Render(Graphics graphics, Rectangle clip);
        }

        private class RenderNode : Node
        {
            public RenderNode(VisualGraph owner, Point position)
                : base(owner, position)
            {
            }

            public override void Render(Graphics graphics, Rectangle clip)
            {
                Point origin = owner.TransformToScreen(position);
                Point end = owner.TransformToScreen(new Point(position.X + 150, position.Y + 40));
                Size size = new Size(end.X - origin.X, end.Y - origin.Y);
                Rectangle node = new Rectangle(origin, size);
                graphics.FillRectangle(Brushes.LimeGreen, node);
                graphics.DrawRectangle(new Pen(Brushes.Black), node); 
            }
        }
	}
}
