namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    ///   Represents a validator
    /// </summary>
    public abstract class Validator
    {
        /// <summary>
        ///   Validates a scenario.
        /// </summary>
        /// <returns> True if valid and false if not. </returns>
        public abstract bool Validate();
    }
}