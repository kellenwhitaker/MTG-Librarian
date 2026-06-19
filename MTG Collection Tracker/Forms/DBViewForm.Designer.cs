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
            this.clearAllButton = new System.Windows.Forms.Button();
            this.uniqueComboBox = new System.Windows.Forms.ComboBox();
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
            this.setsPanel = new System.Windows.Forms.Panel();
            this.setListView = new BrightIdeasSoftware.TreeListView();
            this.SetName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ReleaseDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.completeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.complete4Column = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateThisSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compatibleTabControl1 = new CompatibleTabControl.CompatibleTabControl();
            this.searchParametersPanel = new System.Windows.Forms.Panel();
            this.includeVariationsCheckBox = new System.Windows.Forms.CheckBox();
            this.languageClearButton = new System.Windows.Forms.Button();
            this.flavorTextClearButton = new System.Windows.Forms.Button();
            this.artistClearButton = new System.Windows.Forms.Button();
            this.pricesClearButton = new System.Windows.Forms.Button();
            this.attributesClearButton = new System.Windows.Forms.Button();
            this.loyaltyClearButton = new System.Windows.Forms.Button();
            this.toughnessClearButton = new System.Windows.Forms.Button();
            this.powerClearButton = new System.Windows.Forms.Button();
            this.cmcClearButton = new System.Windows.Forms.Button();
            this.manaCostClearButton = new System.Windows.Forms.Button();
            this.commanderClearButton = new System.Windows.Forms.Button();
            this.colorsClearButton = new System.Windows.Forms.Button();
            this.rarityClearButton = new System.Windows.Forms.Button();
            this.gameClearButton = new System.Windows.Forms.Button();
            this.rarityMythicCheckBox = new System.Windows.Forms.CheckBox();
            this.rarityRareCheckBox = new System.Windows.Forms.CheckBox();
            this.rarityUncommonCheckBox = new System.Windows.Forms.CheckBox();
            this.rarityCommonCheckBox = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.magicOnlineCheckBox = new System.Windows.Forms.CheckBox();
            this.arenaCheckBox = new System.Windows.Forms.CheckBox();
            this.paperCheckBox = new System.Windows.Forms.CheckBox();
            this.manaCostTypeComboBox = new System.Windows.Forms.ComboBox();
            this.colorsOperatorComboBox = new System.Windows.Forms.ComboBox();
            this.colorsComboBox = new System.Windows.Forms.ComboBox();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.flavorTextTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.artistTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.pricesPriceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pricesOperatorComboBox = new System.Windows.Forms.ComboBox();
            this.pricesCurrencyComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.attributesObjectListView = new BrightIdeasSoftware.FastObjectListView();
            this.notColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.attributeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.descriptionColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.attributesComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.loyaltyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.loyaltyComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.toughnessNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.toughnessComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.powerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.powerComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmcNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.cmcOperatorComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.manaCostComboBox = new System.Windows.Forms.ComboBox();
            this.manaCostTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.commanderWhiteButton = new CustomControls.FlatButton();
            this.commanderBlueButton = new CustomControls.FlatButton();
            this.commanderBlackButton = new CustomControls.FlatButton();
            this.commanderRedButton = new CustomControls.FlatButton();
            this.commanderGreenButton = new CustomControls.FlatButton();
            this.commanderColorlessButton = new CustomControls.FlatButton();
            this.label2 = new System.Windows.Forms.Label();
            this.colorsWhiteButton = new CustomControls.FlatButton();
            this.colorsBlueButton = new CustomControls.FlatButton();
            this.colorsBlackButton = new CustomControls.FlatButton();
            this.colorsRedButton = new CustomControls.FlatButton();
            this.colorsGreenButton = new CustomControls.FlatButton();
            this.colorsColorlessButton = new CustomControls.FlatButton();
            this.cardListView = new BrightIdeasSoftware.FastObjectListView();
            this.copiesOwnedColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.priceColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.DisplayName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ManaCost = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Set = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CollectorNumber = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cardTextColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.manaButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.setsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setListView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.compatibleTabControl1.SuspendLayout();
            this.searchParametersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pricesPriceNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributesObjectListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loyaltyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toughnessNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmcNumericUpDown)).BeginInit();
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
            this.splitContainer2.Panel1.Controls.Add(this.clearAllButton);
            this.splitContainer2.Panel1.Controls.Add(this.uniqueComboBox);
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
            // clearAllButton
            // 
            this.clearAllButton.Location = new System.Drawing.Point(1494, 4);
            this.clearAllButton.Name = "clearAllButton";
            this.clearAllButton.Size = new System.Drawing.Size(75, 28);
            this.clearAllButton.TabIndex = 16;
            this.clearAllButton.Text = "Clear all";
            this.clearAllButton.UseVisualStyleBackColor = true;
            this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click);
            // 
            // uniqueComboBox
            // 
            this.uniqueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uniqueComboBox.FormattingEnabled = true;
            this.uniqueComboBox.Items.AddRange(new object[] {
            "Unique cards",
            "Unique art",
            "Unique prints"});
            this.uniqueComboBox.Location = new System.Drawing.Point(1367, 5);
            this.uniqueComboBox.Name = "uniqueComboBox";
            this.uniqueComboBox.Size = new System.Drawing.Size(121, 24);
            this.uniqueComboBox.TabIndex = 15;
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
            this.splitContainer1.Panel1.Controls.Add(this.setsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.compatibleTabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cardListView);
            this.splitContainer1.Size = new System.Drawing.Size(1903, 539);
            this.splitContainer1.SplitterDistance = 741;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // setsPanel
            // 
            this.setsPanel.Controls.Add(this.setListView);
            this.setsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.setsPanel.Location = new System.Drawing.Point(0, 0);
            this.setsPanel.Name = "setsPanel";
            this.setsPanel.Size = new System.Drawing.Size(93, 539);
            this.setsPanel.TabIndex = 1;
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
            this.setListView.Size = new System.Drawing.Size(93, 539);
            this.setListView.TabIndex = 5;
            this.setListView.UnfocusedSelectedBackColor = System.Drawing.Color.LightGray;
            this.setListView.UseCellFormatEvents = true;
            this.setListView.UseCompatibleStateImageBehavior = false;
            this.setListView.View = System.Windows.Forms.View.Details;
            this.setListView.VirtualMode = true;
            this.setListView.SelectionChanged += new System.EventHandler(this.setListView_SelectionChanged);
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
            // compatibleTabControl1
            // 
            this.compatibleTabControl1.Controls.Add(this.searchParametersPanel);
            this.compatibleTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compatibleTabControl1.Location = new System.Drawing.Point(0, 0);
            this.compatibleTabControl1.Name = "compatibleTabControl1";
            this.compatibleTabControl1.Size = new System.Drawing.Size(741, 539);
            this.compatibleTabControl1.TabIndex = 2;
            // 
            // searchParametersPanel
            // 
            this.searchParametersPanel.AutoScroll = true;
            this.searchParametersPanel.AutoScrollMargin = new System.Drawing.Size(0, 40);
            this.searchParametersPanel.BackColor = System.Drawing.Color.White;
            this.searchParametersPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchParametersPanel.Controls.Add(this.includeVariationsCheckBox);
            this.searchParametersPanel.Controls.Add(this.languageClearButton);
            this.searchParametersPanel.Controls.Add(this.flavorTextClearButton);
            this.searchParametersPanel.Controls.Add(this.artistClearButton);
            this.searchParametersPanel.Controls.Add(this.pricesClearButton);
            this.searchParametersPanel.Controls.Add(this.attributesClearButton);
            this.searchParametersPanel.Controls.Add(this.loyaltyClearButton);
            this.searchParametersPanel.Controls.Add(this.toughnessClearButton);
            this.searchParametersPanel.Controls.Add(this.powerClearButton);
            this.searchParametersPanel.Controls.Add(this.cmcClearButton);
            this.searchParametersPanel.Controls.Add(this.manaCostClearButton);
            this.searchParametersPanel.Controls.Add(this.commanderClearButton);
            this.searchParametersPanel.Controls.Add(this.colorsClearButton);
            this.searchParametersPanel.Controls.Add(this.rarityClearButton);
            this.searchParametersPanel.Controls.Add(this.gameClearButton);
            this.searchParametersPanel.Controls.Add(this.rarityMythicCheckBox);
            this.searchParametersPanel.Controls.Add(this.rarityRareCheckBox);
            this.searchParametersPanel.Controls.Add(this.rarityUncommonCheckBox);
            this.searchParametersPanel.Controls.Add(this.rarityCommonCheckBox);
            this.searchParametersPanel.Controls.Add(this.label14);
            this.searchParametersPanel.Controls.Add(this.label8);
            this.searchParametersPanel.Controls.Add(this.magicOnlineCheckBox);
            this.searchParametersPanel.Controls.Add(this.arenaCheckBox);
            this.searchParametersPanel.Controls.Add(this.paperCheckBox);
            this.searchParametersPanel.Controls.Add(this.manaCostTypeComboBox);
            this.searchParametersPanel.Controls.Add(this.colorsOperatorComboBox);
            this.searchParametersPanel.Controls.Add(this.colorsComboBox);
            this.searchParametersPanel.Controls.Add(this.languageComboBox);
            this.searchParametersPanel.Controls.Add(this.label13);
            this.searchParametersPanel.Controls.Add(this.flavorTextTextBox);
            this.searchParametersPanel.Controls.Add(this.label12);
            this.searchParametersPanel.Controls.Add(this.artistTextBox);
            this.searchParametersPanel.Controls.Add(this.label11);
            this.searchParametersPanel.Controls.Add(this.pricesPriceNumericUpDown);
            this.searchParametersPanel.Controls.Add(this.pricesOperatorComboBox);
            this.searchParametersPanel.Controls.Add(this.pricesCurrencyComboBox);
            this.searchParametersPanel.Controls.Add(this.label10);
            this.searchParametersPanel.Controls.Add(this.attributesObjectListView);
            this.searchParametersPanel.Controls.Add(this.attributesComboBox);
            this.searchParametersPanel.Controls.Add(this.label9);
            this.searchParametersPanel.Controls.Add(this.loyaltyNumericUpDown);
            this.searchParametersPanel.Controls.Add(this.loyaltyComboBox);
            this.searchParametersPanel.Controls.Add(this.label7);
            this.searchParametersPanel.Controls.Add(this.toughnessNumericUpDown);
            this.searchParametersPanel.Controls.Add(this.toughnessComboBox);
            this.searchParametersPanel.Controls.Add(this.label6);
            this.searchParametersPanel.Controls.Add(this.powerNumericUpDown);
            this.searchParametersPanel.Controls.Add(this.powerComboBox);
            this.searchParametersPanel.Controls.Add(this.label5);
            this.searchParametersPanel.Controls.Add(this.cmcNumericUpDown);
            this.searchParametersPanel.Controls.Add(this.cmcOperatorComboBox);
            this.searchParametersPanel.Controls.Add(this.label4);
            this.searchParametersPanel.Controls.Add(this.manaCostComboBox);
            this.searchParametersPanel.Controls.Add(this.manaCostTextBox);
            this.searchParametersPanel.Controls.Add(this.label3);
            this.searchParametersPanel.Controls.Add(this.commanderWhiteButton);
            this.searchParametersPanel.Controls.Add(this.commanderBlueButton);
            this.searchParametersPanel.Controls.Add(this.commanderBlackButton);
            this.searchParametersPanel.Controls.Add(this.commanderRedButton);
            this.searchParametersPanel.Controls.Add(this.commanderGreenButton);
            this.searchParametersPanel.Controls.Add(this.commanderColorlessButton);
            this.searchParametersPanel.Controls.Add(this.label2);
            this.searchParametersPanel.Controls.Add(this.colorsWhiteButton);
            this.searchParametersPanel.Controls.Add(this.colorsBlueButton);
            this.searchParametersPanel.Controls.Add(this.colorsBlackButton);
            this.searchParametersPanel.Controls.Add(this.colorsRedButton);
            this.searchParametersPanel.Controls.Add(this.colorsGreenButton);
            this.searchParametersPanel.Controls.Add(this.colorsColorlessButton);
            this.searchParametersPanel.Location = new System.Drawing.Point(99, 0);
            this.searchParametersPanel.Name = "searchParametersPanel";
            this.searchParametersPanel.Size = new System.Drawing.Size(607, 527);
            this.searchParametersPanel.TabIndex = 3;
            // 
            // includeVariationsCheckBox
            // 
            this.includeVariationsCheckBox.AutoSize = true;
            this.includeVariationsCheckBox.Checked = true;
            this.includeVariationsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeVariationsCheckBox.Location = new System.Drawing.Point(15, 702);
            this.includeVariationsCheckBox.Name = "includeVariationsCheckBox";
            this.includeVariationsCheckBox.Size = new System.Drawing.Size(133, 20);
            this.includeVariationsCheckBox.TabIndex = 86;
            this.includeVariationsCheckBox.Text = "Include variations";
            this.includeVariationsCheckBox.UseVisualStyleBackColor = true;
            // 
            // languageClearButton
            // 
            this.languageClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.languageClearButton.Location = new System.Drawing.Point(337, 672);
            this.languageClearButton.Name = "languageClearButton";
            this.languageClearButton.Size = new System.Drawing.Size(30, 30);
            this.languageClearButton.TabIndex = 85;
            this.languageClearButton.UseVisualStyleBackColor = true;
            this.languageClearButton.Visible = false;
            this.languageClearButton.Click += new System.EventHandler(this.languageClearButton_Click);
            // 
            // flavorTextClearButton
            // 
            this.flavorTextClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.flavorTextClearButton.Location = new System.Drawing.Point(413, 639);
            this.flavorTextClearButton.Name = "flavorTextClearButton";
            this.flavorTextClearButton.Size = new System.Drawing.Size(30, 30);
            this.flavorTextClearButton.TabIndex = 84;
            this.flavorTextClearButton.UseVisualStyleBackColor = true;
            this.flavorTextClearButton.Visible = false;
            this.flavorTextClearButton.Click += new System.EventHandler(this.flavorTextClearButton_Click);
            // 
            // artistClearButton
            // 
            this.artistClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.artistClearButton.Location = new System.Drawing.Point(368, 603);
            this.artistClearButton.Name = "artistClearButton";
            this.artistClearButton.Size = new System.Drawing.Size(30, 30);
            this.artistClearButton.TabIndex = 83;
            this.artistClearButton.UseVisualStyleBackColor = true;
            this.artistClearButton.Visible = false;
            this.artistClearButton.Click += new System.EventHandler(this.artistClearButton_Click);
            // 
            // pricesClearButton
            // 
            this.pricesClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.pricesClearButton.Location = new System.Drawing.Point(427, 570);
            this.pricesClearButton.Name = "pricesClearButton";
            this.pricesClearButton.Size = new System.Drawing.Size(30, 30);
            this.pricesClearButton.TabIndex = 82;
            this.pricesClearButton.UseVisualStyleBackColor = true;
            this.pricesClearButton.Visible = false;
            this.pricesClearButton.Click += new System.EventHandler(this.pricesClearButton_Click);
            // 
            // attributesClearButton
            // 
            this.attributesClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.attributesClearButton.Location = new System.Drawing.Point(609, 329);
            this.attributesClearButton.Name = "attributesClearButton";
            this.attributesClearButton.Size = new System.Drawing.Size(30, 30);
            this.attributesClearButton.TabIndex = 81;
            this.attributesClearButton.UseVisualStyleBackColor = true;
            this.attributesClearButton.Visible = false;
            this.attributesClearButton.Click += new System.EventHandler(this.attributesClearButton_Click);
            // 
            // loyaltyClearButton
            // 
            this.loyaltyClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.loyaltyClearButton.Location = new System.Drawing.Point(216, 294);
            this.loyaltyClearButton.Name = "loyaltyClearButton";
            this.loyaltyClearButton.Size = new System.Drawing.Size(30, 30);
            this.loyaltyClearButton.TabIndex = 80;
            this.loyaltyClearButton.UseVisualStyleBackColor = true;
            this.loyaltyClearButton.Visible = false;
            this.loyaltyClearButton.Click += new System.EventHandler(this.loyaltyClearButton_Click);
            // 
            // toughnessClearButton
            // 
            this.toughnessClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.toughnessClearButton.Location = new System.Drawing.Point(216, 263);
            this.toughnessClearButton.Name = "toughnessClearButton";
            this.toughnessClearButton.Size = new System.Drawing.Size(30, 30);
            this.toughnessClearButton.TabIndex = 79;
            this.toughnessClearButton.UseVisualStyleBackColor = true;
            this.toughnessClearButton.Visible = false;
            this.toughnessClearButton.Click += new System.EventHandler(this.toughnessClearButton_Click);
            // 
            // powerClearButton
            // 
            this.powerClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.powerClearButton.Location = new System.Drawing.Point(216, 232);
            this.powerClearButton.Name = "powerClearButton";
            this.powerClearButton.Size = new System.Drawing.Size(30, 30);
            this.powerClearButton.TabIndex = 78;
            this.powerClearButton.UseVisualStyleBackColor = true;
            this.powerClearButton.Visible = false;
            this.powerClearButton.Click += new System.EventHandler(this.powerClearButton_Click);
            // 
            // cmcClearButton
            // 
            this.cmcClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.cmcClearButton.Location = new System.Drawing.Point(216, 202);
            this.cmcClearButton.Name = "cmcClearButton";
            this.cmcClearButton.Size = new System.Drawing.Size(30, 30);
            this.cmcClearButton.TabIndex = 77;
            this.cmcClearButton.UseVisualStyleBackColor = true;
            this.cmcClearButton.Visible = false;
            this.cmcClearButton.Click += new System.EventHandler(this.cmcClearButton_Click);
            // 
            // manaCostClearButton
            // 
            this.manaCostClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.manaCostClearButton.Location = new System.Drawing.Point(670, 165);
            this.manaCostClearButton.Name = "manaCostClearButton";
            this.manaCostClearButton.Size = new System.Drawing.Size(30, 30);
            this.manaCostClearButton.TabIndex = 76;
            this.manaCostClearButton.UseVisualStyleBackColor = true;
            this.manaCostClearButton.Visible = false;
            this.manaCostClearButton.Click += new System.EventHandler(this.manaCostClearButton_Click);
            // 
            // commanderClearButton
            // 
            this.commanderClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.commanderClearButton.Location = new System.Drawing.Point(313, 130);
            this.commanderClearButton.Name = "commanderClearButton";
            this.commanderClearButton.Size = new System.Drawing.Size(30, 30);
            this.commanderClearButton.TabIndex = 75;
            this.commanderClearButton.UseVisualStyleBackColor = true;
            this.commanderClearButton.Visible = false;
            this.commanderClearButton.Click += new System.EventHandler(this.commanderClearButton_Click);
            // 
            // colorsClearButton
            // 
            this.colorsClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.colorsClearButton.Location = new System.Drawing.Point(414, 90);
            this.colorsClearButton.Name = "colorsClearButton";
            this.colorsClearButton.Size = new System.Drawing.Size(30, 30);
            this.colorsClearButton.TabIndex = 74;
            this.colorsClearButton.UseVisualStyleBackColor = true;
            this.colorsClearButton.Visible = false;
            this.colorsClearButton.Click += new System.EventHandler(this.colorsClearButton_Click);
            // 
            // rarityClearButton
            // 
            this.rarityClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.rarityClearButton.Location = new System.Drawing.Point(289, 34);
            this.rarityClearButton.Name = "rarityClearButton";
            this.rarityClearButton.Size = new System.Drawing.Size(30, 30);
            this.rarityClearButton.TabIndex = 73;
            this.rarityClearButton.UseVisualStyleBackColor = true;
            this.rarityClearButton.Visible = false;
            this.rarityClearButton.Click += new System.EventHandler(this.rarityClearButton_Click);
            // 
            // gameClearButton
            // 
            this.gameClearButton.Image = global::MTG_Librarian.Properties.Resources.clear_16;
            this.gameClearButton.Location = new System.Drawing.Point(351, 8);
            this.gameClearButton.Name = "gameClearButton";
            this.gameClearButton.Size = new System.Drawing.Size(30, 30);
            this.gameClearButton.TabIndex = 72;
            this.gameClearButton.UseVisualStyleBackColor = true;
            this.gameClearButton.Visible = false;
            this.gameClearButton.Click += new System.EventHandler(this.gameClearbutton_Click);
            // 
            // rarityMythicCheckBox
            // 
            this.rarityMythicCheckBox.AutoSize = true;
            this.rarityMythicCheckBox.Location = new System.Drawing.Point(185, 66);
            this.rarityMythicCheckBox.Name = "rarityMythicCheckBox";
            this.rarityMythicCheckBox.Size = new System.Drawing.Size(94, 20);
            this.rarityMythicCheckBox.TabIndex = 71;
            this.rarityMythicCheckBox.Text = "Mythic rare";
            this.rarityMythicCheckBox.UseVisualStyleBackColor = true;
            this.rarityMythicCheckBox.CheckedChanged += new System.EventHandler(this.rarityCommonCheckBox_CheckedChanged);
            // 
            // rarityRareCheckBox
            // 
            this.rarityRareCheckBox.AutoSize = true;
            this.rarityRareCheckBox.Location = new System.Drawing.Point(96, 66);
            this.rarityRareCheckBox.Name = "rarityRareCheckBox";
            this.rarityRareCheckBox.Size = new System.Drawing.Size(59, 20);
            this.rarityRareCheckBox.TabIndex = 70;
            this.rarityRareCheckBox.Text = "Rare";
            this.rarityRareCheckBox.UseVisualStyleBackColor = true;
            this.rarityRareCheckBox.CheckedChanged += new System.EventHandler(this.rarityCommonCheckBox_CheckedChanged);
            // 
            // rarityUncommonCheckBox
            // 
            this.rarityUncommonCheckBox.AutoSize = true;
            this.rarityUncommonCheckBox.Location = new System.Drawing.Point(185, 40);
            this.rarityUncommonCheckBox.Name = "rarityUncommonCheckBox";
            this.rarityUncommonCheckBox.Size = new System.Drawing.Size(98, 20);
            this.rarityUncommonCheckBox.TabIndex = 69;
            this.rarityUncommonCheckBox.Text = "Uncommon";
            this.rarityUncommonCheckBox.UseVisualStyleBackColor = true;
            this.rarityUncommonCheckBox.CheckedChanged += new System.EventHandler(this.rarityCommonCheckBox_CheckedChanged);
            // 
            // rarityCommonCheckBox
            // 
            this.rarityCommonCheckBox.AutoSize = true;
            this.rarityCommonCheckBox.Location = new System.Drawing.Point(96, 40);
            this.rarityCommonCheckBox.Name = "rarityCommonCheckBox";
            this.rarityCommonCheckBox.Size = new System.Drawing.Size(83, 20);
            this.rarityCommonCheckBox.TabIndex = 68;
            this.rarityCommonCheckBox.Text = "Common";
            this.rarityCommonCheckBox.UseVisualStyleBackColor = true;
            this.rarityCommonCheckBox.CheckedChanged += new System.EventHandler(this.rarityCommonCheckBox_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 16);
            this.label14.TabIndex = 67;
            this.label14.Text = "Game:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 16);
            this.label8.TabIndex = 66;
            this.label8.Text = "Rarity:";
            // 
            // magicOnlineCheckBox
            // 
            this.magicOnlineCheckBox.AutoSize = true;
            this.magicOnlineCheckBox.Location = new System.Drawing.Point(238, 14);
            this.magicOnlineCheckBox.Name = "magicOnlineCheckBox";
            this.magicOnlineCheckBox.Size = new System.Drawing.Size(107, 20);
            this.magicOnlineCheckBox.TabIndex = 65;
            this.magicOnlineCheckBox.Text = "Magic Online";
            this.magicOnlineCheckBox.UseVisualStyleBackColor = true;
            this.magicOnlineCheckBox.CheckedChanged += new System.EventHandler(this.paperCheckBox_CheckedChanged);
            // 
            // arenaCheckBox
            // 
            this.arenaCheckBox.AutoSize = true;
            this.arenaCheckBox.Location = new System.Drawing.Point(167, 14);
            this.arenaCheckBox.Name = "arenaCheckBox";
            this.arenaCheckBox.Size = new System.Drawing.Size(65, 20);
            this.arenaCheckBox.TabIndex = 64;
            this.arenaCheckBox.Text = "Arena";
            this.arenaCheckBox.UseVisualStyleBackColor = true;
            this.arenaCheckBox.CheckedChanged += new System.EventHandler(this.paperCheckBox_CheckedChanged);
            // 
            // paperCheckBox
            // 
            this.paperCheckBox.AutoSize = true;
            this.paperCheckBox.Location = new System.Drawing.Point(96, 14);
            this.paperCheckBox.Name = "paperCheckBox";
            this.paperCheckBox.Size = new System.Drawing.Size(66, 20);
            this.paperCheckBox.TabIndex = 63;
            this.paperCheckBox.Text = "Paper";
            this.paperCheckBox.UseVisualStyleBackColor = true;
            this.paperCheckBox.CheckedChanged += new System.EventHandler(this.paperCheckBox_CheckedChanged);
            // 
            // manaCostTypeComboBox
            // 
            this.manaCostTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manaCostTypeComboBox.FormattingEnabled = true;
            this.manaCostTypeComboBox.Items.AddRange(new object[] {
            "Mono",
            "Generic",
            "Hybrid",
            "Phyrexian"});
            this.manaCostTypeComboBox.Location = new System.Drawing.Point(252, 167);
            this.manaCostTypeComboBox.Name = "manaCostTypeComboBox";
            this.manaCostTypeComboBox.Size = new System.Drawing.Size(121, 24);
            this.manaCostTypeComboBox.TabIndex = 62;
            this.manaCostTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.manaCostTypeComboBox_SelectedIndexChanged);
            // 
            // colorsOperatorComboBox
            // 
            this.colorsOperatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorsOperatorComboBox.FormattingEnabled = true;
            this.colorsOperatorComboBox.Items.AddRange(new object[] {
            "=",
            "<=",
            ">="});
            this.colorsOperatorComboBox.Location = new System.Drawing.Point(142, 92);
            this.colorsOperatorComboBox.Name = "colorsOperatorComboBox";
            this.colorsOperatorComboBox.Size = new System.Drawing.Size(55, 24);
            this.colorsOperatorComboBox.TabIndex = 57;
            this.colorsOperatorComboBox.SelectedIndexChanged += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
            // 
            // colorsComboBox
            // 
            this.colorsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorsComboBox.FormattingEnabled = true;
            this.colorsComboBox.Items.AddRange(new object[] {
            "Colors",
            "Color identity"});
            this.colorsComboBox.Location = new System.Drawing.Point(15, 92);
            this.colorsComboBox.Name = "colorsComboBox";
            this.colorsComboBox.Size = new System.Drawing.Size(121, 24);
            this.colorsComboBox.TabIndex = 56;
            this.colorsComboBox.SelectedIndexChanged += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
            // 
            // languageComboBox
            // 
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Items.AddRange(new object[] {
            "English",
            "Spanish",
            "French",
            "German",
            "Italian",
            "Portuguese",
            "Japanese",
            "Korean",
            "Russian",
            "Simplified Chinese",
            "Traditional Chinese",
            "Hebrew",
            "Latin",
            "Ancient Greek",
            "Arabic",
            "Sanskrit",
            "Phyrexian",
            "Quenya"});
            this.languageComboBox.Location = new System.Drawing.Point(94, 672);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(232, 24);
            this.languageComboBox.TabIndex = 54;
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.languageComboBox_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 675);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 16);
            this.label13.TabIndex = 53;
            this.label13.Text = "Language:";
            // 
            // flavorTextTextBox
            // 
            this.flavorTextTextBox.Location = new System.Drawing.Point(94, 639);
            this.flavorTextTextBox.Name = "flavorTextTextBox";
            this.flavorTextTextBox.Size = new System.Drawing.Size(309, 22);
            this.flavorTextTextBox.TabIndex = 52;
            this.flavorTextTextBox.TextChanged += new System.EventHandler(this.flavorTextTextBox_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 642);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 16);
            this.label12.TabIndex = 51;
            this.label12.Text = "Flavor Text:";
            // 
            // artistTextBox
            // 
            this.artistTextBox.Location = new System.Drawing.Point(63, 607);
            this.artistTextBox.Name = "artistTextBox";
            this.artistTextBox.Size = new System.Drawing.Size(294, 22);
            this.artistTextBox.TabIndex = 50;
            this.artistTextBox.TextChanged += new System.EventHandler(this.artistTextBox_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 610);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 16);
            this.label11.TabIndex = 49;
            this.label11.Text = "Artist:";
            // 
            // pricesPriceNumericUpDown
            // 
            this.pricesPriceNumericUpDown.DecimalPlaces = 2;
            this.pricesPriceNumericUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.pricesPriceNumericUpDown.Location = new System.Drawing.Point(241, 572);
            this.pricesPriceNumericUpDown.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.pricesPriceNumericUpDown.Name = "pricesPriceNumericUpDown";
            this.pricesPriceNumericUpDown.Size = new System.Drawing.Size(176, 22);
            this.pricesPriceNumericUpDown.TabIndex = 48;
            this.pricesPriceNumericUpDown.ValueChanged += new System.EventHandler(this.pricesCurrencyComboBox_SelectedIndexChanged);
            // 
            // pricesOperatorComboBox
            // 
            this.pricesOperatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pricesOperatorComboBox.FormattingEnabled = true;
            this.pricesOperatorComboBox.Items.AddRange(new object[] {
            "<",
            "<=",
            ">",
            ">="});
            this.pricesOperatorComboBox.Location = new System.Drawing.Point(160, 571);
            this.pricesOperatorComboBox.Name = "pricesOperatorComboBox";
            this.pricesOperatorComboBox.Size = new System.Drawing.Size(75, 24);
            this.pricesOperatorComboBox.TabIndex = 47;
            this.pricesOperatorComboBox.SelectedIndexChanged += new System.EventHandler(this.pricesCurrencyComboBox_SelectedIndexChanged);
            // 
            // pricesCurrencyComboBox
            // 
            this.pricesCurrencyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pricesCurrencyComboBox.FormattingEnabled = true;
            this.pricesCurrencyComboBox.Items.AddRange(new object[] {
            "USD",
            "EUR",
            "TIX"});
            this.pricesCurrencyComboBox.Location = new System.Drawing.Point(63, 571);
            this.pricesCurrencyComboBox.Name = "pricesCurrencyComboBox";
            this.pricesCurrencyComboBox.Size = new System.Drawing.Size(91, 24);
            this.pricesCurrencyComboBox.TabIndex = 46;
            this.pricesCurrencyComboBox.SelectedIndexChanged += new System.EventHandler(this.pricesCurrencyComboBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 574);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 45;
            this.label10.Text = "Prices:";
            // 
            // attributesObjectListView
            // 
            this.attributesObjectListView.AllColumns.Add(this.notColumn);
            this.attributesObjectListView.AllColumns.Add(this.attributeColumn);
            this.attributesObjectListView.AllColumns.Add(this.descriptionColumn);
            this.attributesObjectListView.CellEditUseWholeCell = false;
            this.attributesObjectListView.CheckBoxes = true;
            this.attributesObjectListView.CheckedAspectName = "Not";
            this.attributesObjectListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.notColumn,
            this.attributeColumn,
            this.descriptionColumn});
            this.attributesObjectListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.attributesObjectListView.EmptyListMsg = "";
            this.attributesObjectListView.FullRowSelect = true;
            this.attributesObjectListView.HeaderWordWrap = true;
            this.attributesObjectListView.HideSelection = false;
            this.attributesObjectListView.IsSimpleDragSource = true;
            this.attributesObjectListView.Location = new System.Drawing.Point(114, 361);
            this.attributesObjectListView.Margin = new System.Windows.Forms.Padding(4);
            this.attributesObjectListView.Name = "attributesObjectListView";
            this.attributesObjectListView.SelectedBackColor = System.Drawing.Color.SteelBlue;
            this.attributesObjectListView.SelectedForeColor = System.Drawing.Color.White;
            this.attributesObjectListView.ShowGroups = false;
            this.attributesObjectListView.ShowImagesOnSubItems = true;
            this.attributesObjectListView.Size = new System.Drawing.Size(466, 202);
            this.attributesObjectListView.TabIndex = 44;
            this.attributesObjectListView.UnfocusedSelectedBackColor = System.Drawing.Color.LightGray;
            this.attributesObjectListView.UseCompatibleStateImageBehavior = false;
            this.attributesObjectListView.View = System.Windows.Forms.View.Details;
            this.attributesObjectListView.VirtualMode = true;
            this.attributesObjectListView.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(this.attributesObjectListView_ItemsChanged);
            this.attributesObjectListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.attributesObjectListView_KeyDown);
            // 
            // notColumn
            // 
            this.notColumn.AspectName = "";
            this.notColumn.Text = "Not";
            this.notColumn.Width = 35;
            // 
            // attributeColumn
            // 
            this.attributeColumn.AspectName = "Attribute";
            this.attributeColumn.IsEditable = false;
            this.attributeColumn.Text = "";
            this.attributeColumn.Width = 31;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.AspectName = "Description";
            this.descriptionColumn.IsEditable = false;
            this.descriptionColumn.Text = "";
            this.descriptionColumn.Width = 31;
            // 
            // attributesComboBox
            // 
            this.attributesComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.attributesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.attributesComboBox.FormattingEnabled = true;
            this.attributesComboBox.Items.AddRange(new object[] {
            "Adventure | use the Adventure layout",
            "Arena ID | have an Arena ID",
            "Art Series",
            "Artist | have artists",
            "Artist Misprint | have a misprinted artist",
            "Attraction Lights | have attraction lights",
            "Atypical | aren\'t printed with standard frames and effects",
            "Augment | are augment pieces",
            "Back | have non-standard backs",
            "Bear | are 2/2/2 bears",
            "Beginner Box",
            "Booster | included in standard contents of draft boosters",
            "Borderless",
            "Brawl Commander | can be your Brawl commander",
            "Buy-a-Box | Buy-a-Box promos",
            "Cardmarket ID | have a Cardmarket ID",
            "Class Layout | are Class-type",
            "Color Indicator | have color indicators",
            "Colorshifted | have a colorshifted frame",
            "Commander | can be your commander",
            "Companion",
            "Content Warning | have content warnings",
            "Covered",
            "Creature Land | are lands that become creatures",
            "Datestamped",
            "Default | printed with standard frames and effects",
            "Digital | are digital prints",
            "Double Sided",
            "Duel Commander | can be your Duel Commander",
            "ETB | have an ETB effect",
            "English Art | have art that has been printed in English",
            "Etched | are available in etched foil",
            "Extended Art | have extended art frames",
            "Final Fantasy | from Final Fantasy",
            "First Printing",
            "Flavor Name | have flavor names",
            "Flavor Text | have flavor text",
            "Flip",
            "Foil | are available in foil",
            "Foreign Black Border | printed in black border in non-English editions of white-b" +
                "order sets",
            "Foreign White Border | foreign white border prints",
            "French Vanilla | are French vanilla",
            "Full Art | have full extended art",
            "Funny",
            "Future | have the future frame",
            "Game Changer | on the Commander Game Changer list",
            "Game Day | Game Day promos",
            "Historic",
            "Hybrid Mana | have hybrid mana",
            "Illustration | have illustration IDs",
            "Intro Pack | exclusive Intro Pack cards",
            "Invitational Card",
            "Leveler | have level up",
            "Localized Name | have localized names",
            "MTGO ID | have MTGO Ids",
            "Masterpiece",
            "Meld",
            "Modal | have modal effects",
            "Modal Double Faced",
            "Modern | have the 2003 frame",
            "Multiverse ID | have a Multivierse ID",
            "New | have a new frame",
            "Nonfoil | are available in nonfoil",
            "Oathbreaker | can be your oathbreaker",
            "Old | have the 93/97 frame",
            "Outlaw | are Assassins, Mercenaries, Pirates, Rogues, or Warlocks",
            "Oversized | larger than standard card size",
            "Paired Commander | have multi-commander effects",
            "Paper Art | have art that has been printed in paper",
            "Party | are Clerics, Rogues, Warriors, or Wizards",
            "Permanent | become permanents",
            "Phyrexian Mana | have Phyrexian mana",
            "Planar | Planar deck cards",
            "Planeswalker Deck | exclusive Planeswalker Deck cards",
            "Prepare | have prepared spell parts",
            "Prerelease Promo | set prerelease event promos",
            "Printed Text | have their printed text listed",
            "Promo | promotional prints",
            "Related | have related cards",
            "Release Promo | set release event promos",
            "Reprint",
            "Reserved List",
            "Reversible",
            "Security Stamp | have a security stamp",
            "Showcase",
            "Spell",
            "Spellbook | have spellbooks",
            "Spikey | have ever been banned or restricted",
            "Split Card",
            "Stamped | have a non-date stamp",
            "Starter Collection",
            "Starter Deck | exclusive to a Starter Deck",
            "Story Spotlight",
            "TCGPlayer ID | have a TCGPlayer ID",
            "Textless | printed without rules text",
            "Token",
            "Tombstone | have the Odyssey tombstone mark",
            "Transform",
            "Translucent | have a translucent card frame",
            "Unique | have been printed exactly once",
            "Universes Beyond | from a Universes Beyond edition",
            "Vanilla",
            "Variation | variations of standard printings",
            "Watermark | have watermarks",
            "Worthy | creatures are considered worthy"});
            this.attributesComboBox.Location = new System.Drawing.Point(114, 331);
            this.attributesComboBox.Name = "attributesComboBox";
            this.attributesComboBox.Size = new System.Drawing.Size(489, 23);
            this.attributesComboBox.TabIndex = 43;
            this.attributesComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.attributesComboBox_DrawItem);
            this.attributesComboBox.SelectedIndexChanged += new System.EventHandler(this.attributesComboBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 334);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 16);
            this.label9.TabIndex = 41;
            this.label9.Text = "Misc Attributes:";
            // 
            // loyaltyNumericUpDown
            // 
            this.loyaltyNumericUpDown.Location = new System.Drawing.Point(157, 295);
            this.loyaltyNumericUpDown.Name = "loyaltyNumericUpDown";
            this.loyaltyNumericUpDown.Size = new System.Drawing.Size(54, 22);
            this.loyaltyNumericUpDown.TabIndex = 35;
            this.loyaltyNumericUpDown.ValueChanged += new System.EventHandler(this.loyaltyComboBox_SelectedIndexChanged);
            // 
            // loyaltyComboBox
            // 
            this.loyaltyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loyaltyComboBox.FormattingEnabled = true;
            this.loyaltyComboBox.Items.AddRange(new object[] {
            "=",
            "<",
            ">"});
            this.loyaltyComboBox.Location = new System.Drawing.Point(96, 293);
            this.loyaltyComboBox.Name = "loyaltyComboBox";
            this.loyaltyComboBox.Size = new System.Drawing.Size(55, 24);
            this.loyaltyComboBox.TabIndex = 34;
            this.loyaltyComboBox.SelectedIndexChanged += new System.EventHandler(this.loyaltyComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 297);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 16);
            this.label7.TabIndex = 33;
            this.label7.Text = "Loyalty:";
            // 
            // toughnessNumericUpDown
            // 
            this.toughnessNumericUpDown.Location = new System.Drawing.Point(157, 265);
            this.toughnessNumericUpDown.Name = "toughnessNumericUpDown";
            this.toughnessNumericUpDown.Size = new System.Drawing.Size(54, 22);
            this.toughnessNumericUpDown.TabIndex = 32;
            this.toughnessNumericUpDown.ValueChanged += new System.EventHandler(this.toughnessComboBox_SelectedIndexChanged);
            // 
            // toughnessComboBox
            // 
            this.toughnessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toughnessComboBox.FormattingEnabled = true;
            this.toughnessComboBox.Items.AddRange(new object[] {
            "=",
            "<",
            ">"});
            this.toughnessComboBox.Location = new System.Drawing.Point(96, 263);
            this.toughnessComboBox.Name = "toughnessComboBox";
            this.toughnessComboBox.Size = new System.Drawing.Size(55, 24);
            this.toughnessComboBox.TabIndex = 31;
            this.toughnessComboBox.SelectedIndexChanged += new System.EventHandler(this.toughnessComboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 16);
            this.label6.TabIndex = 30;
            this.label6.Text = "Toughness:";
            // 
            // powerNumericUpDown
            // 
            this.powerNumericUpDown.Location = new System.Drawing.Point(157, 234);
            this.powerNumericUpDown.Name = "powerNumericUpDown";
            this.powerNumericUpDown.Size = new System.Drawing.Size(54, 22);
            this.powerNumericUpDown.TabIndex = 29;
            this.powerNumericUpDown.ValueChanged += new System.EventHandler(this.powerComboBox_SelectedIndexChanged);
            // 
            // powerComboBox
            // 
            this.powerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.powerComboBox.FormattingEnabled = true;
            this.powerComboBox.Items.AddRange(new object[] {
            "=",
            "<",
            ">"});
            this.powerComboBox.Location = new System.Drawing.Point(96, 232);
            this.powerComboBox.Name = "powerComboBox";
            this.powerComboBox.Size = new System.Drawing.Size(55, 24);
            this.powerComboBox.TabIndex = 28;
            this.powerComboBox.SelectedIndexChanged += new System.EventHandler(this.powerComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 27;
            this.label5.Text = "Power:";
            // 
            // cmcNumericUpDown
            // 
            this.cmcNumericUpDown.Location = new System.Drawing.Point(157, 204);
            this.cmcNumericUpDown.Name = "cmcNumericUpDown";
            this.cmcNumericUpDown.Size = new System.Drawing.Size(54, 22);
            this.cmcNumericUpDown.TabIndex = 26;
            this.cmcNumericUpDown.ValueChanged += new System.EventHandler(this.cmcOperatorComboBox_SelectedIndexChanged);
            // 
            // cmcOperatorComboBox
            // 
            this.cmcOperatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmcOperatorComboBox.FormattingEnabled = true;
            this.cmcOperatorComboBox.Items.AddRange(new object[] {
            "=",
            "<",
            ">"});
            this.cmcOperatorComboBox.Location = new System.Drawing.Point(96, 202);
            this.cmcOperatorComboBox.Name = "cmcOperatorComboBox";
            this.cmcOperatorComboBox.Size = new System.Drawing.Size(55, 24);
            this.cmcOperatorComboBox.TabIndex = 24;
            this.cmcOperatorComboBox.SelectedIndexChanged += new System.EventHandler(this.cmcOperatorComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 16);
            this.label4.TabIndex = 23;
            this.label4.Text = "CMC:";
            // 
            // manaCostComboBox
            // 
            this.manaCostComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manaCostComboBox.FormattingEnabled = true;
            this.manaCostComboBox.Location = new System.Drawing.Point(379, 167);
            this.manaCostComboBox.Name = "manaCostComboBox";
            this.manaCostComboBox.Size = new System.Drawing.Size(285, 24);
            this.manaCostComboBox.TabIndex = 22;
            this.manaCostComboBox.SelectedIndexChanged += new System.EventHandler(this.manaCostComboBox_SelectedIndexChanged);
            // 
            // manaCostTextBox
            // 
            this.manaCostTextBox.Location = new System.Drawing.Point(96, 169);
            this.manaCostTextBox.Name = "manaCostTextBox";
            this.manaCostTextBox.Size = new System.Drawing.Size(150, 22);
            this.manaCostTextBox.TabIndex = 21;
            this.manaCostTextBox.TextChanged += new System.EventHandler(this.manaCostTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Mana cost:";
            // 
            // commanderWhiteButton
            // 
            this.commanderWhiteButton.Checked = false;
            this.commanderWhiteButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.commanderWhiteButton.FlatAppearance.BorderSize = 0;
            this.commanderWhiteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.commanderWhiteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commanderWhiteButton.Location = new System.Drawing.Point(103, 128);
            this.commanderWhiteButton.Margin = new System.Windows.Forms.Padding(4);
            this.commanderWhiteButton.Name = "commanderWhiteButton";
            this.commanderWhiteButton.Size = new System.Drawing.Size(33, 31);
            this.commanderWhiteButton.TabIndex = 14;
            this.commanderWhiteButton.UseVisualStyleBackColor = false;
            this.commanderWhiteButton.Click += new System.EventHandler(this.commanderWhiteButton_Click);
            // 
            // commanderBlueButton
            // 
            this.commanderBlueButton.Checked = false;
            this.commanderBlueButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.commanderBlueButton.FlatAppearance.BorderSize = 2;
            this.commanderBlueButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.commanderBlueButton.Location = new System.Drawing.Point(138, 128);
            this.commanderBlueButton.Margin = new System.Windows.Forms.Padding(4);
            this.commanderBlueButton.Name = "commanderBlueButton";
            this.commanderBlueButton.Size = new System.Drawing.Size(33, 31);
            this.commanderBlueButton.TabIndex = 15;
            this.commanderBlueButton.UseVisualStyleBackColor = false;
            this.commanderBlueButton.Click += new System.EventHandler(this.commanderWhiteButton_Click);
            // 
            // commanderBlackButton
            // 
            this.commanderBlackButton.Checked = false;
            this.commanderBlackButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.commanderBlackButton.FlatAppearance.BorderSize = 2;
            this.commanderBlackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.commanderBlackButton.Location = new System.Drawing.Point(172, 128);
            this.commanderBlackButton.Margin = new System.Windows.Forms.Padding(4);
            this.commanderBlackButton.Name = "commanderBlackButton";
            this.commanderBlackButton.Size = new System.Drawing.Size(33, 31);
            this.commanderBlackButton.TabIndex = 16;
            this.commanderBlackButton.Text = "B";
            this.commanderBlackButton.UseVisualStyleBackColor = false;
            this.commanderBlackButton.Click += new System.EventHandler(this.commanderWhiteButton_Click);
            // 
            // commanderRedButton
            // 
            this.commanderRedButton.Checked = false;
            this.commanderRedButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.commanderRedButton.FlatAppearance.BorderSize = 2;
            this.commanderRedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.commanderRedButton.Location = new System.Drawing.Point(207, 128);
            this.commanderRedButton.Margin = new System.Windows.Forms.Padding(4);
            this.commanderRedButton.Name = "commanderRedButton";
            this.commanderRedButton.Size = new System.Drawing.Size(33, 31);
            this.commanderRedButton.TabIndex = 17;
            this.commanderRedButton.Text = "R";
            this.commanderRedButton.UseVisualStyleBackColor = false;
            this.commanderRedButton.Click += new System.EventHandler(this.commanderWhiteButton_Click);
            // 
            // commanderGreenButton
            // 
            this.commanderGreenButton.Checked = false;
            this.commanderGreenButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.commanderGreenButton.FlatAppearance.BorderSize = 2;
            this.commanderGreenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.commanderGreenButton.Location = new System.Drawing.Point(239, 128);
            this.commanderGreenButton.Margin = new System.Windows.Forms.Padding(4);
            this.commanderGreenButton.Name = "commanderGreenButton";
            this.commanderGreenButton.Size = new System.Drawing.Size(33, 31);
            this.commanderGreenButton.TabIndex = 18;
            this.commanderGreenButton.Text = "G";
            this.commanderGreenButton.UseVisualStyleBackColor = false;
            this.commanderGreenButton.Click += new System.EventHandler(this.commanderWhiteButton_Click);
            // 
            // commanderColorlessButton
            // 
            this.commanderColorlessButton.Checked = false;
            this.commanderColorlessButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.commanderColorlessButton.FlatAppearance.BorderSize = 0;
            this.commanderColorlessButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.commanderColorlessButton.Location = new System.Drawing.Point(273, 128);
            this.commanderColorlessButton.Margin = new System.Windows.Forms.Padding(4);
            this.commanderColorlessButton.Name = "commanderColorlessButton";
            this.commanderColorlessButton.Size = new System.Drawing.Size(33, 31);
            this.commanderColorlessButton.TabIndex = 19;
            this.commanderColorlessButton.Text = "flatButton1";
            this.commanderColorlessButton.UseVisualStyleBackColor = false;
            this.commanderColorlessButton.Click += new System.EventHandler(this.commanderWhiteButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Commander:";
            // 
            // colorsWhiteButton
            // 
            this.colorsWhiteButton.Checked = false;
            this.colorsWhiteButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorsWhiteButton.FlatAppearance.BorderSize = 0;
            this.colorsWhiteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorsWhiteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorsWhiteButton.Location = new System.Drawing.Point(204, 88);
            this.colorsWhiteButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorsWhiteButton.Name = "colorsWhiteButton";
            this.colorsWhiteButton.Size = new System.Drawing.Size(33, 31);
            this.colorsWhiteButton.TabIndex = 7;
            this.colorsWhiteButton.UseVisualStyleBackColor = false;
            this.colorsWhiteButton.Click += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
            // 
            // colorsBlueButton
            // 
            this.colorsBlueButton.Checked = false;
            this.colorsBlueButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorsBlueButton.FlatAppearance.BorderSize = 2;
            this.colorsBlueButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorsBlueButton.Location = new System.Drawing.Point(239, 88);
            this.colorsBlueButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorsBlueButton.Name = "colorsBlueButton";
            this.colorsBlueButton.Size = new System.Drawing.Size(33, 31);
            this.colorsBlueButton.TabIndex = 8;
            this.colorsBlueButton.UseVisualStyleBackColor = false;
            this.colorsBlueButton.Click += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
            // 
            // colorsBlackButton
            // 
            this.colorsBlackButton.Checked = false;
            this.colorsBlackButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorsBlackButton.FlatAppearance.BorderSize = 2;
            this.colorsBlackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorsBlackButton.Location = new System.Drawing.Point(273, 88);
            this.colorsBlackButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorsBlackButton.Name = "colorsBlackButton";
            this.colorsBlackButton.Size = new System.Drawing.Size(33, 31);
            this.colorsBlackButton.TabIndex = 9;
            this.colorsBlackButton.Text = "B";
            this.colorsBlackButton.UseVisualStyleBackColor = false;
            this.colorsBlackButton.Click += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
            // 
            // colorsRedButton
            // 
            this.colorsRedButton.Checked = false;
            this.colorsRedButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorsRedButton.FlatAppearance.BorderSize = 2;
            this.colorsRedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorsRedButton.Location = new System.Drawing.Point(308, 88);
            this.colorsRedButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorsRedButton.Name = "colorsRedButton";
            this.colorsRedButton.Size = new System.Drawing.Size(33, 31);
            this.colorsRedButton.TabIndex = 10;
            this.colorsRedButton.Text = "R";
            this.colorsRedButton.UseVisualStyleBackColor = false;
            this.colorsRedButton.Click += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
            // 
            // colorsGreenButton
            // 
            this.colorsGreenButton.Checked = false;
            this.colorsGreenButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorsGreenButton.FlatAppearance.BorderSize = 2;
            this.colorsGreenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorsGreenButton.Location = new System.Drawing.Point(340, 88);
            this.colorsGreenButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorsGreenButton.Name = "colorsGreenButton";
            this.colorsGreenButton.Size = new System.Drawing.Size(33, 31);
            this.colorsGreenButton.TabIndex = 11;
            this.colorsGreenButton.Text = "G";
            this.colorsGreenButton.UseVisualStyleBackColor = false;
            this.colorsGreenButton.Click += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
            // 
            // colorsColorlessButton
            // 
            this.colorsColorlessButton.Checked = false;
            this.colorsColorlessButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.colorsColorlessButton.FlatAppearance.BorderSize = 0;
            this.colorsColorlessButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorsColorlessButton.Location = new System.Drawing.Point(374, 88);
            this.colorsColorlessButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorsColorlessButton.Name = "colorsColorlessButton";
            this.colorsColorlessButton.Size = new System.Drawing.Size(33, 31);
            this.colorsColorlessButton.TabIndex = 12;
            this.colorsColorlessButton.Text = "flatButton1";
            this.colorsColorlessButton.UseVisualStyleBackColor = false;
            this.colorsColorlessButton.Click += new System.EventHandler(this.colorsComboBox_SelectedIndexChanged);
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
            this.cardListView.EmptyListMsg = "F2 to search";
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
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(192, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(192, 0);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "tabPage2";
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(384, 0);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Sets";
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(384, 0);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(383, 0);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "tabPage5";
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(0, 0);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(200, 100);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "Sets";
            // 
            // tabPage7
            // 
            this.tabPage7.Location = new System.Drawing.Point(0, 0);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(200, 100);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "Additional search parameters";
            // 
            // tabPage8
            // 
            this.tabPage8.Location = new System.Drawing.Point(0, 0);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(200, 100);
            this.tabPage8.TabIndex = 0;
            this.tabPage8.Text = "tabPage8";
            // 
            // DBViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1903, 570);
            this.Controls.Add(this.splitContainer2);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DBViewForm";
            this.Text = "Catalog";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DBViewForm_KeyDown);
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
            this.setsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.setListView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.compatibleTabControl1.ResumeLayout(false);
            this.searchParametersPanel.ResumeLayout(false);
            this.searchParametersPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pricesPriceNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attributesObjectListView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loyaltyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toughnessNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmcNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
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
        private System.Windows.Forms.Panel setsPanel;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private CompatibleTabControl.CompatibleTabControl compatibleTabControl1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Panel searchParametersPanel;
        private CustomControls.FlatButton colorsWhiteButton;
        private CustomControls.FlatButton colorsBlueButton;
        private CustomControls.FlatButton colorsBlackButton;
        private CustomControls.FlatButton colorsRedButton;
        private CustomControls.FlatButton colorsGreenButton;
        private CustomControls.FlatButton colorsColorlessButton;
        public BrightIdeasSoftware.TreeListView setListView;
        private BrightIdeasSoftware.OLVColumn SetName;
        private BrightIdeasSoftware.OLVColumn completeColumn;
        private BrightIdeasSoftware.OLVColumn complete4Column;
        private CustomControls.FlatButton commanderWhiteButton;
        private CustomControls.FlatButton commanderBlueButton;
        private CustomControls.FlatButton commanderBlackButton;
        private CustomControls.FlatButton commanderRedButton;
        private CustomControls.FlatButton commanderGreenButton;
        private CustomControls.FlatButton commanderColorlessButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox manaCostTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox manaCostComboBox;
        private System.Windows.Forms.ComboBox cmcOperatorComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown cmcNumericUpDown;
        private System.Windows.Forms.NumericUpDown powerNumericUpDown;
        private System.Windows.Forms.ComboBox powerComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown toughnessNumericUpDown;
        private System.Windows.Forms.ComboBox toughnessComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown loyaltyNumericUpDown;
        private System.Windows.Forms.ComboBox loyaltyComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox attributesComboBox;
        public BrightIdeasSoftware.FastObjectListView attributesObjectListView;
        private BrightIdeasSoftware.OLVColumn notColumn;
        private BrightIdeasSoftware.OLVColumn attributeColumn;
        private BrightIdeasSoftware.OLVColumn descriptionColumn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox pricesCurrencyComboBox;
        private System.Windows.Forms.ComboBox pricesOperatorComboBox;
        private System.Windows.Forms.NumericUpDown pricesPriceNumericUpDown;
        private System.Windows.Forms.TextBox artistTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox flavorTextTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.ComboBox colorsComboBox;
        private System.Windows.Forms.ComboBox colorsOperatorComboBox;
        private System.Windows.Forms.ComboBox manaCostTypeComboBox;
        private System.Windows.Forms.CheckBox rarityMythicCheckBox;
        private System.Windows.Forms.CheckBox rarityRareCheckBox;
        private System.Windows.Forms.CheckBox rarityUncommonCheckBox;
        private System.Windows.Forms.CheckBox rarityCommonCheckBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.CheckBox magicOnlineCheckBox;
        public System.Windows.Forms.CheckBox arenaCheckBox;
        public System.Windows.Forms.CheckBox paperCheckBox;
        private System.Windows.Forms.Button gameClearButton;
        private System.Windows.Forms.Button rarityClearButton;
        private System.Windows.Forms.Button colorsClearButton;
        private System.Windows.Forms.Button commanderClearButton;
        private System.Windows.Forms.Button manaCostClearButton;
        private System.Windows.Forms.Button cmcClearButton;
        private System.Windows.Forms.Button powerClearButton;
        private System.Windows.Forms.Button toughnessClearButton;
        private System.Windows.Forms.Button loyaltyClearButton;
        private System.Windows.Forms.Button attributesClearButton;
        private System.Windows.Forms.Button pricesClearButton;
        private System.Windows.Forms.Button artistClearButton;
        private System.Windows.Forms.Button flavorTextClearButton;
        private System.Windows.Forms.Button languageClearButton;
        private System.Windows.Forms.CheckBox includeVariationsCheckBox;
        private System.Windows.Forms.ComboBox uniqueComboBox;
        private System.Windows.Forms.Button clearAllButton;
    }
}