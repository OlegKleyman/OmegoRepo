using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using NUnit.Framework;
using Oleg.Kleyman.Core.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Oleg.Kleyman.Utorrent.Core.Tests.Integration
{
    [Binding]
    public class UtorrentSteps
    {
        private const string TORRENT_HASH_KEY = "TORRENT_HASH";
        private const string UTORRENT_SERVICE_KEY = "UTORRENT_SERVICE";
        private const string SERVICE_TOKEN_KEY = "SERVICE_TOKEN";
        private const string TORRENTLIST_KEY = "TORRENTLIST_KEY";
        internal static IUtorrentService ServiceClient { get; set; }
        private Torrent Torrent { get; set; }

        [BeforeFeature]
        public static void Setup()
        {
            var builder = new UtorrentServiceBuilder(new DefaultSettings());
            ServiceClient = builder.GetService();
            FeatureContext.Current.Set(ServiceClient, UTORRENT_SERVICE_KEY);
        }

        [Then(@"It should result in returning the key to use for this utorrent session")]
        public void ThenItShouldResultInReturningTheKeyToUseForThisUtorrentSession()
        {
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);
            Assert.NotNull(token);
            Assert.AreEqual(64, token.Length);
        }

        [When(@"I call the method GetKey")]
        [Given(@"I have attained an API key")]
        public void GivenIHaveAttainedAnApiKey()
        {
            ScenarioContext.Current.Set(ServiceClient.GetKey(), SERVICE_TOKEN_KEY);
        }

        [When(@"I call the method GetTorrentFile with a hash of ""(.*)""")]
        public void WhenICallTheMethodGetTorrentFileWithAHashOf(string hash)
        {
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);
            Torrent = ServiceClient.GetTorrentFiles(token, hash);
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

        [Given(@"I have a torrent with the hash of ([A-F0-9]{40})")]
        public void GivenIHaveATorrentWithTheHashOf(string hash)
        {
            ScenarioContext.Current.Set(hash, TORRENT_HASH_KEY);
        }

        [When(@"I call the Remove method on it")]
        public void WhenICallTheRemoveMethodOnIt()
        {
            var service = FeatureContext.Current.Get<IUtorrentService>(UTORRENT_SERVICE_KEY);
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);
            var torrentHash = ScenarioContext.Current.Get<string>(TORRENT_HASH_KEY);
            var result = service.Remove(token, torrentHash);
            Assert.That(result.BuildNumber, Is.EqualTo(30303));
        }

        [Then(@"the torrent should be removed list")]
        public void ThenTheTorrentShouldBeRemovedList()
        {
            var service = FeatureContext.Current.Get<IUtorrentService>(UTORRENT_SERVICE_KEY);
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);
            var torrentHash = ScenarioContext.Current.Get<string>(TORRENT_HASH_KEY);

            var result = service.GetTorrentFiles(token, torrentHash);
            Assert.That(result.TorrentFiles.Length, Is.EqualTo(0));
        }

        [Given(@"I added all torrents")]
        public void GivenIAddedAllTorrents()
        {
            var utorrentPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TorrentTest\Utorrent\uTorrent.exe");
            var torrentsLocation = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TorrentTest\torrents");
            var downloadLocationPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TorrentTest\DownloadDirectory");
            const string arguments = "/directory {0} {1}";

            Directory.GetFiles(torrentsLocation).ForEach(file =>
            {
                var torrentLocation = Path.Combine(torrentsLocation, file);
                var process = Process.Start(utorrentPath, string.Format(CultureInfo.InvariantCulture,
                                                                        arguments,
                                                                        downloadLocationPath,
                                                                        torrentLocation));
                if (process == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not start utorrent process at path {0}", utorrentPath));
                }
            });
        }

        [When(@"I call the GetAll method")]
        public void WhenICallTheGetAllMethod()
        {
            var service = FeatureContext.Current.Get<IUtorrentService>(UTORRENT_SERVICE_KEY);
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);
            var torrentList = service.GetList(token);
            ScenarioContext.Current.Set(torrentList, TORRENTLIST_KEY);
        }

        [Then(@"I should get all torrents")]
        public void ThenIShouldGetAllTorrents()
        {
            var torrentList = ScenarioContext.Current.Get<UTorrentList>(TORRENTLIST_KEY);

            Assert.That(torrentList.Torrents, Is.Not.Null, "Torrents is null");
            Assert.That(torrentList.Torrents.Length, Is.EqualTo(4));

            Assert.That(torrentList.Torrents[0].Hash, Is.Not.Null);
            Assert.That(torrentList.Torrents[0].Hash.Value, Is.EqualTo("15DB4BB838E03B08A732584826E180C87CD2FD90"));

            Assert.That(torrentList.Torrents[1].Hash, Is.Not.Null);
            Assert.That(torrentList.Torrents[1].Hash.Value, Is.EqualTo("93ED1869741FBB7075831742539E6530C8A0661F"));

            Assert.That(torrentList.Torrents[2].Hash, Is.Not.Null);
            Assert.That(torrentList.Torrents[2].Hash.Value, Is.EqualTo("9488865BD3C39371AD92775E23A971DD125A6E9A"));

            Assert.That(torrentList.Torrents[3].Hash, Is.Not.Null);
            Assert.That(torrentList.Torrents[3].Hash.Value, Is.EqualTo("D4AD03979D0676F22A0724599FE96FC8BD610877"));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var service = FeatureContext.Current.Get<IUtorrentService>(UTORRENT_SERVICE_KEY);
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);

            var torrentList = service.GetList(token);
            foreach (var torrent in torrentList.Torrents)
            {
                service.Remove(token, torrent.Hash.ToString());
            }

            var utorrentProcesses = Process.GetProcessesByName("utorrent");
            if (utorrentProcesses.Length == 0)
            {
                throw new InvalidOperationException("Could not locate processes with the name utorrent");
            }

            utorrentProcesses.ForEach(process => process.Kill());
        }
    }
}