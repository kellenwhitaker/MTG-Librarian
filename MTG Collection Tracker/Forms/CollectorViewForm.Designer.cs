namespace MTG_Collection_Tracker
{
    partial class CollectorViewForm
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
            this.collectorListView = new BrightIdeasSoftware.TreeListView();
            ((System.ComponentModel.ISupportInitialize)(this.collectorListView)).BeginInit();
            this.SuspendLayout();
            // 
            // collectorListView
            // 
            this.collectorListView.Location = new System.Drawing.Point(12, 12);
            this.collectorListView.Name = "collectorListView";
            this.collectorListView.ShowGroups = false;
            this.collectorListView.Size = new System.Drawing.Size(465, 416);
            this.collectorListView.TabIndex = 0;
            this.collectorListView.UseCompatibleStateImageBehavior = false;
            this.collectorListView.View = System.Windows.Forms.View.Details;
            this.collectorListView.VirtualMode = true;
            // 
            // CollectorViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 440);
            this.Controls.Add(this.collectorListView);
            this.Name = "CollectorViewForm";
            this.Text = "CollectorViewForm";
            ((System.ComponentModel.ISupportInitialize)(this.collectorListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView collectorListView;
    }
}