namespace MTG_Collection_Tracker
{
    partial class CollectionViewForm
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
            this.fastObjectListView1 = new BrightIdeasSoftware.FastObjectListView();
            this.CardName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ManaCost = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CountColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CostColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.TagsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // fastObjectListView1
            // 
            this.fastObjectListView1.AllColumns.Add(this.CardName);
            this.fastObjectListView1.AllColumns.Add(this.olvColumn10);
            this.fastObjectListView1.AllColumns.Add(this.olvColumn11);
            this.fastObjectListView1.AllColumns.Add(this.olvColumn2);
            this.fastObjectListView1.AllColumns.Add(this.ManaCost);
            this.fastObjectListView1.AllColumns.Add(this.olvColumn4);
            this.fastObjectListView1.AllColumns.Add(this.olvColumn5);
            this.fastObjectListView1.AllColumns.Add(this.CountColumn);
            this.fastObjectListView1.AllColumns.Add(this.CostColumn);
            this.fastObjectListView1.AllColumns.Add(this.TagsColumn);
            this.fastObjectListView1.AllColumns.Add(this.olvColumn9);
            this.fastObjectListView1.AllColumns.Add(this.olvColumn1);
            this.fastObjectListView1.AllowDrop = true;
            this.fastObjectListView1.BackColor = System.Drawing.Color.White;
            this.fastObjectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.fastObjectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CardName,
            this.olvColumn11,
            this.olvColumn2,
            this.ManaCost,
            this.olvColumn4,
            this.olvColumn5,
            this.CountColumn,
            this.CostColumn,
            this.TagsColumn,
            this.olvColumn9,
            this.olvColumn1});
            this.fastObjectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastObjectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastObjectListView1.FullRowSelect = true;
            this.fastObjectListView1.GridLines = true;
            this.fastObjectListView1.HideSelection = false;
            this.fastObjectListView1.IsSimpleDropSink = true;
            this.fastObjectListView1.Location = new System.Drawing.Point(0, 0);
            this.fastObjectListView1.Name = "fastObjectListView1";
            this.fastObjectListView1.SelectedBackColor = System.Drawing.Color.SteelBlue;
            this.fastObjectListView1.SelectedForeColor = System.Drawing.Color.White;
            this.fastObjectListView1.ShowGroups = false;
            this.fastObjectListView1.Size = new System.Drawing.Size(975, 435);
            this.fastObjectListView1.TabIndex = 0;
            this.fastObjectListView1.UnfocusedSelectedBackColor = System.Drawing.Color.LightGray;
            this.fastObjectListView1.UseCompatibleStateImageBehavior = false;
            this.fastObjectListView1.View = System.Windows.Forms.View.Details;
            this.fastObjectListView1.VirtualMode = true;
            this.fastObjectListView1.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.fastObjectListView1_CellEditFinished);
            this.fastObjectListView1.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.fastObjectListView1_CellClick);
            this.fastObjectListView1.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.fastObjectListView1_ModelCanDrop);
            this.fastObjectListView1.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.fastObjectListView1_ModelDropped);
            this.fastObjectListView1.SelectionChanged += new System.EventHandler(this.fastObjectListView1_SelectionChanged);
            this.fastObjectListView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fastObjectListView1_KeyPress);
            // 
            // CardName
            // 
            this.CardName.AspectName = "PaddedName";
            this.CardName.ImageAspectName = "ImageKey";
            this.CardName.IsEditable = false;
            this.CardName.MinimumWidth = 200;
            this.CardName.Text = "Card";
            this.CardName.Width = 200;
            // 
            // olvColumn10
            // 
            this.olvColumn10.AspectName = "MVid";
            this.olvColumn10.DisplayIndex = 1;
            this.olvColumn10.IsVisible = false;
            this.olvColumn10.Text = "MVid";
            // 
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "CardInstanceId";
            this.olvColumn11.Text = "CardInstanceId";
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Type";
            this.olvColumn2.IsEditable = false;
            this.olvColumn2.MinimumWidth = 100;
            this.olvColumn2.Text = "Type";
            this.olvColumn2.Width = 100;
            // 
            // ManaCost
            // 
            this.ManaCost.AspectName = "ManaCost";
            this.ManaCost.IsEditable = false;
            this.ManaCost.MinimumWidth = 100;
            this.ManaCost.Text = "Mana Cost";
            this.ManaCost.Width = 100;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "Edition";
            this.olvColumn4.IsEditable = false;
            this.olvColumn4.MinimumWidth = 100;
            this.olvColumn4.Text = "Set";
            this.olvColumn4.Width = 100;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "ColNumber";
            this.olvColumn5.IsEditable = false;
            this.olvColumn5.MinimumWidth = 50;
            this.olvColumn5.Text = "#";
            // 
            // CountColumn
            // 
            this.CountColumn.AspectName = "Count";
            this.CountColumn.MinimumWidth = 50;
            this.CountColumn.Text = "Count";
            // 
            // CostColumn
            // 
            this.CostColumn.AspectName = "Cost";
            this.CostColumn.MinimumWidth = 50;
            this.CostColumn.Text = "Cost";
            // 
            // TagsColumn
            // 
            this.TagsColumn.AspectName = "Tags";
            this.TagsColumn.MinimumWidth = 200;
            this.TagsColumn.Text = "Tags";
            this.TagsColumn.Width = 200;
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "SortableTimeAdded";
            this.olvColumn9.AspectToStringFormat = "";
            this.olvColumn9.IsEditable = false;
            this.olvColumn9.MinimumWidth = 100;
            this.olvColumn9.Text = "Added";
            this.olvColumn9.Width = 100;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "InsertionIndex";
            this.olvColumn1.Text = "InsertionIndex";
            // 
            // CollectionViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 435);
            this.Controls.Add(this.fastObjectListView1);
            this.DoubleBuffered = true;
            this.Name = "CollectionViewForm";
            this.Text = "Collection View";
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.OLVColumn CardName;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn ManaCost;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        internal BrightIdeasSoftware.FastObjectListView fastObjectListView1;
        private BrightIdeasSoftware.OLVColumn CountColumn;
        private BrightIdeasSoftware.OLVColumn CostColumn;
        private BrightIdeasSoftware.OLVColumn TagsColumn;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private BrightIdeasSoftware.OLVColumn olvColumn11;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
    }
}