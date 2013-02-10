namespace Oleg.Kleyman.Winrar.Core
{
    public interface IMemberExtractor
    {
        /// <summary>
        /// Extracts a member.
        /// </summary>
        /// <param name="destinationPath">The destination path to extract to.</param>
        /// <returns>The status of the extraction.</returns>
        RarStatus Extract(string destinationPath);

        /// <summary>
        /// Gets the current member extracted.
        /// </summary>
        ArchiveMember CurrentMember { get; }
    }
}