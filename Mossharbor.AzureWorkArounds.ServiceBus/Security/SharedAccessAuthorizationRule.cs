using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    [XmlRoot(ElementName = "AuthorizationRule", DataType = "SharedAccessAuthorizationRule", IsNullable = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    public class SharedAccessAuthorizationRule : AuthorizationRule
    {

        [XmlAttribute("type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Type { get; set; }

        private string keyName;
        /// <summary>Gets or sets the authorization rule key name.</summary>
		/// <value>The authorization rule key name.</value>
		public sealed override string KeyName
        {
            get
            {
                return this.keyName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("KeyName");
                }
                if (string.Equals(value, "$system"))
                {
                    throw new ArgumentException("Shared Access Authorization Rule Key Contains Invalid Characters");
                }
                if (!string.Equals(this.keyName, HttpUtility.UrlEncode(this.keyName)))
                {
                    throw new ArgumentException("Shared Access Authorization Rule Key Contains Invalid Characters");
                }
                if (value.Length > 256)
                {
                    throw new ArgumentOutOfRangeException("Key Name is limited to 256 characters");
                }
                this.keyName = value;
            }
        }

        private string primaryKey;
        /// <summary>Gets or sets the primary key for the authorization rule.</summary>
		/// <value>The primary key for the authorization rule.</value>
		public string PrimaryKey
        {
            get
            {
                return this.primaryKey;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("PrimaryKey");
                }
                if (value.Length > 256)
                {
                    throw new ArgumentOutOfRangeException("PrimaryKey is limted to 256");
                }
                this.primaryKey = value;
            }
        }

        private string secondaryKey;
        /// <summary>Gets or sets the secondary key for the authorization rule.</summary>
        /// <value>The secondary key for the authorization rule.</value>
        public string SecondaryKey
        {
            get
            {
                return this.secondaryKey;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.secondaryKey = value;
                }
                else if (value.Length > 256)
                {
                    throw new ArgumentOutOfRangeException("SecondaryKey is limted to 256");
                }
                this.secondaryKey = value;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.SharedAccessAuthorizationRule" /> class.</summary>
		/// <param name="keyName">The authorization rule key name.</param>
		/// <param name="rights">The list of rights.</param>
		public SharedAccessAuthorizationRule(string keyName, IEnumerable<AccessRights> rights) : this(keyName, SharedAccessAuthorizationRule.GenerateRandomKey(), SharedAccessAuthorizationRule.GenerateRandomKey(), rights)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.SharedAccessAuthorizationRule" /> class.</summary>
        /// <param name="keyName">The authorization rule key name.</param>
        /// <param name="primaryKey">The primary key for the authorization rule.</param>
        /// <param name="rights">The list of rights.</param>
        public SharedAccessAuthorizationRule(string keyName, string primaryKey, IEnumerable<AccessRights> rights) : this(keyName, primaryKey, SharedAccessAuthorizationRule.GenerateRandomKey(), rights)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.SharedAccessAuthorizationRule" /> class.</summary>
        /// <param name="keyName">The authorization rule key name.</param>
        /// <param name="primaryKey">The primary key for the authorization rule.</param>
        /// <param name="secondaryKey">The secondary key for the authorization rule.</param>
        /// <param name="rights">The list of rights.</param>
        public SharedAccessAuthorizationRule(string keyName, string primaryKey, string secondaryKey, IEnumerable<AccessRights> rights)
        {
            this.Type = "SharedAccessAuthorizationRule";
            base.ClaimType = "SharedAccessKey";
            base.ClaimValue = "None";
            this.PrimaryKey = primaryKey;
            this.SecondaryKey = secondaryKey;
            base.Rights = new List<AccessRights>(rights);
            this.KeyName = keyName;
        }

        public SharedAccessAuthorizationRule()
        {
            this.Type = "SharedAccessAuthorizationRule";
        }

        private static bool CheckBase64(string base64EncodedString)
        {
            bool flag;
            try
            {
                Convert.FromBase64String(base64EncodedString);
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
            {
                return false;
            }
            SharedAccessAuthorizationRule sharedAccessAuthorizationRule = (SharedAccessAuthorizationRule)obj;
            if (!string.Equals(this.KeyName, sharedAccessAuthorizationRule.KeyName, StringComparison.OrdinalIgnoreCase) || !string.Equals(this.PrimaryKey, sharedAccessAuthorizationRule.PrimaryKey, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return string.Equals(this.SecondaryKey, sharedAccessAuthorizationRule.SecondaryKey, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Generates the random key for the authorization rule.</summary>
		/// <returns>The random key for the authorization rule.</returns>
		public static string GenerateRandomKey()
        {
            byte[] numArray = new byte[32];
            using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rNGCryptoServiceProvider.GetBytes(numArray);
            }
            return Convert.ToBase64String(numArray);
        }

        /// <summary>Returns the hash code for this instance.</summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            string[] keyName = new string[] { this.KeyName, this.PrimaryKey, this.SecondaryKey };
            for (int i = 0; i < (int)keyName.Length; i++)
            {
                string str = keyName[i];
                if (!string.IsNullOrEmpty(str))
                {
                    hashCode += str.GetHashCode();
                }
            }
            return hashCode;
        }

        private static bool IsValidCombinationOfRights(IEnumerable<AccessRights> rights)
        {
            if (!rights.Contains<AccessRights>(AccessRights.Manage))
            {
                return true;
            }
            return rights.Count<AccessRights>() == 3;
        }

        /// <summary>Checks the validity of the specified access rights.</summary>
		/// <param name="value">The access rights to check.</param>
		protected override void ValidateRights(IEnumerable<AccessRights> value)
        {
            base.ValidateRights(value);
            if (!SharedAccessAuthorizationRule.IsValidCombinationOfRights(value))
            {
                throw new ArgumentException("Manage permission should also include Send and Listen.");
            }
        }
    }
}
