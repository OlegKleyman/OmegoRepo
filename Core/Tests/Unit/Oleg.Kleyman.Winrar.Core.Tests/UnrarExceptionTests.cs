using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class UnrarExceptionTests : TestsBase
    {
        public override void Setup()
        {
            
        }

        [Test]
        public void ConstructorShouldSetPropertiesCorrectly()
        {
            var exception = new UnrarException("Test message", RarStatus.BadData);
            Assert.That(exception.Message, Is.EqualTo("Test message"));
            Assert.That(exception.Status, Is.EqualTo(RarStatus.BadData));
        }

        [Test]
        public void ObjectShouldSerializeCorrectly()
        {
            var exception = new UnrarException("Test message", RarStatus.BadData);
            using (Stream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, exception);
                Assert.That(stream.Length, Is.EqualTo(467));
            }
        }

        [Test]
        public void ObjectShouldDeserializeCorrectly()
        {
            var exception = new UnrarException("Test message", RarStatus.BadData);
            using (Stream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, exception);
                stream.Position = 0;
                exception = (UnrarException)formatter.Deserialize(stream);
            }

            Assert.That(exception.Message, Is.EqualTo("Test message"));
            Assert.That(exception.Status, Is.EqualTo(RarStatus.BadData));
        }
    }
}
