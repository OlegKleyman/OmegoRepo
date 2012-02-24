using System.Diagnostics;

namespace Oleg.Kleyman.Core
{
    public class SystemProcess : IProcess
    {
        protected Process Process { get; private set; }

        internal SystemProcess(Process systemProcess)
        {
            Process = systemProcess;
        }

        #region Implementation of IProcess

        public bool HasExited
        {
            get { return Process.HasExited; }
        }

        public void WaitForExit()
        {
            Process.WaitForExit();
        }
        
        public ProcessPriorityClass PriorityClass
        {
            get { return Process.PriorityClass; }
            set { Process.PriorityClass = value; }
        }

        public void Kill()
        {
            Process.Kill();
        }

        #endregion
    }
}