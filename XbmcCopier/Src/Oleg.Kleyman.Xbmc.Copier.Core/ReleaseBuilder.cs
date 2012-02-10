using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class ReleaseBuilder
    {
        private readonly string[] _keywords;
        private readonly Regex _tvDelimeter;
        private readonly ISettingsProvider _settings;

        public ReleaseBuilder(ISettingsProvider settings, string name)
        {
            _settings = settings;
            const string tvDelimeter = @"\.S\d{2}E\d{2}\.";
            const RegexOptions regexOptions = RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline;
            _tvDelimeter = new Regex(tvDelimeter, regexOptions);
            _keywords = new[] {".720P.", ".1080P.", ".DVDRIP.", ".PAL.DVDR.", ".NTSC.DVDR.", ".XVID."};
            Name = name;
        }

        public string Name { get; set; }

        public Release Build()
        {
            var release = new Release(GetReleaseType());
            release.Name = GetReleaseName(release);
            return release;
        }

        private string GetReleaseName(Release release)
        {
            string name = Name;
            if (release.ReleaseType == ReleaseType.Tv)
            {
                name = GetTvName(name);
            }
            return name;
        }

        private string GetTvName(string name)
        {
            string[] nameSplit = _tvDelimeter.Split(name);
            const char originalWordDelimeter = '.';
            const char newWordDelimeter = ' ';
            name = nameSplit[0].Replace(originalWordDelimeter, newWordDelimeter);
            return name;
        }

        private ReleaseType GetReleaseType()
        {
            ReleaseType type = default(ReleaseType);
            var isTvType = _settings.TvFilters.Any(filter => filter.IsMatch(Name));
            if(isTvType)
            {
                type = ReleaseType.Tv;
            }
            else if (_settings.MovieFilters.Any(movieFilter => movieFilter.IsMatch(Name)))
            {
                type = ReleaseType.Movie;
            }

            return type;
        }
    }
}