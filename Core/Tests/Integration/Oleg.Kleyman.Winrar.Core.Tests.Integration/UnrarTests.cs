using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration
{
    [TestFixture]
    public class UnrarTests : TestsBase
    {
        private IUnrarDll UnrarDll { get; set; }

        public override void Setup()
        {
            UnrarDll = new UnrarDll();
        }

        [Test]
        public void OpenTest()
        {
        }
    }
}