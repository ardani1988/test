using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Xml;

namespace MC_Sicherung.Helper
{
    #region interfaces

    interface IConfig
    {
        string getPath();
        Dictionary<string, VariableReference> getFieldMapping();
    }

    #endregion interfaces

    #region Helper

    public class VariableReference
    {
        public Func<object> Get { get; private set; }
        public Action<object> Set { get; private set; }

        public VariableReference(Func<object> getter, Action<object> setter)
        {
            Get = getter;
            Set = setter;
        }
    }

    #endregion Helper

    #region ConfigClasses

    public class ValidatorConfig : IConfig
    {
        //https://stackoverflow.com/questions/24329012/store-reference-to-an-object-in-dictionary
        //https://www.google.de/search?q=c%23+store+reference+variable+in+dictionary&rlz=1C1CHBD_deDE737DE737&oq=c%23+store+reference+variable+in+dictionary&aqs=chrome..69i57j69i58.28495j0j7&sourceid=chrome&ie=UTF-8

        #region Properties  

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, VariableReference> FieldMapping = new Dictionary<string, VariableReference>();
        private string _path;

        public string ClassNode { private set; get; } = "ValidatorSettings";
        public bool checkIfDriveExists { set; get; } = true;

        private void initFieldMapping()
        {
            PropertyInfo[] properties = typeof(ValidatorConfig).GetProperties();
            log.Debug(properties);

            foreach (PropertyInfo propertie in properties)
            {
                FieldMapping.Add(propertie.Name, new VariableReference(
                    () => propertie.GetValue(this),
                    ActionConverter.getAction(this, propertie.PropertyType.ToString(), propertie)
                    ));
                log.DebugFormat("added {0} to FieldMapping", propertie.Name);
            }
        }

        #endregion Properties

        #region Constructor

        public ValidatorConfig(ValidatorConfig other)
        {
            log.Info("Entry - with Object");
            this._path = other._path;
            this.ClassNode = other.ClassNode;
            this.checkIfDriveExists = other.checkIfDriveExists;
            initFieldMapping();
            log.Info("Exit");
        }

        public ValidatorConfig(string path)
        {
            log.Info("Entry");
            this._path = path;
            initFieldMapping();
            log.Info("Exit");
        }

        #endregion Constructor

        #region Methods

        public string getPath()
        {
            return this._path;
        }

        public Dictionary<string, VariableReference> getFieldMapping()
        {
            return this.FieldMapping;
        }

        #endregion Methods
    }

    public class BackupConfig : IConfig
    {
        #region Properties 

        // If adding a Property, you have to add it to the copy Constructor
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, VariableReference> FieldMapping = new Dictionary<string, VariableReference>();
        private string _path;

        public string ClassNode { private set; get; } = "BackupSettings";

        public string source { set; get; } = "C:\\";
        public string destination { set; get; } = "C:\\";
        public Ionic.Zlib.CompressionLevel level { set; get; } = Ionic.Zlib.CompressionLevel.Default;
        public string schema { set; get; } = "{O}_{DU}_{T}";
        public bool saveAsZIP { set; get; } = true;

        private void initFieldMapping()
        {
            PropertyInfo[] properties = typeof(BackupConfig).GetProperties();
            log.Debug(properties);

            foreach (PropertyInfo propertie in properties)
            {
                FieldMapping.Add(propertie.Name, new VariableReference(
                    () => propertie.GetValue(this),
                    ActionConverter.getAction(this, propertie.PropertyType.ToString(), propertie)
                    ));
                log.DebugFormat("added {0} to FieldMapping", propertie.Name);
            }
        }

        #endregion Properties

        #region Constructor

        public BackupConfig(BackupConfig other)
        {
            log.Info("Entry - with Object");
            this._path = other._path;
            this.ClassNode = other.ClassNode;
            this.source = other.source;
            this.destination = other.destination;
            this.level = other.level;
            this.schema = other.schema;
            this.saveAsZIP = other.saveAsZIP;
            initFieldMapping();
            log.Info("Exit");
        }

        public BackupConfig(string path)
        {
            log.Info("Entry");
            this._path = path;
            initFieldMapping();
            log.Info("Exit");
        }

        #endregion Constructor

        #region Methods

        public string getPath()
        {
            return this._path;
        }

        public Dictionary<string, VariableReference> getFieldMapping()
        {
            return this.FieldMapping;
        }

        #endregion Methods

    }

    public class ConfigManager
    {
        #region Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ValidatorConfig ValidatorSettings;
        public BackupConfig BackupSettings;

        private Dictionary<string, IConfig> _ConfigObject = new Dictionary<string, IConfig>();
        private delegate string PathHandler();

        #endregion // Properties

        #region Constructor

        // https://msdn.microsoft.com/de-de/library/ms173116(VS.80).aspx
        public ConfigManager(ConfigManager other)
        {
            // Create new ValidatorConfig
            this.BackupSettings = new BackupConfig(other.BackupSettings);
            this.ValidatorSettings = new ValidatorConfig(other.ValidatorSettings);

            // Add ValidatorConfig to Dictionary, this is nessesary for path -> Object Mapping
            this._ConfigObject.Add("BackupSettings", this.BackupSettings);
            this._ConfigObject.Add("ValidatorSettings", this.ValidatorSettings);
        }

