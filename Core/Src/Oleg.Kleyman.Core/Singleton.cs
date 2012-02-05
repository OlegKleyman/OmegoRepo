namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents a singleton object.
    /// </summary>
    public class Singleton
    {
        private static readonly Singleton __instance;

        /// <summary>
        /// Default static constructor.
        /// </summary>
        static Singleton()
        {
            __instance = new Singleton();
        }

        private Singleton()
        {
        }

        /// <summary>
        /// Gets singleton object instance of this type.
        /// </summary>
        public static Singleton Instance
        {
            get { return __instance; }
        }
    }
}