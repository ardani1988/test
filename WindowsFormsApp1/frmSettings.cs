using System;
using System.ComponentModel;
using System.Windows.Forms;
using MC_Sicherung.Helper;
using MC_Sicherung.Config;
using MC_Sicherung.Validator;

namespace MC_Sicherung
{
    public partial class frmSettings : Form
    {
        #region GlobalVariables

        ConfigManager conf;
        FormValidator fvVal = new FormValidator();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool bCancel = false;

        #endregion GlobalVariables

        #region InitAndConstructor

        public frmSettings(ConfigManager conf)
        {
            log.Info("Entry");
            InitializeComponent();

            log.Debug("registering ValidationHandler for fields");
            fvVal.registerValidationHandler(tbSource, new PathValidator(conf.ValidatorSettings.checkIfDriveExists));
            fvVal.registerValidationHandler(tbDestination, new PathValidator(conf.ValidatorSettings.checkIfDriveExists));

            this.conf = conf;
            this.conf.takeSnapshot();

            // call initSettings to initialize the Controls
            initSettings();
            log.Info("Exit");
        }

        public void initSettings()
        {
            log.Info("Entry");

            tbSource.Text = this.conf.BackupSettings.source;
            tbDestination.Text = this.conf.BackupSettings.destination;
            cbLevel.SelectedItem = Converter.TypeConverter.CompressionLevelToString(this.conf.BackupSettings.level);
            tbDName.Text = this.conf.BackupSettings.schema;
            cbSaveAsZIP.Checked = this.conf.BackupSettings.saveAsZIP;
         
            log.Info("Exit");
        }

        #endregion InitAndConstructor

        #region SelectPath

        private void btnfbdSource_Click(object sender, EventArgs e)
        {
            log.Info("Entry");
            fbdSource.ShowNewFolderButton = true;
            if (fbdSource.ShowDialog() == DialogResult.OK)
            {
                log.Debug(String.Format("Path '{0}' selected", fbdSource.SelectedPath));
                tbSource.Text = fbdSource.SelectedPath;
                if (fvVal.Validate(sender))
                {
                    conf.BackupSettings.source = tbSource.Text;
                }
            }
            log.Info("Exit");
        }

        private void btnfbdDestination_Click(object sender, EventArgs e)
        {
            log.Info("Entry");
            fbdSource.ShowNewFolderButton = true;
            if (fbdSource.ShowDialog() == DialogResult.OK)
            {
                log.Debug(String.Format("Path '{0}' selected", fbdSource.SelectedPath));
                tbDestination.Text = fbdSource.SelectedPath;
                if (fvVal.Validate(sender))
                {
                    conf.BackupSettings.destination = tbDestination.Text;
                }
            }
            log.Info("Exit");
        }

        #endregion SelectPath

        #region PropertyChanged

        private void cbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            conf.BackupSettings.level = Converter.TypeConverter.ToCompressionLevel(cbLevel.SelectedItem.ToString());
        }

        private void tbSource_Validating(object sender, CancelEventArgs e)
        {
            if (conf.BackupSettings.source != tbSource.Text)
            {
                if (fvVal.Validate(sender))
                {
                    conf.BackupSettings.source = tbSource.Text;
                }
                else
                {
                    if (!bCancel)
                        e.Cancel = true;
                }
            }
        }

        private void tbDestination_Validating(object sender, CancelEventArgs e)
        {
            if (conf.BackupSettings.destination != tbDestination.Text)
            {
                if (fvVal.Validate(sender))
                {
                    conf.BackupSettings.destination = tbDestination.Text;
                }
                else
                {
                    if (!bCancel)
                        e.Cancel = true;
                }
            }
        }

        private void tbDName_TextChanged(object sender, EventArgs e)
        {
            lblExample.Text = String.Format("Example: {0}", Placeholder.replace(tbDName.Text, conf.BackupSettings.source));
            //ToDo: Validierung: https://de.wikipedia.org/wiki/Dateiname && https://msdn.microsoft.com/de-de/library/twcw2f1c(v=vs.110).aspx
        }

        private void cbSaveAsZIP_CheckedChanged(object sender, EventArgs e)
        {
            conf.BackupSettings.saveAsZIP = cbSaveAsZIP.Checked;
        }

        #endregion PropertyChanged

        #region MethodsForSavingOrClosing

        private bool IsSaveable(Control.ControlCollection controls)
        {
            return fvVal.ValidateForm(controls);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Info("Entry");
            if (IsSaveable(this.Controls))
            {
                if (this.conf.hasChanged())
                {
                    DialogResult dResult = MessageBox.Show("Soll die Konfiguration gespeichert werden?", "Speichern?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dResult == DialogResult.Yes)
                        if (!this.conf.saveConfig())
                        {
                            MessageBox.Show("Die Konfiguration konnte nicht gespeichert werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (this.conf.hasSnapshot)
                                this.conf.deleteSnapshot();
                            log.Debug("Config was saved to file.");
                            log.Info("Exit");
                            this.Close();
                        }
                }
                else
                {
                    log.Debug("No Changes made, config was not saved.");
                    MessageBox.Show("Es wurden keine Änderungen vorgenommen.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    log.Info("Exit");
                    this.Close();
                }
            }
            else
            {
                // throw Validation Error (?)
                MessageBox.Show("Das Formular kann nicht gespeichert werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                log.Info("Exit"); 
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Info("Formular Canceled");
            this.Close();
        }

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.conf.hasSnapshot && this.conf.hasChanged())
            {
                DialogResult dResult = MessageBox.Show("Es gibt ungespeicherte Änderungen. Sollen diese verworfen werden?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dResult == DialogResult.Yes)
                {
                    if (this.conf.hasSnapshot)
                        this.conf.revertSnapshot();
                }
                else
                {
                    e.Cancel = true;
                }

            }
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            bCancel = true;
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            bCancel = false;
        }
    }
    #endregion MethodsForSavingOrClosing
}
