using System.IO;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents an object extractor.
    /// </summary>
    /// <remarks>Such as a compressed archive like a zip file.</remarks>
    public abstract class Extractor
    {
        /// <summary>
        /// Extracts an object.
        /// </summary>
        /// <param name="target">The path to the object to extract.</param>
        /// <param name="destination">The extraction destination.</param>
        /// <returns>The extracted <see cref="IFileSystemMember" /> object.</returns>
        public abstract IFileSystemMember Extract(string target, string destination);
    }
}