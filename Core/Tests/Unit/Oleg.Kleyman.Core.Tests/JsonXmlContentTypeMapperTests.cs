using System.ServiceModel.Channels;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class JsonXmlContentTypeMapperTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {

        }

        #endregion

        [Test]
        public void GetMessageFormatForContentTypeShouldReturnJsonContentType()
        {
            var contentTypeMapper = GetJsonXmlContentTypeMapper();
            var result = contentTypeMapper.GetMessageFormatForContentType("text/javascript");
            Assert.AreEqual(WebContentFormat.Json, result);
        }

        [Test]
        public void GetMessageFormatForContentTypeShouldReturnDefaultContentType()
        {
            var contentTypeMapper = GetJsonXmlContentTypeMapper();
            var result = contentTypeMapper.GetMessageFormatForContentType("text/xml");
            Assert.AreEqual(WebContentFormat.Default, result);
        }

        private static JsonXmlContentTypeMapper GetJsonXmlContentTypeMapper()
        {
            return new JsonXmlContentTypeMapper();
        }
    }
}