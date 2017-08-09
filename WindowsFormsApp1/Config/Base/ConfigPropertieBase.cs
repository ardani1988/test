using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using MC_Sicherung.Converter;

namespace MC_Sicherung.Config.Base
{
    public abstract class ConfigPropertieBase
    {
        /// <summary>
        /// This Method is used to generate a XML Representation of this Object
        /// </summary>
        /// <returns>Xml Representation of this Object</returns>    
        public virtual XmlNode ToXml()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            var xmlNode = new XElement(this.GetType().Name.ToLower());

            foreach (PropertyInfo propertie in properties)
                xmlNode.Add(new XAttribute(propertie.Name, propertie.GetValue(this)));

            return TypeConverter.ToXmlNode(xmlNode);
        }

        /// <summary>
        /// This Method is used to read the Objects Properties from XML Representation
        /// </summary>
        /// <param name="node">takes an Object as XmlNode</param>
        public virtual void FromXmlNode(XmlNode node)
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo propertie in properties)
            {
                string value = node.Attributes[propertie.Name].Value;
                var convertedValue = value == null ? null : Convert.ChangeType(value, propertie.PropertyType);
                propertie.SetValue(this, convertedValue);
            }
        }

        //https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
        public int getHashCode()
        {
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
            return hash;
        }

    }
}
