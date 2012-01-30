using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Oleg.Kleyman.Sandbox.Javascription.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T target)
        {
            var memoryStream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(memoryStream, target);
            memoryStream.Position = 0;
            var sr = new StreamReader(memoryStream);
            
            return sr.ReadToEnd();
        }

        public T Deserialize<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var res = (T) serializer.ReadObject(memoryStream);
            memoryStream.Position = 0;
            var r = JsonReaderWriterFactory.CreateJsonReader(memoryStream, XmlDictionaryReaderQuotas.Max);
            var res1 = XElement.Load(r);
         //   var m = XElement.Load(serializer.ReadObject(memoryStream));
            return res;
        }
    }
}
