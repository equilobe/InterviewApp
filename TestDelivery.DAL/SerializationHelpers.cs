using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestDelivery.Common.Extensions;

namespace TestDelivery.DAL
{
    class SerializationHelpers
    {
        public static string GetXml(ref string xml, bool used, object obj, Func<XmlSerializer> getSerializer)
        {
            if (used)
            {
                if (obj == null)
                    xml = null;
                else
                    xml = getSerializer().SerializeToString(obj);
            }
            return xml;
        }

        public static T GetObject<T>(ref T obj, ref bool used, string xml, Func<XmlSerializer> getSerializer) where T : new()
        {
            if (!used)
            {
                if (xml != null)
                    obj = getSerializer().DeserializeFromString<T>(xml);
                else
                    obj = new T();
                used = true;
            }
            return obj;
        }

        public static void SetObject<T>(ref T obj, ref bool used, T value)
        {
            if (!object.ReferenceEquals(obj, value))
            {
                obj = value;
                used = true;
            }
        }
    }
}
