namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///     Represents the high flags of the archive member.
    /// </summary>
    public enum HighMemberFlags : uint
    {
        DictionarySize64K = 0x00,
        DictionarySize128K = 0x20,
        DictionarySize256K = 0x40,
        DictionarySize512K = 0x60,
        DictionarySize1024K = 0x80,
        DictionarySize2048K = 0xA0,
        DictionarySize4096K = 0xC0,
        DirectoryRecord = 0xE0
    }
}