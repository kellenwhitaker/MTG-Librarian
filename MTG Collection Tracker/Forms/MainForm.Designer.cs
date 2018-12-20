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
            this.vS2015LightTheme1 = new KW.WinFormsUI.Docking.VS2015LightTheme();
            this.CheckForNewSetsWorker = new System.ComponentModel.BackgroundWorker();
            this.InitUIWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dockPanel1 = new KW.WinFormsUI.Docking.DockPanel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.TasksProgressBar = new CustomControls.BlockProgressBar();
            this.TasksLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
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
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dockPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.mainMenuStrip);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.statusPanel);
            this.splitContainer1.Panel2MinSize = 25;
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
            this.dockPanel1.Theme = this.vS2015LightTheme1;
            this.dockPanel1.DocumentStyle = KW.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 24);
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel1.ShowAutoHideContentOnHover = false;
            this.dockPanel1.Size = new System.Drawing.Size(1465, 476);
            this.dockPanel1.TabIndex = 1;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1465, 24);
            this.mainMenuStrip.TabIndex = 2;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cardInfoToolStripMenuItem,
            this.dBToolStripMenuItem,
            this.navigatorToolStripMenuItem,
            this.tasksToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // cardInfoToolStripMenuItem
            // 
            this.cardInfoToolStripMenuItem.Name = "cardInfoToolStripMenuItem";
            this.cardInfoToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.cardInfoToolStripMenuItem.Text = "Card Info";
            this.cardInfoToolStripMenuItem.Click += new System.EventHandler(this.cardInfoToolStripMenuItem_Click);
            // 
            // dBToolStripMenuItem
            // 
            this.dBToolStripMenuItem.Name = "dBToolStripMenuItem";
            this.dBToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.dBToolStripMenuItem.Text = "DB";
            this.dBToolStripMenuItem.Click += new System.EventHandler(this.dBToolStripMenuItem_Click);
            // 
            // navigatorToolStripMenuItem
            // 
            this.navigatorToolStripMenuItem.Name = "navigatorToolStripMenuItem";
            this.navigatorToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.navigatorToolStripMenuItem.Text = "Navigator";
            this.navigatorToolStripMenuItem.Click += new System.EventHandler(this.navigatorToolStripMenuItem_Click);
            // 
            // tasksToolStripMenuItem
            // 
            this.tasksToolStripMenuItem.Name = "tasksToolStripMenuItem";
            this.tasksToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.tasksToolStripMenuItem.Text = "Tasks";
            this.tasksToolStripMenuItem.Click += new System.EventHandler(this.tasksToolStripMenuItem_Click);
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.statusPanel.Controls.Add(this.mainPanel);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusPanel.Location = new System.Drawing.Point(0, 0);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(1465, 134);
            this.statusPanel.TabIndex = 3;
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Controls.Add(this.TasksProgressBar);
            this.mainPanel.Controls.Add(this.TasksLabel);
            this.mainPanel.Location = new System.Drawing.Point(831, 109);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(631, 25);
            this.mainPanel.TabIndex = 0;
            // 
            // TasksProgressBar
            // 
            this.TasksProgressBar.BarColor = System.Drawing.Color.White;
            this.TasksProgressBar.BlankBarColor = System.Drawing.Color.DodgerBlue;
            this.TasksProgressBar.BorderColor = System.Drawing.Color.White;
            this.TasksProgressBar.CurrentBlocks = 3;
            this.TasksProgressBar.Location = new System.Drawing.Point(522, 11);
            this.TasksProgressBar.MaxBlocks = 0;
            this.TasksProgressBar.Name = "TasksProgressBar";
            this.TasksProgressBar.Progress = 0;
            this.TasksProgressBar.Size = new System.Drawing.Size(100, 7);
            this.TasksProgressBar.TabIndex = 1;
            // 
            // TasksLabel
            // 
            this.TasksLabel.ForeColor = System.Drawing.Color.White;
            this.TasksLabel.Location = new System.Drawing.Point(197, 7);
            this.TasksLabel.Name = "TasksLabel";
            this.TasksLabel.Size = new System.Drawing.Size(319, 13);
            this.TasksLabel.TabIndex = 0;
            this.TasksLabel.Text = "No active tasks";
            this.TasksLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1465, 635);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "MTG Librarian";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.statusPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private KW.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private System.ComponentModel.BackgroundWorker CheckForNewSetsWorker;
        private System.ComponentModel.BackgroundWorker InitUIWorker;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private KW.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.Panel mainPanel;
        public System.Windows.Forms.ImageList largeIconLists;
        public System.Windows.Forms.Label TasksLabel;
        public CustomControls.BlockProgressBar TasksProgressBar;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cardInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem navigatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tasksToolStripMenuItem;
    }
}

