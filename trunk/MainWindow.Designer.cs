namespace texforge
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.graphContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRenderNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenderPreviewActive = new System.Windows.Forms.PictureBox();
            this.RenderPreviewFull = new System.Windows.Forms.PictureBox();
            this.GraphRender = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.graphContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewFull)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphRender)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 313);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GraphRender);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(624, 289);
            this.splitContainer1.SplitterDistance = 223;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.RenderPreviewActive);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.RenderPreviewFull);
            this.splitContainer2.Size = new System.Drawing.Size(223, 289);
            this.splitContainer2.SplitterDistance = 145;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(397, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // graphContextMenu
            // 
            this.graphContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRenderNodeToolStripMenuItem});
            this.graphContextMenu.Name = "graphContextMenu";
            this.graphContextMenu.Size = new System.Drawing.Size(160, 26);
            // 
            // addRenderNodeToolStripMenuItem
            // 
            this.addRenderNodeToolStripMenuItem.Name = "addRenderNodeToolStripMenuItem";
            this.addRenderNodeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.addRenderNodeToolStripMenuItem.Text = "Add Render Node";
            this.addRenderNodeToolStripMenuItem.Click += new System.EventHandler(this.addRenderNodeToolStripMenuItem_Click);
            // 
            // RenderPreviewActive
            // 
            this.RenderPreviewActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPreviewActive.InitialImage = global::texforge.Properties.Resources.blank1;
            this.RenderPreviewActive.Location = new System.Drawing.Point(0, 0);
            this.RenderPreviewActive.Name = "RenderPreviewActive";
            this.RenderPreviewActive.Size = new System.Drawing.Size(221, 143);
            this.RenderPreviewActive.TabIndex = 0;
            this.RenderPreviewActive.TabStop = false;
            this.RenderPreviewActive.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderPreviewActive_Paint);
            // 
            // RenderPreviewFull
            // 
            this.RenderPreviewFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPreviewFull.Location = new System.Drawing.Point(0, 0);
            this.RenderPreviewFull.Name = "RenderPreviewFull";
            this.RenderPreviewFull.Size = new System.Drawing.Size(221, 141);
            this.RenderPreviewFull.TabIndex = 0;
            this.RenderPreviewFull.TabStop = false;
            // 
            // GraphRender
            // 
            this.GraphRender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphRender.Location = new System.Drawing.Point(0, 25);
            this.GraphRender.Name = "GraphRender";
            this.GraphRender.Size = new System.Drawing.Size(397, 264);
            this.GraphRender.TabIndex = 1;
            this.GraphRender.TabStop = false;
            this.GraphRender.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphRender_Paint);
            this.GraphRender.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GraphRender_MouseClick);
            this.GraphRender.MouseEnter += new System.EventHandler(this.GraphRender_MouseEnter);
            this.GraphRender.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphRender_MouseMove);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 335);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "texforge";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.graphContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewFull)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphRender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox RenderPreviewActive;
        private System.Windows.Forms.PictureBox RenderPreviewFull;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.PictureBox GraphRender;
        private System.Windows.Forms.ContextMenuStrip graphContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addRenderNodeToolStripMenuItem;

    }
}

