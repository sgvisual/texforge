namespace texforge
{
    partial class SaveAnimatedOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tileOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.exportSingleImage = new System.Windows.Forms.Button();
            this.cancelSingleImage = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.exportImageSeries = new System.Windows.Forms.Button();
            this.cancelImageSeries = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(530, 217);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(522, 191);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Single Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tileOptions);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.exportSingleImage);
            this.splitContainer1.Panel2.Controls.Add(this.cancelSingleImage);
            this.splitContainer1.Size = new System.Drawing.Size(516, 185);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 2;
            // 
            // tileOptions
            // 
            this.tileOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileOptions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.tileOptions.Location = new System.Drawing.Point(0, 0);
            this.tileOptions.Name = "tileOptions";
            this.tileOptions.Size = new System.Drawing.Size(291, 185);
            this.tileOptions.TabIndex = 0;
            // 
            // exportSingleImage
            // 
            this.exportSingleImage.Location = new System.Drawing.Point(60, 157);
            this.exportSingleImage.Name = "exportSingleImage";
            this.exportSingleImage.Size = new System.Drawing.Size(75, 23);
            this.exportSingleImage.TabIndex = 3;
            this.exportSingleImage.Text = "Export";
            this.exportSingleImage.UseVisualStyleBackColor = true;
            this.exportSingleImage.Click += new System.EventHandler(this.exportSingleImage_Click);
            // 
            // cancelSingleImage
            // 
            this.cancelSingleImage.Location = new System.Drawing.Point(141, 157);
            this.cancelSingleImage.Name = "cancelSingleImage";
            this.cancelSingleImage.Size = new System.Drawing.Size(75, 23);
            this.cancelSingleImage.TabIndex = 2;
            this.cancelSingleImage.Text = "Cancel";
            this.cancelSingleImage.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(522, 191);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Image Series";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.exportImageSeries);
            this.splitContainer2.Panel2.Controls.Add(this.cancelImageSeries);
            this.splitContainer2.Size = new System.Drawing.Size(516, 185);
            this.splitContainer2.SplitterDistance = 298;
            this.splitContainer2.TabIndex = 0;
            // 
            // exportImageSeries
            // 
            this.exportImageSeries.Location = new System.Drawing.Point(53, 157);
            this.exportImageSeries.Name = "exportImageSeries";
            this.exportImageSeries.Size = new System.Drawing.Size(75, 23);
            this.exportImageSeries.TabIndex = 5;
            this.exportImageSeries.Text = "Export";
            this.exportImageSeries.UseVisualStyleBackColor = true;
            this.exportImageSeries.Click += new System.EventHandler(this.exportImageSeries_Click);
            // 
            // cancelImageSeries
            // 
            this.cancelImageSeries.Location = new System.Drawing.Point(134, 157);
            this.cancelImageSeries.Name = "cancelImageSeries";
            this.cancelImageSeries.Size = new System.Drawing.Size(75, 23);
            this.cancelImageSeries.TabIndex = 4;
            this.cancelImageSeries.Text = "Cancel";
            this.cancelImageSeries.UseVisualStyleBackColor = true;
            // 
            // SaveAnimatedOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 217);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveAnimatedOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Animated Texture Options";
            this.Load += new System.EventHandler(this.SaveAnimatedOptions_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button exportSingleImage;
        private System.Windows.Forms.Button cancelSingleImage;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button exportImageSeries;
        private System.Windows.Forms.Button cancelImageSeries;
        private System.Windows.Forms.FlowLayoutPanel tileOptions;
    }
}