namespace MTG_Librarian
{
    partial class TasksForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tasksListView = new MTG_Librarian.EnhancedOLV();
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tasksListView)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;            
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(528, 437);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // tasksListView
            // 
            this.tasksListView.AllColumns.Add(this.olvColumn3);
            this.tasksListView.AllColumns.Add(this.olvColumn1);
            this.tasksListView.BackColor = System.Drawing.Color.Transparent;
            this.tasksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn3,
            this.olvColumn1});
            this.tasksListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.tasksListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tasksListView.FullRowSelect = true;
            this.tasksListView.GridLines = true;
            this.tasksListView.HasCollapsibleGroups = false;
            this.tasksListView.HeaderMaximumHeight = 0;
            this.tasksListView.HeaderMinimumHeight = 0;
            this.tasksListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tasksListView.Location = new System.Drawing.Point(0, 0);
            this.tasksListView.MultiSelect = false;
            this.tasksListView.Name = "tasksListView";
            this.tasksListView.OverlayImage.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.tasksListView.OverlayImage.InsetX = 0;
            this.tasksListView.OverlayImage.InsetY = 0;
            this.tasksListView.RowHeight = 26;
            this.tasksListView.ShowGroups = false;
            this.tasksListView.ShowSortIndicators = false;
            this.tasksListView.Size = new System.Drawing.Size(528, 437);
            this.tasksListView.TabIndex = 0;
            this.tasksListView.UseCompatibleStateImageBehavior = false;
            this.tasksListView.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn3
            // 
            this.olvColumn3.MaximumWidth = 40;
            this.olvColumn3.MinimumWidth = 40;
            this.olvColumn3.Width = 40;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Caption";
            this.olvColumn1.FillsFreeSpace = true;
            this.olvColumn1.MinimumWidth = 200;
            this.olvColumn1.Text = "Task";
            this.olvColumn1.Width = 200;
            // 
            // TasksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 437);
            this.Controls.Add(this.tasksListView);
            this.Controls.Add(this.pictureBox1);
            this.Name = "TasksForm";
            this.Text = "Tasks";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tasksListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        public EnhancedOLV tasksListView;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}