namespace MTG_Librarian
{
    partial class ImportCollectionForm
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
            this.platformComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.importButton = new System.Windows.Forms.Button();
            this.collectionNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.collectionsListView = new BrightIdeasSoftware.FastObjectListView();
            this.nameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.platformColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.blockProgressBar = new CustomControls.BlockProgressBar();
            this.importWorker = new System.ComponentModel.BackgroundWorker();
            this.progressLabel = new System.Windows.Forms.Label();
            this.failedTextBox = new System.Windows.Forms.TextBox();
            this.failedLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionsListView)).BeginInit();
            this.SuspendLayout();
            // 
            // platformComboBox
            // 
            this.platformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformComboBox.FormattingEnabled = true;
            this.platformComboBox.Items.AddRange(new object[] {
            "Paper",
            "Arena",
            "Magic Online"});
            this.platformComboBox.Location = new System.Drawing.Point(852, 208);
            this.platformComboBox.Name = "platformComboBox";
            this.platformComboBox.Size = new System.Drawing.Size(148, 24);
            this.platformComboBox.TabIndex = 15;
            this.platformComboBox.SelectedIndexChanged += new System.EventHandler(this.platformComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(849, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Platform";
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(1006, 210);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 13;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // collectionNameTextBox
            // 
            this.collectionNameTextBox.Location = new System.Drawing.Point(21, 383);
            this.collectionNameTextBox.Name = "collectionNameTextBox";
            this.collectionNameTextBox.Size = new System.Drawing.Size(825, 22);
            this.collectionNameTextBox.TabIndex = 12;
            this.collectionNameTextBox.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Collection name:";
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Location = new System.Drawing.Point(12, 35);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(44, 16);
            this.filenameLabel.TabIndex = 10;
            this.filenameLabel.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Filename:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 21);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(197, 20);
            this.radioButton1.TabIndex = 17;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Import into existing collection";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(15, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 100);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 47);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(170, 20);
            this.radioButton2.TabIndex = 19;
            this.radioButton2.Text = "Import as new collection";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // collectionsListView
            // 
            this.collectionsListView.AllColumns.Add(this.nameColumn);
            this.collectionsListView.AllColumns.Add(this.groupColumn);
            this.collectionsListView.AllColumns.Add(this.platformColumn);
            this.collectionsListView.CellEditUseWholeCell = false;
            this.collectionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.groupColumn,
            this.platformColumn});
            this.collectionsListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.collectionsListView.FullRowSelect = true;
            this.collectionsListView.HideSelection = false;
            this.collectionsListView.Location = new System.Drawing.Point(21, 208);
            this.collectionsListView.MultiSelect = false;
            this.collectionsListView.Name = "collectionsListView";
            this.collectionsListView.ShowGroups = false;
            this.collectionsListView.Size = new System.Drawing.Size(825, 169);
            this.collectionsListView.TabIndex = 19;
            this.collectionsListView.UseCompatibleStateImageBehavior = false;
            this.collectionsListView.UseFiltering = true;
            this.collectionsListView.View = System.Windows.Forms.View.Details;
            this.collectionsListView.VirtualMode = true;
            // 
            // nameColumn
            // 
            this.nameColumn.AspectName = "CollectionName";
            this.nameColumn.Text = "Name";
            // 
            // groupColumn
            // 
            this.groupColumn.AspectName = "GroupName";
            this.groupColumn.Text = "Group";
            // 
            // platformColumn
            // 
            this.platformColumn.AspectName = "Platform";
            this.platformColumn.Text = "Platform";
            this.platformColumn.Width = 87;
            // 
            // blockProgressBar
            // 
            this.blockProgressBar.BarColor = System.Drawing.Color.SlateGray;
            this.blockProgressBar.BlankBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.blockProgressBar.BorderColor = System.Drawing.Color.DimGray;
            this.blockProgressBar.CurrentBlocks = 0;
            this.blockProgressBar.Location = new System.Drawing.Point(21, 454);
            this.blockProgressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.blockProgressBar.MaxBlocks = 1;
            this.blockProgressBar.Name = "blockProgressBar";
            this.blockProgressBar.Progress = 0;
            this.blockProgressBar.Size = new System.Drawing.Size(1076, 27);
            this.blockProgressBar.TabIndex = 20;
            // 
            // importWorker
            // 
            this.importWorker.WorkerReportsProgress = true;
            this.importWorker.WorkerSupportsCancellation = true;
            this.importWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.importWorker_DoWork);
            this.importWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.importWorker_ProgressChanged);
            this.importWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.importWorker_RunWorkerCompleted);
            // 
            // progressLabel
            // 
            this.progressLabel.Location = new System.Drawing.Point(18, 427);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(679, 23);
            this.progressLabel.TabIndex = 21;
            // 
            // failedTextBox
            // 
            this.failedTextBox.Location = new System.Drawing.Point(21, 523);
            this.failedTextBox.Multiline = true;
            this.failedTextBox.Name = "failedTextBox";
            this.failedTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.failedTextBox.Size = new System.Drawing.Size(677, 146);
            this.failedTextBox.TabIndex = 22;
            this.failedTextBox.Visible = false;
            // 
            // failedLabel
            // 
            this.failedLabel.AutoSize = true;
            this.failedLabel.Location = new System.Drawing.Point(18, 504);
            this.failedLabel.Name = "failedLabel";
            this.failedLabel.Size = new System.Drawing.Size(216, 16);
            this.failedLabel.TabIndex = 23;
            this.failedLabel.Text = "The following cards failed to import:";
            this.failedLabel.Visible = false;
            // 
            // ImportCollectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 502);
            this.Controls.Add(this.failedLabel);
            this.Controls.Add(this.failedTextBox);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.blockProgressBar);
            this.Controls.Add(this.collectionsListView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.platformComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.collectionNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filenameLabel);
            this.Controls.Add(this.label1);
            this.Name = "ImportCollectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Collection";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportCollectionForm_FormClosing);
            this.Shown += new System.EventHandler(this.ImportCollectionForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionsListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox platformComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button importButton;
        public System.Windows.Forms.TextBox collectionNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label filenameLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private BrightIdeasSoftware.FastObjectListView collectionsListView;
        private BrightIdeasSoftware.OLVColumn nameColumn;
        private BrightIdeasSoftware.OLVColumn groupColumn;
        private BrightIdeasSoftware.OLVColumn platformColumn;
        private CustomControls.BlockProgressBar blockProgressBar;
        private System.ComponentModel.BackgroundWorker importWorker;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.TextBox failedTextBox;
        private System.Windows.Forms.Label failedLabel;
    }
}