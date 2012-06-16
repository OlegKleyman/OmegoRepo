namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Mode in which archive is to be opened for processing.
    /// </summary>
    public enum OpenMode : uint
    {
        /// <summary>
        /// Open archive for listing contents only
        /// </summary>
        List = 0,
        /// <summary>
        /// Open archive for testing or extracting contents
        /// </summary>
        Extract = 1
    }
}