using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class Archive : IArchive
    {
        public IFileSystem FileSystem { get; private set; }
        public IUnrarHandle Handle { get; internal set; }
        public ReadOnlyCollection<ArchiveMember> Files { get; private set; }
        public string FilePath { get; private set; }
        public static IUnrar Unrar { get; private set; }

        private Archive(IFileSystem fileSystem, IUnrarHandle handle)
        {
            FileSystem = fileSystem;
            Handle = handle;
            Files = new ReadOnlyCollection<ArchiveMember>(new ArchiveMember[] { });
        }

        /// <summary>
        /// Gets the unrar Archive.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/> to use for file operations.</param>
        /// <param name="unrar"><see cref="IUnrar" /> to use when getting the Archive</param>
        /// <returns>The Archive.</returns>
        public static IArchive Open(IFileSystem fileSystem, IUnrar unrar)
        {
            Unrar = unrar;
            var reader = unrar.ExecuteReader();

            return Open(fileSystem, reader);
        }

        private static void FillArchive(Archive archive, IArchiveReader reader)
        {
            var members = new Collection<ArchiveMember>();
            while (reader.Status != RarStatus.EndOfArchive)
            {
                var member = reader.Read();
                members.Add(member);
            }

            archive.Files = new ReadOnlyCollection<ArchiveMember>(members);
        }

        private static Archive GetArchive(IFileSystem fileSystem, IUnrarHandle handle)
        {
            var archive = new Archive(fileSystem, handle)
                              {
                                  FilePath = handle.RarFilePath
                              };
            return archive;
        }

        /// <summary>
        /// Gets the unrar Archive.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/> to use for file operations.</param>
        /// <param name="reader"><see cref="IArchiveReader"/> to use when getting the Archive</param>
        /// <returns>The Archive.</returns>
        public static IArchive Open(IFileSystem fileSystem, IArchiveReader reader)
        {
            var archive = GetArchive(fileSystem, reader.Handle);
            FillArchive(archive, reader);

            archive.Handle.Close();
            archive.Handle.Mode = OpenMode.Extract;
            archive.Handle.Open();

            return archive;
        }

        /// <summary>
        /// Extracts the contents of the archive.
        /// </summary>
        /// <param name="destination">The destination file path for extraction.</param>
        /// <returns>The destination file path.</returns>
        public FileSystemInfo Extract(string destination)
        {
            var destinationDirectory = ExtractArchive(destination);
            return destinationDirectory;
        }

        private FileSystemInfo ExtractArchive(string destination)
        {
            if(Unrar == null)
            {
                Unrar = new Unrar(Handle, FileSystem);
            }
            var extractPath = Unrar.Extract(destination);
            return extractPath;
        }
    }
}