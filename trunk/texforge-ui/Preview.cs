using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace texforge
{
    public abstract class Preview
    {
        Form window = null;
        protected VisualGraph graph;
        PictureBox render;
        protected Point offset = new Point();
        protected float zoom = 100.0f;
        Point mouseLastPosition = new Point();

        public static T PreviewFactory<T>(ToolStripItemCollection subMenu, VisualGraph graph)
        {
            T preview = (T)System.Activator.CreateInstance(typeof(T), new object[] { subMenu, graph });
            return preview;
        }

        protected Preview(ToolStripItemCollection subMenu, VisualGraph graph)
        {
            ToolStripItem item = subMenu.Add(GetToolStripName());
            item.Click += new EventHandler(item_Click);
            item.Tag = this;
            this.graph = graph;
        }

        public virtual void Invalidate()
        {
            if( render != null )
                render.Invalidate();
        }

        void item_Click(object sender, EventArgs e)
        {
            if (window == null)
            {
                window = new Form();
                window.Text = GetToolStripName();
                window.FormClosed += new FormClosedEventHandler(window_FormClosed);
                window.Move += new EventHandler(window_Move);
                render = new PictureBox();
                render.Dock = DockStyle.Fill;
                render.Paint += new PaintEventHandler(render_Paint);
                render.MouseWheel += new MouseEventHandler(render_MouseWheel);
                render.MouseMove += new MouseEventHandler(render_MouseMove);
                render.MouseEnter += new EventHandler(render_MouseEnter);
                window.Controls.Add(render);
                window.Show();
                graph.CurrentPreview = this;
            }
            window.BringToFront();
        }

        void window_Move(object sender, EventArgs e)
        {
            graph.Invalidate();
        }

        void render_MouseEnter(object sender, EventArgs e)
        {
            Rectangle origin = render.RectangleToScreen(new Rectangle());
            mouseLastPosition = new Point(Control.MousePosition.X - origin.X, Control.MousePosition.Y - origin.Y);
        }

        void render_MouseMove(object sender, MouseEventArgs e)
        {
            bool left = (Control.MouseButtons & MouseButtons.Left) > 0;
            bool right = (Control.MouseButtons & MouseButtons.Right) > 0;
            bool middle = (Control.MouseButtons & MouseButtons.Middle) > 0;
            if ((left && right) || middle)
            {
                Point delta = new Point(e.Location.X - mouseLastPosition.X, e.Location.Y - mouseLastPosition.Y);
                offset.X += delta.X;
                offset.Y += delta.Y;
                Image output = graph.Output;
                if (output != null)
                {
                    while (offset.X > output.Width)
                        offset.X -= output.Width;
                    while (-offset.X > output.Width)
                        offset.X += output.Width;
                    while (offset.Y > output.Height)
                        offset.Y -= output.Height;
                    while (-offset.Y > output.Height)
                        offset.Y += output.Height;
                }
                render.Invalidate();
            }
            mouseLastPosition = e.Location;
        }

        void render_MouseWheel(object sender, MouseEventArgs e)
        {
            zoom += (float)e.Delta / 10.0f;
            if (zoom < 30.0f)
                zoom = 30.0f;
            if (zoom > 300.0f)
                zoom = 300.0f;
            render.Invalidate();
        }

        protected abstract void Render(PaintEventArgs e);

        void render_Paint(object sender, PaintEventArgs e)
        {
            Render(e);
        }

        void window_FormClosed(object sender, FormClosedEventArgs e)
        {
            window.Dispose();
            window = null;
            graph.CurrentPreview = null;
        }

        protected abstract string GetToolStripName();
    }

    public class TiledPreview : Preview
    {
        Dictionary<int, Bitmap> tileCache = new Dictionary<int, Bitmap>();

        protected override string GetToolStripName()
        {
            return "Tiled Preview";
        }

        public override void Invalidate()
        {
            base.Invalidate();
            tileCache.Clear();
        }
        protected override void Render(PaintEventArgs e)
        {
            Image output = graph.Output;
            if (output != null)
            {
                int width = (int)((float)output.Width * zoom / 100.0f);
                int height = (int)((float)output.Height * zoom / 100.0f);
                int offsetX = (int)((float)offset.X * zoom / 100.0f);
                int offsetY = (int)((float)offset.Y * zoom / 100.0f);
                Bitmap image = null;
                if (tileCache.ContainsKey(width))
                {
                    image = tileCache[width];
                }
                else
                {
                    image = new Bitmap(width, height);
                    Graphics.FromImage(image).DrawImage(output, 0, 0, width, height);
                    if (tileCache.Count >= 10)
                        tileCache.Clear();
                    tileCache[width] = image;
                }
                for (int x = e.ClipRectangle.X - width; x < e.ClipRectangle.X + e.ClipRectangle.Width + width; x += width)
                {
                    for (int y = e.ClipRectangle.Y - height; y < e.ClipRectangle.Y + e.ClipRectangle.Height + height; y += height)
                    {
                        e.Graphics.DrawImageUnscaled(image, new Point(x + offsetX, y + offsetY));
                    }
                }
            }
            //e.Graphics.DrawString("Zoom: " + zoom, new Font("Terminal", 12.0f), Brushes.Black, new PointF(10.0f, 10.0f));
            //e.Graphics.DrawString("Offset: " + offset.X + ", " + offset.Y, new Font("Terminal", 12.0f), Brushes.Black, new PointF(10.0f, 30.0f));
        }

        public TiledPreview(ToolStripItemCollection subMenu, VisualGraph graph)
            : base(subMenu, graph)
        {
        }
    }

}
