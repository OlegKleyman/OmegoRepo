using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class JsonSerializerTests : TestsBase
    {
        public override void Setup()
        {
            
        }

        [Test]
        public void DeserializeTests()
        {
            var serializer = new JsonSerializer<TestJsonObject>();
            var deserializedObject = serializer.Deserialize("{\"Name\":\"testing\",\"Price\":100}");
            Assert.AreEqual("testing", deserializedObject.Name);
            Assert.AreEqual(100, deserializedObject.Price);
        }
    }
}