        public ConfigManager(Dictionary<string, string> path)
        {
            log.Info("Entry");
            //log.Debug(String.Format("Params - path: {0}", path.ToString()));
            // Create new ValidatorConfig
            BackupSettings = new BackupConfig(path["BackupSettings"]);
            ValidatorSettings = new ValidatorConfig(path["ValidatorSettings"]);

            // Add ValidatorConfig to Dictionary, this is nessesary for path -> Object Mapping
            this._ConfigObject.Add("BackupSettings", this.BackupSettings);
            this._ConfigObject.Add("ValidatorSettings", this.ValidatorSettings);

            initXML();
            loadConfig();
            log.Info("Exit");
        }

        #endregion // Constructor

        # region Methods

        private void initXML()
        {
            log.Info("Entry");
            foreach (KeyValuePair<string, IConfig> entry in _ConfigObject)
            {
                string path = entry.Value.getPath();
                bool changed = false;

                if (!File.Exists(path))
                {
                    log.Warn(String.Format("Die Datei '{0}' wurde nicht gefunden. Die Datei wird angelegt", path));

                    XmlDocument xDoc = new XmlDocument();

                    XmlNode xnDoc = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xDoc.AppendChild(xnDoc); 

                    XmlNode nSettings = xDoc.CreateElement("Settings");
                    xDoc.AppendChild(nSettings); 

                    XmlNode nSetting = xDoc.CreateElement(entry.Key);
                    nSettings.AppendChild(nSetting);

                    XmlNode nNode;
                    foreach (KeyValuePair<string, VariableReference> kvSet in entry.Value.getFieldMapping())
                    {
                        if (kvSet.Key != "ClassNode")
                        {
                            nNode = xDoc.CreateElement(kvSet.Key);
                            nNode.InnerText = kvSet.Value.Get().ToString();
                            nSetting.AppendChild(nNode);
                        }
                    }
                    try
                    {
                        xDoc.Save(path);
                    }
                    catch (XmlException ex)
                    {
                        log.Error("An Erroroccured while Saving", ex);
                    }
                }
                else
                {
                    log.InfoFormat("Die Konfigurationsdatei {0} wurde gefunden. Lade {1}", path, entry.Value.getFieldMapping()["ClassNode"].Get());

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(path);

                    XmlNode nSettings = xDoc.SelectSingleNode("/Settings");

                    XmlNode nSetting = nSettings.SelectSingleNode(entry.Key);
                    if (nSetting == null)
                    {
                        nSetting = xDoc.CreateElement(entry.Key);
                        nSettings.AppendChild(nSetting);
                        changed = true;
                    }

                    XmlNode nNode;
                    foreach (KeyValuePair<string, VariableReference> kvSet in entry.Value.getFieldMapping())
                    {
                        if (kvSet.Key != "ClassNode")
                        {
                            nNode = nSetting.SelectSingleNode(kvSet.Key);
                            if (nNode == null)
                            {
                                nNode = xDoc.CreateElement(kvSet.Key);
                                nNode.InnerText = kvSet.Value.Get().ToString();
                                nSetting.AppendChild(nNode);
                                changed = true;

                                nNode = null;
                            }
                        }
                    }
                    if (changed)
                    {
                        try
                        {
                            xDoc.Save(path);
                        }
                        catch (XmlException ex)
                        {
                            log.Error("An Erroroccured while Saving", ex);
                        }
                    }
                }
            }
            log.Debug("Exit");
        }

        private void loadConfig()
        {
            foreach (KeyValuePair<string, IConfig> entry in _ConfigObject)
            {
                loadFromXML(entry.Value);
            }
        }

        private void loadFromXML(IConfig cObject)
        {
            string path = cObject.getPath();
            Dictionary<string, VariableReference> FieldMapping = cObject.getFieldMapping();
            Dictionary<string, VariableReference> FieldMap = new Dictionary<string, VariableReference>(FieldMapping);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            XmlNode nSetting = xDoc.SelectSingleNode("/Settings/" + FieldMapping["ClassNode"].Get().ToString());

            XmlNode nNode;

            foreach (KeyValuePair<string, VariableReference> kvSet in FieldMap)
            {
                if (kvSet.Key != "ClassNode")
                {
                    nNode = nSetting.SelectSingleNode(kvSet.Key);
                    FieldMapping[kvSet.Key].Set(nNode.InnerText);
                }
            }
        }

        public bool saveConfig()
        {
            foreach (KeyValuePair<string, IConfig> entry in _ConfigObject)
            {
                saveToXML(entry.Value);
            }
            return true;
        }

        public bool saveConfig(string Type)
        {
            saveToXML(_ConfigObject[Type]);
            return true;
        }

        private void saveToXML(IConfig cObject)
        {
            string path = cObject.getPath();
            Dictionary<string, VariableReference> FieldMapping = cObject.getFieldMapping();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            XmlNode nSetting = xDoc.SelectSingleNode("/Settings/" + FieldMapping["ClassNode"].Get().ToString());

            XmlNode nNode;
            foreach (KeyValuePair<string, VariableReference> kvSet in FieldMapping)
            {
                if (kvSet.Key != "ClassNode")
                {
                    nNode = nSetting.SelectSingleNode(kvSet.Key);
                    if (nNode != null)
                    {
                        nNode.InnerText = kvSet.Value.Get().ToString();

                        nNode = null;
                    }
                    else
                        nSetting.AppendChild(nNode);

                }
            }
            xDoc.Save(path);

        }

        #endregion Methods

    }

    #endregion ConfigClasses
}