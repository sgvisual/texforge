using System;
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
            // Origin cross
            Point center = new Point((clip.Width - clip.X) / 2 + (int)((float)offset.X / 100.0f * zoom), (clip.Height - clip.Y) / 2 + (int)((float)offset.Y / 100.0f * zoom));
            Rectangle horizon = new Rectangle(clip.X, center.Y - originWidth / 2, clip.X + clip.Width, originWidth);
            Rectangle zenith = new Rectangle(center.X - originWidth / 2, clip.Y, originWidth, clip.Y + clip.Height);
            horizon.Intersect(clip);
            zenith.Intersect(clip);
            graphics.FillRectangle(Brushes.Black, horizon);
            graphics.FillRectangle(Brushes.Black, zenith);
        }

	}
}
