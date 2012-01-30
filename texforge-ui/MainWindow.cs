using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using texforge_generator.Base;
using texforge_definitions.Settings;

namespace texforge
{
    public partial class MainWindow : Form
    {
        protected VisualGraph graph;
        Point mouseLastPosition = new Point();
        bool abortedSave = false;

        public MainWindow()
        {
            InitializeComponent();
            GraphRender.MouseWheel += new MouseEventHandler(GraphRender_MouseWheel);
            GraphRender.AllowDrop = true;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            graph = new VisualGraph();
            Preview.PreviewFactory<TiledPreview>(previewToolStripMenuItem.DropDownItems, graph);
        }

        private void GraphRender_Paint(object sender, PaintEventArgs e)
        {
            graph.Render(e.Graphics, e.ClipRectangle, GraphRender);
            Text = "texforge";
            if (graph.AssociatedFile == "")
            {
                if (graph.Modified)
                {
                    Text += " - Unsaved graph";
                }
            }
            else
            {
                Text += " - " + graph.AssociatedFile;
                if (graph.Modified)
                {
                    Text += "*";
                }
            }
            RenderPreviewActive.Invalidate();
            RenderPreviewFull.Invalidate();
            foreach (ToolStripItem item in previewToolStripMenuItem.DropDownItems)
                ((Preview)item.Tag).Invalidate();
        }

        private void RenderPreviewActive_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Green, e.ClipRectangle);
            if (graph.ActiveObject != null && graph.ActiveObject.GetPreview() != null)
            {
                e.Graphics.DrawImageUnscaled(graph.ActiveObject.GetPreview(), e.ClipRectangle);
            }
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
            graph.DropDraggedObject((VisualGraph.DraggableObject)e.Data.GetData(e.Data.GetFormats()[0]), mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

        private void ChangeActiveObject(VisualGraph.DraggableObject draggable)
        {
            if (draggable != graph.ActiveObject)
            {
                graph.ActiveObject = draggable;
                RenderPreviewActive.Invalidate();
                PanelProperties.Controls.Clear();
                Label name = new Label();
                name.Text = draggable.GetName();
                name.AutoSize = false;
                name.Width = PanelProperties.Width - 5;
                PanelProperties.Controls.Add(name);
                foreach (SettingBase setting in draggable.GetSettings())
                {
                    SettingComponentFactory.CreateComponent(setting, draggable, PanelProperties, GraphRender);
                }
            }
        }

        private void GraphRender_MouseDown(object sender, MouseEventArgs e)
        {
            bool right = (Control.MouseButtons & MouseButtons.Right) > 0;
            bool middle = (Control.MouseButtons & MouseButtons.Middle) > 0;
            if ( e.Button == MouseButtons.Left && !right && !middle)
            {
                VisualGraph.DraggableObject draggable = graph.GetDraggableObject(mouseLastPosition, GraphRender.ClientRectangle);
                if( draggable != null )
                {
                    ChangeActiveObject(draggable);
                    splitGraphProperties.Panel1.DoDragDrop(draggable, DragDropEffects.Move);
                }
            }
        }

        private void GraphRender_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            mouseLastPosition = GraphRender.PointToClient(new Point(e.X, e.Y));
            graph.DraggingObject((VisualGraph.DraggableObject)e.Data.GetData(e.Data.GetFormats()[0]), mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

        private bool CheckAbortModifiedGraph()
        {
            if (graph.Modified)
            {
                switch (MessageBox.Show("Graph has unmodified changes, do you wish to save the changes before proceeding?",
                        "texforge Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SaveGraph();
                        return abortedSave;

                    case DialogResult.No:
                        return false;

                    case DialogResult.Cancel:
                        return true;
                }
            }
            return false;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAbortModifiedGraph())
            {
                graph.Clear();
                PanelProperties.Controls.Clear();
                GraphRender.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void debugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.Debug = debugInfoToolStripMenuItem.Checked;
            GraphRender.Invalidate();
        }

        private void SaveGraphAs()
        {
            abortedSave = true;
            SaveFileDialog saveAsDialog = new SaveFileDialog();
            saveAsDialog.Filter = "texforge Graph|*.texforge";
            saveAsDialog.Title = "Save a texforge Graph";
            saveAsDialog.ShowDialog();
            if (saveAsDialog.FileName != "")
            {
                graph.Save(saveAsDialog.FileName);
                abortedSave = false;
            }
        }

        private void SaveGraph()
        {
            abortedSave = false;
            if (graph.AssociatedFile == "")
            {
                SaveGraphAs();
            }
            else
            {
                graph.Save();
            }
            GraphRender.Invalidate();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGraphAs();
            GraphRender.Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGraph();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAbortModifiedGraph())
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "texforge Graph|*.texforge";
                openDialog.Title = "Load a texforge Graph";
                openDialog.ShowDialog();
                if (openDialog.FileName != "")
                {
                    graph.Load(openDialog.FileName);
                    PanelProperties.Controls.Clear();
                    GraphRender.Invalidate();
                }
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = CheckAbortModifiedGraph();
        }

        private void RenderPreviewFull_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Green, e.ClipRectangle);
            if (graph.Output != null)
            {
                e.Graphics.DrawImageUnscaled(graph.Output, e.ClipRectangle);
            }
        }

        private void exportOutputAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graph.Output != null)
            {
                SaveFileDialog exportDialog = new SaveFileDialog();
                exportDialog.Filter = "Portable Network Graphics|*.png|JPEG|*.jpg|Graphics Interchange Format|*.gif|Windows Bitmap|*.bmp";
                exportDialog.Title = "Export texforge Graph output";
                exportDialog.ShowDialog();
                if (exportDialog.FileName != "")
                {
                    graph.Output.Save(exportDialog.FileName);
                }
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (PanelProperties.Controls.Count > 0)
            {
                PanelProperties.Controls[0].Width = PanelProperties.Width - 5;
            }
        }

        private void addColorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.AddColorNode(mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

        private void addGeneratorNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.AddGeneratorNode(mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

        private void addOperationNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.AddOperationNode(mouseLastPosition, GraphRender.ClientRectangle);
            GraphRender.Invalidate();
        }

    }
}
