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
        public XmlNode ToXml()
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
        /// <param name="node"></param>
        public void FromXmlNode(XmlNode node)
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo propertie in properties)
            {
                string value = node.Attributes[propertie.Name].Value;
                var convertedValue = value == null ? null : Convert.ChangeType(value, propertie.PropertyType);
                propertie.SetValue(this, convertedValue);
            }
        }
    }
}
