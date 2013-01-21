namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///     Represents a rar status.
    /// </summary>
    public enum RarStatus
    {
        Success = 0,
        EndOfArchive = 10,
        InsufficientMemory = 11,
        BadData = 12,
        BadArchive = 13,
        UnknownFormat = 14,
        OpenError = 15,
        CreateError = 16,
        CloseError = 17,
        ReadError = 18,
        WriteError = 19,
        BufferTooSmall = 20,
        UnknownError = 21,
        MissingPassword = 22
    }
}