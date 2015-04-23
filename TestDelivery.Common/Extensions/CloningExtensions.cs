using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.Common.Extensions
{
    public static class CloningExtensions
    {
        public static T DeepCloneWithSerialization<T>(this T toClone)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, toClone);
                ms.Position = 0;
                return (T)bf.Deserialize(ms);
            }
        }

        public static void CopyProperties<T>(T source, T destination)
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                propertyInfo.SetValue(destination, propertyInfo.GetValue(source, null), null);
            }
        }
    }
}
