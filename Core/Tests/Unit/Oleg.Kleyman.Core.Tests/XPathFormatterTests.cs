using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;
using System.Xml.Linq;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    public class XPathFormatterTests : TestsBase
    {
        private const string XTEXT_XPATH_QUERY = "./div/text()";
        private const string XELEMENT_XPATH_QUERY = "./div";
        private const string INVALID_XPATH_QUERY = "./invalid";
        private Mock<IClientMessageFormatter> _mockClientMessageFormatter;
        private Mock<XmlObjectSerializer> _mockSerializer;

        #region Overrides of TestsBase

        public override void Setup()
        {
            _mockClientMessageFormatter = new Mock<IClientMessageFormatter>();
            _mockSerializer = new Mock<XmlObjectSerializer>();
            _mockSerializer.Setup(x => x.ReadObject(It.IsAny<XmlDictionaryReader>(), It.IsAny<bool>())).Returns("key");
            _mockClientMessageFormatter.Setup(x => x.SerializeRequest(MessageVersion.None, new object[] { })).Returns(
                Message.CreateMessage(MessageVersion.None, null));
        }

        #endregion

        [Test]
        public void SerializeRequestTest()
        {
            var formatter = CreateFormatter(XTEXT_XPATH_QUERY);
            var message = formatter.SerializeRequest(MessageVersion.None, new object[] { });
            Assert.AreEqual(MessageVersion.None, message.Version);
        }

        [Test]
        public void DeserializeShouldReturnKey()
        {
            var formatter = CreateFormatter(XTEXT_XPATH_QUERY);

            var reader = XmlReader.Create(new StringReader("<html><div>key</div></html>"));
            var message = Message.CreateMessage(MessageVersion.Default, "action", reader);

            var obj = formatter.DeserializeReply(message, new object[] { });
            Assert.AreEqual("key", obj);
        }

        [Test]
        public void DeserializeShouldReturnKey1()
        {
            var formatter = CreateFormatter(XTEXT_XPATH_QUERY);
            var reader = XmlReader.Create(new StringReader("<html><div>key1</div></html>"));
            var message = Message.CreateMessage(MessageVersion.Default, "action", reader);
            var obj = formatter.DeserializeReply(message, new object[] { });
            Assert.AreEqual("key1", obj);
        }

        [Test]
        public void DeserializeShouldReturnElement()
        {
            var formatter = CreateFormatter(XELEMENT_XPATH_QUERY);
            var reader = XmlReader.Create(new StringReader("<html><div>key</div></html>"));
            var message = Message.CreateMessage(MessageVersion.Default, "action", reader);
            var obj = formatter.DeserializeReply(message, new object[] { });
            Assert.IsInstanceOf<XElement>(obj);
            Assert.IsTrue(XNode.DeepEquals(new XElement("div", "key"), (XElement)obj));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "XPath query did not yield any results.", MatchType = MessageMatch.Regex)]
        public void DeserializeReplyShouldThrowExceptionWhenXPathDoesntMatch()
        {
            var formatter = CreateFormatter(INVALID_XPATH_QUERY);
            var reader = XmlReader.Create(new StringReader("<html><div>key</div></html>"));
            var message = Message.CreateMessage(MessageVersion.Default, "action", reader);
            formatter.DeserializeReply(message, new object[] { });
        }

        private XPathFormatter CreateFormatter(string regEx)
        {
            return new XPathFormatter(_mockClientMessageFormatter.Object, regEx);
        }
    }
}
