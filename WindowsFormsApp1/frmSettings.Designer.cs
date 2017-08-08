namespace MC_Sicherung
{
    partial class frmSettings
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
            this.lblSource = new System.Windows.Forms.Label();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.fbdSource = new System.Windows.Forms.FolderBrowserDialog();
            this.btnfbdSource = new System.Windows.Forms.Button();
            this.lblDestination = new System.Windows.Forms.Label();
            this.tbDestination = new System.Windows.Forms.TextBox();
            this.btnfbdDestination = new System.Windows.Forms.Button();
            this.cbLevel = new System.Windows.Forms.ComboBox();
            this.lblLevel = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDName = new System.Windows.Forms.Label();
            this.tbDName = new System.Windows.Forms.TextBox();
            this.lblExample = new System.Windows.Forms.Label();
            this.cbSaveAsZIP = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(12, 20);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(61, 13);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "Quellordner";
            // 
            // tbSource
            // 
            this.tbSource.Location = new System.Drawing.Point(89, 13);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(335, 20);
            this.tbSource.TabIndex = 1;
            this.tbSource.Validating += new System.ComponentModel.CancelEventHandler(this.tbSource_Validating);
            // 
            // btnfbdSource
            // 
            this.btnfbdSource.Location = new System.Drawing.Point(430, 8);
            this.btnfbdSource.Name = "btnfbdSource";
            this.btnfbdSource.Size = new System.Drawing.Size(25, 23);
            this.btnfbdSource.TabIndex = 2;
            this.btnfbdSource.Text = "...";
            this.btnfbdSource.UseVisualStyleBackColor = true;
            this.btnfbdSource.Click += new System.EventHandler(this.btnfbdSource_Click);
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(12, 55);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(54, 13);
            this.lblDestination.TabIndex = 3;
            this.lblDestination.Text = "Zielordner";
            // 
            // tbDestination
            // 
            this.tbDestination.Location = new System.Drawing.Point(89, 48);
            this.tbDestination.Name = "tbDestination";
            this.tbDestination.Size = new System.Drawing.Size(335, 20);
            this.tbDestination.TabIndex = 4;
            this.tbDestination.Validating += new System.ComponentModel.CancelEventHandler(this.tbDestination_Validating);
            // 
            // btnfbdDestination
            // 
            this.btnfbdDestination.Location = new System.Drawing.Point(430, 43);
            this.btnfbdDestination.Name = "btnfbdDestination";
            this.btnfbdDestination.Size = new System.Drawing.Size(25, 23);
            this.btnfbdDestination.TabIndex = 5;
            this.btnfbdDestination.Text = "...";
            this.btnfbdDestination.UseVisualStyleBackColor = true;
            this.btnfbdDestination.Click += new System.EventHandler(this.btnfbdDestination_Click);
            // 
            // cbLevel
            // 
            this.cbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLevel.FormattingEnabled = true;
            this.cbLevel.Items.AddRange(new object[] {
            "None",
            "BestSpeed",
            "Level2",
            "Level3",
            "Level4",
            "Level5",
            "Default",
            "Level7",
            "Level8",
            "BestCompression"});
            this.cbLevel.Location = new System.Drawing.Point(89, 86);
            this.cbLevel.Name = "cbLevel";
            this.cbLevel.Size = new System.Drawing.Size(121, 21);
            this.cbLevel.TabIndex = 6;
            this.cbLevel.SelectedIndexChanged += new System.EventHandler(this.cbLevel_SelectedIndexChanged);
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(12, 94);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(67, 13);
            this.lblLevel.TabIndex = 7;
            this.lblLevel.Text = "Compression";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(297, 248);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Speichern";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(379, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.btnCancel_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.btnCancel_MouseLeave);
            // 
            // lblDName
            // 
            this.lblDName.AutoSize = true;
            this.lblDName.Location = new System.Drawing.Point(12, 129);
            this.lblDName.Name = "lblDName";
            this.lblDName.Size = new System.Drawing.Size(58, 13);
            this.lblDName.TabIndex = 10;
            this.lblDName.Text = "Dateiname";
            // 
            // tbDName
            // 
            this.tbDName.Location = new System.Drawing.Point(89, 122);
            this.tbDName.Name = "tbDName";
            this.tbDName.Size = new System.Drawing.Size(335, 20);
            this.tbDName.TabIndex = 11;
            this.tbDName.TextChanged += new System.EventHandler(this.tbDName_TextChanged);
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.Location = new System.Drawing.Point(86, 154);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(53, 13);
            this.lblExample.TabIndex = 12;
            this.lblExample.Text = "Example: ";
            // 
            // cbSaveAsZIP
            // 
            this.cbSaveAsZIP.AutoSize = true;
            this.cbSaveAsZIP.Location = new System.Drawing.Point(240, 89);
            this.cbSaveAsZIP.Name = "cbSaveAsZIP";
            this.cbSaveAsZIP.Size = new System.Drawing.Size(113, 17);
            this.cbSaveAsZIP.TabIndex = 13;
            this.cbSaveAsZIP.Text = "Als Zip speichern?";
            this.cbSaveAsZIP.UseVisualStyleBackColor = true;
            this.cbSaveAsZIP.CheckedChanged += new System.EventHandler(this.cbSaveAsZIP_CheckedChanged);
            // 
            // frmSettings
            // 
            this.ClientSize = new System.Drawing.Size(466, 283);
            this.Controls.Add(this.cbSaveAsZIP);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.tbDName);
            this.Controls.Add(this.lblDName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.cbLevel);
            this.Controls.Add(this.btnfbdDestination);
            this.Controls.Add(this.tbDestination);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.btnfbdSource);
            this.Controls.Add(this.tbSource);
            this.Controls.Add(this.lblSource);
            this.Name = "frmSettings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSettings_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.FolderBrowserDialog fbdSource;
        private System.Windows.Forms.Button btnfbdSource;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.TextBox tbDestination;
        private System.Windows.Forms.Button btnfbdDestination;
        private System.Windows.Forms.ComboBox cbLevel;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDName;
        private System.Windows.Forms.TextBox tbDName;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.CheckBox cbSaveAsZIP;
    }
}