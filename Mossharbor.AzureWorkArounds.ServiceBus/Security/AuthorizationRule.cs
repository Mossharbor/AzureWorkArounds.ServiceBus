using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    public abstract class AuthorizationRule
    {
        public AuthorizationRule()
        {
            this.CreatedTime = DateTime.UtcNow;
            this.ModifiedTime = DateTime.UtcNow;
            this.Revision = (long)0;
        }

        /// <summary>Gets or sets the claim type.</summary>
        /// <value>The claim type.</value>
        public string ClaimType
        {
            get; set;
        }

        /// <summary>Gets or sets the claim value which is either ‘Send’, ‘Listen’, or ‘Manage’.</summary>
		/// <value>The claim value which is either ‘Send’, ‘Listen’, or ‘Manage’.</value>
		public string ClaimValue
        {
            get;
            set;
        }

        /// <summary>Gets or sets the date and time when the authorization rule was created.</summary>
		/// <value>The date and time when the authorization rule was created.</value>
        public DateTime CreatedTime
        {
            get;
            set;
        }

        /// <summary>Gets or sets the date and time when the authorization rule was modified.</summary>
		/// <value>The date and time when the authorization rule was modified.</value>
        public DateTime ModifiedTime
        {
            get;
            set;
        }

        /// <summary>Gets or sets the modification revision number.</summary>
		/// <value>The modification revision number.</value>
        public long Revision
        {
            get;
            set;
        }

        /// <summary>Gets or sets the name identifier of the issuer.</summary>
		/// <value>The name identifier of the issuer.</value>
		public string IssuerName
        {
            get;set;
        }

        /// <summary>Gets or sets the authorization rule key name.</summary>
		/// <value>The authorization rule key name.</value>
		public virtual string KeyName
        {
            get;
            set;
        }

        private List<AccessRights> rights;
        /// <summary>Gets or sets the list of rights.</summary>
        /// <value>The list of rights.</value>
        public List<AccessRights> Rights
        {
            get { return rights; }
            set
            {
                if (null != this.rights)
                    this.ValidateRights(value);
                this.rights = value;
            }
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            AuthorizationRule authorizationRule = (AuthorizationRule)obj;
            if (!string.Equals(this.IssuerName, authorizationRule.IssuerName, StringComparison.OrdinalIgnoreCase) || !string.Equals(this.ClaimType, authorizationRule.ClaimType, StringComparison.OrdinalIgnoreCase) || !string.Equals(this.ClaimValue, authorizationRule.ClaimValue, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (this.Rights != null && authorizationRule.Rights == null || this.Rights == null && authorizationRule.Rights != null)
            {
                return false;
            }
            if (this.Rights == null || authorizationRule.Rights == null)
            {
                return true;
            }
            HashSet<AccessRights> accessRights = new HashSet<AccessRights>(this.Rights);
            HashSet<AccessRights> accessRights1 = new HashSet<AccessRights>(authorizationRule.Rights);
            if (accessRights1.Count != accessRights.Count)
            {
                return false;
            }
            HashSet<AccessRights> accessRights2 = accessRights1;
            return accessRights.All<AccessRights>(new Func<AccessRights, bool>(accessRights2.Contains));
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            string[] issuerName = new string[] { this.IssuerName, this.ClaimValue, this.ClaimType };
            for (int i = 0; i < (int)issuerName.Length; i++)
            {
                string str = issuerName[i];
                if (!string.IsNullOrEmpty(str))
                {
                    hashCode += str.GetHashCode();
                }
            }
            return hashCode;
        }

        /// <summary>Checks the validity of the specified access rights.</summary>
		/// <param name="value">The access rights to check.</param>
		protected virtual void ValidateRights(IEnumerable<AccessRights> value)
        {
            if (value == null || !value.Any<AccessRights>() || value.Count<AccessRights>() > 3)
            {
                throw new ArgumentException(string.Format("Rights cannot be null, empty or greater than {0}.",3));
            }
            if (!AuthorizationRule.AreAccessRightsUnique(value))
            {
                throw new ArgumentException("The AccessRights on an Authorization Rule must be unique.");
            }
        }

        private static bool AreAccessRightsUnique(IEnumerable<AccessRights> rights)
        {
            HashSet<AccessRights> accessRights = new HashSet<AccessRights>(rights);
            return rights.Count<AccessRights>() == accessRights.Count;
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
    }
}
