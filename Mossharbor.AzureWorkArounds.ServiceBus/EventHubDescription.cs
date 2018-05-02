using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect", IsNullable = false)]
    public partial class EventHubDescription
    {

        private long messageRetentionInDaysField;

        private object authorizationRulesField;

        private string statusField;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private int partitionCountField;

        private byte[] partitionIdsField;

        /// <remarks/>
        public long MessageRetentionInDays
        {
            get
            {
                return this.messageRetentionInDaysField;
            }
            set
            {
                this.messageRetentionInDaysField = value;
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
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
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
        public int PartitionCount
        {
            get
            {
                return this.partitionCountField;
            }
            set
            {
                this.partitionCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute()]
        [System.Xml.Serialization.XmlArrayItemAttribute("string", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
        public byte[] PartitionIds
        {
            get
            {
                return this.partitionIdsField;
            }
            set
            {
                this.partitionIdsField = value;
            }
        }
    }


}
