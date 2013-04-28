namespace Oleg.Kleyman.Winrar.Core.Extensions
{
    internal static class RarStatusExtensions
    {
        public static void ThrowOnInvalidStatus(this RarStatus status, RarOperation operation)
        {
            if (status != RarStatus.Success && status != RarStatus.EndOfArchive)
            {
                switch (operation)
                {
                    case RarOperation.ReadHeader:
                        const string unableToReadHeaderDataMessage = "Unable to read header data.";
                        throw new UnrarException(unableToReadHeaderDataMessage, status);
                    case RarOperation.Process:
                        const string unableToProcessFileMessage = "Unable to process member";
                        throw new UnrarException(unableToProcessFileMessage, status);
                }
            }
        }
    }
}
