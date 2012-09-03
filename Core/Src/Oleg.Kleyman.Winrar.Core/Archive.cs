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
        /// <summary>
        /// Gets the files in the archive.
        /// </summary>
        public ReadOnlyCollection<ArchiveMember> Files { get; private set; }
        /// <summary>
        /// Gets the file path of the archive.
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// Gets the <see cref="IUnrar"/> object that the instance is interfacing with.
        /// </summary>
        public IUnrar Unrar { get; private set; }

        private Archive(IUnrar unrar)
        {
            Unrar = unrar;
            Files = new ReadOnlyCollection<ArchiveMember>(new ArchiveMember[] { });
        }

        /// <summary>
        /// Gets the unrar Archive.
        /// </summary>
        /// <param name="unrar"><see cref="IUnrar" /> to use when getting the Archive</param>
        /// <returns>The Archive.</returns>
        /// <remarks>This method changes the Mode property of the Handle in the <see cref="IUnrar"/> object to <see cref="OpenMode.Extract"/>.</remarks>
        public static IArchive Open(IUnrar unrar)
        {
            var archive = GetArchive(unrar);
            var reader = unrar.ExecuteReader();

            FillArchive(archive, reader);

            archive.Unrar.Handle.Close();
            archive.Unrar.Handle.Mode = OpenMode.Extract;
            archive.Unrar.Handle.Open();

            return archive;
        }

        private static void FillArchive(Archive archive, IArchiveReader reader)
        {
            var members = new Collection<ArchiveMember>();
            while (reader.Status != RarStatus.EndOfArchive)
            {
                var member = reader.Read();
                if (member != null)
                {
                    members.Add(member);
                }
            }

            archive.Files = new ReadOnlyCollection<ArchiveMember>(members);
        }

        private static Archive GetArchive(IUnrar unrar)
        {
            var archive = new Archive(unrar)
                              {
                                  FilePath = unrar.Handle.RarFilePath
                              };
            return archive;
        }


        /// <summary>
        /// Extracts the contents of the archive.
        /// </summary>
        /// <param name="destination">The destination file path for extraction.</param>
        /// <returns>The extracted members.</returns>
        public IFileSystemMember[] Extract(string destination)
        {
            var extractedMembers = ExtractArchive(destination);
            return extractedMembers;
        }

        private IFileSystemMember[] ExtractArchive(string destination)
        {
            var extractedMembers = Unrar.Extract(destination);
            return extractedMembers;
        }
    }
}