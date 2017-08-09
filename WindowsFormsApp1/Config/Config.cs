using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MC_Sicherung.Config.Base;
using MC_Sicherung.Config.Interface;
using System.Reflection;
using System.Linq;

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

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<string> SpecialMapping = new List<string>();

        public string ClassNode { private set; get; } = "UserSettings";
        public List<User> users = new List<User>();

        #endregion Properties

        #region Constructor

        public UserConfig(UserConfig other) : this(other._path)
        {
            log.Info("Entry - with Object");
            copyWithObject(other);

            FieldInfo[] fields = other.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                var tempList = (List<User>)field.GetValue(other);
                foreach (User entry in tempList)
                {
                    AddUserFromUser(new User(entry.ToXml()));
                }
            }
            log.Info("Exit - with Object");
        }

        public UserConfig(string path)
        {
            log.Info("Entry");
            this._path = path;

            initListMethods();
            initFieldMapping(SpecialMapping);
            log.Info("Exit");
        }

        #endregion Constructor

        #region Methods

        private void initListMethods()
        {
            Dictionary<string, Delegate> tmpDic = new Dictionary<string, Delegate>();

            Func<string> getNodeName = () => "users/user";
            tmpDic.Add("NodeName", getNodeName);
            tmpDic.Add("set", new Action<XmlNodeList>(AddUsersFromXmlNodeList));
            tmpDic.Add("get", new Func<XmlNodeList>(GetUsersAsXmlNodeList));
            tmpDic.Add("setFromList", new Action<List<User>, bool>(AddUsersFromList));
            tmpDic.Add("getAsList", new Func<List<User>>(GetUsersAsList));

            ListMethods.Add(tmpDic);
        }

        public void AddUsersFromList(List<User> userList, bool add = false)
        {
            if (add)
            {
                users.AddRange(userList);
            }
            else
            {
                users.Clear();
                foreach (User user in userList)
                {
                    users.Add(new User(user.ToXml()));
                }
            }
        }

        public void AddUsersFromXmlNodeList(XmlNodeList List)
        {
            log.Info("Entry");
            foreach (XmlNode node in List)
            {
                users.Add(new User(node));
                log.DebugFormat("added user {0} with kurzel {1} to list. Actual Object Count {2}",
                    users[users.Count - 1].username, users[users.Count - 1].kuerzel, users.Count);
            }
            log.Info("Exit");
        }

        public void AddUserFromUser(User newUser)
        {
            log.Info("Entry");
            users.Add(newUser);
            log.DebugFormat("added user {0} with kurzel {1} to list. Actual Object Count {2}",
                users[users.Count - 1].username, users[users.Count - 1].kuerzel, users.Count);
            log.Info("Exit");
        }

        public List<User> GetUsersAsList()
        {
            return users;
        }

        public XmlNodeList GetUsersAsXmlNodeList()
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

        public override int getHashCode()
        {
            //log.Info("Entry");
            int hash = base.getHashCode();

            FieldInfo[] fields = this.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                var tempList = (List<User>)field.GetValue(this);
                foreach (User entry in tempList)
                {
                    hash += entry.getHashCode();
                }
            }
            //log.InfoFormat("Exit - with value {0}", hash.ToString());
            return hash;
        }

        #endregion Methods
    }

    public class ValidatorConfig : ConfigBase
    {
        //https://stackoverflow.com/questions/24329012/store-reference-to-an-object-in-dictionary
        //https://www.google.de/search?q=c%23+store+reference+variable+in+dictionary&rlz=1C1CHBD_deDE737DE737&oq=c%23+store+reference+variable+in+dictionary&aqs=chrome..69i57j69i58.28495j0j7&sourceid=chrome&ie=UTF-8

        #region Properties  

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ClassNode { private set; get; } = "ValidatorSettings";
        public bool checkIfDriveExists { set; get; } = true;

        #endregion Properties

        #region Constructor

        public ValidatorConfig(ValidatorConfig other)
        {
            log.Info("Entry - with Object");
            this._path = other._path;
            copyWithObject(other);
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

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            copyWithObject(other);
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

    public sealed class ConfigManager
    {
        #region Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ValidatorConfig ValidatorSettings;
        public BackupConfig BackupSettings;
        public UserConfig UserSettings;

        private Dictionary<string, IConfig> TempConfigObject;
        private Dictionary<string, IConfig> _ConfigObject = new Dictionary<string, IConfig>();
        private delegate string PathHandler();

        public bool hasSnapshot { get => (TempConfigObject != null); }

        #endregion // Properties

        #region Singleton

        private static ConfigManager _instance = null;
        private static object _mutex = new Object();

        public static ConfigManager GetInstance(Dictionary<string, string> path)
        {
            if (_instance == null)
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigManager(path);
                    }
                }
            }
            return _instance;
        }

        public static ConfigManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigManager();
                    }
                }
            }
            return _instance;
        }

        #endregion Singleton

        #region Constructor

        private ConfigManager() { }

        private ConfigManager(Dictionary<string, string> path)
        {
            log.Info("Entry");
            //log.Debug(String.Format("Params - path: {0}", path.ToString()));
            // Create new Config of types
            BackupSettings = new BackupConfig(path["BackupSettings"]);
            ValidatorSettings = new ValidatorConfig(path["ValidatorSettings"]);
            UserSettings = new UserConfig("config.xml");

            // Add Config to Dictionary, this is nessesary for path -> Object Mapping
            this._ConfigObject.Add("BackupSettings", this.BackupSettings);
            this._ConfigObject.Add("ValidatorSettings", this.ValidatorSettings);
            this._ConfigObject.Add("UserSettings", this.UserSettings);

            initXML();
            loadConfig();

            //var types = GetDerivedTypesFor(typeof(ConfigBase));
            //foreach (Type type in types)
            //    log.DebugFormat(type.ToString());
            log.Info("Exit");
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// this method is used to check if the config has changed since freeze
        /// </summary>
        /// <returns>false if same and true if something changed</returns>
        public bool hasChanged()
        {

            int _cObjectHash = new int();
            int _cSnapshotHash = new int();


            foreach (KeyValuePair<string, IConfig> entry in this._ConfigObject)
            {
                _cObjectHash += entry.Value.getHashCode();
            }

            foreach (KeyValuePair<string, IConfig> entry in this.TempConfigObject)
            {
                _cSnapshotHash += entry.Value.getHashCode();
            }

            log.DebugFormat("Hashcodes actual: {0}, snapshot: {1}", _cObjectHash, _cSnapshotHash);
            return !(_cObjectHash == _cSnapshotHash);
        }

        private IEnumerable<Type> GetDerivedTypesFor(Type baseType)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetTypes().Where(baseType.IsAssignableFrom).Where(t => baseType != t);
        }

        public bool takeSnapshot()
        {
            log.Info("Entry");
            if (!this.hasSnapshot)
            {
                TempConfigObject = new Dictionary<string, IConfig>();

                foreach (KeyValuePair<string, IConfig> entry in _ConfigObject)
                {
                    log.DebugFormat("taking Snapshot for {0}", entry.Key);
                    TempConfigObject.Add(entry.Key, (IConfig)Activator.CreateInstance(entry.Value.GetType(), entry.Value));
                }
                log.Info("Exit");
                return TempConfigObject != null;
            }
            log.Info("Exit");
            return false;
        }

        public bool deleteSnapshot()
        {
            log.Info("Entry");
            if (this.hasSnapshot)
                TempConfigObject = null;
            log.Info("Exit");
            return TempConfigObject == null;
        }

        public bool revertSnapshot()
        {
            log.Info("Entry");
            bool _hasChanged = this.hasChanged();
            log.DebugFormat("check if it is nessesary to revert. hasSnapshot: {0}, hasChanged: {1}", this.hasSnapshot, _hasChanged);
            if (this.hasSnapshot && _hasChanged)
            {
                foreach (KeyValuePair<string, IConfig> entry in _ConfigObject)
                {
                    log.DebugFormat("Reverting Snapshot for {0}", entry.Key);
                    Dictionary<string, VariableReference> OrgFieldMapping = entry.Value.getFieldMapping();
                    Dictionary<string, VariableReference> TempFieldMapping = TempConfigObject[entry.Key].getFieldMapping();
                    int actualHash = entry.Value.getHashCode();
                    int snapshotHash = TempConfigObject[entry.Key].getHashCode();

                    string entryKey = entry.Key;

                    log.DebugFormat("check if config Object {0} changed. ActualHash: {1}, SnapshotHash: {2}", entryKey, actualHash, snapshotHash);
                    if (!(actualHash == snapshotHash))
                    {
                        foreach (KeyValuePair<string, VariableReference> field in OrgFieldMapping)
                        {
                            log.DebugFormat("Set Value for Propertie {0} actual: {1}, new {2}", field.Key, field.Value.Get(), TempFieldMapping[field.Key].Get());
                            field.Value.Set(TempFieldMapping[field.Key].Get());
                        }

                        log.DebugFormat("check for List Methods, result: {0}", entry.Value.getListMethods() != null);
                        if (entry.Value.getListMethods() != null)
                        {
                            int i = 0;
                            foreach (Dictionary<string, Delegate> listMethod in entry.Value.getListMethods())
                            {
                                log.DebugFormat("overriding List with new Values");
                                listMethod["setFromList"].DynamicInvoke(TempConfigObject[entryKey].getListMethods()[i]["getAsList"].DynamicInvoke(), false);
                            }
                        }
                    }

                }
            }
            TempConfigObject = null;
            log.Info("Exit");
            return TempConfigObject == null;
        }

        /// <summary>
        /// This Method is used to initialize and check the config files
        /// </summary>
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
        /// <param name="cObject">config Object to load from file</param>
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

        /// <summary>
        /// this is an helper Method to Trigger the saveToXML for a specific Object
        /// </summary>
        /// <returns></returns>
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