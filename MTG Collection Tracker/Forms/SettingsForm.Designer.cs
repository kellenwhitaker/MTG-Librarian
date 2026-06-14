namespace MTG_Librarian.Forms
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.defaultCurrencyComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.defaultSearchLanguageComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.paperCheckBox = new System.Windows.Forms.CheckBox();
            this.arenaCheckBox = new System.Windows.Forms.CheckBox();
            this.magicOnlineCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default search language: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Default currency: ";
            // 
            // defaultCurrencyComboBox
            // 
            this.defaultCurrencyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultCurrencyComboBox.FormattingEnabled = true;
            this.defaultCurrencyComboBox.Items.AddRange(new object[] {
            "USD",
            "EUR",
            "TIX"});
            this.defaultCurrencyComboBox.Location = new System.Drawing.Point(174, 36);
            this.defaultCurrencyComboBox.Name = "defaultCurrencyComboBox";
            this.defaultCurrencyComboBox.Size = new System.Drawing.Size(121, 24);
            this.defaultCurrencyComboBox.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(236, 380);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(317, 380);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // defaultSearchLanguageComboBox
            // 
            this.defaultSearchLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultSearchLanguageComboBox.FormattingEnabled = true;
            this.defaultSearchLanguageComboBox.Items.AddRange(new object[] {
            "English",
            "Spanish",
            "French",
            "German",
            "Italian",
            "Portuguese",
            "Japanese",
            "Korean",
            "Russian",
            "Simplified Chinese",
            "Traditional Chinese",
            "Hebrew",
            "Latin",
            "Ancient Greek",
            "Arabic",
            "Sanskrit",
            "Phyrexian",
            "Quenya"});
            this.defaultSearchLanguageComboBox.Location = new System.Drawing.Point(174, 6);
            this.defaultSearchLanguageComboBox.Name = "defaultSearchLanguageComboBox";
            this.defaultSearchLanguageComboBox.Size = new System.Drawing.Size(121, 24);
            this.defaultSearchLanguageComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Default platforms:";
            // 
            // paperCheckBox
            // 
            this.paperCheckBox.AutoSize = true;
            this.paperCheckBox.Location = new System.Drawing.Point(174, 66);
            this.paperCheckBox.Name = "paperCheckBox";
            this.paperCheckBox.Size = new System.Drawing.Size(66, 20);
            this.paperCheckBox.TabIndex = 7;
            this.paperCheckBox.Text = "Paper";
            this.paperCheckBox.UseVisualStyleBackColor = true;
            // 
            // arenaCheckBox
            // 
            this.arenaCheckBox.AutoSize = true;
            this.arenaCheckBox.Location = new System.Drawing.Point(246, 66);
            this.arenaCheckBox.Name = "arenaCheckBox";
            this.arenaCheckBox.Size = new System.Drawing.Size(65, 20);
            this.arenaCheckBox.TabIndex = 8;
            this.arenaCheckBox.Text = "Arena";
            this.arenaCheckBox.UseVisualStyleBackColor = true;
            // 
            // magicOnlineCheckBox
            // 
            this.magicOnlineCheckBox.AutoSize = true;
            this.magicOnlineCheckBox.Location = new System.Drawing.Point(317, 66);
            this.magicOnlineCheckBox.Name = "magicOnlineCheckBox";
            this.magicOnlineCheckBox.Size = new System.Drawing.Size(107, 20);
            this.magicOnlineCheckBox.TabIndex = 9;
            this.magicOnlineCheckBox.Text = "Magic Online";
            this.magicOnlineCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.magicOnlineCheckBox);
            this.Controls.Add(this.arenaCheckBox);
            this.Controls.Add(this.paperCheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.defaultSearchLanguageComboBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.defaultCurrencyComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.ComboBox defaultCurrencyComboBox;
        public System.Windows.Forms.ComboBox defaultSearchLanguageComboBox;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox paperCheckBox;
        public System.Windows.Forms.CheckBox arenaCheckBox;
        public System.Windows.Forms.CheckBox magicOnlineCheckBox;
    }
}