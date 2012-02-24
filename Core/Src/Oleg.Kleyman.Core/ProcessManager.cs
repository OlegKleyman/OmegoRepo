using System.Diagnostics;

namespace Oleg.Kleyman.Core
{
    public class ProcessManager : IProcessManager
    {
        #region Implementation of IProcessManager

        public IProcess Start(ProcessStartInfo startInfo)
        {
            var process = Process.Start(startInfo);
            return new SystemProcess(process);
        }

        #endregion
    }
}