using System;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///   Represents the low flags of the archive member.
    /// </summary>
    [Flags]
    public enum LowMemberFlags
    {
        None = 0,
        FileContinuedFromPerviousVolume = 0x01,
        FileContinuedOnNextVolume = 0x02,
        FileEncryptedWithPassword = 0x04,
        FileCommentPresent = 0x08,
        PreviousFilesDataIsUsed = 0x10,
    }
}