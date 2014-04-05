using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Oleg.Kleyman.Utorrent.Core.Tests.Integration
{
    [Binding]
    public class UtorrentSteps
    {
        internal static string Key { get; set; }
        internal static IUtorrentService ServiceClient { get; set; }
        private Torrent Torrent { get; set; }

        [BeforeFeature]
        public static void Setup()
        {
            var builder = new UtorrentServiceBuilder(new DefaultSettings());
            ServiceClient = builder.GetService();
        }

        [When(@"I call the method GetKey")]
        public void WhenICallTheMethodGetKey()
        {
            Key = ServiceClient.GetKey();
        }

        [Then(@"It should result in returning the key to use for this utorrent session")]
        public void ThenItShouldResultInReturningTheKeyToUseForThisUtorrentSession()
        {
            Assert.NotNull(Key);
            Assert.AreEqual(64, Key.Length);
        }

        [Given(@"I have attained an API key")]
        public void GivenIHaveAttainedAnAPIKey()
        {
            Key = ServiceClient.GetKey();
        }

        [When(@"I call the method GetTorrentFile with a hash of ""(.*)""")]
        public void WhenICallTheMethodGetTorrentFileWithAHashOf(string hash)
        {
            Torrent = ServiceClient.GetTorrentFiles(Key, hash);
        }

        [Then(@"It should return a torrent with build number ""(.*)""")]
        public void ThenItShouldReturnATorrentWithBuildNumber(int build)
        {
            Assert.AreEqual(build, Torrent.BuildNumber);
        }

        [Then(@"the torrent should have a hash value of ""(.*)""")]
        public void ThenTheTorrentShouldHaveAHashValueOf(string hash)
        {
            Assert.AreEqual(hash, Torrent.Hash.Value);
        }

        [Then(@"the torrent should have a count of ""(.*)"" files")]
        public void ThenTheTorrentShouldHaveACountOfFiles(int numberOfFiles)
        {
            Assert.AreEqual(numberOfFiles, Torrent.TorrentFiles.Length);
        }

        [Then(@"the file names should be")]
        public void ThenTheFileNamesShouldBe(Table table)
        {
            var files = table.CreateSet<TorrentFile>().ToArray();
            Assert.That(files.Length, Is.EqualTo(Torrent.TorrentFiles.Length));
            for (int i = 0; i < files.Length; i++)
            {
                Assert.AreEqual(files[i].Name, Torrent.TorrentFiles[i].Name);
            }
        }
    }
}