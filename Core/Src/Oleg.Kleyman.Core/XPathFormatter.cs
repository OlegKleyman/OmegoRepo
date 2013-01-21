using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///     Represents an XPath formatter.
    /// </summary>
    public class XPathFormatter : IClientMessageFormatter
    {
        /// <summary>
        ///     Instantiates an <see cref="XPathFormatter" /> object.
        /// </summary>
        /// <param name="clientMessageFormatter">
        ///     The <see cref="IClientMessageFormatter" /> to use with this instance.
        /// </param>
        /// <param name="xpathExpression">The XPath expression to use.</param>
        public XPathFormatter(IClientMessageFormatter clientMessageFormatter, string xpathExpression)
        {
            Formatter = clientMessageFormatter;
            XpathExpression = xpathExpression;
        }

        #region Implementation of IClientMessageFormatter

        /// <summary>
        ///     Converts an <see cref="T:System.Object" /> array into an outbound
        ///     <see
        ///         cref="T:System.ServiceModel.Channels.Message" />
        ///     .
        /// </summary>
        /// <returns>
        ///     The SOAP message sent to the service operation.
        /// </returns>
        /// <param name="messageVersion">The version of the SOAP message to use.</param>
        /// <param name="parameters">The parameters passed to the WCF client operation.</param>
        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            return Formatter.SerializeRequest(messageVersion, parameters);
        }

        /// <summary>
        ///     Converts a message into a return value and out parameters that are passed back to the calling operation.
        /// </summary>
        /// <returns>
        ///     The return value of the operation.
        /// </returns>
        /// <param name="message">The inbound message.</param>
        /// <param name="parameters">Any out values.</param>
        public object DeserializeReply(Message message, object[] parameters)
        {
            var deserializedMessage = message.GetBody<XElement>();
            var entities = (IEnumerable<object>) deserializedMessage.XPathEvaluate(XpathExpression);
            if (entities.Any())
            {
                var result = entities.First();

                if (result.GetType() == typeof (XText))
                {
                    result = ((XText) entities.First()).Value;
                }

                return result;
            }

            throw new InvalidOperationException("XPath query did not yield any results.");
        }

        #endregion

        /// <summary>
        ///     Gets the <see cref="IClientMessageFormatter" /> object used by this instance.
        /// </summary>
        public IClientMessageFormatter Formatter { get; private set; }

        /// <summary>
        ///     Gets the XPath expression used by this object.
        /// </summary>
        public string XpathExpression { get; private set; }
    }
}