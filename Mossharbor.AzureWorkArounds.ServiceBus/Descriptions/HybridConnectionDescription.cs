using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    public class HybridConnectionDescription 
    {
        internal HybridConnectionDescriptionXml xml;

        public HybridConnectionDescription(string path)
        {
            this.xml = new HybridConnectionDescriptionXml(path);
        }

        internal HybridConnectionDescription(string path, HybridConnectionDescriptionXml xml)
        {
            this.xml = xml;
            if (null != xml)
            {
                this.xml.Path = path;
            }
        }

        /// <summary>Gets or sets the number of listeners for this HybridConnection.</summary>
		/// <value>The number of listeners for this HybridConnection.</value>
		public int ListenerCount
        {
            get
            {
                return xml.ListenerCount;
            }
        }

        /// <summary>Gets the <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" />.</summary>
		/// <value>The <see cref="T:Microsoft.ServiceBus.Messaging.AuthorizationRules" />.</value>
		public AuthorizationRules Authorization
        {
            get{ return xml.Authorization; }
        }

        /// <summary>Gets or sets the name of the collection associated with the HybridConnection.</summary>
		/// <value>The name of the collection associated with the HybridConnection.</value>
		public string CollectionName
        {
            get
            {
                return "HybridConnections";
            }
        }

        /// <summary>Gets or sets the exact time the HybridConnection was created.</summary>
        /// <value>The exact time the HybridConnection was created.</value>
        public DateTime CreatedAt
        {
            get
            {
                return xml.CreatedAt;
            }
        }

        /// <summary>Gets or sets the date when the HybridConnection was updated.</summary>
		/// <value>The date when the HybridConnection was updated.</value>
		public DateTime UpdatedAt
        {
            get
            {
                return xml.UpdatedAt;
            }
        }

        /// <summary>Gets or sets the relative path of the HybridConnection.</summary>
		/// <value>The relative path of the HybridConnection.</value>
		public string Path
        {
            get
            {
                return xml.Path;
            }
            set
            {
                xml.Path = value;
            }
        }

        /// <summary>Gets or sets whether client authorization is needed for this HybridConnection.</summary>
        /// <value>true if client authorization is needed for this HybridConnection; otherwise, false.</value>
        public bool RequiresClientAuthorization
        {
            get
            {
                return xml.RequiresClientAuthorization;
            }
            set
            {
                xml.RequiresClientAuthorization = value;
            }
        }

        /// <summary>Gets or sets the user metadata associated with this instance.</summary>
		/// <value>The user metadata associated with this instance.</value>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if the length of value is greater than 1024 characters.</exception>
		public string UserMetadata
        {
            get
            {
                return xml.UserMetadata;
            }
            set
            {
                xml.UserMetadata = value;  
            }
        }
    }
}
