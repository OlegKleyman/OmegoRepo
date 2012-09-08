namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///   Represents a file copier.
    /// </summary>
    public abstract class FileCopier
    {
        /// <summary>
        ///   The output to use to copy.
        /// </summary>
        /// <param name="output"> The <see cref="Output" /> object to copy. </param>
        /// <returns> </returns>
        public abstract Output Copy(Output output);
    }
}