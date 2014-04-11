using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Oleg.Kleyman.Core.Linq;
using Oleg.Kleyman.Utorrent.Core;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Tests.Integration
{
    [Binding]
    public class UtorrentSteps : Steps
    {
        private const string UTORRENT_SERVICE_KEY = "UTORRENT_SERVICE";
        private const string SERVICE_TOKEN_KEY = "SERVICE_TOKEN";

        [BeforeFeature]
        public static void BeforeFeature()
        {
            var builder = new UtorrentServiceBuilder(new DefaultSettings());
            FeatureContext.Current.Set(builder.GetService(), UTORRENT_SERVICE_KEY);
        }

        [When(@"I call the method GetKey")]
        [Given(@"I have attained an API key")]
        public void GivenIHaveAttainedAnApiKey()
        {
            var service = FeatureContext.Current.Get<IUtorrentService>(UTORRENT_SERVICE_KEY);
            ScenarioContext.Current.Set(service.GetKey(), SERVICE_TOKEN_KEY);
        }

        [Given(@"UTorrent is running")]
        public void GivenIAddedAllTorrents()
        {
            var utorrentPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TorrentTest\Utorrent\uTorrent.exe");
            var process = Process.Start(utorrentPath);
            if (process == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not start utorrent process at path {0}", utorrentPath));
            }
        }

        [When(@"I add ([.\\\w:]+) torrent")]
        public void WhenAddTorrent(string torrentPath)
        {
            const string arguments = "/directory {0} {1}";

            var process = Process.Start(GlobalValues.UTorrentPath, string.Format(CultureInfo.InvariantCulture,
                                                                                 arguments,
                                                                                 GlobalValues.DownloadPath,
                                                                                 torrentPath));
            if (process == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not start utorrent process at path {0}", GlobalValues.UTorrentPath));
            }
        }

        [AfterScenario]
        public void AfterUTorrentScenario()
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
