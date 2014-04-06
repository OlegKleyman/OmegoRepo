using System.ServiceModel;
using System.ServiceModel.Web;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents a UTorrent service.
    /// </summary>
    [ServiceContract]
    public interface IUtorrentService
    {
        /// <summary>
        /// Gets the session key to use for UTorrent operations.
        /// </summary>
        /// <returns>The session key</returns>
        [OperationContract]
        [WebGet(UriTemplate = "/token.html")]
        [XPathBehavior("./div/text()")]
        string GetKey();

        /// <summary>
        /// Gets a torrent by hash.
        /// </summary>
        /// <param name="key">The session key for UTorrent operations.</param>
        /// <param name="hash">The target torrent hash.</param>
        /// <returns>A <see cref="Torrent"/> object.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "/?token={key}&action=getfiles&hash={hash}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Torrent GetTorrentFiles(string key, string hash);

        /// <summary>
        /// Gets the UTorrent list.
        /// </summary>
        /// <param name="key">The session key for UTorrent operations.</param>
        /// <returns>A <see cref="UTorrentList"/> object with service information.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "/?token={key}&list=1",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        UTorrentList GetList(string key);

        [OperationContract]
        [WebGet(UriTemplate = "/?token={key}&action=rss-update&feed-id={id}&update=1")]
        UTorrentBase UpdateRssFeed(string key, int id);

        [OperationContract]
        [WebGet(UriTemplate = "/?token={key}&action=remove&hash={hash}")]
        UTorrentBase Remove(string key, string hash);
    }
}