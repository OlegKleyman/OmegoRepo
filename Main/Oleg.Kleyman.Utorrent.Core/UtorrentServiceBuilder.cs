using System;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents a utorrent service builder.
    /// </summary>
    public class UtorrentServiceBuilder : IUtorrentServiceBuilder
    {
        /// <summary>
        /// Gets or sets the service URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the username for the service.
        /// </summary>
        public string Username { get; set; }

        private SecureString _password;
        /// <summary>
        /// Sets the password for the service.
        /// </summary>
        public string Password
        {
            set
            {
                _password = new SecureString();

                foreach (var character in value)
                {
                    _password.AppendChar(character);
                }

                _password.MakeReadOnly();
            }
        }

        /// <summary>
        /// Creates a utorrent service object.
        /// </summary>
        /// <param name="settings">The <see cref="ISettingsProvider"/> object containing the settings for this instance.</param>
        public UtorrentServiceBuilder(ISettingsProvider settings)
        {
            Url = settings.Url;
            Username = settings.Username;
            Password = settings.Password;
        }

        /// <summary>
        /// Gets the utorrent service.
        /// </summary>
        /// <returns>A <see cref="IUtorrentService"/> object.</returns>
        public IUtorrentService GetService()
        {
            var uTorrentCustomBinding = GetCustomBinding();
            var channelFactory = GetChannelFactory(uTorrentCustomBinding);

            var serviceClient = channelFactory.CreateChannel();
            return serviceClient;
        }

        private WebChannelFactory<IUtorrentService> GetChannelFactory(Binding uTorrentCustomBinding)
        {
            var uTorrentEndpointAddress = new EndpointAddress(Url);
            var channelFactory = new WebChannelFactory<IUtorrentService>(uTorrentCustomBinding);
            channelFactory.Endpoint.Address = uTorrentEndpointAddress;

            // ReSharper disable PossibleNullReferenceException
            channelFactory.Credentials.UserName.UserName = Username;
            // ReSharper restore PossibleNullReferenceException
            channelFactory.Credentials.UserName.Password = _password.ToUnsecureString();

            return channelFactory;
        }

        private static CustomBinding GetCustomBinding()
        {
            const string realm = "uTorrent";
            var customBinding = new CustomBinding(new WebMessageEncodingBindingElement
                                                                                     {
                                                                                         ContentTypeMapper = new JsonXmlContentTypeMapper()
                                                                                     }, 
                                                  new HttpTransportBindingElement
                                                                                {
                                                                                    ManualAddressing = true,
                                                                                    AuthenticationScheme = System.Net.AuthenticationSchemes.Basic,
                                                                                    Realm = realm,
                                                                                    AllowCookies = true
                                                                                });
            return customBinding;
        }
    }
}