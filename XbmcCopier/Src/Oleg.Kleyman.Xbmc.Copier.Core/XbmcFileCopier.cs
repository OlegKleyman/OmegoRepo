using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public sealed class XbmcFileCopier
    {
        public XbmcFileCopier(ISettingsProvider settings, Extractor extractor, IFileSystem fileSystem)
        {
            ConfigSettings = settings;
            Extractor = extractor;
            FileSystem = fileSystem;
        }

        private Extractor Extractor { get; set; }
        private ISettingsProvider ConfigSettings { get; set; }
        private IFileSystem FileSystem { get; set; }

        public void Copy(ReleaseOutput output)
        {
            switch (output.Release.ReleaseType)
            {
                case ReleaseType.Tv:
                    CopyTvRelease(output);
                    break;
                case ReleaseType.Movie:
                    CopyMovieRelease(output);
                    break;
                case ReleaseType.Other:
                    goto default;
                default:
                    throw new ApplicationException(string.Format("Release type of {0} is not supported.",
                                                                 Enum.GetName(output.Release.ReleaseType.GetType(),
                                                                              output.Release.ReleaseType)));
            }
        }

        private void CopyMovieRelease(ReleaseOutput output)
        {
            var moviePath = Path.Combine(ConfigSettings.MoviesPath, output.Release.Name);
            if (!Directory.Exists(moviePath))
            {
                Directory.CreateDirectory(moviePath);
            }

            var compressedMovieFiles = GetFiles(new[] {".rar"}, output);
// ReSharper disable PossibleMultipleEnumeration
            if (compressedMovieFiles.Any())
// ReSharper restore PossibleMultipleEnumeration
            {
// ReSharper disable PossibleMultipleEnumeration
                ExtractFiles(compressedMovieFiles, moviePath);
// ReSharper restore PossibleMultipleEnumeration
            }
            else
            {
                CopySingleFile(moviePath, output);
            }
        }

        private void ExtractFiles(IEnumerable<FileInfo> compressedMovieFiles, string destinationPath)
        {
            foreach (var compressedMovieFile in compressedMovieFiles)
            {
                Extractor.Extract(compressedMovieFile.FullName, destinationPath);
            }
        }

        private void CopyTvRelease(ReleaseOutput output)
        {
            if (!Directory.Exists(ConfigSettings.TvPath + output.Release.Name))
            {
                Directory.CreateDirectory(ConfigSettings.TvPath + output.Release.Name);
            }

            if (string.IsNullOrEmpty(output.FileName))
            {
                var tvFiles = GetFiles(new[] {".mkv", ".avi", ".wmv"}, output);

// ReSharper disable PossibleMultipleEnumeration
                if (tvFiles.Any())
// ReSharper restore PossibleMultipleEnumeration
                {
// ReSharper disable PossibleMultipleEnumeration
                    CopyFiles(tvFiles, ConfigSettings.TvPath, output);
// ReSharper restore PossibleMultipleEnumeration
                }
                else
                {
                    var tvPath = Path.Combine(ConfigSettings.TvPath, output.Release.Name);
                    tvFiles = GetFiles(new[] {".rar"}, output);
                    ExtractFiles(tvFiles, tvPath);
                }
            }
            else
            {
                CopySingleFile(ConfigSettings.TvPath + output.Release.Name, output);
            }
        }

        private void CopySingleFile(string destination, ReleaseOutput output)
        {
            var sourceFilePath = Path.Combine(output.DownloadPath, output.FileName);
            destination = Path.Combine(destination, output.FileName);
            FileSystem.CopyFile(sourceFilePath, destination);
        }

        private void CopyFiles(IEnumerable<FileInfo> files, string destination, ReleaseOutput output)
        {
            destination = Path.Combine(destination, output.Release.Name);
            foreach (var file in files)
            {
                var fileDestination = Path.Combine(destination, file.Name);
                FileSystem.CopyFile(file.FullName, fileDestination);
            }
        }

        private IEnumerable<FileInfo> GetFiles(IEnumerable<string> extentions, ReleaseOutput output)
        {
            var files = FileSystem.GetFilesByExtensions(output.DownloadPath, extentions);
            
            return files;
        }
    }
}