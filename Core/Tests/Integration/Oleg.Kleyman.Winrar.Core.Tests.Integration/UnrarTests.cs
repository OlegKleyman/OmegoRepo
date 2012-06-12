using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration
{
    [TestFixture]
    public class UnrarTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {
            
        }

        #endregion

        [Test]
        public void OpenTest()
        {
            var archive = Unrar.Open(@"C:\test\test.rar");

            Assert.IsNotNull(archive);
            
            archive.Close();
        }
    }
}
