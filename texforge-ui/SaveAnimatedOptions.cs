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
    public partial class SaveAnimatedOptions : Form
    {
        public bool cancel = true;
        public VisualGraph.ExportOptions exportOptions = new VisualGraph.ExportOptions();
        List<RadioButton> imageBlock = new List<RadioButton>();

        public SaveAnimatedOptions()
        {
            InitializeComponent();
        }

        private void cancelImageSeries_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cancelSingleImage_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveAnimatedOptions_Load(object sender, EventArgs e)
        {
            int square = exportOptions.nbFrames;
            while (square * square >= exportOptions.nbFrames)
            {
                --square;
            }
            ++square;
            RadioButton option = new RadioButton();
            option.Tag = square;
            option.Text = square.ToString() + "x" + square;
            option.Checked = true;
            imageBlock.Add(option);
            tileOptions.Controls.Add(option);
            for (int i = 1; i <= Math.Floor(Math.Sqrt(exportOptions.nbFrames)); i++)
            {
                if (i == square)
                    break;
                bool remainder = (exportOptions.nbFrames % i != 0);
                int x = exportOptions.nbFrames / i;
                int y = remainder ? (exportOptions.nbFrames / x + 1) : (exportOptions.nbFrames / x);
                option = new RadioButton();
                option.Text = y.ToString() + "x" + x;
                tileOptions.Controls.Add(option);
                imageBlock.Add(option);
                option = new RadioButton();
                option.Text = x.ToString() + "x" + y;
                tileOptions.Controls.Add(option);
                imageBlock.Add(option);
            }
            // The last one is usually the best unless its perfectly square
            if (exportOptions.nbFrames % square != 0)
            {
                option.Checked = true;
            }
        }

        private void exportSingleImage_Click(object sender, EventArgs e)
        {
            cancel = false;
            foreach (RadioButton option in imageBlock)
            {
                if (option.Checked)
                {
                    string[] tile = option.Text.Split('x');
                    exportOptions.tileX = int.Parse(tile[0]);
                    exportOptions.tileY = int.Parse(tile[1]);
                    break;
                }
            }
            exportOptions.type = VisualGraph.ExportOptions.ExportType.SingleImage;
            Close();
        }

        private void exportImageSeries_Click(object sender, EventArgs e)
        {
            cancel = false;
            exportOptions.type = VisualGraph.ExportOptions.ExportType.MultipleImages;
            Close();
        }
    }
}
