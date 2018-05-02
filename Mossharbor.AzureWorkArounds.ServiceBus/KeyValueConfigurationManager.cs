using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    class KeyValueConfigurationManager
    {
        private readonly static Regex ValueRegex;
        private readonly static Regex KeyRegex;
        string connectionString = null;
        private KeyValueConfigurationManager.ReadonlyNameValueCollection connectionProperties;

        static KeyValueConfigurationManager()
        {
            KeyValueConfigurationManager.KeyRegex = new Regex("(OperationTimeout|Endpoint|EntityPath|Publisher|RuntimePort|ManagementPort|StsEndpoint|ViaEndpoint|WindowsDomain|WindowsUsername|WindowsPassword|OAuthDomain|OAuthUsername|OAuthPassword|SharedSecretIssuer|SharedSecretValue|SharedAccessKeyName|SharedAccessKey|SharedAccessSignature|TransportType|EnableAmqpLinkRedirect|AmqpSecurityScheme|HostName)", RegexOptions.IgnoreCase);
            KeyValueConfigurationManager.ValueRegex = new Regex("([^\\s]+)");
        }

        public KeyValueConfigurationManager(string connectionString)
        {
            this.Initialize(connectionString);
        }

        public string this[string key]
        {
            get
            {
                return this.connectionProperties[key];
            }
        }

        private void Initialize(string connection)
        {
            this.connectionString = connection;
            this.connectionProperties = new KeyValueConfigurationManager.ReadonlyNameValueCollection(KeyValueConfigurationManager.CreateNameValueCollectionFromConnectionString(this.connectionString));
        }

        private static NameValueCollection CreateNameValueCollectionFromConnectionString(string connectionString)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                string str = string.Concat(";", connectionString);
                string[] strArrays = Regex.Split(str, ";(OperationTimeout|Endpoint|EntityPath|Publisher|RuntimePort|ManagementPort|StsEndpoint|ViaEndpoint|WindowsDomain|WindowsUsername|WindowsPassword|OAuthDomain|OAuthUsername|OAuthPassword|SharedSecretIssuer|SharedSecretValue|SharedAccessKeyName|SharedAccessKey|SharedAccessSignature|TransportType|EnableAmqpLinkRedirect|AmqpSecurityScheme|HostName)=", RegexOptions.IgnoreCase);
                if (strArrays.Length != 0)
                {
                    if (!string.IsNullOrWhiteSpace(strArrays[0]))
                    {
                        throw new Exception("ConnectionStringInvalidFormat");
                    }
                    if ((int)strArrays.Length % 2 != 1)
                    {
                        throw new ConfigurationErrorsException("ConnectionStringInvalidFormat");
                    }
                    for (int i = 1; i < (int)strArrays.Length; i++)
                    {
                        string str1 = strArrays[i];
                        if (string.IsNullOrWhiteSpace(str1) || !KeyValueConfigurationManager.KeyRegex.IsMatch(str1))
                        {
                            if (string.IsNullOrWhiteSpace(str1))
                            {
                                str1 = "NullKeyName";
                            }
                            throw new ConfigurationErrorsException("AppSettingsConfigSettingInvalidKey");
                        }
                        string str2 = strArrays[i + 1];
                        if (string.IsNullOrWhiteSpace(str2) || !KeyValueConfigurationManager.ValueRegex.IsMatch(str2))
                        {
                            throw new ConfigurationErrorsException("AppSettingsConfigSettingInvalidValue");
                        }
                        if (nameValueCollection[str1] != null)
                        {
                            throw new ConfigurationErrorsException("AppSettingsConfigDuplicateSetting");
                        }
                        nameValueCollection[str1] = str2;
                        i++;
                    }
                }
            }
            return nameValueCollection;
        }

        internal sealed class ReadonlyNameValueCollection : NameValueCollection
        {
            public ReadonlyNameValueCollection(NameValueCollection collection) : base(collection)
            {
                base.IsReadOnly = true;
            }
        }

        public static IList<Uri> GetEndpointAddresses(string uriEndpoints, string portString)
        {
            return KeyValueConfigurationManager.GetEndpointAddresses(uriEndpoints, string.Empty, portString);
        }

        public static IList<Uri> GetEndpointAddresses(string uriEndpoints, string hostNames, string portString)
        {
            string[] strArrays;
            int num;
            List<Uri> uris = new List<Uri>();
            if (string.IsNullOrWhiteSpace(uriEndpoints) && string.IsNullOrWhiteSpace(hostNames))
            {
                return uris;
            }
            bool flag = false;
            if (string.IsNullOrWhiteSpace(uriEndpoints))
            {
                flag = true;
                strArrays = hostNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (strArrays.Length == 0)
                {
                    return uris;
                }
            }
            else
            {
                strArrays = uriEndpoints.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (strArrays.Length == 0)
                {
                    return uris;
                }
            }
            if (!int.TryParse(portString, out num))
            {
                num = -1;
            }
            string[] strArrays1 = strArrays;
            for (int i = 0; i < (int)strArrays1.Length; i++)
            {
                UriBuilder uriBuilder = new UriBuilder(strArrays1[i]);
                if (num > 0)
                {
                    uriBuilder.Port = num;
                }
                if (flag)
                {
                    uriBuilder.Scheme = "sb";
                }
                uris.Add(uriBuilder.Uri);
            }
            return uris;
        }

        public NamespaceManager CreateNamespaceManager()
        {
            NamespaceManager namespaceManager;
            IEnumerable<Uri> endpointAddresses = KeyValueConfigurationManager.GetEndpointAddresses(this.connectionProperties["Endpoint"], this.connectionProperties["ManagementPort"]);
            string str1 = this.connectionProperties["SharedAccessKeyName"];
            string item2 = this.connectionProperties["SharedAccessKey"];

            SharedAccessSignatureTokenProvider tokenProvider = new SharedAccessSignatureTokenProvider(str1, item2, SharedAccessSignatureTokenProvider.DefaultTokenTimeout);
            namespaceManager = new NamespaceManager(endpointAddresses, tokenProvider);

            return namespaceManager;
        }
    }
}
