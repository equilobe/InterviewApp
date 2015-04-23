using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.Common
{
    public static class SecretCodeGenerator
    {
        public static string GetNewCode()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                sb.Append(Path.GetRandomFileName().Replace(".", ""));
            }

            return sb.ToString();
        }
    }
}
