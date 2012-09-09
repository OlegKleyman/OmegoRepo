using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace Oleg.Kleyman.Core
{
    public class JsonSerializer<T>
    {
        public T Deserialize(string json)
        {
            var ser = new JavaScriptSerializer();
            var deserialized = ser.Deserialize<T>(json);
            return deserialized;
        }
    }
}