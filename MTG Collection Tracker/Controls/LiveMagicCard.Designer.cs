namespace MTG_Librarian
{
    partial class LiveMagicCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.playButton = new System.Windows.Forms.Button();
            this.discardButton = new System.Windows.Forms.Button();
            this.countLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.P1P1Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(0, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            // 
            // discardButton
            // 
            this.discardButton.Location = new System.Drawing.Point(142, 3);
            this.discardButton.Name = "discardButton";
            this.discardButton.Size = new System.Drawing.Size(75, 23);
            this.discardButton.TabIndex = 2;
            this.discardButton.Text = "Discard";
            this.discardButton.UseVisualStyleBackColor = true;
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.BackColor = System.Drawing.Color.Black;
            this.countLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countLabel.ForeColor = System.Drawing.Color.White;
            this.countLabel.Location = new System.Drawing.Point(13, 63);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(0, 25);
            this.countLabel.TabIndex = 3;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 32);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(217, 280);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // P1P1Label
            // 
            this.P1P1Label.AutoSize = true;
            this.P1P1Label.BackColor = System.Drawing.Color.Black;
            this.P1P1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P1P1Label.ForeColor = System.Drawing.Color.White;
            this.P1P1Label.Location = new System.Drawing.Point(127, 216);
            this.P1P1Label.Name = "P1P1Label";
            this.P1P1Label.Size = new System.Drawing.Size(0, 25);
            this.P1P1Label.TabIndex = 4;
            // 
            // LiveMagicCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.P1P1Label);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.discardButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.pictureBox);
            this.Name = "LiveMagicCard";
            this.Size = new System.Drawing.Size(221, 315);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Button playButton;
        public System.Windows.Forms.Button discardButton;
        public System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label P1P1Label;
    }
}
