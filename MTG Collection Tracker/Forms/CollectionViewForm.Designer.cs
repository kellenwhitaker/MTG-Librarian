namespace MTG_Librarian
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
            this.components = new System.ComponentModel.Container();
            this.cardListView = new BrightIdeasSoftware.FastObjectListView();
            this.CardName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FoilColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ManaCost = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CountColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CostColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.TagsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.SortableTimeAdded = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.TimeAddedColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cardListViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteCardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardNameFilterTextBox = new EnhancedTextBox.EnhancedTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rarityFilterComboBox = new System.Windows.Forms.ComboBox();
            this.genericManaButton = new CustomControls.FlatButton();
            this.colorlessManaButton = new CustomControls.FlatButton();
            this.greenManaButton = new CustomControls.FlatButton();
            this.redManaButton = new CustomControls.FlatButton();
            this.blackManaButton = new CustomControls.FlatButton();
            this.blueManaButton = new CustomControls.FlatButton();
            this.whiteManaButton = new CustomControls.FlatButton();
            this.setFilterTextBox = new EnhancedTextBox.EnhancedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cardListView)).BeginInit();
            this.cardListViewMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cardListView
            // 
            this.cardListView.AllColumns.Add(this.CardName);
            this.cardListView.AllColumns.Add(this.FoilColumn);
            this.cardListView.AllColumns.Add(this.olvColumn10);
            this.cardListView.AllColumns.Add(this.olvColumn2);
            this.cardListView.AllColumns.Add(this.ManaCost);
            this.cardListView.AllColumns.Add(this.olvColumn4);
            this.cardListView.AllColumns.Add(this.olvColumn5);
            this.cardListView.AllColumns.Add(this.CountColumn);
            this.cardListView.AllColumns.Add(this.CostColumn);
            this.cardListView.AllColumns.Add(this.TagsColumn);
            this.cardListView.AllColumns.Add(this.SortableTimeAdded);
            this.cardListView.AllColumns.Add(this.olvColumn1);
            this.cardListView.AllColumns.Add(this.TimeAddedColumn);
            this.cardListView.AllowDrop = true;
            this.cardListView.BackColor = System.Drawing.Color.White;
            this.cardListView.CellEditUseWholeCell = false;
            this.cardListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CardName,
            this.FoilColumn,
            this.olvColumn2,
            this.ManaCost,
            this.olvColumn4,
            this.olvColumn5,
            this.CountColumn,
            this.CostColumn,
            this.TagsColumn,
            this.TimeAddedColumn});
            this.cardListView.ContextMenuStrip = this.cardListViewMenuStrip;
            this.cardListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardListView.FullRowSelect = true;
            this.cardListView.GridLines = true;
            this.cardListView.HideSelection = false;
            this.cardListView.IsSimpleDragSource = true;
            this.cardListView.IsSimpleDropSink = true;
            this.cardListView.Location = new System.Drawing.Point(0, 0);
            this.cardListView.Name = "cardListView";
            this.cardListView.SelectedBackColor = System.Drawing.Color.SteelBlue;
            this.cardListView.SelectedForeColor = System.Drawing.Color.White;
            this.cardListView.ShowGroups = false;
            this.cardListView.Size = new System.Drawing.Size(975, 401);
            this.cardListView.TabIndex = 0;
            this.cardListView.UnfocusedSelectedBackColor = System.Drawing.Color.LightGray;
            this.cardListView.UseCellFormatEvents = true;
            this.cardListView.UseCompatibleStateImageBehavior = false;
            this.cardListView.View = System.Windows.Forms.View.Details;
            this.cardListView.VirtualMode = true;
            this.cardListView.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.fastObjectListView1_CellEditFinished);
            this.cardListView.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.cardListView_CellEditFinishing);
            this.cardListView.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.cardListView_CellEditStarting);
            this.cardListView.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.fastObjectListView1_CellClick);
            this.cardListView.SubItemChecking += new System.EventHandler<BrightIdeasSoftware.SubItemCheckingEventArgs>(this.cardListView_SubItemChecking);
            this.cardListView.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.fastObjectListView1_ModelCanDrop);
            this.cardListView.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.fastObjectListView1_ModelDropped);
            this.cardListView.SelectionChanged += new System.EventHandler(this.fastObjectListView1_SelectionChanged);
            this.cardListView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fastObjectListView1_KeyPress);
            this.cardListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cardListView_KeyUp);
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
            // FoilColumn
            // 
            this.FoilColumn.AspectName = "Foil";
            this.FoilColumn.CheckBoxes = true;
            this.FoilColumn.Text = "Foil";
            this.FoilColumn.Width = 30;
            // 
            // olvColumn10
            // 
            this.olvColumn10.AspectName = "multiverseId";
            this.olvColumn10.DisplayIndex = 2;
            this.olvColumn10.IsVisible = false;
            this.olvColumn10.Text = "MVid";
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "type";
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
            this.olvColumn5.AspectName = "number";
            this.olvColumn5.IsEditable = false;
            this.olvColumn5.MinimumWidth = 50;
            this.olvColumn5.Text = "#";
            // 
            // CountColumn
            // 
            this.CountColumn.AspectName = "Count";
            this.CountColumn.AutoCompleteEditor = false;
            this.CountColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.CountColumn.MinimumWidth = 50;
            this.CountColumn.Text = "Count";
            // 
            // CostColumn
            // 
            this.CostColumn.AspectName = "Cost";
            this.CostColumn.AutoCompleteEditor = false;
            this.CostColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.CostColumn.MinimumWidth = 50;
            this.CostColumn.Text = "Cost";
            // 
            // TagsColumn
            // 
            this.TagsColumn.AspectName = "Tags";
            this.TagsColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.TagsColumn.MinimumWidth = 200;
            this.TagsColumn.Text = "Tags";
            this.TagsColumn.Width = 200;
            // 
            // SortableTimeAdded
            // 
            this.SortableTimeAdded.AspectName = "SortableTimeAdded";
            this.SortableTimeAdded.AspectToStringFormat = "";
            this.SortableTimeAdded.DisplayIndex = 9;
            this.SortableTimeAdded.IsEditable = false;
            this.SortableTimeAdded.IsVisible = false;
            this.SortableTimeAdded.MinimumWidth = 100;
            this.SortableTimeAdded.Text = "SortableTimeAdded";
            this.SortableTimeAdded.Width = 100;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "InsertionIndex";
            this.olvColumn1.DisplayIndex = 9;
            this.olvColumn1.IsVisible = false;
            this.olvColumn1.Text = "InsertionIndex";
            // 
            // TimeAddedColumn
            // 
            this.TimeAddedColumn.AspectName = "TimeAdded";
            this.TimeAddedColumn.IsEditable = false;
            this.TimeAddedColumn.Text = "Added";
            this.TimeAddedColumn.Width = 200;
            // 
            // cardListViewMenuStrip
            // 
            this.cardListViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteCardsToolStripMenuItem});
            this.cardListViewMenuStrip.Name = "cardListViewMenuStrip";
            this.cardListViewMenuStrip.Size = new System.Drawing.Size(147, 26);
            this.cardListViewMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.cardListViewMenuStrip_Opening);
            // 
            // deleteCardsToolStripMenuItem
            // 
            this.deleteCardsToolStripMenuItem.Name = "deleteCardsToolStripMenuItem";
            this.deleteCardsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.deleteCardsToolStripMenuItem.Text = "Delete card(s)";
            this.deleteCardsToolStripMenuItem.Click += new System.EventHandler(this.deleteCardsToolStripMenuItem_Click);
            // 
            // cardNameFilterTextBox
            // 
            this.cardNameFilterTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.cardNameFilterTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cardNameFilterTextBox.Location = new System.Drawing.Point(3, 8);
            this.cardNameFilterTextBox.Name = "cardNameFilterTextBox";
            this.cardNameFilterTextBox.Placeholder = "Card Name Filter";
            this.cardNameFilterTextBox.Size = new System.Drawing.Size(189, 20);
            this.cardNameFilterTextBox.TabIndex = 1;
            this.cardNameFilterTextBox.Text = "Card Name Filter";
            this.cardNameFilterTextBox.TextChanged += new System.EventHandler(this.cardNameFilterTextBox_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rarityFilterComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.genericManaButton);
            this.splitContainer1.Panel1.Controls.Add(this.colorlessManaButton);
            this.splitContainer1.Panel1.Controls.Add(this.greenManaButton);
            this.splitContainer1.Panel1.Controls.Add(this.redManaButton);
            this.splitContainer1.Panel1.Controls.Add(this.blackManaButton);
            this.splitContainer1.Panel1.Controls.Add(this.blueManaButton);
            this.splitContainer1.Panel1.Controls.Add(this.whiteManaButton);
            this.splitContainer1.Panel1.Controls.Add(this.setFilterTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.cardNameFilterTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cardListView);
            this.splitContainer1.Size = new System.Drawing.Size(975, 435);
            this.splitContainer1.SplitterDistance = 30;
            this.splitContainer1.TabIndex = 2;
            // 
            // rarityFilterComboBox
            // 
            this.rarityFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rarityFilterComboBox.FormattingEnabled = true;
            this.rarityFilterComboBox.Items.AddRange(new object[] {
            "All Rarities",
            "Basic Land",
            "Common",
            "Uncommon",
            "Rare",
            "Mythic"});
            this.rarityFilterComboBox.Location = new System.Drawing.Point(576, 8);
            this.rarityFilterComboBox.Name = "rarityFilterComboBox";
            this.rarityFilterComboBox.Size = new System.Drawing.Size(121, 21);
            this.rarityFilterComboBox.TabIndex = 16;
            this.rarityFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.rarityFilterComboBox_SelectedIndexChanged);
            // 
            // genericManaButton
            // 
            this.genericManaButton.Checked = false;
            this.genericManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.genericManaButton.FlatAppearance.BorderSize = 0;
            this.genericManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.genericManaButton.Location = new System.Drawing.Point(384, 5);
            this.genericManaButton.Name = "genericManaButton";
            this.genericManaButton.Size = new System.Drawing.Size(25, 25);
            this.genericManaButton.TabIndex = 14;
            this.genericManaButton.Text = "flatButton1";
            this.genericManaButton.UseVisualStyleBackColor = false;
            this.genericManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // colorlessManaButton
            // 
            this.colorlessManaButton.Checked = false;
            this.colorlessManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorlessManaButton.FlatAppearance.BorderSize = 0;
            this.colorlessManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorlessManaButton.Location = new System.Drawing.Point(353, 5);
            this.colorlessManaButton.Name = "colorlessManaButton";
            this.colorlessManaButton.Size = new System.Drawing.Size(25, 25);
            this.colorlessManaButton.TabIndex = 13;
            this.colorlessManaButton.Text = "flatButton1";
            this.colorlessManaButton.UseVisualStyleBackColor = false;
            this.colorlessManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // greenManaButton
            // 
            this.greenManaButton.Checked = false;
            this.greenManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.greenManaButton.FlatAppearance.BorderSize = 2;
            this.greenManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.greenManaButton.Location = new System.Drawing.Point(322, 5);
            this.greenManaButton.Name = "greenManaButton";
            this.greenManaButton.Size = new System.Drawing.Size(25, 25);
            this.greenManaButton.TabIndex = 12;
            this.greenManaButton.Text = "G";
            this.greenManaButton.UseVisualStyleBackColor = false;
            this.greenManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // redManaButton
            // 
            this.redManaButton.Checked = false;
            this.redManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.redManaButton.FlatAppearance.BorderSize = 2;
            this.redManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.redManaButton.Location = new System.Drawing.Point(291, 5);
            this.redManaButton.Name = "redManaButton";
            this.redManaButton.Size = new System.Drawing.Size(25, 25);
            this.redManaButton.TabIndex = 11;
            this.redManaButton.Text = "R";
            this.redManaButton.UseVisualStyleBackColor = false;
            this.redManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // blackManaButton
            // 
            this.blackManaButton.Checked = false;
            this.blackManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.blackManaButton.FlatAppearance.BorderSize = 2;
            this.blackManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blackManaButton.Location = new System.Drawing.Point(260, 5);
            this.blackManaButton.Name = "blackManaButton";
            this.blackManaButton.Size = new System.Drawing.Size(25, 25);
            this.blackManaButton.TabIndex = 10;
            this.blackManaButton.Text = "B";
            this.blackManaButton.UseVisualStyleBackColor = false;
            this.blackManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // blueManaButton
            // 
            this.blueManaButton.Checked = false;
            this.blueManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.blueManaButton.FlatAppearance.BorderSize = 2;
            this.blueManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blueManaButton.Location = new System.Drawing.Point(229, 5);
            this.blueManaButton.Name = "blueManaButton";
            this.blueManaButton.Size = new System.Drawing.Size(25, 25);
            this.blueManaButton.TabIndex = 9;
            this.blueManaButton.UseVisualStyleBackColor = false;
            this.blueManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // whiteManaButton
            // 
            this.whiteManaButton.Checked = false;
            this.whiteManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.whiteManaButton.FlatAppearance.BorderSize = 0;
            this.whiteManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.whiteManaButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteManaButton.Location = new System.Drawing.Point(198, 5);
            this.whiteManaButton.Name = "whiteManaButton";
            this.whiteManaButton.Size = new System.Drawing.Size(25, 25);
            this.whiteManaButton.TabIndex = 8;
            this.whiteManaButton.UseVisualStyleBackColor = false;
            this.whiteManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // setFilterTextBox
            // 
            this.setFilterTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.setFilterTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.setFilterTextBox.Location = new System.Drawing.Point(415, 8);
            this.setFilterTextBox.Name = "setFilterTextBox";
            this.setFilterTextBox.Placeholder = "Set Filter";
            this.setFilterTextBox.Size = new System.Drawing.Size(155, 20);
            this.setFilterTextBox.TabIndex = 2;
            this.setFilterTextBox.Text = "Set Filter";
            this.setFilterTextBox.TextChanged += new System.EventHandler(this.cardNameFilterTextBox_TextChanged);
            // 
            // CollectionViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 435);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "CollectionViewForm";
            this.Text = "Collection View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CollectionViewForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.cardListView)).EndInit();
            this.cardListViewMenuStrip.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.OLVColumn CardName;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn ManaCost;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        public BrightIdeasSoftware.FastObjectListView cardListView;
        private BrightIdeasSoftware.OLVColumn CountColumn;
        private BrightIdeasSoftware.OLVColumn CostColumn;
        private BrightIdeasSoftware.OLVColumn TagsColumn;
        private BrightIdeasSoftware.OLVColumn SortableTimeAdded;
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn TimeAddedColumn;
        private BrightIdeasSoftware.OLVColumn FoilColumn;
        private System.Windows.Forms.ContextMenuStrip cardListViewMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteCardsToolStripMenuItem;
        private EnhancedTextBox.EnhancedTextBox cardNameFilterTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private EnhancedTextBox.EnhancedTextBox setFilterTextBox;
        private CustomControls.FlatButton genericManaButton;
        private CustomControls.FlatButton colorlessManaButton;
        private CustomControls.FlatButton greenManaButton;
        private CustomControls.FlatButton redManaButton;
        private CustomControls.FlatButton blackManaButton;
        private CustomControls.FlatButton blueManaButton;
        private CustomControls.FlatButton whiteManaButton;
        private System.Windows.Forms.ComboBox rarityFilterComboBox;
    }
}