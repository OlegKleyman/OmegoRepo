using System.Diagnostics;

namespace Oleg.Kleyman.Core
{
    public interface IProcess
    {
        bool HasExited { get; }
        void WaitForExit();
        ProcessPriorityClass PriorityClass { get; set; }
        void Kill();
    }
}