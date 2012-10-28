using System;
using System.Web.Script.Serialization;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests.Integration
{
    [TestFixture]
    public class UtorrentServiceTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {

        }

        #endregion

        private static UtorrentServiceBuilder GetUtorrentServiceBuilder()
        {
            return new UtorrentServiceBuilder(new DefaultSettings());
        }

        [Test]
        public void GetKeyTest()
        {
            var utorrentServiceBuilder = GetUtorrentServiceBuilder();
            var serviceClient = utorrentServiceBuilder.GetService();
            var result = serviceClient.GetKey();
            Assert.NotNull(result);
            Assert.AreEqual(64, result.Length);
        }

        [Test]
        public void GetTorrentFilesTest()
        {
            var utorrentServiceBuilder = GetUtorrentServiceBuilder();
            var serviceClient = utorrentServiceBuilder.GetService();
            var key = serviceClient.GetKey();
            const string hash = "FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC";
            var torrent = serviceClient.GetTorrentFiles(key, hash);
            Assert.AreEqual(27498, torrent.BuildNumber);
            Assert.AreEqual(2, torrent.Files.Length);
            Assert.AreEqual("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC", torrent.Hash.Value);
            Assert.AreEqual(2, torrent.TorrentFiles.Length);
            Assert.AreEqual("daa-alvh-1080p.mkv", torrent.TorrentFiles[0].Name);
            Assert.AreEqual("daa-alvh-1080p.nfo", torrent.TorrentFiles[1].Name);

        }
    }
}