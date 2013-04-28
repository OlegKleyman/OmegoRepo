using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class DestinationPathBuilderTests : TestsBase
    {
        private Mock<IPathBuilder> MockPathBuilder { get; set; }

        public override void Setup()
        {
            MockPathBuilder = new Mock<IPathBuilder>();
            MockPathBuilder.Setup(
                x =>
                x.Combine(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing",
                          @"TestFolder\testFile.txt")).Returns(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing\TestFolder\testFile.txt");
            MockPathBuilder.Setup(
                x =>
                x.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing\TestFolder\testFile.txt")).Returns(@"C:\GitRepos\MainDefault\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing\TestFolder\testFile.txt");
        }

        [Test]
        public void BuildShouldReturnTheFullUncPath()
        {
            IPathBuilder builder = new DestinationPathBuilder(MockPathBuilder.Object);
            var result =
                builder.Build(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing", @"TestFolder\testFile.txt");
            Assert.That(result, Is.EqualTo(@"C:\GitRepos\MainDefault\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing\TestFolder\testFile.txt"));
        }

        [Test]
        public void BuildShouldReturnNullPath()
        {
            IPathBuilder builder = new DestinationPathBuilder(MockPathBuilder.Object);
            var result =
                builder.Build(null, @"TestFolder\testFile.txt");
            Assert.That(result, Is.Null);
        }
    }
}
