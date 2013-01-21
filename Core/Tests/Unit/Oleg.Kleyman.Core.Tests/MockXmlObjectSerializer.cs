using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Oleg.Kleyman.Core.Tests
{
    public class MockXmlObjectSerializer : XmlObjectSerializer
    {
        #region Overrides of XmlObjectSerializer

        public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
        {
        }

        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
        {
        }

        public override void WriteEndObject(XmlDictionaryWriter writer)
        {
        }

        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            return ReturnType;
        }

        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            throw new NotImplementedException();
        }

        #endregion

        public XText ReturnType { get; set; }
    }
}