using System.Diagnostics;

namespace Oleg.Kleyman.Core
{
    public interface IProcessManager
    {
        IProcess Start(ProcessStartInfo startInfo);
    }
}