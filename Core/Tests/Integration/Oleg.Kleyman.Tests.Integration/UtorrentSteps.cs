using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Tests.Integration
{
    [Binding]
    public class UtorrentSteps : Steps
    {
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
                                                                                 GlobalValues.TorrentFilesPath));
            if (process == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not start utorrent process at path {0}", GlobalValues.UTorrentPath));
            }
        }
    }
}
