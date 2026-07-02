using BetterPanel;

namespace MTG_Librarian
{
    partial class ZoneSearchForm
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
            this.cardsPanel = new BetterPanel.BetterPanel();
            this.SuspendLayout();
            // 
            // cardsPanel
            // 
            this.cardsPanel.AutoScroll = true;
            this.cardsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardsPanel.Location = new System.Drawing.Point(0, 0);
            this.cardsPanel.Name = "cardsPanel";
            this.cardsPanel.Size = new System.Drawing.Size(800, 420);
            this.cardsPanel.TabIndex = 0;
            // 
            // ZoneSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cardsPanel);
            this.Name = "ZoneSearchForm";
            this.Text = "Search Zone";
            this.ResumeLayout(false);

        }

        #endregion

        public BetterPanel.BetterPanel cardsPanel;
    }
}