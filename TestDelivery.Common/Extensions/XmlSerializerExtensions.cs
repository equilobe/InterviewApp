using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestDelivery.Common.Extensions
{
    public static class XmlSerializerExtensions
    {
        public static string SerializeToString(this XmlSerializer serializer, object obj)
        {
            using(var sw = new StringWriter())
            {
                serializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        public static T DeserializeFromString<T>(this XmlSerializer serializer, string serializedObject)
        {
            using (var sr = new StringReader(serializedObject))
            {
                return (T)serializer.Deserialize(sr);
            }
        }
    }
}
