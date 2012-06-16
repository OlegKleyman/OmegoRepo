using System;

namespace Oleg.Kleyman.Winrar.Core
{
    public enum ArchiveMemberFlags : uint
    {
        DictionarySize64K = 0,
        FileContinuedFromPerviousVolume = 1,
        FileContinuedOnNextVolume = 2,
        FileEncryptedWithPassword = 4,
        FileCommentPresent = 8,
        PreviousFilesDataIsUsed = 16,
        DictionarySize128K = 32,
        DictionarySize256K = 64,
        DictionarySize512K = 0x60,
        DictionarySize1024K = 0x80,
        DictionarySize2048K = 0xA0,
        DictionarySize4096K = 0xC0,
        DirectoryRecord = 0xE0
    }
}
