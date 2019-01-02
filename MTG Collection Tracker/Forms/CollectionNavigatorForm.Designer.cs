namespace MTG_Librarian
{
    partial class CollectionNavigatorForm
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
            this.navigatorListView = new BrightIdeasSoftware.TreeListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.navigatorListView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigatorListView
            // 
            this.navigatorListView.AllColumns.Add(this.olvColumn1);
            this.navigatorListView.AllowDrop = true;
            this.navigatorListView.CellEditUseWholeCell = false;
            this.navigatorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.navigatorListView.ContextMenuStrip = this.contextMenuStrip1;
            this.navigatorListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigatorListView.FullRowSelect = true;
            this.navigatorListView.HeaderMaximumHeight = 0;
            this.navigatorListView.IsSimpleDragSource = true;
            this.navigatorListView.IsSimpleDropSink = true;
            this.navigatorListView.Location = new System.Drawing.Point(0, 0);
            this.navigatorListView.MultiSelect = false;
            this.navigatorListView.Name = "navigatorListView";
            this.navigatorListView.ShowGroups = false;
            this.navigatorListView.Size = new System.Drawing.Size(409, 480);
            this.navigatorListView.TabIndex = 0;
            this.navigatorListView.UseCompatibleStateImageBehavior = false;
            this.navigatorListView.View = System.Windows.Forms.View.Details;
            this.navigatorListView.VirtualMode = true;
            this.navigatorListView.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.navigatorListView_CellEditFinishing);
            this.navigatorListView.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.navigatorListView_CellEditStarting);
            this.navigatorListView.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.navigatorListView_ModelCanDrop);
            this.navigatorListView.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.navigatorListView_ModelDropped);
            this.navigatorListView.ItemActivate += new System.EventHandler(this.navigatorListView_ItemActivate);
            this.navigatorListView.SelectedIndexChanged += new System.EventHandler(this.navigatorListView_SelectedIndexChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Text";
            this.olvColumn1.AutoCompleteEditor = false;
            this.olvColumn1.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvColumn1.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGroupToolStripMenuItem,
            this.newCollectionToolStripMenuItem,
            this.editNameToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(156, 92);
            // 
            // newGroupToolStripMenuItem
            // 
            this.newGroupToolStripMenuItem.Name = "newGroupToolStripMenuItem";
            this.newGroupToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.newGroupToolStripMenuItem.Text = "New Group";
            this.newGroupToolStripMenuItem.Click += new System.EventHandler(this.newGroupToolStripMenuItem_Click);
            // 
            // newCollectionToolStripMenuItem
            // 
            this.newCollectionToolStripMenuItem.Name = "newCollectionToolStripMenuItem";
            this.newCollectionToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.newCollectionToolStripMenuItem.Text = "New Collection";
            this.newCollectionToolStripMenuItem.Visible = false;
            this.newCollectionToolStripMenuItem.Click += new System.EventHandler(this.newCollectionToolStripMenuItem_Click);
            // 
            // editNameToolStripMenuItem
            // 
            this.editNameToolStripMenuItem.Name = "editNameToolStripMenuItem";
            this.editNameToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.editNameToolStripMenuItem.Text = "Rename";
            this.editNameToolStripMenuItem.Visible = false;
            this.editNameToolStripMenuItem.Click += new System.EventHandler(this.editNameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Visible = false;
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // CollectionNavigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 480);
            this.Controls.Add(this.navigatorListView);
            this.Name = "CollectionNavigatorForm";
            this.Text = "Collections";
            ((System.ComponentModel.ISupportInitialize)(this.navigatorListView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        public BrightIdeasSoftware.TreeListView navigatorListView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newCollectionToolStripMenuItem;
    }
}