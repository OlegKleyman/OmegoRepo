using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class XbmcFileCopier : FileCopier
    {
        public Release Release { get; set; }
        public string FileName { get; set; }
        public string DownloadPath { get; set; }
        protected ISettingsProvider ConfigSettings { get; set; }

        public XbmcFileCopier(ISettingsProvider settings, Release release, string fileName, string downloadPath)
        {
            Release = release;
            FileName = fileName;
            DownloadPath = downloadPath;
            ConfigSettings = settings;
        }

        public override void Copy()
        {
            switch (Release.ReleaseType)
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
                    throw new ApplicationException(string.Format("Release type of {0} is not supported.", Enum.GetName(Release.ReleaseType.GetType(), Release.ReleaseType)));
            }
        }

        private void CopyMovieRelease()
        {
            if (!Directory.Exists(ConfigSettings.TvPath + Release.Name))
            {
                Directory.CreateDirectory(ConfigSettings.MoviesPath + Release.Name);
            }

            var compressedMovieFiles = GetFiles(new[] { ".rar" });
            if(compressedMovieFiles.Any())
            {
                ExtractFiles(compressedMovieFiles, ConfigSettings.MoviesPath + Release.Name);
            }
            else
            {
                CopySingleFile(ConfigSettings.MoviesPath + Release.Name);
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
            if (!Directory.Exists(ConfigSettings.TvPath + Release.Name))
            {
                Directory.CreateDirectory(ConfigSettings.TvPath + Release.Name);
            }

            if (string.IsNullOrEmpty(FileName))
            {
                var tvFiles = GetFiles(new[] { ".mkv", ".avi", ".wmv" });

                if (tvFiles.Any())
                {
                    CopyFiles(tvFiles, ConfigSettings.TvPath);
                }
                else
                {
                    tvFiles = GetFiles(new[] { ".rar"});
                    ExtractFiles(tvFiles, ConfigSettings.TvPath + Release.Name);
                }
            }
            else
            {
                CopySingleFile(ConfigSettings.TvPath + Release.Name);
            }
        }

        private void CopySingleFile(string destination)
        {
            Directory.SetCurrentDirectory(DownloadPath);
            if(!destination.EndsWith("\\"))
            {
                destination += "\\";
            }
            File.Copy(FileName, destination + FileName);
        }

        private void CopyFiles(IEnumerable<FileInfo> files, string destination)
        {
            foreach (var file in files)
            {
                File.Copy(file.FullName, destination + Release.Name + @"\" + file.Name);
            }
        }

        private IEnumerable<FileInfo> GetFiles(IEnumerable<string> extentions)
        {
            var sourceDirectory = new DirectoryInfo(DownloadPath);
            var files = sourceDirectory.GetFiles();

            files = (from extention in extentions
                     from file in files
                     where file.Name.EndsWith(extention)
                     select file).ToArray();
            return files;
        }
    }
}