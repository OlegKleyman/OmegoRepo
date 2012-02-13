using System.Text.RegularExpressions;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public interface ISettingsProvider
    {
        string UnrarPath { get; }
        string MoviesPath { get; }
        string TvPath { get; }
        Regex[] MovieFilters { get; }
        Regex[] TvFilters { get; }
        IFileSystem FileSystem { get; }
    }
}