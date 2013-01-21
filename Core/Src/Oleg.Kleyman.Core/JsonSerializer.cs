using System.Web.Script.Serialization;

namespace Oleg.Kleyman.Core
{
    public class JsonSerializer<T>
    {
        public T Deserialize(string json)
        {
            var serializer = new JavaScriptSerializer();
            var deserialized = serializer.Deserialize<T>(json);
            return deserialized;
        }
    }
}