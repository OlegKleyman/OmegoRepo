using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class OutputTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void ConstructorTest()
        {
            var output = new Output("test.txt", "C:\\testOutput");
            Assert.AreEqual("test.txt", output.FileName);
            Assert.AreEqual("C:\\testOutput", output.TargetDirectory);
        }
    }
}