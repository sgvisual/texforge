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
        protected Graph.Graph graph;
        PictureBox render;

        public static void PreviewFactory<T>(ToolStripItemCollection subMenu, Graph.Graph graph)
        {
            T preview = (T)System.Activator.CreateInstance(typeof(T), new object[] { subMenu, graph });
        }

        protected Preview(ToolStripItemCollection subMenu, Graph.Graph graph)
        {
            ToolStripItem item = subMenu.Add(GetToolStripName());
            item.Click += new EventHandler(item_Click);
            item.Tag = this;
            this.graph = graph;
        }

        public void Invalidate()
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
                render = new PictureBox();
                render.Dock = DockStyle.Fill;
                render.Paint += new PaintEventHandler(render_Paint);
                window.Controls.Add(render);
                window.Show();
            }
            window.BringToFront();
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
        }

        protected abstract string GetToolStripName();
    }

    public class TiledPreview : Preview
    {
        protected override string GetToolStripName()
        {
            return "Tiled Preview";
        }

        protected override void Render(PaintEventArgs e)
        {
            if( graph.Final != null && graph.Final.Data.atom != null && graph.Final.Data.atom.Result != null )
            {
                Bitmap output = graph.Final.Data.atom.Result;
                for (int x = e.ClipRectangle.X; x < e.ClipRectangle.X + e.ClipRectangle.Width; x += output.Width)
                {
                    for (int y = e.ClipRectangle.Y; y < e.ClipRectangle.Y + e.ClipRectangle.Height; y += output.Height)
                    {
                        e.Graphics.DrawImageUnscaled(output, new Point(x, y));
                    }
                }
                
            }
        }

        public TiledPreview(ToolStripItemCollection subMenu, Graph.Graph graph)
            : base(subMenu, graph)
        {
        }
    }

}
