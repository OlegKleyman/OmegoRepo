using System.IO;

namespace Oleg.Kleyman.Tests.Integration
{
    public static class GlobalValues
    {
        /// <summary>
        /// Gets the root repository path based on current working directory
        /// </summary>
        /// <remarks>
        /// This will only return the correct path when used from a test 
        /// project following the conventional directory structure and
        /// with an unaltered working directory. 
        /// </remarks>
        public static string RepositoryPath
        {
            get { return Path.GetFullPath(@"..\..\..\..\..\..\"); }
        }

        /// <summary>
        /// Gets the path to UTorrent based on the <see cref="RepositoryPath"/> property
        /// </summary>
        public static string UTorrentPath
        {
            get { return Path.Combine(RepositoryPath, @"Common\Test\TorrentTest\Utorrent\uTorrent.exe"); }
        }

        /// <summary>
        /// Gets the path to torrent files based on the <see cref="RepositoryPath"/> property
        /// </summary>
        public static string TorrentFilesPath
        {
            get { return Path.Combine(RepositoryPath, @"Common\Test\TorrentTest\torrents"); }
        }

        /// <summary>
        /// Gets the path to torrent download directory based on the <see cref="RepositoryPath"/> property
        /// </summary>
        public static string DownloadPath
        {
            get { return Path.Combine(RepositoryPath, @"Common\Test\TorrentTest\DownloadDirectory"); }
        }
    }
}
