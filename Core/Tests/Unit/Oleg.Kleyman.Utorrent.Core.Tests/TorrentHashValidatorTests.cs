using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class TorrentHashValidatorTests : TestsBase
    {
        public override void Setup()
        {
        }

        private static TorrentHashValidator CreateValidator(string hash)
        {
            return new TorrentHashValidator(hash);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            ExpectedMessage = "Value cannot be null.\r\nParameter name: hash", MatchType = MessageMatch.Exact)]
        public void ConstructorShouldThrowArgumentNullExceptionWhenPassedANullHashTest()
        {
            CreateValidator(null);
        }

        [Test]
        public void HashShouldBeSetTest()
        {
            var hashValidator = CreateValidator("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC");
            Assert.AreEqual("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC", hashValidator.Hash);
        }

        [Test]
        public void ValidateShouldReturnFalseTest()
        {
            var hashValidator = CreateValidator("invalid hash");
            var result = hashValidator.Validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateShouldReturnTrueTest()
        {
            var hashValidator = CreateValidator("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC");
            var result = hashValidator.Validate();
            Assert.IsTrue(result);
        }
    }
}