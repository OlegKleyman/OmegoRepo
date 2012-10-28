using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class UtorrentServiceBuilderTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {

        }

        #endregion

        [Test]
        public void ConstructorShouldReturnServiceBuilderWithTheCorrectProperties()
        {
            var serviceBuilder = CreateServiceBuilder();
            Assert.AreEqual("http://someurl.com:8085/gui", serviceBuilder.Url.AbsoluteUri);
            Assert.AreEqual("someusername", serviceBuilder.Username);
        }

        [Test]
        public void GetServiceShouldReturnAnInstanceOfTheUtorrentService()
        {
            var serviceBuilder = CreateServiceBuilder();
            var service = serviceBuilder.GetService();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<IUtorrentService>(service);
        }

        [Test]
        public void SettingThePasswordShouldNotCauseAnException()
        {
            var serviceBuilder = CreateServiceBuilder();
            serviceBuilder.Password = "password";
            Assert.Pass("Reaching this point of the test means that setting the password did not cause any issues.");
        }
        private UtorrentServiceBuilder CreateServiceBuilder()
        {
            return new UtorrentServiceBuilder(new MockUtorrentSettings());
        }
    }
}