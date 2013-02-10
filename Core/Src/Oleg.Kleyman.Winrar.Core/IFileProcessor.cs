namespace Oleg.Kleyman.Winrar.Core
{
    public interface IFileProcessor
    {
        /// <summary>
        /// Processes a file.
        /// </summary>
        /// <param name="destinationPath">The target destination to extract to.</param>
        void ProcessFile(string destinationPath);
    }
}