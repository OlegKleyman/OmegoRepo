namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///     Represents a singleton object.
    /// </summary>
    public class Singleton
    {
        private static readonly Singleton r__instance;

        /// <summary>
        ///     Default static constructor.
        /// </summary>
        static Singleton()
        {
            r__instance = new Singleton();
        }

        private Singleton()
        {
        }

        /// <summary>
        ///     Gets singleton object instance of this type.
        /// </summary>
        public static Singleton Instance
        {
            get { return r__instance; }
        }
    }
}