using System.ServiceModel;
using System.ServiceModel.Web;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Utorrent.Core
{
    [ServiceContract]
    public interface IUtorrentService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/token.html")]
        [XPathBehavior("./div/text()")]
        string GetKey();

        [OperationContract]
        [WebGet(UriTemplate = "/?token={key}&action=getfiles&hash={hash}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Torrent GetTorrentFiles(string key, string hash);
    }
}