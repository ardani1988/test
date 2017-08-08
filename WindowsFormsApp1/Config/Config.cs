using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MC_Sicherung.Config.Base;
using MC_Sicherung.Config.Interface;

namespace MC_Sicherung.Config
{
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

    public class User : ConfigPropertieBase
    {
        public string username { set; get; }
        public string kuerzel { set; get; }
        public bool active { set; get; }

        public User()
        {

        }

        /// <summary>
        /// This Constructor is used to initialize an Object out of the XML Representation
        /// </summary>
        /// <param name="node"></param>
        public User(XmlNode node)
        {
            FromXmlNode(node);
        }
    }

    #region ConfigClasses

    public class UserConfig : ConfigBase
    {
        #region Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<string> SpecialMapping = new List<string>();

        public string ClassNode { private set; get; } = "UserSettings";
        public List<User> users = new List<User>();

        #endregion Properties

        #region Constructor

        public UserConfig(string path)
        {
            log.Info("Entry");
            this._path = path;

            Dictionary<string, Delegate> tmpDic = new Dictionary<string, Delegate>();

            Func<string> getNodeName = () => "users/user";
            tmpDic.Add("NodeName", getNodeName);
            tmpDic.Add("set", new Action<XmlNodeList>(AddUsers));
            tmpDic.Add("get", new Func<XmlNodeList>(GetUsers));

            ListMethods.Add(tmpDic);

            initFieldMapping(SpecialMapping);
            log.Info("Exit");
        }

        #endregion Constructor

        #region Methods

        public void AddUsers(XmlNodeList List)
        {
            log.Info("Entry");
            foreach (XmlNode node in List)
            {
                users.Add(new User(node));
                log.DebugFormat("added user {0} with kurzel {1} to list. Actual Object Count {2}",
                    users[users.Count -1].username, users[users.Count - 1].kuerzel, users.Count);
            }   
            log.Info("Exit");
        }

        public void AddUser(User newUser)
        {
            log.Info("Entry");
            users.Add(newUser);
            log.DebugFormat("added user {0} with kurzel {1} to list. Actual Object Count {2}",
                users[users.Count - 1].username, users[users.Count - 1].kuerzel, users.Count);
            log.Info("Exit");
        }

        public XmlNodeList GetUsers()
        {
            log.Info("Entry");
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nSetting = xmlDoc.CreateElement("users");
            xmlDoc.AppendChild(nSetting);
        
            foreach (User user in users)
            {
                nSetting.AppendChild(xmlDoc.ImportNode(user.ToXml(), true));
            }
            log.Info("Exit");
            return xmlDoc.SelectNodes("/users/user");
        }

        #endregion Methods
    }

    public class ValidatorConfig : ConfigBase
    {
        //https://stackoverflow.com/questions/24329012/store-reference-to-an-object-in-dictionary
        //https://www.google.de/search?q=c%23+store+reference+variable+in+dictionary&rlz=1C1CHBD_deDE737DE737&oq=c%23+store+reference+variable+in+dictionary&aqs=chrome..69i57j69i58.28495j0j7&sourceid=chrome&ie=UTF-8

        #region Properties  

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ClassNode { private set; get; } = "ValidatorSettings";
        public bool checkIfDriveExists { set; get; } = true;

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
    }

    public class BackupConfig : ConfigBase
    {
        #region Properties 

        // If adding a Property, you have to add it to the copy Constructor
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ClassNode { private set; get; } = "BackupSettings";

        public string source { set; get; } = "C:\\";
        public string destination { set; get; } = "C:\\";
        public Ionic.Zlib.CompressionLevel level { set; get; } = Ionic.Zlib.CompressionLevel.Default;
        public string schema { set; get; } = "{O}_{DU}_{T}";
        public bool saveAsZIP { set; get; } = true;

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
    }

    public class ConfigManager
    {
        #region Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ValidatorConfig ValidatorSettings;
        public BackupConfig BackupSettings;
        public UserConfig UserSettings;

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
            this.UserSettings = new UserConfig("config.xml");

            // Add ValidatorConfig to Dictionary, this is nessesary for path -> Object Mapping
            this._ConfigObject.Add("BackupSettings", this.BackupSettings);
            this._ConfigObject.Add("ValidatorSettings", this.ValidatorSettings);
            this._ConfigObject.Add("UserSettings", this.UserSettings);
        }

        public ConfigManager(Dictionary<string, string> path)
        {
            log.Info("Entry");
            //log.Debug(String.Format("Params - path: {0}", path.ToString()));
            // Create new ValidatorConfig
            BackupSettings = new BackupConfig(path["BackupSettings"]);
            ValidatorSettings = new ValidatorConfig(path["ValidatorSettings"]);
            UserSettings = new UserConfig("config.xml");

            // Add ValidatorConfig to Dictionary, this is nessesary for path -> Object Mapping
            this._ConfigObject.Add("BackupSettings", this.BackupSettings);
            this._ConfigObject.Add("ValidatorSettings", this.ValidatorSettings);
            this._ConfigObject.Add("UserSettings", this.UserSettings);

            initXML();
            loadConfig();

            this.UserSettings.AddUser(new User() { username = "Hans Wurst", kuerzel = "HW" });

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

        /// <summary>
        /// this is an helper Method to Trigger the loadFromXML for a specific Object
        /// </summary>       
        private void loadConfig()
        {
            foreach (KeyValuePair<string, IConfig> entry in _ConfigObject)
            {
                loadFromXML(entry.Value);
            }
        }

        /// <summary>
        /// Method for Reading all Config Values of an specific Config Object
        /// </summary>
        /// <param name="cObject"></param>
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

            // This part is used for getting an XmlNodeList and give the List to an AddMethod of the Class
            // This is used to handle a List of Nodes
            List<Dictionary<string, Delegate>> listMethods = cObject.getListMethods();

            if (listMethods.Count != 0)
            {
                foreach (Dictionary<string, Delegate> listMethod in listMethods)
                {
                    if (listMethod.Count != 0)
                    {
                        string NodeName = (string)listMethod["NodeName"].DynamicInvoke();

                        XmlNodeList nlNode = nSetting.SelectNodes(NodeName);
                        listMethod["set"].DynamicInvoke(nlNode);
                    }
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

            // This part is used for getting an XmlNodeList and give the List to an AddMethod of the Class
            // This is used to handle a List of Nodes
            List<Dictionary<string, Delegate>> listMethods = cObject.getListMethods();

            if (listMethods.Count != 0)
            {
                foreach (Dictionary<string, Delegate> listMethod in listMethods)
                {
                    if (listMethod.Count != 0)
                    {
                        string NodeName = (string)listMethod["NodeName"].DynamicInvoke();

                        nNode = nSetting.SelectSingleNode(NodeName.Split('/')[0]);
                        if (nNode != null)
                            nNode.RemoveAll();

                        XmlNodeList tempList = (XmlNodeList)listMethod["get"].DynamicInvoke();

                        foreach (XmlNode node in tempList)
                            nNode.AppendChild(xDoc.ImportNode(node, true));

                        nSetting.AppendChild(nNode);
                    }
                }
            }
            xDoc.Save(path);
        }

        #endregion Methods

    }

    #endregion ConfigClasses
}