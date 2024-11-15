using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.ComponentModel.Design.Serialization;

namespace Agent
{
    public static class Extension
    {
        public static byte[] Serialise<T>(this T data)
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serialiser.WriteObject(ms, data);
                return ms.ToArray();
            }
        }

        public static T Deseiralize<T>(this byte[] data)
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(data))
            {
                return (T) serialiser.ReadObject(ms);

            }
        }



    }
}
