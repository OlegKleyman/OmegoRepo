using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class XbmcFileCopier : FileCopier
    {
        public XbmcFileCopier(ISettingsProvider settings, ReleaseOutput output)
        {
            Output = output;
            ConfigSettings = settings;
        }

        public ReleaseOutput Output { get; set; }
        protected ISettingsProvider ConfigSettings { get; set; }

        public override void Copy()
        {
            switch (Output.Release.ReleaseType)
            {
                case ReleaseType.Tv:
                    CopyTvRelease();
                    break;
                case ReleaseType.Movie:
                    CopyMovieRelease();
                    break;
                case ReleaseType.Other:
                    goto default;
                default:
                    throw new ApplicationException(string.Format("Release type of {0} is not supported.",
                                                                 Enum.GetName(Output.Release.ReleaseType.GetType(),
                                                                              Output.Release.ReleaseType)));
            }
        }

        private void CopyMovieRelease()
        {
            var moviePath = Path.Combine(ConfigSettings.MoviesPath, Output.Release.Name);
            if (!Directory.Exists(moviePath))
            {
                Directory.CreateDirectory(moviePath);
            }

            IEnumerable<FileInfo> compressedMovieFiles = GetFiles(new[] {".rar"});
            if (compressedMovieFiles.Any())
            {
                ExtractFiles(compressedMovieFiles, moviePath);
            }
            else
            {
                CopySingleFile(moviePath);
            }
        }

        private void ExtractFiles(IEnumerable<FileInfo> compressedMovieFiles, string destinationPath)
        {
            var extractor = new RarExtractor(ConfigSettings);
            foreach (var compressedMovieFile in compressedMovieFiles)
            {
                extractor.Extract(compressedMovieFile.FullName, destinationPath);
            }
        }

        private void CopyTvRelease()
        {
            if (!Directory.Exists(ConfigSettings.TvPath + Output.Release.Name))
            {
                Directory.CreateDirectory(ConfigSettings.TvPath + Output.Release.Name);
            }

            if (string.IsNullOrEmpty(Output.FileName))
            {
                var tvFiles = GetFiles(new[] {".mkv", ".avi", ".wmv"});

                if (tvFiles.Any())
                {
                    CopyFiles(tvFiles, ConfigSettings.TvPath);
                }
                else
                {
                    var tvPath = Path.Combine(ConfigSettings.TvPath, Output.Release.Name);
                    tvFiles = GetFiles(new[] {".rar"});
                    ExtractFiles(tvFiles, tvPath);
                }
            }
            else
            {
                CopySingleFile(ConfigSettings.TvPath + Output.Release.Name);
            }
        }

        private void CopySingleFile(string destination)
        {
            Directory.SetCurrentDirectory(Output.DownloadPath);
            destination = Path.Combine(destination, Output.FileName);
            File.Copy(Output.FileName, destination);
        }

        private void CopyFiles(IEnumerable<FileInfo> files, string destination)
        {
            destination = Path.Combine(destination, Output.Release.Name);
            foreach (var file in files)
            {
                var fileDestination = Path.Combine(destination, file.Name);
                File.Copy(file.FullName, fileDestination);
            }
        }

        private IEnumerable<FileInfo> GetFiles(IEnumerable<string> extentions)
        {
            var sourceDirectory = new DirectoryInfo(Output.DownloadPath);
            var files = sourceDirectory.GetFiles();

            files = (from extention in extentions
                     from file in files
                     where file.Name.EndsWith(extention)
                     select file).ToArray();
            return files;
        }
    }
}