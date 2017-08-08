using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MC_Sicherung.Helper
{
    interface IValidator
    {
        bool Validate(object sender, string data);
    }

    class FormValidator : IValidator
    {
        private Dictionary<string, IValidator> dVal = new Dictionary<string, IValidator>();
        private delegate bool ValidationHandler(object sender, string data);
        private string[] canHaveValidator = new string[] { "System.Windows.Forms.TextBox" };

        public void registerValidationHandler(object sender, IValidator Validator)
        {
            try
            {
                dVal.Add(ComponentAndText(sender)["Name"], Validator);
            }
            catch
            {
                throw;
            }

        }

        private Dictionary<string, string> ComponentAndText(object sender)
        {
            Dictionary<string, string> component = new Dictionary<string, string>();

            switch (sender.GetType().ToString())
            {
                case "System.Windows.Forms.TextBox":
                    TextBox tbTemp = (TextBox)sender;
                    component.Add("Name", tbTemp.Name);
                    component.Add("Text", tbTemp.Text);
                    break;
                    //case "System.Windows.Forms.Label":
                    //    Label lbTemp = (Label)sender;
                    //    component.Add("Name", lbTemp.Name);
                    //    component.Add("Text", lbTemp.Text);
                    //    break;
            }

            return component;
        }

        public bool hasValidator(string sender)
        {
            return dVal.ContainsKey(sender);
        }

        public IValidator DispatchValidator(string sender)
        {
            return dVal[sender];
        }

        public bool Validate(object sender)
        {
            Dictionary<string, string> sComATxt = ComponentAndText(sender);

            if ((Array.Exists(canHaveValidator, element => element == sender.GetType().ToString()))
                        && (hasValidator(sComATxt["Name"])))
            {
                ValidationHandler methodCall = DispatchValidator(sComATxt["Name"]).Validate;
                bool temp = methodCall(sender, sComATxt["Text"]);
                return temp;
            }
            else
                return true;
        }

        public bool Validate(object sender, string data)
        {
            Dictionary<string, string> sComATxt = ComponentAndText(sender);

            if ((Array.Exists(canHaveValidator, element => element == sender.GetType().ToString()))
                        && (hasValidator(sComATxt["Name"])))
            {
                ValidationHandler methodCall = DispatchValidator(sComATxt["Name"]).Validate;
                bool temp = methodCall(sender, data);
                return temp;
            }
            else
                return true;
        }

        public bool ValidateForm(System.Windows.Forms.Control.ControlCollection controls)
        {
            bool bValid = true;

            foreach (Control control in controls)
            {
                // Validate one Single Control
                bool validControl = Validate(control);

                // If the Control isn't valid set the Flag to true;
                if (!validControl)
                    bValid = false;

                // check if control is an Container and then do it recursive
                if (control.HasChildren)
                {
                    if (ValidateForm(control.Controls))
                        bValid = false;
                }
            }
            return bValid;
        }

    }

    class PathValidator : IValidator
    {
        private bool checkIfDriveExists;

        public PathValidator(bool checkIfDriveExists = true)
        {
            this.checkIfDriveExists = checkIfDriveExists;
        }
        
        //https://stackoverflow.com/questions/6198392/check-whether-a-path-is-valid
        public bool Validate(object sender, string path)
        {
            Regex drive = new Regex(@"^[a-zA-Z]:\\$");

            if (string.IsNullOrWhiteSpace(path) || path.Length < 3) return false;
            if (!drive.IsMatch(path.Substring(0, 3))) return false;

            string sSub = path.Substring(3, path.Length - 3);
            string sInvalidChars = new string(Path.GetInvalidPathChars());
            sInvalidChars += @":?*";
            Regex rInvalidChars = new Regex("[" + Regex.Escape(sInvalidChars) + "]");

            if (rInvalidChars.IsMatch(sSub)) return false;

            if (this.checkIfDriveExists)
            {
                var driveLetter = Path.GetPathRoot(path);

                if (!DriveInfo.GetDrives().Any(x => x.Name == driveLetter.ToUpper())) return false;
            }

            return true;
        }
    }

    class PlaceholderValidator : IValidator
    {
        public bool Validate(object sender, string data)
        {
            return true;
        }

    }
    /* 
     - umstellen auf Formular Validatoren
     - Felder & FeldTypen werden registriert
     - Methode Invoke prüft welches Feld und führt die entsprechende Methode aus
     - Rückgabe immer als bool

     MessageBox.Show(sender.GetType().ToString());
     TextBox lblTest = (TextBox)sender;
     MessageBox.Show(lblTest.Name);
     
     */
}