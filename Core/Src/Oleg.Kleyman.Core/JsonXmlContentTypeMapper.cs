using System.ServiceModel.Channels;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents a json xml content type mapper
    /// </summary>
    public class JsonXmlContentTypeMapper : WebContentTypeMapper
    {
        #region Overrides of WebContentTypeMapper

        /// <summary>
        /// Returns the message format used for a specified content type.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ServiceModel.Channels.WebContentFormat"/> that specifies the format to which the message content type is mapped. 
        /// </returns>
        /// <param name="contentType">The content type that indicates the MIME type of data to be interpreted.</param>
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            if (contentType.ToLower() == "text/plain" || contentType == "text/javascript")
            {
                return WebContentFormat.Json;
            }
            
            return WebContentFormat.Default;
        }

        #endregion
    }
}