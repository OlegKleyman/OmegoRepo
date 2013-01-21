using System.ServiceModel.Channels;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class JsonXmlContentTypeMapperTests : TestsBase
    {
        public override void Setup()
        {
        }

        private static JsonXmlContentTypeMapper GetJsonXmlContentTypeMapper()
        {
            return new JsonXmlContentTypeMapper();
        }

        [Test]
        public void GetMessageFormatForContentTypeShouldReturnDefaultContentType()
        {
            var contentTypeMapper = GetJsonXmlContentTypeMapper();
            var result = contentTypeMapper.GetMessageFormatForContentType("text/xml");
            Assert.AreEqual(WebContentFormat.Default, result);
        }

        [Test]
        public void GetMessageFormatForContentTypeShouldReturnJsonContentType()
        {
            var contentTypeMapper = GetJsonXmlContentTypeMapper();
            var result = contentTypeMapper.GetMessageFormatForContentType("text/javascript");
            Assert.AreEqual(WebContentFormat.Json, result);
        }
    }
}