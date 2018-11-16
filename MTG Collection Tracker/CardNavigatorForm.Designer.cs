namespace MTG_Collection_Tracker
{
    partial class CardNavigatorForm
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
            this.navigatorListView = new BrightIdeasSoftware.TreeListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.navigatorListView)).BeginInit();
            this.SuspendLayout();
            // 
            // navigatorListView
            // 
            this.navigatorListView.AllColumns.Add(this.olvColumn1);
            this.navigatorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.navigatorListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigatorListView.FullRowSelect = true;
            this.navigatorListView.HeaderMaximumHeight = 0;
            this.navigatorListView.Location = new System.Drawing.Point(0, 0);
            this.navigatorListView.Name = "navigatorListView";
            this.navigatorListView.ShowGroups = false;
            this.navigatorListView.Size = new System.Drawing.Size(409, 480);
            this.navigatorListView.TabIndex = 0;
            this.navigatorListView.UseCompatibleStateImageBehavior = false;
            this.navigatorListView.View = System.Windows.Forms.View.Details;
            this.navigatorListView.VirtualMode = true;
            this.navigatorListView.ItemActivate += new System.EventHandler(this.navigatorListView_ItemActivate);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Text";
            this.olvColumn1.Width = 200;
            // 
            // CardNavigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 480);
            this.Controls.Add(this.navigatorListView);
            this.Name = "CardNavigatorForm";
            this.Text = "Card Navigator";
            ((System.ComponentModel.ISupportInitialize)(this.navigatorListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        internal BrightIdeasSoftware.TreeListView navigatorListView;
    }
}