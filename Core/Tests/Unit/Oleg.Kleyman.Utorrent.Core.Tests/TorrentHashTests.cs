using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class TorrentHashTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void ParseShouldParseStringCorrectlyTest()
        {
            var hash = TorrentHash.Parse("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC");
            Assert.AreEqual("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC", hash.Value);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException),
            ExpectedMessage = "The torrent hash must be a 40 character string.", MatchType = MessageMatch.Exact)]
        public void ParseShouldThrowInvalidOperationExceptionOnInvalidHashStringTest()
        {
            TorrentHash.Parse("invalid hash");
        }

        [Test]
        public void ToStringShouldReturnAStringRepresentationOfTheHash()
        {
            var hash = TorrentHash.Parse("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC");
            var result = hash.ToString();
            Assert.AreEqual("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC", result);
        }
    }
}