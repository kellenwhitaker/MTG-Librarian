namespace MTG_Librarian
{
    partial class DBViewForm
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cardTextFilterTextBox = new EnhancedTextBox.EnhancedTextBox();
            this.typeFilterTextBox = new EnhancedTextBox.EnhancedTextBox();
            this.manaButtonsPanel = new System.Windows.Forms.Panel();
            this.whiteManaButton = new CustomControls.FlatButton();
            this.blueManaButton = new CustomControls.FlatButton();
            this.blackManaButton = new CustomControls.FlatButton();
            this.redManaButton = new CustomControls.FlatButton();
            this.genericManaButton = new CustomControls.FlatButton();
            this.greenManaButton = new CustomControls.FlatButton();
            this.colorlessManaButton = new CustomControls.FlatButton();
            this.formatFilterComboBox = new System.Windows.Forms.ComboBox();
            this.cardNameFilterBox = new EnhancedTextBox.EnhancedTextBox();
            this.setFilterBox = new EnhancedTextBox.EnhancedTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.setListView = new BrightIdeasSoftware.TreeListView();
            this.SetName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ReleaseDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.completeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.complete4Column = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateThisSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardListView = new BrightIdeasSoftware.FastObjectListView();
            this.copiesOwnedColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.priceColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.DisplayName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ManaCost = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Set = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CollectorNumber = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cardTextColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.manaButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setListView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cardListView)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cardTextFilterTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.typeFilterTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.manaButtonsPanel);
            this.splitContainer2.Panel1.Controls.Add(this.formatFilterComboBox);
            this.splitContainer2.Panel1.Controls.Add(this.cardNameFilterBox);
            this.splitContainer2.Panel1.Controls.Add(this.setFilterBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(1903, 570);
            this.splitContainer2.SplitterDistance = 30;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 2;
            // 
            // cardTextFilterTextBox
            // 
            this.cardTextFilterTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.cardTextFilterTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cardTextFilterTextBox.Location = new System.Drawing.Point(1121, 5);
            this.cardTextFilterTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.cardTextFilterTextBox.Name = "cardTextFilterTextBox";
            this.cardTextFilterTextBox.Placeholder = "Card Text Filter";
            this.cardTextFilterTextBox.Size = new System.Drawing.Size(239, 23);
            this.cardTextFilterTextBox.TabIndex = 13;
            this.cardTextFilterTextBox.Text = "Card Text Filter";
            this.cardTextFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cardNameFilterBox_KeyDown);
            // 
            // typeFilterTextBox
            // 
            this.typeFilterTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.typeFilterTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.typeFilterTextBox.Location = new System.Drawing.Point(690, 5);
            this.typeFilterTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.typeFilterTextBox.Name = "typeFilterTextBox";
            this.typeFilterTextBox.Placeholder = "Type Filter";
            this.typeFilterTextBox.Size = new System.Drawing.Size(199, 23);
            this.typeFilterTextBox.TabIndex = 12;
            this.typeFilterTextBox.Text = "Type Filter";
            this.typeFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cardNameFilterBox_KeyDown);
            // 
            // manaButtonsPanel
            // 
            this.manaButtonsPanel.Controls.Add(this.whiteManaButton);
            this.manaButtonsPanel.Controls.Add(this.blueManaButton);
            this.manaButtonsPanel.Controls.Add(this.blackManaButton);
            this.manaButtonsPanel.Controls.Add(this.redManaButton);
            this.manaButtonsPanel.Controls.Add(this.genericManaButton);
            this.manaButtonsPanel.Controls.Add(this.greenManaButton);
            this.manaButtonsPanel.Controls.Add(this.colorlessManaButton);
            this.manaButtonsPanel.Location = new System.Drawing.Point(898, 0);
            this.manaButtonsPanel.Margin = new System.Windows.Forms.Padding(4);
            this.manaButtonsPanel.Name = "manaButtonsPanel";
            this.manaButtonsPanel.Size = new System.Drawing.Size(253, 33);
            this.manaButtonsPanel.TabIndex = 11;
            // 
            // whiteManaButton
            // 
            this.whiteManaButton.Checked = false;
            this.whiteManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.whiteManaButton.FlatAppearance.BorderSize = 0;
            this.whiteManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.whiteManaButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteManaButton.Location = new System.Drawing.Point(4, 1);
            this.whiteManaButton.Margin = new System.Windows.Forms.Padding(4);
            this.whiteManaButton.Name = "whiteManaButton";
            this.whiteManaButton.Size = new System.Drawing.Size(33, 31);
            this.whiteManaButton.TabIndex = 1;
            this.whiteManaButton.UseVisualStyleBackColor = false;
            this.whiteManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // blueManaButton
            // 
            this.blueManaButton.Checked = false;
            this.blueManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.blueManaButton.FlatAppearance.BorderSize = 2;
            this.blueManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blueManaButton.Location = new System.Drawing.Point(39, 1);
            this.blueManaButton.Margin = new System.Windows.Forms.Padding(4);
            this.blueManaButton.Name = "blueManaButton";
            this.blueManaButton.Size = new System.Drawing.Size(33, 31);
            this.blueManaButton.TabIndex = 2;
            this.blueManaButton.UseVisualStyleBackColor = false;
            this.blueManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // blackManaButton
            // 
            this.blackManaButton.Checked = false;
            this.blackManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.blackManaButton.FlatAppearance.BorderSize = 2;
            this.blackManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blackManaButton.Location = new System.Drawing.Point(73, 1);
            this.blackManaButton.Margin = new System.Windows.Forms.Padding(4);
            this.blackManaButton.Name = "blackManaButton";
            this.blackManaButton.Size = new System.Drawing.Size(33, 31);
            this.blackManaButton.TabIndex = 3;
            this.blackManaButton.Text = "B";
            this.blackManaButton.UseVisualStyleBackColor = false;
            this.blackManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // redManaButton
            // 
            this.redManaButton.Checked = false;
            this.redManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.redManaButton.FlatAppearance.BorderSize = 2;
            this.redManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.redManaButton.Location = new System.Drawing.Point(108, 1);
            this.redManaButton.Margin = new System.Windows.Forms.Padding(4);
            this.redManaButton.Name = "redManaButton";
            this.redManaButton.Size = new System.Drawing.Size(33, 31);
            this.redManaButton.TabIndex = 4;
            this.redManaButton.Text = "R";
            this.redManaButton.UseVisualStyleBackColor = false;
            this.redManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // genericManaButton
            // 
            this.genericManaButton.Checked = false;
            this.genericManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.genericManaButton.FlatAppearance.BorderSize = 0;
            this.genericManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.genericManaButton.Location = new System.Drawing.Point(212, 1);
            this.genericManaButton.Margin = new System.Windows.Forms.Padding(4);
            this.genericManaButton.Name = "genericManaButton";
            this.genericManaButton.Size = new System.Drawing.Size(33, 31);
            this.genericManaButton.TabIndex = 7;
            this.genericManaButton.Text = "flatButton1";
            this.genericManaButton.UseVisualStyleBackColor = false;
            this.genericManaButton.Visible = false;
            this.genericManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // greenManaButton
            // 
            this.greenManaButton.Checked = false;
            this.greenManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.greenManaButton.FlatAppearance.BorderSize = 2;
            this.greenManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.greenManaButton.Location = new System.Drawing.Point(143, 1);
            this.greenManaButton.Margin = new System.Windows.Forms.Padding(4);
            this.greenManaButton.Name = "greenManaButton";
            this.greenManaButton.Size = new System.Drawing.Size(33, 31);
            this.greenManaButton.TabIndex = 5;
            this.greenManaButton.Text = "G";
            this.greenManaButton.UseVisualStyleBackColor = false;
            this.greenManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // colorlessManaButton
            // 
            this.colorlessManaButton.Checked = false;
            this.colorlessManaButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorlessManaButton.FlatAppearance.BorderSize = 0;
            this.colorlessManaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorlessManaButton.Location = new System.Drawing.Point(177, 1);
            this.colorlessManaButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorlessManaButton.Name = "colorlessManaButton";
            this.colorlessManaButton.Size = new System.Drawing.Size(33, 31);
            this.colorlessManaButton.TabIndex = 6;
            this.colorlessManaButton.Text = "flatButton1";
            this.colorlessManaButton.UseVisualStyleBackColor = false;
            this.colorlessManaButton.Click += new System.EventHandler(this.whiteManaButton_Click);
            // 
            // formatFilterComboBox
            // 
            this.formatFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatFilterComboBox.FormattingEnabled = true;
            this.formatFilterComboBox.Items.AddRange(new object[] {
            "Any format",
            "Standard",
            "Modern",
            "Pioneer",
            "Legacy",
            "Vintage",
            "Commander"});
            this.formatFilterComboBox.Location = new System.Drawing.Point(293, 6);
            this.formatFilterComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.formatFilterComboBox.Name = "formatFilterComboBox";
            this.formatFilterComboBox.Size = new System.Drawing.Size(160, 24);
            this.formatFilterComboBox.TabIndex = 10;
            this.formatFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.formatFilterComboBox_SelectedIndexChanged);
            // 
            // cardNameFilterBox
            // 
            this.cardNameFilterBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.cardNameFilterBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cardNameFilterBox.Location = new System.Drawing.Point(459, 5);
            this.cardNameFilterBox.Margin = new System.Windows.Forms.Padding(4);
            this.cardNameFilterBox.Name = "cardNameFilterBox";
            this.cardNameFilterBox.Placeholder = "Card Name Filter";
            this.cardNameFilterBox.Size = new System.Drawing.Size(221, 23);
            this.cardNameFilterBox.TabIndex = 8;
            this.cardNameFilterBox.Text = "Card Name Filter";
            this.cardNameFilterBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cardNameFilterBox_KeyDown);
            // 
            // setFilterBox
            // 
            this.setFilterBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.setFilterBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.setFilterBox.Location = new System.Drawing.Point(4, 6);
            this.setFilterBox.Margin = new System.Windows.Forms.Padding(4);
            this.setFilterBox.Name = "setFilterBox";
            this.setFilterBox.Placeholder = "Set Filter";
            this.setFilterBox.Size = new System.Drawing.Size(280, 23);
            this.setFilterBox.TabIndex = 0;
            this.setFilterBox.Text = "Set Filter";
            this.setFilterBox.TextChanged += new System.EventHandler(this.setFilterBox_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.setListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cardListView);
            this.splitContainer1.Size = new System.Drawing.Size(1903, 539);
            this.splitContainer1.SplitterDistance = 741;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // setListView
            // 
            this.setListView.AllColumns.Add(this.SetName);
            this.setListView.AllColumns.Add(this.ReleaseDate);
            this.setListView.AllColumns.Add(this.completeColumn);
            this.setListView.AllColumns.Add(this.complete4Column);
            this.setListView.CellEditUseWholeCell = false;
            this.setListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SetName,
            this.completeColumn,
            this.complete4Column});
            this.setListView.ContextMenuStrip = this.contextMenuStrip1;
            this.setListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setListView.HeaderMaximumHeight = 0;
            this.setListView.HideSelection = false;
            this.setListView.IsSimpleDragSource = true;
            this.setListView.Location = new System.Drawing.Point(0, 0);
            this.setListView.Margin = new System.Windows.Forms.Padding(4);
            this.setListView.MultiSelect = false;
            this.setListView.Name = "setListView";
            this.setListView.OverlayText.Text = "";
            this.setListView.SelectedBackColor = System.Drawing.Color.SteelBlue;
            this.setListView.SelectedForeColor = System.Drawing.Color.White;
            this.setListView.ShowGroups = false;
            this.setListView.Size = new System.Drawing.Size(741, 539);
            this.setListView.TabIndex = 3;
            this.setListView.UnfocusedSelectedBackColor = System.Drawing.Color.LightGray;
            this.setListView.UseCellFormatEvents = true;
            this.setListView.UseCompatibleStateImageBehavior = false;
            this.setListView.View = System.Windows.Forms.View.Details;
            this.setListView.VirtualMode = true;
            this.setListView.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.treeListView1_FormatCell);
            this.setListView.SelectionChanged += new System.EventHandler(this.setListView_SelectionChanged);
            this.setListView.ItemActivate += new System.EventHandler(this.treeListView1_ItemActivate);
            // 
            // SetName
            // 
            this.SetName.AspectName = "Text";
            this.SetName.ImageAspectName = "ImageKey";
            this.SetName.IsTileViewColumn = true;
            this.SetName.Width = 300;
            // 
            // ReleaseDate
            // 
            this.ReleaseDate.AspectName = "ReleaseDate";
            this.ReleaseDate.DisplayIndex = 1;
            this.ReleaseDate.IsVisible = false;
            // 
            // completeColumn
            // 
            this.completeColumn.AspectName = "Complete";
            this.completeColumn.Text = "Complete";
            // 
            // complete4Column
            // 
            this.complete4Column.AspectName = "Complete4";
            this.complete4Column.Text = "Complete (4)";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateThisSetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(178, 28);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // updateThisSetToolStripMenuItem
            // 
            this.updateThisSetToolStripMenuItem.Name = "updateThisSetToolStripMenuItem";
            this.updateThisSetToolStripMenuItem.Size = new System.Drawing.Size(177, 24);
            this.updateThisSetToolStripMenuItem.Text = "Update this set";
            this.updateThisSetToolStripMenuItem.Click += new System.EventHandler(this.updateThisSetToolStripMenuItem_Click);
            // 
            // cardListView
            // 
            this.cardListView.AllColumns.Add(this.copiesOwnedColumn);
            this.cardListView.AllColumns.Add(this.priceColumn);
            this.cardListView.AllColumns.Add(this.DisplayName);
            this.cardListView.AllColumns.Add(this.olvColumn2);
            this.cardListView.AllColumns.Add(this.ManaCost);
            this.cardListView.AllColumns.Add(this.Set);
            this.cardListView.AllColumns.Add(this.CollectorNumber);
            this.cardListView.AllColumns.Add(this.cardTextColumn);
            this.cardListView.CellEditUseWholeCell = false;
            this.cardListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.copiesOwnedColumn,
            this.priceColumn,
            this.DisplayName,
            this.olvColumn2,
            this.ManaCost,
            this.Set,
            this.CollectorNumber,
            this.cardTextColumn});
            this.cardListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardListView.EmptyListMsg = "No cards";
            this.cardListView.FullRowSelect = true;
            this.cardListView.HeaderWordWrap = true;
            this.cardListView.HideSelection = false;
            this.cardListView.IsSimpleDragSource = true;
            this.cardListView.Location = new System.Drawing.Point(0, 0);
            this.cardListView.Margin = new System.Windows.Forms.Padding(4);
            this.cardListView.Name = "cardListView";
            this.cardListView.SelectedBackColor = System.Drawing.Color.SteelBlue;
            this.cardListView.SelectedForeColor = System.Drawing.Color.White;
            this.cardListView.ShowGroups = false;
            this.cardListView.Size = new System.Drawing.Size(1157, 539);
            this.cardListView.TabIndex = 0;
            this.cardListView.UnfocusedSelectedBackColor = System.Drawing.Color.LightGray;
            this.cardListView.UseCompatibleStateImageBehavior = false;
            this.cardListView.View = System.Windows.Forms.View.Details;
            this.cardListView.VirtualMode = true;
            this.cardListView.BeforeSorting += new System.EventHandler<BrightIdeasSoftware.BeforeSortingEventArgs>(this.cardListView_BeforeSorting);
            this.cardListView.Scroll += new System.EventHandler<System.Windows.Forms.ScrollEventArgs>(this.cardListView_Scroll);
            this.cardListView.ItemActivate += new System.EventHandler(this.fastObjectListView1_ItemActivate);
            this.cardListView.SelectedIndexChanged += new System.EventHandler(this.fastObjectListView1_SelectedIndexChanged);
            this.cardListView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.fastObjectListView1_GiveFeedback);
            this.cardListView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cardListView_KeyPress);
            // 
            // copiesOwnedColumn
            // 
            this.copiesOwnedColumn.AspectName = "CopiesOwned";
            this.copiesOwnedColumn.Text = "Owned";
            this.copiesOwnedColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.copiesOwnedColumn.Width = 50;
            // 
            // priceColumn
            // 
            this.priceColumn.AspectName = "Price";
            this.priceColumn.AspectToStringFormat = "{0:0.00}";
            this.priceColumn.Text = "Price";
            // 
            // DisplayName
            // 
            this.DisplayName.AspectName = "DisplayName";
            this.DisplayName.ImageAspectName = "ImageKey";
            this.DisplayName.MinimumWidth = 200;
            this.DisplayName.Text = "Card Name";
            this.DisplayName.Width = 200;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "DisplayTypeLine";
            this.olvColumn2.MinimumWidth = 100;
            this.olvColumn2.Text = "Type";
            this.olvColumn2.Width = 100;
            // 
            // ManaCost
            // 
            this.ManaCost.AspectName = "ManaCost";
            this.ManaCost.MinimumWidth = 100;
            this.ManaCost.Text = "Mana Cost";
            this.ManaCost.Width = 100;
            // 
            // Set
            // 
            this.Set.AspectName = "Set";
            this.Set.MinimumWidth = 100;
            this.Set.Text = "Set";
            this.Set.Width = 100;
            // 
            // CollectorNumber
            // 
            this.CollectorNumber.AspectName = "CollectorNumber";
            this.CollectorNumber.MinimumWidth = 40;
            this.CollectorNumber.Text = "#";
            this.CollectorNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CollectorNumber.Width = 40;
            // 
            // cardTextColumn
            // 
            this.cardTextColumn.AspectName = "DisplayText";
            this.cardTextColumn.Text = "Text";
            this.cardTextColumn.Width = 200;
            // 
            // DBViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1903, 570);
            this.Controls.Add(this.splitContainer2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DBViewForm";
            this.Text = "Catalog";
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.manaButtonsPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.setListView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cardListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public BrightIdeasSoftware.TreeListView setListView;
        private BrightIdeasSoftware.OLVColumn SetName;
        private EnhancedTextBox.EnhancedTextBox setFilterBox;
        private BrightIdeasSoftware.OLVColumn DisplayName;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn ManaCost;
        private BrightIdeasSoftware.OLVColumn Set;
        private BrightIdeasSoftware.OLVColumn CollectorNumber;
        private CustomControls.FlatButton whiteManaButton;
        private CustomControls.FlatButton blueManaButton;
        private CustomControls.FlatButton blackManaButton;
        private CustomControls.FlatButton greenManaButton;
        private CustomControls.FlatButton redManaButton;
        private CustomControls.FlatButton colorlessManaButton;
        private CustomControls.FlatButton genericManaButton;
        private BrightIdeasSoftware.OLVColumn ReleaseDate;
        private EnhancedTextBox.EnhancedTextBox cardNameFilterBox;
        private BrightIdeasSoftware.OLVColumn completeColumn;
        private BrightIdeasSoftware.OLVColumn complete4Column;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem updateThisSetToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn copiesOwnedColumn;
        private System.Windows.Forms.ComboBox formatFilterComboBox;
        public BrightIdeasSoftware.FastObjectListView cardListView;
        private System.Windows.Forms.Panel manaButtonsPanel;
        private EnhancedTextBox.EnhancedTextBox typeFilterTextBox;
        private EnhancedTextBox.EnhancedTextBox cardTextFilterTextBox;
        private BrightIdeasSoftware.OLVColumn cardTextColumn;
        private BrightIdeasSoftware.OLVColumn priceColumn;
    }
}