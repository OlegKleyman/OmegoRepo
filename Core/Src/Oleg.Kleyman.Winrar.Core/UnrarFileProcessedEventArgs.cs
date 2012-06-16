using System;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarFileProcessedEventArgs : EventArgs
    {
        public ArchiveMember ArchiveMember { get; internal set; }

        public UnrarFileProcessedEventArgs(ArchiveMember archiveMember)
        {
            ArchiveMember = archiveMember;
        }
    }
}