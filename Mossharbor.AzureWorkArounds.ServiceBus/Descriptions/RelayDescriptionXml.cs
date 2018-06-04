using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect", IsNullable = false)]
    public partial class RelayDescriptionXml
    {
        private string pathField;

        private ListenerType? listenerTypeField;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private RelayType? relayTypeField;

        private bool requiresClientAuthorizationField;

        private bool requiresTransportSecurityField;

        private bool publishToRegistryField;

        private object authorizationRulesField;

        private bool isDynamicField;

        private bool requiresEncryption;

        private int listenerCountField;

        private string userMetadataField;

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> class.</summary>
		/// <param name="relayPath">The path of the relay.</param>
		/// <param name="type">The relay type.</param>
		public RelayDescriptionXml(string relayPath, RelayType type)
        {
            this.Path = relayPath;
            this.RelayType = type;
        }

        public RelayDescriptionXml()
        { }

        internal void ResetSerialization()
        {
            // do nothing right now
        }

        /// <summary>Gets or sets the path of the relay.</summary>
        /// <value>The path of the relay.</value>
        public string Path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <summary>Gets or sets the number of listeners for this relay.</summary>
		/// <value>The number of listeners for this relay.</value>
		public int ListenerCount
        {
            get
            {
                return listenerCountField;
            }
            set
            {
                listenerCountField = value;
            }
        }

        /// <remarks/>
        public ListenerType ListenerType
        {
            get
            {
                if (listenerTypeField.HasValue)
                {
                    return listenerTypeField.GetValueOrDefault();
                }
                return RelayDescriptionXml.MapRelayTypeToListenerType(relayTypeField);
            }
            set
            {
                this.listenerTypeField = new ListenerType?(value);
            }
        }

        /// <remarks/>
        public System.DateTime CreatedAt
        {
            get
            {
                return this.createdAtField;
            }
            set
            {
                this.createdAtField = value;
            }
        }

        /// <remarks/>
        public System.DateTime UpdatedAt
        {
            get
            {
                return this.updatedAtField;
            }
            set
            {
                this.updatedAtField = value;
            }
        }

        /// <remarks/>
        public RelayType RelayType
        {
            get
            {
                if (relayTypeField.HasValue)
                {
                    return relayTypeField.GetValueOrDefault();
                }
                return RelayDescriptionXml.MapListenerTypeToRelayType(listenerTypeField);
            }
            set
            {
                this.relayTypeField = new RelayType?(value);
            }
        }

        /// <remarks/>
        public bool RequiresClientAuthorization
        {
            get
            {
                return this.requiresClientAuthorizationField;
            }
            set
            {
                this.requiresClientAuthorizationField = value;
            }
        }

        /// <summary>Gets or sets whether transport security is needed for this relay.</summary>
		/// <value>true if transport security is needed for this relay; otherwise, false.</value>
        public bool RequiresTransportSecurity
        {
            get
            {
                return this.requiresTransportSecurityField;
            }
            set
            {
                this.requiresTransportSecurityField = value;
            }
        }
        
        /// <remarks/>
        public bool PublishToRegistry
        {
            get
            {
                return this.publishToRegistryField;
            }
            set
            {
                this.publishToRegistryField = value;
            }
        }

        /// <remarks/>
        public object AuthorizationRules
        {
            get
            {
                return this.authorizationRulesField;
            }
            set
            {
                this.authorizationRulesField = value;
            }
        }

        /// <remarks/>
        public bool IsDynamic
        {
            get
            {
                return this.isDynamicField;
            }
            set
            {
                this.isDynamicField = value;
            }
        }


        /// <summary>Gets or sets the user metadata.</summary>
        /// <value>The user metadata.</value>
        public string UserMetadata
        {
            get
            {
                return this.userMetadataField;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.userMetadataField = null;
                    return;
                }
                if (value.Length > 1024)
                {
                    throw new ArgumentOutOfRangeException("user metadata is limited to 1024 bytes in length");
                }
                this.userMetadataField = value;
            }
        }

        internal static RelayType MapListenerTypeToRelayType(ListenerType? listenerType)
        {
            if (!listenerType.HasValue)
            {
                return RelayType.None;
            }

            if (listenerType.HasValue)
            {
                switch (listenerType.GetValueOrDefault())
                {
                    case ListenerType.Unicast:
                        {
                            return RelayType.NetOneway;
                        }
                    case ListenerType.Multicast:
                        {
                            return RelayType.NetEvent;
                        }
                    case ListenerType.RelayedConnection:
                        {
                            return RelayType.NetTcp;
                        }
                    case ListenerType.RelayedHttp:
                        {
                            return RelayType.Http;
                        }
                }
            }
            return RelayType.None;
        }

        internal static ListenerType MapRelayTypeToListenerType(RelayType? relayType)
        {
            if (!relayType.HasValue)
            {
                return ListenerType.None;
            }

            if (relayType.HasValue)
            {
                switch (relayType.GetValueOrDefault())
                {
                    case RelayType.NetTcp:
                        {
                            return ListenerType.RelayedConnection;
                        }
                    case RelayType.Http:
                        {
                            return ListenerType.RelayedHttp;
                        }
                    case RelayType.NetEvent:
                        {
                            return ListenerType.Multicast;
                        }
                    case RelayType.NetOneway:
                        {
                            return ListenerType.Unicast;
                        }
                }
            }
            return ListenerType.None;
        }
    }


}
