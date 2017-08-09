using System;
using Ionic.Zip;
using System.ComponentModel;
using System.Windows.Forms;
using MC_Sicherung.Helper;
using MC_Sicherung.Config;

namespace MC_Sicherung
{
    public partial class frmBackup : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private delegate void UpdateFile(string data);
        private delegate void UpdateTextBox(string data);
        private delegate void UpdateFileProgress(int data);
        private delegate void UpdateStatusCount(int data, int data2);

        private UpdateFile updateFile;
        private UpdateTextBox updateTextBox;
        private UpdateFileProgress updateFileProgress;
        private UpdateStatusCount updateStatusCount;


        ConfigManager conf;


        public frmBackup()
        {
            log.Info("Entry");
            InitializeComponent();
            //this.conf = new ConfigManager(conf);
            this.conf = ConfigManager.GetInstance();
            this.Text += String.Format(" {0} -> {1}\\{2}.zip", this.conf.BackupSettings.source, 
                this.conf.BackupSettings.destination, Helper.Placeholder.replace(this.conf.BackupSettings.schema, this.conf.BackupSettings.source));
            updateFileProgress = UpdateProgress;
            updateStatusCount = UpdatelblStatusCount;
            updateFile = UpdatelblActualFile;
            updateTextBox = UpdatetbHistory;
            log.Info("Exit");
        }


        public void UpdatetbHistory(string message)
        {
            tbHistory.AppendText(String.Format("{0:HH:mm:ss} - {1}"+ Environment.NewLine, DateTime.Now, message));
        }

        public void UpdatelblActualFile(string Filename)
        {
            lblActualFile.Text = Filename;
        }

        public void UpdatelblStatusCount(int saved, int total)
        {
            lblStatusCount.Text = "(" + saved + "/" + total + ")";
            pbAll.Maximum = total;
            pbAll.Value = saved;

        }

        public void UpdateProgress(int data)
        { 
            pbDatei.Value = data;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            log.Info("Entry");
            log.Info(String.Format("Backup Started params - source: {0}, destination: {1}, compression: {2}",
                                   conf.BackupSettings.source,
                                   conf.BackupSettings.destination,
                                   Converter.TypeConverter.CompressionLevelToString(conf.BackupSettings.level)
                                   ));
            //MessageBox.Show(conf.BackupSettings.destination + "\\" + Placeholder.replace(conf.BackupSettings.schema, conf.BackupSettings.source) + ".zip");
            bwBackup.RunWorkerAsync();
            btnStart.Enabled = false;
            log.Info("Exit");
        }

        //https://www.whitebyte.info/programming/saveprogress-example-for-dotnetzip
        public void SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Saving_Started)
            {
                log.Info("Begin Saving: " + e.ArchiveName);
                this.Invoke(updateTextBox, "Begin Saving: " + e.ArchiveName);
            }
            else if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
            {
                this.Invoke(updateStatusCount, e.EntriesSaved + 1, e.EntriesTotal);
                this.Invoke(updateFile, e.CurrentEntry.FileName);
                this.Invoke(updateTextBox, conf.BackupSettings.source +"\\"+ e.CurrentEntry.FileName);
                log.DebugFormat("Saving File {0} ({1}/{2})", e.CurrentEntry.FileName, e.EntriesSaved + 1, e.EntriesTotal);

            }
            else if (e.EventType == ZipProgressEventType.Saving_EntryBytesRead)
            {
                this.Invoke(updateFileProgress, (int)((e.BytesTransferred * 100) / e.TotalBytesToTransfer));
            }
            else if (e.EventType == ZipProgressEventType.Saving_Completed)
            {
                log.Info("Finished Saving: " + e.ArchiveName);
                this.Invoke(updateTextBox, "Finished Saving: " + e.ArchiveName);
                this.Invoke(updateFile, "");
            }
        }

        private void bwBackup_DoWork(object sender, DoWorkEventArgs e)
        {
            log.Info("Entry");
            using (ZipFile zip = new ZipFile())
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Default;
                zip.SaveProgress += SaveProgress;

                zip.AddDirectory(conf.BackupSettings.source);
                zip.Save(conf.BackupSettings.destination + "\\" + Placeholder.replace(conf.BackupSettings.schema, conf.BackupSettings.source) + ".zip");
            }
            log.Info("Exit");
        }

        private void bwBackup_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bwBackup_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                log.Warn("Backup canceled through user input");
            }
            else if (e.Error != null)
            {
                log.Error("Error while performing background operation.");
            }
            else
            {
                log.Info("Backup Completed...");
            }
            btnStart.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Info("Entry");
            bwBackup.CancelAsync();
            log.Info("Exit");
        }
    }
}
