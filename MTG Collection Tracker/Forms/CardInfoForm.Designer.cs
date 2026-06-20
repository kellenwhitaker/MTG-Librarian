namespace MTG_Librarian
{
    partial class CardInfoForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.flipButton = new System.Windows.Forms.Button();
            this.cardTextHtmlPanel = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
            this.cardPictureBox = new System.Windows.Forms.PictureBox();
            this.legalitiesTabPage = new System.Windows.Forms.TabPage();
            this.legalitiesListView = new BrightIdeasSoftware.FastObjectListView();
            this.formatColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.legalityColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.rulingsTabPage = new System.Windows.Forms.TabPage();
            this.rulingsHtmlPanel = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
            this.printingsTabPage = new System.Windows.Forms.TabPage();
            this.printingsListView = new BrightIdeasSoftware.FastObjectListView();
            this.setColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.numberColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.priceColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabControl.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cardPictureBox)).BeginInit();
            this.legalitiesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.legalitiesListView)).BeginInit();
            this.rulingsTabPage.SuspendLayout();
            this.printingsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printingsListView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Controls.Add(this.mainTabPage);
            this.tabControl.Controls.Add(this.legalitiesTabPage);
            this.tabControl.Controls.Add(this.rulingsTabPage);
            this.tabControl.Controls.Add(this.printingsTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(379, 970);
            this.tabControl.TabIndex = 3;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.flipButton);
            this.mainTabPage.Controls.Add(this.cardTextHtmlPanel);
            this.mainTabPage.Controls.Add(this.cardPictureBox);
            this.mainTabPage.Location = new System.Drawing.Point(4, 4);
            this.mainTabPage.Name = "mainTabPage";
            this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mainTabPage.Size = new System.Drawing.Size(371, 941);
            this.mainTabPage.TabIndex = 0;
            this.mainTabPage.Text = "Main";
            this.mainTabPage.UseVisualStyleBackColor = true;
            // 
            // flipButton
            // 
            this.flipButton.Location = new System.Drawing.Point(122, 391);
            this.flipButton.Margin = new System.Windows.Forms.Padding(4);
            this.flipButton.Name = "flipButton";
            this.flipButton.Size = new System.Drawing.Size(65, 28);
            this.flipButton.TabIndex = 5;
            this.flipButton.Text = "Flip";
            this.flipButton.UseVisualStyleBackColor = true;
            this.flipButton.Visible = false;
            // 
            // cardTextHtmlPanel
            // 
            this.cardTextHtmlPanel.AutoScroll = true;
            this.cardTextHtmlPanel.BackColor = System.Drawing.Color.Transparent;
            this.cardTextHtmlPanel.BaseStylesheet = null;
            this.cardTextHtmlPanel.Location = new System.Drawing.Point(9, 426);
            this.cardTextHtmlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.cardTextHtmlPanel.Name = "cardTextHtmlPanel";
            this.cardTextHtmlPanel.Size = new System.Drawing.Size(297, 508);
            this.cardTextHtmlPanel.TabIndex = 4;
            this.cardTextHtmlPanel.Text = null;
            // 
            // cardPictureBox
            // 
            this.cardPictureBox.Location = new System.Drawing.Point(9, 4);
            this.cardPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.cardPictureBox.Name = "cardPictureBox";
            this.cardPictureBox.Size = new System.Drawing.Size(297, 382);
            this.cardPictureBox.TabIndex = 3;
            this.cardPictureBox.TabStop = false;
            // 
            // legalitiesTabPage
            // 
            this.legalitiesTabPage.Controls.Add(this.legalitiesListView);
            this.legalitiesTabPage.Location = new System.Drawing.Point(4, 4);
            this.legalitiesTabPage.Name = "legalitiesTabPage";
            this.legalitiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.legalitiesTabPage.Size = new System.Drawing.Size(371, 941);
            this.legalitiesTabPage.TabIndex = 1;
            this.legalitiesTabPage.Text = "Legalities";
            this.legalitiesTabPage.UseVisualStyleBackColor = true;
            // 
            // legalitiesListView
            // 
            this.legalitiesListView.AllColumns.Add(this.formatColumn);
            this.legalitiesListView.AllColumns.Add(this.legalityColumn);
            this.legalitiesListView.CellEditUseWholeCell = false;
            this.legalitiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.formatColumn,
            this.legalityColumn});
            this.legalitiesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legalitiesListView.HideSelection = false;
            this.legalitiesListView.Location = new System.Drawing.Point(3, 3);
            this.legalitiesListView.Name = "legalitiesListView";
            this.legalitiesListView.ShowGroups = false;
            this.legalitiesListView.Size = new System.Drawing.Size(365, 935);
            this.legalitiesListView.TabIndex = 7;
            this.legalitiesListView.UseCompatibleStateImageBehavior = false;
            this.legalitiesListView.View = System.Windows.Forms.View.Details;
            this.legalitiesListView.VirtualMode = true;
            this.legalitiesListView.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.legalitiesListView_FormatRow);
            // 
            // formatColumn
            // 
            this.formatColumn.AspectName = "Format";
            this.formatColumn.Text = "Format";
            // 
            // legalityColumn
            // 
            this.legalityColumn.AspectName = "Legality";
            this.legalityColumn.Text = "Legality";
            // 
            // rulingsTabPage
            // 
            this.rulingsTabPage.Controls.Add(this.rulingsHtmlPanel);
            this.rulingsTabPage.Location = new System.Drawing.Point(4, 4);
            this.rulingsTabPage.Name = "rulingsTabPage";
            this.rulingsTabPage.Size = new System.Drawing.Size(371, 941);
            this.rulingsTabPage.TabIndex = 2;
            this.rulingsTabPage.Text = "Rulings";
            this.rulingsTabPage.UseVisualStyleBackColor = true;
            // 
            // rulingsHtmlPanel
            // 
            this.rulingsHtmlPanel.AutoScroll = true;
            this.rulingsHtmlPanel.BackColor = System.Drawing.SystemColors.Window;
            this.rulingsHtmlPanel.BaseStylesheet = null;
            this.rulingsHtmlPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.rulingsHtmlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rulingsHtmlPanel.Location = new System.Drawing.Point(0, 0);
            this.rulingsHtmlPanel.Name = "rulingsHtmlPanel";
            this.rulingsHtmlPanel.Size = new System.Drawing.Size(371, 941);
            this.rulingsHtmlPanel.TabIndex = 0;
            this.rulingsHtmlPanel.Text = null;
            // 
            // printingsTabPage
            // 
            this.printingsTabPage.Controls.Add(this.printingsListView);
            this.printingsTabPage.Location = new System.Drawing.Point(4, 4);
            this.printingsTabPage.Name = "printingsTabPage";
            this.printingsTabPage.Size = new System.Drawing.Size(371, 941);
            this.printingsTabPage.TabIndex = 3;
            this.printingsTabPage.Text = "Printings";
            this.printingsTabPage.UseVisualStyleBackColor = true;
            // 
            // printingsListView
            // 
            this.printingsListView.AllColumns.Add(this.setColumn);
            this.printingsListView.AllColumns.Add(this.numberColumn);
            this.printingsListView.AllColumns.Add(this.priceColumn);
            this.printingsListView.CellEditUseWholeCell = false;
            this.printingsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.setColumn,
            this.numberColumn,
            this.priceColumn});
            this.printingsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printingsListView.FullRowSelect = true;
            this.printingsListView.HideSelection = false;
            this.printingsListView.Location = new System.Drawing.Point(0, 0);
            this.printingsListView.Name = "printingsListView";
            this.printingsListView.ShowGroups = false;
            this.printingsListView.Size = new System.Drawing.Size(371, 941);
            this.printingsListView.TabIndex = 0;
            this.printingsListView.UseCompatibleStateImageBehavior = false;
            this.printingsListView.View = System.Windows.Forms.View.Details;
            this.printingsListView.VirtualMode = true;
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
            this.priceColumn.Text = "Price";
            // 
            // CardInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 970);
            this.Controls.Add(this.tabControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CardInfoForm";
            this.Text = "Card Info";
            this.Resize += new System.EventHandler(this.CardInfoForm_Resize);
            this.tabControl.ResumeLayout(false);
            this.mainTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cardPictureBox)).EndInit();
            this.legalitiesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.legalitiesListView)).EndInit();
            this.rulingsTabPage.ResumeLayout(false);
            this.printingsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.printingsListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage mainTabPage;
        private System.Windows.Forms.TabPage legalitiesTabPage;
        private System.Windows.Forms.Button flipButton;
        private TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel cardTextHtmlPanel;
        private System.Windows.Forms.PictureBox cardPictureBox;
        private BrightIdeasSoftware.FastObjectListView legalitiesListView;
        private BrightIdeasSoftware.OLVColumn formatColumn;
        private BrightIdeasSoftware.OLVColumn legalityColumn;
        private System.Windows.Forms.TabPage rulingsTabPage;
        private TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel rulingsHtmlPanel;
        private System.Windows.Forms.TabPage printingsTabPage;
        private BrightIdeasSoftware.FastObjectListView printingsListView;
        private BrightIdeasSoftware.OLVColumn setColumn;
        private BrightIdeasSoftware.OLVColumn numberColumn;
        private BrightIdeasSoftware.OLVColumn priceColumn;
        public System.Windows.Forms.TabControl tabControl;
    }
}