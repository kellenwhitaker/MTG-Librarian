namespace MTG_Librarian.Forms
{
    partial class QuickEntryForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.fastObjectListView1 = new BrightIdeasSoftware.FastObjectListView();
            this.nameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.setColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.numberColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.priceColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cardPictureBox = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.fastObjectListView1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cardPictureBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 747);
            this.panel1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(30, 449);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 41);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(119, 15);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(92, 20);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.Text = "Sideboard";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(20, 15);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(93, 20);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Mainboard";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // fastObjectListView1
            // 
            this.fastObjectListView1.AllColumns.Add(this.nameColumn);
            this.fastObjectListView1.AllColumns.Add(this.setColumn);
            this.fastObjectListView1.AllColumns.Add(this.numberColumn);
            this.fastObjectListView1.AllColumns.Add(this.priceColumn);
            this.fastObjectListView1.CellEditUseWholeCell = false;
            this.fastObjectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.setColumn,
            this.numberColumn,
            this.priceColumn});
            this.fastObjectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastObjectListView1.FullRowSelect = true;
            this.fastObjectListView1.HideSelection = false;
            this.fastObjectListView1.Location = new System.Drawing.Point(30, 496);
            this.fastObjectListView1.MultiSelect = false;
            this.fastObjectListView1.Name = "fastObjectListView1";
            this.fastObjectListView1.ShowGroups = false;
            this.fastObjectListView1.Size = new System.Drawing.Size(341, 239);
            this.fastObjectListView1.TabIndex = 8;
            this.fastObjectListView1.UseCompatibleStateImageBehavior = false;
            this.fastObjectListView1.View = System.Windows.Forms.View.Details;
            this.fastObjectListView1.VirtualMode = true;
            this.fastObjectListView1.SelectionChanged += new System.EventHandler(this.fastObjectListView1_SelectionChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.AspectName = "Name";
            this.nameColumn.AspectToStringFormat = "{0:N2}";
            this.nameColumn.Text = "Name";
            // 
            // setColumn
            // 
            this.setColumn.AspectName = "set_name";
            this.setColumn.Text = "Set";
            // 
            // numberColumn
            // 
            this.numberColumn.AspectName = "collector_number";
            this.numberColumn.Text = "#";
            // 
            // priceColumn
            // 
            this.priceColumn.AspectName = "Price";
            this.priceColumn.AspectToStringFormat = "{0:N2}";
            this.priceColumn.Text = "Price";
            this.priceColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(30, 418);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(341, 31);
            this.comboBox1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 399);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Card name or partial name";
            // 
            // cardPictureBox
            // 
            this.cardPictureBox.Location = new System.Drawing.Point(30, 13);
            this.cardPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.cardPictureBox.Name = "cardPictureBox";
            this.cardPictureBox.Size = new System.Drawing.Size(297, 382);
            this.cardPictureBox.TabIndex = 4;
            this.cardPictureBox.TabStop = false;
            // 
            // QuickEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 747);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "QuickEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Quick Entry";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.QuickEntryForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QuickEntryForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox cardPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private BrightIdeasSoftware.FastObjectListView fastObjectListView1;
        private BrightIdeasSoftware.OLVColumn nameColumn;
        private BrightIdeasSoftware.OLVColumn setColumn;
        private BrightIdeasSoftware.OLVColumn numberColumn;
        private BrightIdeasSoftware.OLVColumn priceColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}