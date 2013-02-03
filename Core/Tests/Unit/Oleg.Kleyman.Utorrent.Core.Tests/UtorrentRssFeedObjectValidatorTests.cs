using System.Collections.Generic;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class UtorrentRssFeedObjectValidatorTests : TestsBase
    {
        public override void Setup()
        {
            
        }

        [Test]
        public void ValidationMessageShouldBeSetIndicatingThatNameUrlIndexIsInvalidType()
        {
            var feed = new[] { 1, null, null, null, null, null, new object() };
            var validator = new UtorrentRssFeedObjectValidator(feed);

            var result = validator.Validate();
            Assert.That(result, Is.False);
            Assert.That(validator.Message, Is.EqualTo("Name/Url index 6 must be a string"));
        }

        [Test]
        public void ValidationMessageShouldBeSetIndicatingThatIdIndexIsInvalidType()
        {
            var feed = new[] { new object(), null, null, null, null, null, string.Empty };
            var validator = new UtorrentRssFeedObjectValidator(feed);

            var result = validator.Validate();
            Assert.That(result, Is.False);
            Assert.That(validator.Message, Is.EqualTo("ID index 0 must be an int"));
        }

        [Test]
        public void ValidationMessageShouldBeSetIndicatingThatIdIndexIsNull()
        {

            var feed = new object[] { null, null, null, null, null, null, string.Empty };
            var validator = new UtorrentRssFeedObjectValidator(feed);

            var result = validator.Validate();
            Assert.That(result, Is.False);
            Assert.That(validator.Message, Is.EqualTo("ID index 0 is null"));
        }

        [Test]
        public void ValidationMessageShouldBeSetIndicatingThatNameUrlIndexIsNull()
        {
            var feed = new object[] { 1, null, null, null, null, null, null };
            var validator = new UtorrentRssFeedObjectValidator(feed);

            var result = validator.Validate();
            Assert.That(result, Is.False);
            Assert.That(validator.Message, Is.EqualTo("Name/Url index 6 is null"));
        }
    }
}
