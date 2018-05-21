using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    using System.Xml;
    using System.Xml.Serialization;

    static class WebClientExtensions
    {
        private const string userAgentTemplate = "SERVICEBUS/2017-04(api-origin=DotNetSdk;os={0};os-version={1})";
        
        public static entry DownloadEntryXml(this WebClient request, string saddress)
        {
            string response = request.DownloadString(saddress);
            if (!response.StartsWith("<entry"))
                return null;
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(entry));
            return (entry)xs.Deserialize(new StringReader(response));
        }

        public static entry UploadEntryXml(this WebClient request, string saddress, entry XmlPayload)
        {
            return request.UploadEntryXml(saddress, XmlPayload.ToXml());
        }

        public static entry UploadEntryXml(this WebClient request, string saddress, string xmlPayload)
        {
            string response = request.UploadString(saddress, "PUT", xmlPayload);
            if (!response.StartsWith("<entry"))
                return null;
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(entry));
            return (entry)xs.Deserialize(new StringReader(response));
        }

        public static void AddCommmonHeaders(this WebClient request, SharedAccessSignatureTokenProvider provider, string address, bool addContentType = true, bool addAnonHeader = false, bool addIfMatchheader = false, string qaddress = null)
        { 
            if (addContentType)
                request.AddContentType();
            if (addAnonHeader)
                request.Headers.Add("X-MS-ISANONYMOUSACCESSIBLE", "False");
            if (addIfMatchheader)
                request.Headers.Add("If-Match", "*");
            request.SetUserAgentHeader();
            request.AddXProcessAtHeader();
            request.AddAuthorizationHeader(provider, address);

            if (!string.IsNullOrWhiteSpace(qaddress))
            {
                request.AddServiceBusSupplementaryAuthorizationHeader(provider, qaddress);
            }

            request.AddTrackingIdHeader(Guid.NewGuid());
        }

        public static void AddAuthorizationHeader(this WebClient request, SharedAccessSignatureTokenProvider tokenProvider, string address)
        {
            if (tokenProvider != null)
            {
                string messagingWebToken = tokenProvider.BuildSignature(address);
                request.Headers[HttpRequestHeader.Authorization] = messagingWebToken;
            }
        }

        public static void AddServiceBusSupplementaryAuthorizationHeader(this WebClient request, SharedAccessSignatureTokenProvider tokenProvider, string address)
        {
            if (tokenProvider != null)
            {
                string messagingWebToken = tokenProvider.BuildSignature(address); //tokenProvider.GetMessagingWebToken(namespaceAddress, request.RequestUri.AbsoluteUri, action, false, Constants.TokenRequestOperationTimeout);
                request.Headers["ServiceBusSupplementaryAuthorization"] = messagingWebToken;
            }
        }

        public static void AddContentType(this WebClient request)
        {
            request.Headers["Content-Type"] = "application/atom+xml;type=entry;charset=utf-8";

        }
         public static void AddTrackingIdHeader(this WebClient request, Guid trackingContext)
        {
            if (trackingContext != null)
            {
                request.Headers["TrackingId"] = trackingContext.ToString();
            }
        }

        public static void AddXProcessAtHeader(this WebClient request)
        {
            request.Headers.Add("X-PROCESS-AT", "ServiceBus");
        }
        
        public static void SetUserAgentHeader(this WebClient request)
        {
            request.Headers["User-Agent"] = string.Format(CultureInfo.InvariantCulture, "SERVICEBUS/2017-04(api-origin=DotNetSdk;os={0};os-version={1})", new object[] { Environment.OSVersion.Platform, Environment.OSVersion.Version });
        }
    }
}
