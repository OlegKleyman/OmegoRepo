using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Oleg.Kleyman.Sandbox.Javascription.Serialization.Test
{
    [TestFixture]
    public class JsonSerializerTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void SerializeTest()
        {
            var customer = new Customer {Age = null, Name = "Oleg", Bar = new Testing{Foo="fdsdf"}, Rad = new List<Testing>{new Testing{Foo="dd"}, new Testing{Foo="yy"}}};
            var serializer = new JsonSerializer();
            var result = serializer.Serialize(customer);
        }

        [Test]
        public void DeserializeTest()
        {
            var serializer = new JsonSerializer();
            var result = serializer.Deserialize<Customer>("{\"Bar\":{\"Foo\":\"fdsdf\"},\"Blah\":\"Oleg\"}");

        }
    }
}
