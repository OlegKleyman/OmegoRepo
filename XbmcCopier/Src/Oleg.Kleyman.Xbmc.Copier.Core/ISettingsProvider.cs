using System.Text.RegularExpressions;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public interface ISettingsProvider
    {
        string MoviesPath { get; }
        string TvPath { get; }
        Regex[] MovieFilters { get; }
        Regex[] TvFilters { get; }
    }
}