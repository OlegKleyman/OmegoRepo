using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Sandbox.Javascription.Serialization.Test
{
    [DataContract]
    public class Torrents
    {
        [DataMember(Name = "torrents")]
        public List<List<object>> TorrentList { get; set; }
    }
}