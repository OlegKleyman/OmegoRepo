using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public sealed class XbmcFileCopier : FileCopier
    {
        public XbmcFileCopier(ISettingsProvider settings, XbmcFileCopierDependencies xbmcFileCopierDependencies)
        {
            ConfigSettings = settings;
            Dependencies = xbmcFileCopierDependencies;
        }

        public XbmcFileCopierDependencies Dependencies { get; set; }
        private ISettingsProvider ConfigSettings { get; set; }


        public override Output Copy(Output output)
        {
            ThrowInvalidArgumentExceptionIfOutputIsInvalidType(output);
            var releaseOutput = (ReleaseOutput) output;
            
            switch (releaseOutput.Release.ReleaseType)
            {
                case ReleaseType.Tv:
                    CopyTvRelease(releaseOutput);
                    break;
                case ReleaseType.Movie:
                    CopyMovieRelease(releaseOutput);
                    break;
                case ReleaseType.Other:
                    goto default;
                default:
                    throw new ApplicationException(string.Format("Release type of {0} is not supported.",
                                                                 Enum.GetName(releaseOutput.Release.ReleaseType.GetType(),
                                                                              releaseOutput.Release.ReleaseType)));
            }

            var copiedOutput = GetOutput(releaseOutput);
            return null;
        }

        private Output GetOutput(ReleaseOutput releaseOutput)
        {
            if(releaseOutput.Release.ReleaseType ==  ReleaseType.Tv)
            {
                var targetDirectory = Path.Combine(ConfigSettings.TvPath, releaseOutput.Release.Name);
                if(!string.IsNullOrEmpty(releaseOutput.FileName))
                {
                    var fileExists = Dependencies.FileSystem.FileExists(Path.Combine(targetDirectory, releaseOutput.FileName));
                    if(fileExists)
                    {
                        return new Output(releaseOutput.FileName, targetDirectory);
                    }
                }
            }
            //TODO: Finish
            return null;
        }

        private void ThrowInvalidArgumentExceptionIfOutputIsInvalidType(object output)
        {
            if(!(output is ReleaseOutput))
            {
                const string mustBeOfTypeReleaseoutputMessage = "Must be of type ReleaseOutput";
                const string outputName = "output";
                throw new ArgumentException(mustBeOfTypeReleaseoutputMessage, outputName);
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
                Dependencies.Extractor.Extract(compressedMovieFile.FullName, destinationPath);
            }
        }

        private void CopyTvRelease(ReleaseOutput output)
        {
            if (!Dependencies.FileSystem.DirectoryExists(ConfigSettings.TvPath + output.Release.Name))
            {
                Dependencies.FileSystem.CreateDirectory(ConfigSettings.TvPath + output.Release.Name);
            }

            var destination = Path.Combine(ConfigSettings.TvPath, output.Release.Name);
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
                    tvFiles = GetFiles(new[] {".rar"}, output);
                    ExtractFiles(tvFiles, destination);
                }
            }
            else
            {
                CopySingleFile(destination, output);
            }
        }

        private void CopySingleFile(string destination, ReleaseOutput output)
        {
            var sourceFilePath = Path.Combine(output.TargetDirectory, output.FileName);
            destination = Path.Combine(destination, output.FileName);
            Dependencies.FileSystem.CopyFile(sourceFilePath, destination);
        }

        private void CopyFiles(IEnumerable<FileInfo> files, string destination, ReleaseOutput output)
        {
            destination = Path.Combine(destination, output.Release.Name);
            foreach (var file in files)
            {
                var fileDestination = Path.Combine(destination, file.Name);
                Dependencies.FileSystem.CopyFile(file.FullName, fileDestination);
            }
        }

        private IEnumerable<FileInfo> GetFiles(IEnumerable<string> extentions, ReleaseOutput output)
        {
            var files = Dependencies.FileSystem.GetFilesByExtensions(output.TargetDirectory, extentions);

            return files;
        }
    }
}