using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    [Serializable]
    internal class SharedAccessSignatureTokenProvider
    {
        internal readonly static TimeSpan DefaultTokenTimeout = TimeSpan.FromMinutes(60);
        public readonly static DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private string sharedAccessKeyName;
        private string sharedAccessKey;
        private TimeSpan tokenTimeToLive;
        internal readonly byte[] encodedSharedAccessKey;
        private Func<string, byte[]> customKeyEncoder = new Func<string, byte[]>(Encoding.UTF8.GetBytes);

        public SharedAccessSignatureTokenProvider(string sharedAccessKeyName, string sharedAccessKey, TimeSpan tokenTimeout)
        {
            this.sharedAccessKeyName = sharedAccessKeyName;
            this.sharedAccessKey = sharedAccessKey;
            this.tokenTimeToLive = tokenTimeout;

            if (string.IsNullOrEmpty(sharedAccessKeyName))
            {
                throw new ArgumentNullException("sharedAccessKeyName");
            }
            if (sharedAccessKey.Length > 256)
            {
                throw new ArgumentOutOfRangeException("sharedAccessKey");
            }
            if (string.IsNullOrEmpty(sharedAccessKey))
            {
                throw new ArgumentNullException("sharedAccessKey");
            }
            if (sharedAccessKey.Length > 256)
            {
                throw new ArgumentOutOfRangeException("sharedAccessKey");
            }
            this.encodedSharedAccessKey = customKeyEncoder(sharedAccessKey);
        }
        
        private static string BuildExpiresOn(TimeSpan timeToLive)
        {
            DateTime dateTime = DateTime.UtcNow.Add(timeToLive);
            TimeSpan timeSpan = dateTime.Subtract(SharedAccessSignatureTokenProvider.EpochTime);
            long num = Convert.ToInt64(timeSpan.TotalSeconds, CultureInfo.InvariantCulture);
            return Convert.ToString(num, CultureInfo.InvariantCulture);
        }

        private static string Sign(string requestString, byte[] encodedSharedAccessKey)
        {
            string base64String;
            using (HMACSHA256 hMACSHA256 = new HMACSHA256(encodedSharedAccessKey))
            {
                base64String = Convert.ToBase64String(hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(requestString)));
            }
            return base64String;
        }

        public string BuildSignature(string targetUri)
        {
            return BuildSignature(sharedAccessKeyName, encodedSharedAccessKey, targetUri, tokenTimeToLive);
        }

        private static string BuildSignature(string keyName, byte[] encodedSharedAccessKey, string targetUri, TimeSpan timeToLive)
        {
            string str = BuildExpiresOn(timeToLive);
            string str1 = System.Web.HttpUtility.UrlEncode(targetUri);
            List<string> strs = new List<string>()
                {
                    str1,
                    str
                };
            string str2 = Sign(string.Join("\n", strs), encodedSharedAccessKey);
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}={2}&{3}={4}&{5}={6}&{7}={8}", new object[] { "SharedAccessSignature", "sr", str1, "sig", System.Web.HttpUtility.UrlEncode(str2), "se", System.Web.HttpUtility.UrlEncode(str), "skn", System.Web.HttpUtility.UrlEncode(keyName) });
        }
    }
}