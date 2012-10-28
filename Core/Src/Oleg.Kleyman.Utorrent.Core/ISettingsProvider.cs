using System;

namespace Oleg.Kleyman.Utorrent.Core
{
    public interface ISettingsProvider
    {
        Uri Url { get; }
        string Username { get; }
        string Password { get; }
    }
}