using System;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarFileProcessedEventArgs : EventArgs
    {
        public UnpackedFile UnpackedFile { get; internal set; }

        public UnrarFileProcessedEventArgs(UnpackedFile unpackedFile)
        {
            UnpackedFile = unpackedFile;
        }
    }
}