using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MC_Sicherung.Config;

namespace MC_Sicherung
{
    public partial class Form1 : Form
    {
        // https://csharp.today/log4net-tutorial-great-library-for-logging/
        // https://www.eyecatch.no/blog/logging-with-log4net-in-c/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string ConfigPath = "config.xml";

        ConfigManager conf;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log.Info("Entry");

            Dictionary<string, string> path = new Dictionary<string, string>();

            path.Add("ValidatorSettings", "config.xml");
            path.Add("BackupSettings", "config.xml");

            conf = new ConfigManager(path);
            log.Info("Exit");
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
        frmSettings fSettings = new frmSettings(conf);
        fSettings.ShowDialog();
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBackup fBackup = new frmBackup(conf);
            fBackup.Show();
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
