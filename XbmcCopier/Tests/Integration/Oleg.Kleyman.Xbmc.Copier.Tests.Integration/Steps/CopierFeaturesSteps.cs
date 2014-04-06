using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Xbmc.Copier.Core;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration.Steps
{
    [Binding]
    public class CopierFeaturesSteps
    {
        private const string RELEASE_TYPE_KEY = "RELEASE_TYPE";
        private const string RELEASE_HASH_KEY = "RELEASE_HASH";
        private static readonly Dictionary<ReleaseType, string> r__releaseHashes;

        static CopierFeaturesSteps()
        {
            r__releaseHashes = new Dictionary<ReleaseType, string>
            {
                { ReleaseType.Movie, "C10B5C8673C6ADE0B41CE6BAF0A431E86FD250F3" },
                { ReleaseType.Other, "03D375489A28F2A862B19A03D3918D2100F476B6" },
                { ReleaseType.Tv, "F2AF0EC2FC7D8C4CB8580C45BBFBFF683BB70670" }
            };
        }

        [Given(@"a (Other|Tv|Movie) torrent has finished downloading")]
        public void GivenAtvTorrentHasFinishedDownloading(ReleaseType releaseType)
        {
            ScenarioContext.Current.Set(releaseType, RELEASE_TYPE_KEY);
            ScenarioContext.Current.Set(r__releaseHashes[releaseType], RELEASE_HASH_KEY);
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
            var startInfo = new ProcessStartInfo(Path.GetFullPath(copierFilePath), ScenarioContext.Current.Get<string>(RELEASE_HASH_KEY))
            {
                CreateNoWindow = true,
                UseShellExecute = false
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
        }

        [Then(@"the torrent contents should be transfered to the (Other|Tv|Movie) directory")]
        public void ThenTheTorrentContentsShouldBeTransferedToTheReleaseDirectory(ReleaseType releaseType)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
