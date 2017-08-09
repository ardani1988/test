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

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private log4net.ILog _log;
        protected log4net.ILog log => _log ?? (_log = log4net.LogManager.GetLogger(GetType()));

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

        protected void copyWithObject(IConfig other)
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            Dictionary<string, VariableReference> otherFields = other.getFieldMapping();

            foreach (PropertyInfo propertie in properties)
            {
                propertie.SetValue(this, otherFields[propertie.Name].Get());
                log.DebugFormat("Propertie {0} was set to {1}", propertie.Name, propertie.GetValue(this).ToString());
            }
        }

        //https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
        public virtual int getHashCode()
        {
            //log.Info("Entry");
            PropertyInfo[] theProperties = this.GetType().GetProperties();
            int hash = 31;
            foreach (PropertyInfo info in theProperties)
            {
                if (info != null)
                {
                    var value = info.GetValue(this, null);
                    if (value != null)
                        unchecked
                        {
                            hash = 29 * hash ^ value.GetHashCode();
                        }
                }
            }
            //log.InfoFormat("Exit - with value {0}", hash.ToString());
            return hash;
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
