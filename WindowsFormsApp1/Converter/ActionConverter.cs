using MC_Sicherung.Config;
using System;
using System.Reflection;



namespace MC_Sicherung.Converter
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
                    break;
                case "System.Boolean":
                    action = val => { propertie.SetValue(obj, TypeConverter.ToBool(val.ToString())); };
                    break;
                case "Ionic.Zlib.CompressionLevel":
                    action = val => { propertie.SetValue(obj, TypeConverter.ToCompressionLevel(val.ToString())); };
                    break;
                default:
                    action = val => { propertie.SetValue(obj, val.ToString()); };
                    break;
            }
            return action;
        }
    }
}
