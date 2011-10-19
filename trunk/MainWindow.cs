using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace texforge
{
    public partial class MainWindow : Form
    {
        protected Graph graph;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            graph = new Graph();
        }

        private void GraphRender_Paint(object sender, PaintEventArgs e)
        {
            graph.Render(e.Graphics, e.ClipRectangle);
        }
    }
}
