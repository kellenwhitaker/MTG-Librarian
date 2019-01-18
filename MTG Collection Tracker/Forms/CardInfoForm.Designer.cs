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
            this.cardPictureBox = new System.Windows.Forms.PictureBox();
            this.cardTextHtmlPanel = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
            this.flipButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cardPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cardPictureBox
            // 
            this.cardPictureBox.Location = new System.Drawing.Point(12, 12);
            this.cardPictureBox.Name = "cardPictureBox";
            this.cardPictureBox.Size = new System.Drawing.Size(223, 310);
            this.cardPictureBox.TabIndex = 0;
            this.cardPictureBox.TabStop = false;
            // 
            // cardTextHtmlPanel
            // 
            this.cardTextHtmlPanel.AutoScroll = true;
            this.cardTextHtmlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.cardTextHtmlPanel.BaseStylesheet = null;
            this.cardTextHtmlPanel.Location = new System.Drawing.Point(12, 357);
            this.cardTextHtmlPanel.Name = "cardTextHtmlPanel";
            this.cardTextHtmlPanel.Size = new System.Drawing.Size(223, 448);
            this.cardTextHtmlPanel.TabIndex = 1;
            this.cardTextHtmlPanel.Text = null;
            // 
            // flipButton
            // 
            this.flipButton.Location = new System.Drawing.Point(97, 328);
            this.flipButton.Name = "flipButton";
            this.flipButton.Size = new System.Drawing.Size(49, 23);
            this.flipButton.TabIndex = 2;
            this.flipButton.Text = "Flip";
            this.flipButton.UseVisualStyleBackColor = true;
            this.flipButton.Visible = false;
            this.flipButton.Click += new System.EventHandler(this.flipButton_Click);
            // 
            // CardInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 788);
            this.Controls.Add(this.flipButton);
            this.Controls.Add(this.cardTextHtmlPanel);
            this.Controls.Add(this.cardPictureBox);
            this.Name = "CardInfoForm";
            this.Text = "Card Info";
            ((System.ComponentModel.ISupportInitialize)(this.cardPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox cardPictureBox;
        private TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel cardTextHtmlPanel;
        private System.Windows.Forms.Button flipButton;
    }
}