namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///   Represents a file output.
    /// </summary>
    public class Output
    {
        /// <summary>
        ///   Initializes a <see cref="Output" /> object.
        /// </summary>
        /// <param name="fileName"> The file name of the output object. </param>
        /// <param name="targetDirectory"> The target directory of the output object. </param>
        public Output(string fileName, string targetDirectory)
        {
            FileName = fileName;
            TargetDirectory = targetDirectory;
        }

        /// <summary>
        ///   Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///   Gets or sets the target directory.
        /// </summary>
        public string TargetDirectory { get; set; }
    }
}