using System;
using System.Collections.Generic;
using System.Reflection;
using MC_Sicherung.Config.Interface;
using MC_Sicherung.Converter;

namespace MC_Sicherung.Config.Base
{
    public abstract class ConfigBase : IConfig
    {
        #region Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Dictionary<string, VariableReference> FieldMapping = new Dictionary<string, VariableReference>();
        protected List<Dictionary<string, Delegate>> ListMethods = new List<Dictionary<string, Delegate>>();

        protected string _path;

        #endregion Properties

        #region Methods

        protected void initFieldMapping(List<string> NoMapping = null)
        {
            PropertyInfo[] properties = this.GetType().GetProperties();
            FieldInfo[] properties2 = this.GetType().GetFields();
            log.Debug(properties2);
            log.Debug(properties);

            foreach (PropertyInfo propertie in properties)
            {
                if ((NoMapping == null) || (!NoMapping.Contains(propertie.Name)))
                {
                    FieldMapping.Add(propertie.Name, new VariableReference(
                        () => propertie.GetValue(this),
                        ActionConverter.getAction(this, propertie.PropertyType.ToString(), propertie)
                        ));
                    log.DebugFormat("added {0} to FieldMapping", propertie.Name);
                }
                else
                    log.DebugFormat("found {0} in List for Special Mapping", propertie.Name);
            }
        }

        public string getPath()
        {
            return this._path;
        }

        public Dictionary<string, VariableReference> getFieldMapping()
        {
            return this.FieldMapping;
        }

        public List<Dictionary<string, Delegate>> getListMethods()
        {
            return ListMethods;
        }

        #endregion Methods
    }
}
