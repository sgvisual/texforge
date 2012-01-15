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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportOutputAsImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.RenderPreviewActive = new System.Windows.Forms.PictureBox();
            this.RenderPreviewFull = new System.Windows.Forms.PictureBox();
            this.splitGraphProperties = new System.Windows.Forms.SplitContainer();
            this.GraphRender = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.GroupProperties = new System.Windows.Forms.GroupBox();
            this.PanelProperties = new System.Windows.Forms.FlowLayoutPanel();
            this.graphContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRenderNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBlendNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addColorNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGeneratorNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOperationNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewFull)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitGraphProperties)).BeginInit();
            this.splitGraphProperties.Panel1.SuspendLayout();
            this.splitGraphProperties.Panel2.SuspendLayout();
            this.splitGraphProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GraphRender)).BeginInit();
            this.GroupProperties.SuspendLayout();
            this.graphContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(805, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportOutputAsImageToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportOutputAsImageToolStripMenuItem
            // 
            this.exportOutputAsImageToolStripMenuItem.Name = "exportOutputAsImageToolStripMenuItem";
            this.exportOutputAsImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.E)));
            this.exportOutputAsImageToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.exportOutputAsImageToolStripMenuItem.Text = "&Export output as image...";
            this.exportOutputAsImageToolStripMenuItem.Click += new System.EventHandler(this.exportOutputAsImageToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugInfoToolStripMenuItem,
            this.previewToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // debugInfoToolStripMenuItem
            // 
            this.debugInfoToolStripMenuItem.CheckOnClick = true;
            this.debugInfoToolStripMenuItem.Name = "debugInfoToolStripMenuItem";
            this.debugInfoToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.debugInfoToolStripMenuItem.Text = "&Debug Info";
            this.debugInfoToolStripMenuItem.Click += new System.EventHandler(this.debugInfoToolStripMenuItem_Click);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.previewToolStripMenuItem.Text = "&Preview";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 313);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(805, 22);
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
            this.splitContainer1.Panel2.Controls.Add(this.splitGraphProperties);
            this.splitContainer1.Size = new System.Drawing.Size(805, 289);
            this.splitContainer1.SplitterDistance = 287;
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
            this.splitContainer2.Size = new System.Drawing.Size(287, 289);
            this.splitContainer2.SplitterDistance = 145;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            // 
            // RenderPreviewActive
            // 
            this.RenderPreviewActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPreviewActive.Location = new System.Drawing.Point(0, 0);
            this.RenderPreviewActive.Name = "RenderPreviewActive";
            this.RenderPreviewActive.Size = new System.Drawing.Size(285, 143);
            this.RenderPreviewActive.TabIndex = 0;
            this.RenderPreviewActive.TabStop = false;
            this.RenderPreviewActive.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderPreviewActive_Paint);
            // 
            // RenderPreviewFull
            // 
            this.RenderPreviewFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPreviewFull.Location = new System.Drawing.Point(0, 0);
            this.RenderPreviewFull.Name = "RenderPreviewFull";
            this.RenderPreviewFull.Size = new System.Drawing.Size(285, 141);
            this.RenderPreviewFull.TabIndex = 0;
            this.RenderPreviewFull.TabStop = false;
            this.RenderPreviewFull.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderPreviewFull_Paint);
            // 
            // splitGraphProperties
            // 
            this.splitGraphProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGraphProperties.Location = new System.Drawing.Point(0, 0);
            this.splitGraphProperties.Name = "splitGraphProperties";
            // 
            // splitGraphProperties.Panel1
            // 
            this.splitGraphProperties.Panel1.Controls.Add(this.GraphRender);
            this.splitGraphProperties.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitGraphProperties.Panel2
            // 
            this.splitGraphProperties.Panel2.Controls.Add(this.GroupProperties);
            this.splitGraphProperties.Size = new System.Drawing.Size(514, 289);
            this.splitGraphProperties.SplitterDistance = 380;
            this.splitGraphProperties.TabIndex = 0;
            // 
            // GraphRender
            // 
            this.GraphRender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphRender.Location = new System.Drawing.Point(0, 25);
            this.GraphRender.Name = "GraphRender";
            this.GraphRender.Size = new System.Drawing.Size(380, 264);
            this.GraphRender.TabIndex = 3;
            this.GraphRender.TabStop = false;
            this.GraphRender.DragDrop += new System.Windows.Forms.DragEventHandler(this.GraphRender_DragDrop);
            this.GraphRender.DragOver += new System.Windows.Forms.DragEventHandler(this.GraphRender_DragOver);
            this.GraphRender.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphRender_Paint);
            this.GraphRender.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GraphRender_MouseClick);
            this.GraphRender.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GraphRender_MouseDown);
            this.GraphRender.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphRender_MouseMove);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(380, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // GroupProperties
            // 
            this.GroupProperties.Controls.Add(this.PanelProperties);
            this.GroupProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupProperties.Location = new System.Drawing.Point(0, 0);
            this.GroupProperties.Name = "GroupProperties";
            this.GroupProperties.Size = new System.Drawing.Size(130, 289);
            this.GroupProperties.TabIndex = 0;
            this.GroupProperties.TabStop = false;
            this.GroupProperties.Text = "Properties";
            // 
            // PanelProperties
            // 
            this.PanelProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelProperties.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.PanelProperties.Location = new System.Drawing.Point(3, 16);
            this.PanelProperties.Name = "PanelProperties";
            this.PanelProperties.Size = new System.Drawing.Size(124, 270);
            this.PanelProperties.TabIndex = 0;
            // 
            // graphContextMenu
            // 
            this.graphContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRenderNodeToolStripMenuItem,
            this.addBlendNodeToolStripMenuItem,
            this.addColorNodeToolStripMenuItem,
            this.addGeneratorNodeToolStripMenuItem,
            this.addOperationNodeToolStripMenuItem});
            this.graphContextMenu.Name = "graphContextMenu";
            this.graphContextMenu.Size = new System.Drawing.Size(174, 136);
            // 
            // addRenderNodeToolStripMenuItem
            // 
            this.addRenderNodeToolStripMenuItem.Name = "addRenderNodeToolStripMenuItem";
            this.addRenderNodeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addRenderNodeToolStripMenuItem.Text = "Add Render Node";
            this.addRenderNodeToolStripMenuItem.Click += new System.EventHandler(this.addRenderNodeToolStripMenuItem_Click);
            // 
            // addBlendNodeToolStripMenuItem
            // 
            this.addBlendNodeToolStripMenuItem.Name = "addBlendNodeToolStripMenuItem";
            this.addBlendNodeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addBlendNodeToolStripMenuItem.Text = "Add Blend Node";
            this.addBlendNodeToolStripMenuItem.Click += new System.EventHandler(this.addBlendNodeToolStripMenuItem_Click);
            // 
            // addColorNodeToolStripMenuItem
            // 
            this.addColorNodeToolStripMenuItem.Name = "addColorNodeToolStripMenuItem";
            this.addColorNodeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addColorNodeToolStripMenuItem.Text = "Add Color Node";
            this.addColorNodeToolStripMenuItem.Click += new System.EventHandler(this.addColorNodeToolStripMenuItem_Click);
            // 
            // addGeneratorNodeToolStripMenuItem
            // 
            this.addGeneratorNodeToolStripMenuItem.Name = "addGeneratorNodeToolStripMenuItem";
            this.addGeneratorNodeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addGeneratorNodeToolStripMenuItem.Text = "Add Generator Node";
            this.addGeneratorNodeToolStripMenuItem.Click += new System.EventHandler(this.addGeneratorNodeToolStripMenuItem_Click);
            // 
            // addOperationNodeToolStripMenuItem
            // 
            this.addOperationNodeToolStripMenuItem.Name = "addOperationNodeToolStripMenuItem";
            this.addOperationNodeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addOperationNodeToolStripMenuItem.Text = "Add Operation Node";
            this.addOperationNodeToolStripMenuItem.Click += new System.EventHandler(this.addOperationNodeToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 335);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "texforge";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPreviewFull)).EndInit();
            this.splitGraphProperties.Panel1.ResumeLayout(false);
            this.splitGraphProperties.Panel1.PerformLayout();
            this.splitGraphProperties.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGraphProperties)).EndInit();
            this.splitGraphProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GraphRender)).EndInit();
            this.GroupProperties.ResumeLayout(false);
            this.graphContextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip graphContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addRenderNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBlendNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitGraphProperties;
        private System.Windows.Forms.PictureBox GraphRender;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.GroupBox GroupProperties;
        private System.Windows.Forms.ToolStripMenuItem exportOutputAsImageToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel PanelProperties;
        private System.Windows.Forms.ToolStripMenuItem addColorNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addGeneratorNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addOperationNodeToolStripMenuItem;

    }
}

