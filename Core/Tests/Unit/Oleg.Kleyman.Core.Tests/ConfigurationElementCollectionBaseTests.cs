using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class ConfigurationElementCollectionBaseTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {
        }

        #endregion

        [Test]
        public void CreateNewElementTest()
        {
            var elementCollection = new MockConfigurationElementCollection<SingleValueConfigurationSection>();
            var result = elementCollection.CallCreateNewElement();
            Assert.IsInstanceOf<SingleValueConfigurationSection>(result);
        }
    }
}
