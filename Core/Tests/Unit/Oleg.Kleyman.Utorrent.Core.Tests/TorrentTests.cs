using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class TorrentTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void GetFilesShouldReturnNullWhenNotSet()
        {
            var torrent = new Torrent();
            Assert.That(torrent.Files, Is.Null);
        }

        [Test]
        public void SetFilesShouldSetAllAssociatedPropertiesTest()
        {
            var torrent = new Torrent();
            var file1 = new object[]
                {
                    new object[]
                        {
                            "daa-alvh-1080p.mkv",
                            8210651843,
                            8210651843,
                            2,
                            0,
                            490,
                            true,
                            621537,
                            6300,
                            0,
                            0,
                            -1
                        }
                };
            var file2 = new object[]
                {
                    new object[]
                        {
                            "daa-alvh-1080p.nfo",
                            8210651843,
                            8210651843,
                            2,
                            0,
                            490,
                            true,
                            621537,
                            6300,
                            0,
                            0,
                            -1
                        }
                };
            torrent.Files = new object[] {"FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC", file1, file2};
            Assert.IsNotNull(torrent.Files);
            Assert.IsNotNull(torrent.TorrentFiles);
            Assert.AreEqual("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC", torrent.Hash.Value);
            Assert.AreEqual(2, torrent.TorrentFiles.Length);
            Assert.AreEqual(3, torrent.Files.Length);
            Assert.AreEqual("daa-alvh-1080p.mkv", torrent.TorrentFiles[0].Name);
            Assert.AreEqual("daa-alvh-1080p.nfo", torrent.TorrentFiles[1].Name);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Invalid property array.",
            MatchType = MessageMatch.Exact)]
        public void SetFilesShouldThrowExceptionWhenAnEmptyArrayIsSet()
        {
            var torrent = new Torrent();
            torrent.Files = new object[] {};
        }
    }
}