using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Core.Extensions;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class RarStatusExtensionsTests : TestsBase
    {
        public override void Setup()
        {
            
        }

        [Test]
        public void ThrowOnInvalidStatusShouldThrowExceptionForReadOperation()
        {
            const RarStatus status = RarStatus.BadData;
            var ex = Assert.Throws<UnrarException>(() => status.ThrowOnInvalidStatus(RarOperation.ReadHeader));

            Assert.That(ex.Status, Is.EqualTo(RarStatus.BadData));
            Assert.That(ex.Message, Is.EqualTo("Unable to read header data."));
        }

        [Test]
        public void ThrowOnInvalidStatusShouldThrowExceptionForProcessOperation()
        {
            const RarStatus status = RarStatus.BadData;
            var ex = Assert.Throws<UnrarException>(() => status.ThrowOnInvalidStatus(RarOperation.Process));

            Assert.That(ex.Status, Is.EqualTo(RarStatus.BadData));
            Assert.That(ex.Message, Is.EqualTo("Unable to process member"));
        }
    }
}
