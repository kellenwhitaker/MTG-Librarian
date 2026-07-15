namespace MTG_Librarian
{
    partial class SimulatorForm
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
            this.drawButton = new System.Windows.Forms.Button();
            this.handPanel = new System.Windows.Forms.Panel();
            this.battlefieldPanel = new System.Windows.Forms.Panel();
            this.liveCardMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tapuntapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToGraveyardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToExileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToHandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToBattlefieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.putOnTopOfLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.putOnBottomOfLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.add11CounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.add11CounterToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addCounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoneMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.searchZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.landPanel = new System.Windows.Forms.Panel();
            this.messageLabel = new System.Windows.Forms.Label();
            this.mulliganButton = new System.Windows.Forms.Button();
            this.keepHandButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exilePictureBox = new System.Windows.Forms.PictureBox();
            this.graveyardPictureBox = new System.Windows.Forms.PictureBox();
            this.libraryPictureBox = new System.Windows.Forms.PictureBox();
            this.commandPictureBox = new System.Windows.Forms.PictureBox();
            this.moveToCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveCardMenuStrip.SuspendLayout();
            this.zoneMenuStrip.SuspendLayout();
            this.landPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exilePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.graveyardPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.libraryPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // drawButton
            // 
            this.drawButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.drawButton.Location = new System.Drawing.Point(863, 520);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(75, 23);
            this.drawButton.TabIndex = 1;
            this.drawButton.Text = "Draw";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.drawButton_Click);
            // 
            // handPanel
            // 
            this.handPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.handPanel.AutoScroll = true;
            this.handPanel.BackColor = System.Drawing.Color.Transparent;
            this.handPanel.Location = new System.Drawing.Point(1, 515);
            this.handPanel.Name = "handPanel";
            this.handPanel.Size = new System.Drawing.Size(764, 335);
            this.handPanel.TabIndex = 2;
            // 
            // battlefieldPanel
            // 
            this.battlefieldPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.battlefieldPanel.AutoScroll = true;
            this.battlefieldPanel.BackColor = System.Drawing.Color.Transparent;
            this.battlefieldPanel.Location = new System.Drawing.Point(1, -100);
            this.battlefieldPanel.Name = "battlefieldPanel";
            this.battlefieldPanel.Size = new System.Drawing.Size(764, 315);
            this.battlefieldPanel.TabIndex = 4;
            // 
            // liveCardMenuStrip
            // 
            this.liveCardMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.liveCardMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tapuntapToolStripMenuItem,
            this.moveToGraveyardToolStripMenuItem,
            this.moveToExileToolStripMenuItem,
            this.moveToHandToolStripMenuItem,
            this.moveToBattlefieldToolStripMenuItem,
            this.moveToCommandToolStripMenuItem,
            this.putOnTopOfLibraryToolStripMenuItem,
            this.putOnBottomOfLibraryToolStripMenuItem,
            this.toolStripMenuItem1,
            this.add11CounterToolStripMenuItem,
            this.add11CounterToolStripMenuItem1,
            this.addCounterToolStripMenuItem,
            this.removeCounterToolStripMenuItem});
            this.liveCardMenuStrip.Name = "liveCardMenuStrip";
            this.liveCardMenuStrip.Size = new System.Drawing.Size(239, 326);
            this.liveCardMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.liveCardMenuStrip_Opening);
            // 
            // tapuntapToolStripMenuItem
            // 
            this.tapuntapToolStripMenuItem.Name = "tapuntapToolStripMenuItem";
            this.tapuntapToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.tapuntapToolStripMenuItem.Text = "Tap/untap";
            this.tapuntapToolStripMenuItem.Click += new System.EventHandler(this.tapuntapToolStripMenuItem_Click);
            // 
            // moveToGraveyardToolStripMenuItem
            // 
            this.moveToGraveyardToolStripMenuItem.Name = "moveToGraveyardToolStripMenuItem";
            this.moveToGraveyardToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.moveToGraveyardToolStripMenuItem.Text = "Move to graveyard";
            this.moveToGraveyardToolStripMenuItem.Click += new System.EventHandler(this.moveToGraveyardToolStripMenuItem_Click);
            // 
            // moveToExileToolStripMenuItem
            // 
            this.moveToExileToolStripMenuItem.Name = "moveToExileToolStripMenuItem";
            this.moveToExileToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.moveToExileToolStripMenuItem.Text = "Move to exile";
            this.moveToExileToolStripMenuItem.Click += new System.EventHandler(this.moveToExileToolStripMenuItem_Click);
            // 
            // moveToHandToolStripMenuItem
            // 
            this.moveToHandToolStripMenuItem.Name = "moveToHandToolStripMenuItem";
            this.moveToHandToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.moveToHandToolStripMenuItem.Text = "Move to hand";
            this.moveToHandToolStripMenuItem.Click += new System.EventHandler(this.moveToHandToolStripMenuItem_Click);
            // 
            // moveToBattlefieldToolStripMenuItem
            // 
            this.moveToBattlefieldToolStripMenuItem.Name = "moveToBattlefieldToolStripMenuItem";
            this.moveToBattlefieldToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.moveToBattlefieldToolStripMenuItem.Text = "Move to battlefield";
            this.moveToBattlefieldToolStripMenuItem.Click += new System.EventHandler(this.moveToBattlefieldToolStripMenuItem_Click);
            // 
            // putOnTopOfLibraryToolStripMenuItem
            // 
            this.putOnTopOfLibraryToolStripMenuItem.Name = "putOnTopOfLibraryToolStripMenuItem";
            this.putOnTopOfLibraryToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.putOnTopOfLibraryToolStripMenuItem.Text = "Put on top of library";
            this.putOnTopOfLibraryToolStripMenuItem.Click += new System.EventHandler(this.putOnTopOfLibraryToolStripMenuItem_Click);
            // 
            // putOnBottomOfLibraryToolStripMenuItem
            // 
            this.putOnBottomOfLibraryToolStripMenuItem.Name = "putOnBottomOfLibraryToolStripMenuItem";
            this.putOnBottomOfLibraryToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.putOnBottomOfLibraryToolStripMenuItem.Text = "Put on bottom of library";
            this.putOnBottomOfLibraryToolStripMenuItem.Click += new System.EventHandler(this.putOnBottomOfLibraryToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(235, 6);
            // 
            // add11CounterToolStripMenuItem
            // 
            this.add11CounterToolStripMenuItem.Name = "add11CounterToolStripMenuItem";
            this.add11CounterToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.add11CounterToolStripMenuItem.Text = "Add +1/+1 counter";
            this.add11CounterToolStripMenuItem.Click += new System.EventHandler(this.add11CounterToolStripMenuItem_Click);
            // 
            // add11CounterToolStripMenuItem1
            // 
            this.add11CounterToolStripMenuItem1.Name = "add11CounterToolStripMenuItem1";
            this.add11CounterToolStripMenuItem1.Size = new System.Drawing.Size(238, 24);
            this.add11CounterToolStripMenuItem1.Text = "Add -1/-1 counter";
            this.add11CounterToolStripMenuItem1.Click += new System.EventHandler(this.add11CounterToolStripMenuItem1_Click);
            // 
            // addCounterToolStripMenuItem
            // 
            this.addCounterToolStripMenuItem.Name = "addCounterToolStripMenuItem";
            this.addCounterToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.addCounterToolStripMenuItem.Text = "Add counter";
            this.addCounterToolStripMenuItem.Click += new System.EventHandler(this.addCounterToolStripMenuItem_Click);
            // 
            // removeCounterToolStripMenuItem
            // 
            this.removeCounterToolStripMenuItem.Name = "removeCounterToolStripMenuItem";
            this.removeCounterToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.removeCounterToolStripMenuItem.Text = "Remove counter";
            this.removeCounterToolStripMenuItem.Click += new System.EventHandler(this.removeCounterToolStripMenuItem_Click);
            // 
            // zoneMenuStrip
            // 
            this.zoneMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.zoneMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchZoneToolStripMenuItem,
            this.playToolStripMenuItem});
            this.zoneMenuStrip.Name = "zoneMenuStrip";
            this.zoneMenuStrip.Size = new System.Drawing.Size(168, 52);
            this.zoneMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.zoneMenuStrip_Opening);
            // 
            // searchZoneToolStripMenuItem
            // 
            this.searchZoneToolStripMenuItem.Name = "searchZoneToolStripMenuItem";
            this.searchZoneToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.searchZoneToolStripMenuItem.Text = "Search zone...";
            this.searchZoneToolStripMenuItem.Click += new System.EventHandler(this.searchZoneToolStripMenuItem_Click);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Visible = false;
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // landPanel
            // 
            this.landPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.landPanel.BackColor = System.Drawing.Color.Transparent;
            this.landPanel.Controls.Add(this.messageLabel);
            this.landPanel.Location = new System.Drawing.Point(1, 211);
            this.landPanel.Name = "landPanel";
            this.landPanel.Size = new System.Drawing.Size(764, 298);
            this.landPanel.TabIndex = 7;
            // 
            // messageLabel
            // 
            this.messageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.messageLabel.AutoSize = true;
            this.messageLabel.BackColor = System.Drawing.Color.Black;
            this.messageLabel.ForeColor = System.Drawing.Color.White;
            this.messageLabel.Location = new System.Drawing.Point(3, 277);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(0, 16);
            this.messageLabel.TabIndex = 0;
            // 
            // mulliganButton
            // 
            this.mulliganButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mulliganButton.Enabled = false;
            this.mulliganButton.Location = new System.Drawing.Point(1005, 520);
            this.mulliganButton.Name = "mulliganButton";
            this.mulliganButton.Size = new System.Drawing.Size(75, 23);
            this.mulliganButton.TabIndex = 8;
            this.mulliganButton.Text = "Mulligan";
            this.mulliganButton.UseVisualStyleBackColor = true;
            this.mulliganButton.Click += new System.EventHandler(this.mulliganButton_Click);
            // 
            // keepHandButton
            // 
            this.keepHandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.keepHandButton.Enabled = false;
            this.keepHandButton.Location = new System.Drawing.Point(944, 520);
            this.keepHandButton.Name = "keepHandButton";
            this.keepHandButton.Size = new System.Drawing.Size(55, 23);
            this.keepHandButton.TabIndex = 9;
            this.keepHandButton.Text = "Keep";
            this.keepHandButton.UseVisualStyleBackColor = true;
            this.keepHandButton.Click += new System.EventHandler(this.keepHandButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1092, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // exilePictureBox
            // 
            this.exilePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exilePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.exilePictureBox.ContextMenuStrip = this.zoneMenuStrip;
            this.exilePictureBox.Location = new System.Drawing.Point(863, -96);
            this.exilePictureBox.Name = "exilePictureBox";
            this.exilePictureBox.Size = new System.Drawing.Size(217, 280);
            this.exilePictureBox.TabIndex = 6;
            this.exilePictureBox.TabStop = false;
            // 
            // graveyardPictureBox
            // 
            this.graveyardPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.graveyardPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.graveyardPictureBox.ContextMenuStrip = this.zoneMenuStrip;
            this.graveyardPictureBox.Location = new System.Drawing.Point(863, 224);
            this.graveyardPictureBox.Name = "graveyardPictureBox";
            this.graveyardPictureBox.Size = new System.Drawing.Size(217, 280);
            this.graveyardPictureBox.TabIndex = 5;
            this.graveyardPictureBox.TabStop = false;
            // 
            // libraryPictureBox
            // 
            this.libraryPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.libraryPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.libraryPictureBox.ContextMenuStrip = this.zoneMenuStrip;
            this.libraryPictureBox.Location = new System.Drawing.Point(863, 549);
            this.libraryPictureBox.Name = "libraryPictureBox";
            this.libraryPictureBox.Size = new System.Drawing.Size(217, 280);
            this.libraryPictureBox.TabIndex = 0;
            this.libraryPictureBox.TabStop = false;
            // 
            // commandPictureBox
            // 
            this.commandPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.commandPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.commandPictureBox.ContextMenuStrip = this.zoneMenuStrip;
            this.commandPictureBox.Location = new System.Drawing.Point(640, 549);
            this.commandPictureBox.Name = "commandPictureBox";
            this.commandPictureBox.Size = new System.Drawing.Size(217, 280);
            this.commandPictureBox.TabIndex = 11;
            this.commandPictureBox.TabStop = false;
            this.commandPictureBox.Visible = false;
            // 
            // moveToCommandToolStripMenuItem
            // 
            this.moveToCommandToolStripMenuItem.Name = "moveToCommandToolStripMenuItem";
            this.moveToCommandToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.moveToCommandToolStripMenuItem.Text = "Move to command";
            this.moveToCommandToolStripMenuItem.Click += new System.EventHandler(this.moveToCommandToolStripMenuItem_Click);
            // 
            // SimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MTG_Librarian.Properties.Resources._5020;
            this.ClientSize = new System.Drawing.Size(1092, 856);
            this.Controls.Add(this.commandPictureBox);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.keepHandButton);
            this.Controls.Add(this.mulliganButton);
            this.Controls.Add(this.landPanel);
            this.Controls.Add(this.exilePictureBox);
            this.Controls.Add(this.graveyardPictureBox);
            this.Controls.Add(this.battlefieldPanel);
            this.Controls.Add(this.handPanel);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.libraryPictureBox);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SimulatorForm";
            this.Text = "Simulator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SimulatorForm_FormClosed);
            this.Resize += new System.EventHandler(this.SimulatorForm_Resize);
            this.liveCardMenuStrip.ResumeLayout(false);
            this.zoneMenuStrip.ResumeLayout(false);
            this.landPanel.ResumeLayout(false);
            this.landPanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exilePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.graveyardPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.libraryPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox libraryPictureBox;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Panel handPanel;
        private System.Windows.Forms.Panel battlefieldPanel;
        private System.Windows.Forms.PictureBox graveyardPictureBox;
        private System.Windows.Forms.PictureBox exilePictureBox;
        private System.Windows.Forms.ContextMenuStrip liveCardMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem moveToGraveyardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToExileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip zoneMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem searchZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToHandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToBattlefieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem putOnTopOfLibraryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem putOnBottomOfLibraryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem add11CounterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem add11CounterToolStripMenuItem1;
        private System.Windows.Forms.Panel landPanel;
        private System.Windows.Forms.ToolStripMenuItem addCounterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeCounterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tapuntapToolStripMenuItem;
        private System.Windows.Forms.Button mulliganButton;
        private System.Windows.Forms.Button keepHandButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.PictureBox commandPictureBox;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToCommandToolStripMenuItem;
    }
}