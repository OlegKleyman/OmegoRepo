using System;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class ArchiveReaderBuilderTests : TestsBase
    {
        private Mock<IUnrarHandle> MockHandle { get; set; }

        public override void Setup()
        {
            MockHandle = new Mock<IUnrarHandle>();
            MockHandle.SetupGet(x => x.IsOpen).Returns(true);
        }

        [Test]
        [Ignore]
        public void GetReaderShouldReturnTheReaderObject()
        {
            var builder = new ArchiveReaderBuilder(MockHandle.Object);
            var reader = builder.GetReader();
            Assert.That(reader.Status, Is.EqualTo(RarStatus.Success));
        }
    }
}
