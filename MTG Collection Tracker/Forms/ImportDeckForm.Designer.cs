namespace MTG_Librarian
{
    partial class ImportDeckForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.deckNameTextBox = new System.Windows.Forms.TextBox();
            this.importButton = new System.Windows.Forms.Button();
            this.importWorker = new System.ComponentModel.BackgroundWorker();
            this.progressLabel = new System.Windows.Forms.Label();
            this.blockProgressBar = new CustomControls.BlockProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.platformComboBox = new System.Windows.Forms.ComboBox();
            this.failedTextBox = new System.Windows.Forms.TextBox();
            this.failedLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filename:";
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Location = new System.Drawing.Point(12, 35);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(44, 16);
            this.filenameLabel.TabIndex = 1;
            this.filenameLabel.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Deck name:";
            // 
            // deckNameTextBox
            // 
            this.deckNameTextBox.Location = new System.Drawing.Point(15, 96);
            this.deckNameTextBox.Name = "deckNameTextBox";
            this.deckNameTextBox.Size = new System.Drawing.Size(216, 22);
            this.deckNameTextBox.TabIndex = 3;
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(392, 96);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 4;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // importWorker
            // 
            this.importWorker.WorkerReportsProgress = true;
            this.importWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.importWorker_DoWork);
            this.importWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.importWorker_ProgressChanged);
            this.importWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.importWorker_RunWorkerCompleted);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(17, 130);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(0, 16);
            this.progressLabel.TabIndex = 5;
            // 
            // blockProgressBar
            // 
            this.blockProgressBar.BarColor = System.Drawing.Color.SlateGray;
            this.blockProgressBar.BlankBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.blockProgressBar.BorderColor = System.Drawing.Color.DimGray;
            this.blockProgressBar.CurrentBlocks = 0;
            this.blockProgressBar.Location = new System.Drawing.Point(15, 160);
            this.blockProgressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.blockProgressBar.MaxBlocks = 1;
            this.blockProgressBar.Name = "blockProgressBar";
            this.blockProgressBar.Progress = 0;
            this.blockProgressBar.Size = new System.Drawing.Size(676, 27);
            this.blockProgressBar.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Platform";
            // 
            // platformComboBox
            // 
            this.platformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformComboBox.FormattingEnabled = true;
            this.platformComboBox.Items.AddRange(new object[] {
            "Paper",
            "Arena",
            "Magic Online"});
            this.platformComboBox.Location = new System.Drawing.Point(238, 94);
            this.platformComboBox.Name = "platformComboBox";
            this.platformComboBox.Size = new System.Drawing.Size(148, 24);
            this.platformComboBox.TabIndex = 8;
            // 
            // failedTextBox
            // 
            this.failedTextBox.Location = new System.Drawing.Point(12, 226);
            this.failedTextBox.Multiline = true;
            this.failedTextBox.Name = "failedTextBox";
            this.failedTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.failedTextBox.Size = new System.Drawing.Size(677, 146);
            this.failedTextBox.TabIndex = 9;
            this.failedTextBox.Visible = false;
            // 
            // failedLabel
            // 
            this.failedLabel.AutoSize = true;
            this.failedLabel.Location = new System.Drawing.Point(12, 207);
            this.failedLabel.Name = "failedLabel";
            this.failedLabel.Size = new System.Drawing.Size(216, 16);
            this.failedLabel.TabIndex = 10;
            this.failedLabel.Text = "The following cards failed to import:";
            this.failedLabel.Visible = false;
            // 
            // ImportDeckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 202);
            this.Controls.Add(this.failedLabel);
            this.Controls.Add(this.failedTextBox);
            this.Controls.Add(this.platformComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.blockProgressBar);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.deckNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filenameLabel);
            this.Controls.Add(this.label1);
            this.Name = "ImportDeckForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Deck";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label filenameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button importButton;
        private System.ComponentModel.BackgroundWorker importWorker;
        public System.Windows.Forms.TextBox deckNameTextBox;
        private System.Windows.Forms.Label progressLabel;
        private CustomControls.BlockProgressBar blockProgressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox platformComboBox;
        private System.Windows.Forms.TextBox failedTextBox;
        private System.Windows.Forms.Label failedLabel;
    }
}