using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Integration;
using Oleg.Kleyman.Xbmc.Copier.Core;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration.Steps
{
    [Binding]
    public class CopierFeaturesSteps : TechTalk.SpecFlow.Steps
    {
        private const string RELEASE_TYPE_KEY = "RELEASE_TYPE";
        private const string RELEASE_KEY = "RELEASE";
        private static readonly Dictionary<ReleaseType, Tuple<string, string>> r__releases;

        static CopierFeaturesSteps()
        {
            var moviePath = Path.Combine(GlobalValues.TorrentFilesPath, "Some.Movie.BluRay.1080p.mkv.torrent");
            var tvPath = Path.Combine(GlobalValues.TorrentFilesPath, "Something.Else.txt.torrent");
            var otherPath = Path.Combine(GlobalValues.TorrentFilesPath, "Some.Show.S01E11.mkv.torrent");

            r__releases = new Dictionary<ReleaseType, Tuple<string, string>>
            {
                { ReleaseType.Movie, new Tuple<string, string>("C10B5C8673C6ADE0B41CE6BAF0A431E86FD250F3", moviePath) },
                { ReleaseType.Other, new Tuple<string, string>("03D375489A28F2A862B19A03D3918D2100F476B6", otherPath) },
                { ReleaseType.Tv, new Tuple<string, string>("F2AF0EC2FC7D8C4CB8580C45BBFBFF683BB70670", tvPath) }
            };
        }

        [Given(@"a (Other|Tv|Movie) torrent has finished downloading")]
        public void GivenAtvTorrentHasFinishedDownloading(ReleaseType releaseType)
        {
            var torrentPath = Path.Combine(GlobalValues.TorrentFilesPath, r__releases[releaseType].Item2);
            When(string.Format(CultureInfo.InvariantCulture, @"I add {0} torrent", torrentPath));
            ScenarioContext.Current.Set(releaseType, RELEASE_TYPE_KEY);
            ScenarioContext.Current.Set(r__releases[releaseType], RELEASE_KEY);
        }

        [When(@"the XbmcFilerCopier runs")]
        public void WhenTheXbmcFilerCopierRuns()
        {
            //Assign build configuration tree path based on the current build configuration.
            const string buildConfiguration =
            #if(DEBUG)
                "Debug";
            #else 
                "Release";
            #endif

            var copierFilePath = Path.Combine(@"..\..\..\..\..\Src\Oleg.Kleyman.Xbmc.Copier\bin", buildConfiguration, "Oleg.Kleyman.Xbmc.Copier.exe");
            var startInfo = new ProcessStartInfo(Path.GetFullPath(copierFilePath), ScenarioContext.Current.Get<Tuple<string, string>>(RELEASE_KEY).Item1)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true
            };
            var process = Process.Start(startInfo);
            if (process == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not start process at path {0}", copierFilePath));
            }
            
            var executionTime = TimeSpan.FromMilliseconds(30000);
            if (!process.WaitForExit((int)executionTime.TotalMilliseconds))
            {
                process.Kill();
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "{0} did not finish in {1} seconds", copierFilePath, executionTime.TotalSeconds));
            }

            var errorOutput = process.StandardError.ReadToEnd();

            Console.WriteLine(errorOutput);
            Assert.That(errorOutput, Is.Null);
        }

        [Then(@"the torrent contents should be transfered to the correct directory")]
        public void ThenTheTorrentContentsShouldBeTransferedToTheReleaseDirectory()
        {
            var releaseType = ScenarioContext.Current.Get<ReleaseType>(RELEASE_TYPE_KEY);
            
            switch (releaseType)
            {
                case ReleaseType.Tv:
                    var tvDirectory = Path.Combine(GlobalValues.RepositoryPath, @"Common\Test\TorrentTest\TV");
                    var files = Directory.GetFiles(tvDirectory);
                    Assert.That(files.Length, Is.EqualTo(1), "files.Length");
                    Assert.That(files[0], Is.EqualTo("Some.Show.S01E11.mkv"), "File Name");
                    break;
            }
        }
    }
}
