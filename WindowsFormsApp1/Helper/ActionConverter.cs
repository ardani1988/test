using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_Sicherung.Helper
{
    class ActionConverter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ggf. noch fuer Integer, List etc...
        public static Action<object> getAction(object obj, string type, PropertyInfo propertie)
        {
            Action<object> action;

            log.DebugFormat("type: {0}, properie.Name: {1}", type, propertie.Name);

            switch (type)
            {
                case "System.String":
                    action = val => { propertie.SetValue(obj, val.ToString()); };
                    log.Debug("System.String");
                    break;
                case "System.Boolean":
                    action = val => { propertie.SetValue(obj, TypeConverter.toBool(val.ToString())); };
                    log.Debug("System.Boolean");
                    break;
                case "Ionic.Zlib.CompressionLevel":
                    action = val => { propertie.SetValue(obj, TypeConverter.toCompressionLevel(val.ToString())); };
                    log.Debug("Ionic.Zlib.CompressionLevel");
                    break;
                default:
                    action = val => { propertie.SetValue(obj, val.ToString()); };
                    log.Debug("default");
                    break;
            }
            return action;
        }
    }
}
