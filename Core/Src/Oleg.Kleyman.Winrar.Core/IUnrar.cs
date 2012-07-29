using System.IO;

namespace Oleg.Kleyman.Winrar.Core
{
    public interface IUnrar
    {
        IUnrarHandle Handle { get; set; }
        IArchiveReader ExecuteReader();
        FileSystemInfo Extract(string destinationPath);
    }
}