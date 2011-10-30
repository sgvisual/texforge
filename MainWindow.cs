using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using texforge_generator.Base;

namespace texforge
{
    public partial class MainWindow : Form
    {
        protected VisualGraph graph;
        Point mouseLastPosition = new Point();
        protected Generator generator;

        public MainWindow()
        {
            InitializeComponent();
            GraphRender.MouseWheel += new MouseEventHandler(GraphRender_MouseWheel);
            GraphRender.AllowDrop = true;

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            graph = new VisualGraph();
            generator = new Generator();

            texforge_definitions.settings settings = new texforge_definitions.settings();
            settings.width = 512;
            settings.height = 512;
            generator.Generate(settings);
        }

        private void GraphRender_Paint(object sender, PaintEventArgs e)
        {
            graph.Render(e.Graphics, e.ClipRectangle);

           
        }

        private void RenderPreviewActive_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = generator.ResultBitmap;
            //e.Graphics.DrawImageUnscaled(image, new Point());
            e.Graphics.FillRectangle(Brushes.Green, e.ClipRectangle);
            e.Graphics.DrawImageUnscaled(bmp, e.ClipRectangle);
        }

        private void GraphRender_MouseEnter(object sender, EventArgs e)
        {
            Rectangle origin = GraphRender.RectangleToScreen(new Rectangle());
            mouseLastPosition = new Point(Control.MousePosition.X - origin.X, Control.MousePosition.Y - origin.Y);
            GraphRender.Focus();
        }

        private void GraphRender_MouseWheel(object sender, MouseEventArgs e)
        {
            graph.Zoom((float)e.Delta / 10.0f);
            GraphRender.Invalidate();
        }

        private void GraphRender_MouseMove(object sender, MouseEventArgs e)
        {
            bool left = (Control.MouseButtons & MouseButtons.Left) > 0;
            bool right = (Control.MouseButtons & MouseButtons.Right) > 0;
            bool middle = (Control.MouseButtons & MouseButtons.Middle) > 0;
            if ( (left && right) || middle)
            {
                Point delta = new Point(e.Location.X - mouseLastPosition.X, e.Location.Y - mouseLastPosition.Y);
                graph.Pan(delta);
                GraphRender.Invalidate();
            }
            mouseLastPosition = e.Location;
        }

        private void GraphRender_MouseClick(object sender, MouseEventArgs e)
        {
            bool left = (Control.MouseButtons & MouseButtons.Left) > 0;
            bool middle = (Control.MouseButtons & MouseButtons.Middle) > 0; 
            if (e.Button == MouseButtons.Right && !left && !middle)
            {
                graphContextMenu.Show(Control.MousePosition);
            }
        }

        private void addRenderNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.AddRenderNode(mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

        private void addBlendNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.AddBlendNode(mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

        private void GraphRender_DragDrop(object sender, DragEventArgs e)
        {
            graph.DropDraggedObject(e.Data.GetData(e.Data.GetFormats()[0]), mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

        private void GraphRender_MouseDown(object sender, MouseEventArgs e)
        {
            bool right = (Control.MouseButtons & MouseButtons.Right) > 0;
            bool middle = (Control.MouseButtons & MouseButtons.Middle) > 0;
            if ( e.Button == MouseButtons.Left && !right && !middle)
            {
                object draggable = graph.GetDraggableObject(mouseLastPosition, GraphRender.ClientRectangle);
                if( draggable != null )
                {
                    splitContainer1.Panel2.DoDragDrop(draggable, DragDropEffects.Move);
                }
            }
        }

        private void GraphRender_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            mouseLastPosition = GraphRender.PointToClient(new Point(e.X, e.Y));
            graph.DraggingObject(e.Data.GetData(e.Data.GetFormats()[0]), mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

    }
}
