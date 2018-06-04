using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
   public class RelayDescription
    {
        internal RelayDescriptionXml xml = null;
        internal RelayDescription(string relayPath, RelayDescriptionXml xml)
        {
            this.xml = xml;
            if (null != xml)
            {
                this.xml.Path = relayPath;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> class.</summary>
		/// <param name="relayPath">The path of the relay.</param>
		/// <param name="type">The relay type.</param>
		public RelayDescription(string relayPath, RelayType type)
        {
            this.xml = new RelayDescriptionXml(relayPath, type);
        }

        /// <summary>Gets or sets the path of the relay.</summary>
		/// <value>The path of the relay.</value>
        public string Path
        {
            get
            {
                return this.xml.Path;
            }
        }

        /// <summary>Gets or sets the number of listeners for this relay.</summary>
		/// <value>The number of listeners for this relay.</value>
		public int ListenerCount
        {
            get
            {
                return this.xml.ListenerCount;
            }
        }
        /// <remarks/>
        public ListenerType ListenerType
        {
            get
            {
                return this.xml.ListenerType;
            }
            set
            {
                this.xml.ListenerType = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreatedAt
        {
            get
            {
                return this.xml.CreatedAt;
            }
        }

        /// <remarks/>
        public System.DateTime UpdatedAt
        {
            get
            {
                return this.xml.UpdatedAt;
            }
        }

        /// <remarks/>
        public RelayType RelayType
        {
            get
            {
                return this.xml.RelayType;
            }
            set
            {
                this.xml.RelayType = value;
            }
        }

        /// <remarks/>
        public bool RequiresClientAuthorization
        {
            get
            {
                return this.xml.RequiresClientAuthorization;
            }
            set
            {
                this.xml.RequiresClientAuthorization = value;
            }
        }

        /// <summary>Gets or sets whether transport security is needed for this relay.</summary>
		/// <value>true if transport security is needed for this relay; otherwise, false.</value>
        public bool RequiresTransportSecurity
        {
            get
            {
                return this.xml.RequiresTransportSecurity;
            }
            set
            {
                this.xml.RequiresTransportSecurity = value;
            }
        }

        /// <summary>Gets or sets the user metadata.</summary>
		/// <value>The user metadata.</value>
		public string UserMetadata
        {
            get { return xml.UserMetadata; }
            set { xml.UserMetadata = value; }
        }
    }
}
