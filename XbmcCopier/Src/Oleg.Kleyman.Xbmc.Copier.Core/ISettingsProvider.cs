namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public interface ISettingsProvider
    {
        string UnrarPath { get; }
        string MoviesPath { get; }
        string TvPath { get; }
    }
}