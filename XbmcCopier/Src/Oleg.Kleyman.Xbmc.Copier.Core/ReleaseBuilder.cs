using System.Linq;
using System.Text.RegularExpressions;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class ReleaseBuilder
    {
        private readonly ISettingsProvider _settings;
        private readonly Regex _tvDelimeter;

        public ReleaseBuilder(ISettingsProvider settings, string name)
        {
            _settings = settings;
            const string tvDelimeter = @"\.S\d{2}E\d{2}\.";
            const RegexOptions regexOptions =
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline;
            _tvDelimeter = new Regex(tvDelimeter, regexOptions);

            Name = name;
        }

        public string Name { get; set; }

        public Release Build()
        {
            var releaseType = GetReleaseType();
            var releaseName = GetReleaseName(releaseType);
            var release = new Release(releaseType, releaseName);
            return release;
        }

        private string GetReleaseName(ReleaseType releaseType)
        {
            var name = Name;
            if (releaseType == ReleaseType.Tv)
            {
                name = GetTvName(name);
            }
            return name;
        }

        private string GetTvName(string name)
        {
            var nameSplit = _tvDelimeter.Split(name);
            const char originalWordDelimeter = '.';
            const char newWordDelimeter = ' ';
            name = nameSplit[0].Replace(originalWordDelimeter, newWordDelimeter);
            return name;
        }

        private ReleaseType GetReleaseType()
        {
            var type = default(ReleaseType);
            var isTvType = _settings.TvFilters.Any(filter => filter.IsMatch(Name));
            if (isTvType)
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