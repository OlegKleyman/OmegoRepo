using System.Runtime.Serialization;

namespace Oleg.Kleyman.Utorrent.Core
{
    [DataContract(Namespace = "")]
    public abstract class UTorrentBase
    {
        [DataMember(Name = "build", Order = 1)]
        public int BuildNumber { get; set; }
    }
}