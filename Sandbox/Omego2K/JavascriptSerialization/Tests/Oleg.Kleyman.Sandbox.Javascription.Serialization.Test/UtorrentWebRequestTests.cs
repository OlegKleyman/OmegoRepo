using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace Oleg.Kleyman.Sandbox.Javascription.Serialization.Test
{
    [TestFixture]
    public class UtorrentWebRequestTests
    {
        [Test]
        public void RequestTest()
        {
            var request = new UtorrentWebRequest();
            var res = request.MakeRequest("http://vmst01:8085/gui/token.html", "OKleyman", "Namyelk1", null);
            var element = XElement.Parse(res.Text);
            var token = element.Element("div").Value;
            res = request.MakeRequest(string.Format("http://vmst01:8085/gui/?token={0}&list=1", token), "OKleyman", "Namyelk1", res.Cookie);
            var b = Encoding.UTF8.GetBytes(res.Text);
            var serializer = new JsonSerializer();
            var torrents = serializer.Deserialize<Torrents>(res.Text);
            //var writer = JsonReaderWriterFactory.CreateJsonReader(b, XmlDictionaryReaderQuotas.Max);
            //element = XElement.Load(writer);
        }
    }
}
