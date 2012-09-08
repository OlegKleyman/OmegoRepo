using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class ConfigurationElementCollectionBaseTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void CreateNewElementTest()
        {
            var elementCollection = new MockConfigurationElementCollection<SingleValueConfigurationSection>();
            var result = elementCollection.CallCreateNewElement();
            Assert.IsInstanceOf<SingleValueConfigurationSection>(result);
        }
    }
}