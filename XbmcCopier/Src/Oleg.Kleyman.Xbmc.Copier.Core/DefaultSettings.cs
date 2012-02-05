using System.Collections.Generic;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class DefaultSettings : ISettingsProvider
    {
        #region ISettingsProvider Members

        public string UnrarPath
        {
            get { return Settings.Default.UnrarPath; }
        }

        public string MoviesPath
        {
            get { return Settings.Default.MoviesPath; }
        }

        public string TvPath
        {
            get { return Settings.Default.TvPath; }
        }

        public ICollection<string> Filters
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}