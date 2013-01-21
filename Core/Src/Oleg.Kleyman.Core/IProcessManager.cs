using System.Diagnostics;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///     Represents a process manager.
    /// </summary>
    public interface IProcessManager
    {
        /// <summary>
        ///     Starts a process.
        /// </summary>
        /// <param name="startInfo">
        ///     The <see cref="ProcessStartInfo" /> object containing the process details.
        /// </param>
        /// <returns>
        ///     A <see cref="IProcess" /> object.
        /// </returns>
        IProcess Start(ProcessStartInfo startInfo);
    }
}