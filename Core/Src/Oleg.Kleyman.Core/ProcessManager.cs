using System.Diagnostics;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///     Represents a process manager.
    /// </summary>
    /// <remarks>
    ///     Controls a process or processes.
    /// </remarks>
    public class ProcessManager : IProcessManager
    {
        #region Implementation of IProcessManager

        /// <summary>
        ///     Starts a process.
        /// </summary>
        /// <param name="startInfo">
        ///     The <see cref="ProcessStartInfo" /> object containing the process details.
        /// </param>
        /// <returns>
        ///     A <see cref="IProcess" /> object.
        /// </returns>
        public IProcess Start(ProcessStartInfo startInfo)
        {
            var process = Process.Start(startInfo);
            return new SystemProcess(process);
        }

        #endregion
    }
}