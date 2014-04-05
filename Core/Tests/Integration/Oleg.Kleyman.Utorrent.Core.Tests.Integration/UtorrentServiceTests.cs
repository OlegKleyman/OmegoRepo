using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests.Integration
{
    [TestFixture]
    public class UtorrentServiceTests : TestsBase
    {
        public override void Setup()
        {
        }

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
            const string hash = "25A7640F5E8BDC73EBC08E28D8CD4B044CCEF182";
            var torrent = serviceClient.GetTorrentFiles(key, hash);
            Assert.AreEqual(30303, torrent.BuildNumber);
            Assert.AreEqual(2, torrent.Files.Length);
            Assert.AreEqual("25A7640F5E8BDC73EBC08E28D8CD4B044CCEF182", torrent.Hash.Value);
            Assert.AreEqual(1, torrent.TorrentFiles.Length);
            Assert.AreEqual("Минута славы - Мечты сбываются! - Второй полуфинал_bySat.mpg", torrent.TorrentFiles[0].Name);
        }
    }
}