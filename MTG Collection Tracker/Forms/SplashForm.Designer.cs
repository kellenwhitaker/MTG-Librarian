namespace MTG_Librarian
{
    partial class SplashForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.blockProgressBar2 = new CustomControls.BlockProgressBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bracketLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.blockProgressBar1 = new CustomControls.BlockProgressBar();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.blockProgressBar2);
            this.panel1.Controls.Add(this.statusLabel);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.bracketLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 402);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // blockProgressBar2
            // 
            this.blockProgressBar2.BarColor = System.Drawing.Color.SlateGray;
            this.blockProgressBar2.BlankBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.blockProgressBar2.BorderColor = System.Drawing.Color.DimGray;
            this.blockProgressBar2.CurrentBlocks = 0;
            this.blockProgressBar2.Location = new System.Drawing.Point(66, 364);
            this.blockProgressBar2.MaxBlocks = 3;
            this.blockProgressBar2.Name = "blockProgressBar2";
            this.blockProgressBar2.Progress = 0;
            this.blockProgressBar2.Size = new System.Drawing.Size(402, 10);
            this.blockProgressBar2.TabIndex = 11;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusLabel.Location = new System.Drawing.Point(62, 320);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(107, 26);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Loading...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MTG_Librarian .Properties.Resources.mtgl_logo;
            this.pictureBox1.Location = new System.Drawing.Point(29, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(475, 282);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // bracketLabel
            // 
            this.bracketLabel.AutoSize = true;
            this.bracketLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bracketLabel.ForeColor = System.Drawing.Color.SlateGray;
            this.bracketLabel.Location = new System.Drawing.Point(18, 311);
            this.bracketLabel.Name = "bracketLabel";
            this.bracketLabel.Size = new System.Drawing.Size(497, 63);
            this.bracketLabel.TabIndex = 10;
            this.bracketLabel.Tag = "";
            this.bracketLabel.Text = "{                               }";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 125;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // blockProgressBar1
            // 
            this.blockProgressBar1.BarColor = System.Drawing.Color.Black;
            this.blockProgressBar1.BlankBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.blockProgressBar1.BorderColor = System.Drawing.Color.DimGray;
            this.blockProgressBar1.CurrentBlocks = 0;
            this.blockProgressBar1.Location = new System.Drawing.Point(0, 0);
            this.blockProgressBar1.MaxBlocks = 3;
            this.blockProgressBar1.Name = "blockProgressBar1";
            this.blockProgressBar1.Progress = 0;
            this.blockProgressBar1.Size = new System.Drawing.Size(306, 51);
            this.blockProgressBar1.TabIndex = 11;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(534, 402);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashForm";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label bracketLabel;
        private CustomControls.BlockProgressBar blockProgressBar1;
        private CustomControls.BlockProgressBar blockProgressBar2;
    }
}