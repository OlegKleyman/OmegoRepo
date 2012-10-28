using System;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    internal class MockUtorrentSettings : ISettingsProvider
    {
        #region Implementation of ISettingsProvider

        public Uri Url { get { return new Uri("http://someurl.com:8085/gui"); } }
        public string Username { get { return "someusername"; } }
        public string Password { get { return "password"; } }

        #endregion
    }
}