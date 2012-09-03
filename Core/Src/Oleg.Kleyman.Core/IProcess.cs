using System.Diagnostics;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents a process.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Gets whether the process has exited.
        /// </summary>
        bool HasExited { get; }
        /// <summary>
        /// Blocks the current thread until the process exits.
        /// </summary>
        void WaitForExit();
        /// <summary>
        /// Gets or sets the priority of the process.
        /// </summary>
        ProcessPriorityClass PriorityClass { get; set; }
        /// <summary>
        /// Ends the process forcefully.
        /// </summary>
        void Kill();
    }
}