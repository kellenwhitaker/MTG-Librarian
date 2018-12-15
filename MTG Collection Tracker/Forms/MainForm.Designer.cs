using System.Drawing;

namespace MTG_Librarian
{
    partial class MainForm
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
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.CheckForNewSetsWorker = new System.ComponentModel.BackgroundWorker();
            this.InitUIWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tasksProgressBar = new CustomControls.BlockProgressBar();
            this.tasksLabel = new System.Windows.Forms.Label();
            this.largeIconList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CheckForNewSetsWorker
            // 
            this.CheckForNewSetsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkForNewSetsWorker_DoWork);
            this.CheckForNewSetsWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.CheckForNewSetsWorker_RunWorkerCompleted);
            // 
            // InitUIWorker
            // 
            this.InitUIWorker.WorkerReportsProgress = true;
            this.InitUIWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.InitUIWorker_DoWork);
            this.InitUIWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.InitUIWorker_ProgressChanged);
            this.InitUIWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.InitUIWorker_RunWorkerCompleted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dockPanel1);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2MinSize = 15;
            this.splitContainer1.Size = new System.Drawing.Size(1465, 635);
            this.splitContainer1.SplitterDistance = 500;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 2;
            // 
            // dockPanel1
            // 
            this.dockPanel1.AllowEndUserNestedDocking = false;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.dockPanel1.DockBottomPortion = 0.5D;
            this.dockPanel1.DockLeftPortion = 0.15D;
            this.dockPanel1.DockRightPortion = 0.15D;
            this.dockPanel1.DockTopPortion = 0.5D;
            this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel1.ShowAutoHideContentOnHover = false;
            this.dockPanel1.Size = new System.Drawing.Size(1465, 500);
            this.dockPanel1.TabIndex = 1;
            this.dockPanel1.Theme = this.vS2015LightTheme1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1465, 134);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.tasksProgressBar);
            this.panel2.Controls.Add(this.tasksLabel);
            this.panel2.Location = new System.Drawing.Point(831, 109);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(631, 25);
            this.panel2.TabIndex = 0;
            // 
            // tasksProgressBar
            // 
            this.tasksProgressBar.BarColor = System.Drawing.Color.White;
            this.tasksProgressBar.BlankBarColor = System.Drawing.Color.DodgerBlue;
            this.tasksProgressBar.BorderColor = System.Drawing.Color.White;
            this.tasksProgressBar.CurrentBlocks = 3;
            this.tasksProgressBar.Location = new System.Drawing.Point(522, 11);
            this.tasksProgressBar.MaxBlocks = 0;
            this.tasksProgressBar.Name = "tasksProgressBar";
            this.tasksProgressBar.Progress = 0;
            this.tasksProgressBar.Size = new System.Drawing.Size(100, 7);
            this.tasksProgressBar.TabIndex = 1;
            // 
            // tasksLabel
            // 
            this.tasksLabel.ForeColor = System.Drawing.Color.White;
            this.tasksLabel.Location = new System.Drawing.Point(197, 7);
            this.tasksLabel.Name = "tasksLabel";
            this.tasksLabel.Size = new System.Drawing.Size(319, 13);
            this.tasksLabel.TabIndex = 0;
            this.tasksLabel.Text = "No active tasks";
            this.tasksLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // largeIconList
            // 
            this.largeIconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.largeIconList.ImageSize = new System.Drawing.Size(32, 32);
            this.largeIconList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1465, 635);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "MTG Librarian";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private System.ComponentModel.BackgroundWorker CheckForNewSetsWorker;
        private System.ComponentModel.BackgroundWorker InitUIWorker;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label tasksLabel;
        private CustomControls.BlockProgressBar tasksProgressBar;
        private static System.Windows.Forms.ImageList smallIconList;
        public System.Windows.Forms.ImageList largeIconList;
    }
}

