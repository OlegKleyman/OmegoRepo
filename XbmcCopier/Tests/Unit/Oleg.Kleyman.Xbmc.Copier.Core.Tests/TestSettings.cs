using System.Text.RegularExpressions;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    public class TestSettings : ISettingsProvider
    {
        #region Implementation of ISettingsProvider

        public string MoviesPath
        {
            get { return @"C:\Movies"; }
        }

        public string TvPath
        {
            get { return @"C:\TV"; }
        }

        public Regex[] MovieFilters
        {
            get
            {
                return new[]
                           {
                               new Regex(@"\.720P\.|\.1080P\.|\.DVDRIP\.|\.PAL\.DVDR\.|\.NTSC\.DVDR\.|\.XVID\.",
                                         RegexOptions.CultureInvariant | RegexOptions.IgnoreCase |
                                         RegexOptions.Singleline)
                           };
            }
        }

        public Regex[] TvFilters
        {
            get
            {
                return new[]
                           {
                               new Regex(@"\.S\d{2}E\d{2}\.",
                                         RegexOptions.CultureInvariant | RegexOptions.IgnoreCase |
                                         RegexOptions.Singleline)
                           };
            }
        }

        #endregion
    }
}