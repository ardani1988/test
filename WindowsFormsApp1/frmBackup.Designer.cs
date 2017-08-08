namespace MC_Sicherung
{
    partial class frmBackup
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
            this.pbDatei = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.bwBackup = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblActualFile = new System.Windows.Forms.Label();
            this.pbAll = new System.Windows.Forms.ProgressBar();
            this.lblStatusCount = new System.Windows.Forms.Label();
            this.tbHistory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pbDatei
            // 
            this.pbDatei.Location = new System.Drawing.Point(13, 71);
            this.pbDatei.Name = "pbDatei";
            this.pbDatei.Size = new System.Drawing.Size(575, 23);
            this.pbDatei.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // bwBackup
            // 
            this.bwBackup.WorkerSupportsCancellation = true;
            this.bwBackup.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwBackup_DoWork);
            this.bwBackup.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwBackup_ProgressChanged);
            this.bwBackup.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwBackup_RunWorkerCompleted);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(94, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblActualFile
            // 
            this.lblActualFile.AutoSize = true;
            this.lblActualFile.Location = new System.Drawing.Point(9, 97);
            this.lblActualFile.Name = "lblActualFile";
            this.lblActualFile.Size = new System.Drawing.Size(0, 13);
            this.lblActualFile.TabIndex = 3;
            // 
            // pbAll
            // 
            this.pbAll.Location = new System.Drawing.Point(12, 42);
            this.pbAll.Name = "pbAll";
            this.pbAll.Size = new System.Drawing.Size(575, 23);
            this.pbAll.TabIndex = 4;
            // 
            // lblStatusCount
            // 
            this.lblStatusCount.AutoSize = true;
            this.lblStatusCount.Location = new System.Drawing.Point(175, 17);
            this.lblStatusCount.Name = "lblStatusCount";
            this.lblStatusCount.Size = new System.Drawing.Size(0, 13);
            this.lblStatusCount.TabIndex = 5;
            // 
            // tbHistory
            // 
            this.tbHistory.Location = new System.Drawing.Point(12, 129);
            this.tbHistory.Multiline = true;
            this.tbHistory.Name = "tbHistory";
            this.tbHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbHistory.Size = new System.Drawing.Size(575, 266);
            this.tbHistory.TabIndex = 6;
            // 
            // frmBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 407);
            this.Controls.Add(this.tbHistory);
            this.Controls.Add(this.lblStatusCount);
            this.Controls.Add(this.pbAll);
            this.Controls.Add(this.lblActualFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pbDatei);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBackup";
            this.Text = "Backup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbDatei;
        private System.Windows.Forms.Button btnStart;
        private System.ComponentModel.BackgroundWorker bwBackup;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblActualFile;
        private System.Windows.Forms.ProgressBar pbAll;
        private System.Windows.Forms.Label lblStatusCount;
        private System.Windows.Forms.TextBox tbHistory;
    }
}