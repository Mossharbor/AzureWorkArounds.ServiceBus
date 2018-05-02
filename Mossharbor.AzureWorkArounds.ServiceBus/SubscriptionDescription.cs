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
    public partial class SubscriptionDescription
    {

        private string lockDurationField;

        private bool requiresSessionField;

        private string defaultMessageTimeToLiveField;

        private bool deadLetteringOnMessageExpirationField;

        private bool deadLetteringOnFilterEvaluationExceptionsField;

        private byte messageCountField;

        private byte maxDeliveryCountField;

        private bool enableBatchedOperationsField;

        private string statusField;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private System.DateTime accessedAtField;

        private string autoDeleteOnIdleField;

        private string entityAvailabilityStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string LockDuration
        {
            get
            {
                return this.lockDurationField;
            }
            set
            {
                this.lockDurationField = value;
            }
        }

        /// <remarks/>
        public bool RequiresSession
        {
            get
            {
                return this.requiresSessionField;
            }
            set
            {
                this.requiresSessionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string DefaultMessageTimeToLive
        {
            get
            {
                return this.defaultMessageTimeToLiveField;
            }
            set
            {
                this.defaultMessageTimeToLiveField = value;
            }
        }

        /// <remarks/>
        public bool DeadLetteringOnMessageExpiration
        {
            get
            {
                return this.deadLetteringOnMessageExpirationField;
            }
            set
            {
                this.deadLetteringOnMessageExpirationField = value;
            }
        }

        /// <remarks/>
        public bool DeadLetteringOnFilterEvaluationExceptions
        {
            get
            {
                return this.deadLetteringOnFilterEvaluationExceptionsField;
            }
            set
            {
                this.deadLetteringOnFilterEvaluationExceptionsField = value;
            }
        }

        /// <remarks/>
        public byte MessageCount
        {
            get
            {
                return this.messageCountField;
            }
            set
            {
                this.messageCountField = value;
            }
        }

        /// <remarks/>
        public byte MaxDeliveryCount
        {
            get
            {
                return this.maxDeliveryCountField;
            }
            set
            {
                this.maxDeliveryCountField = value;
            }
        }

        /// <remarks/>
        public bool EnableBatchedOperations
        {
            get
            {
                return this.enableBatchedOperationsField;
            }
            set
            {
                this.enableBatchedOperationsField = value;
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
        public System.DateTime AccessedAt
        {
            get
            {
                return this.accessedAtField;
            }
            set
            {
                this.accessedAtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string AutoDeleteOnIdle
        {
            get
            {
                return this.autoDeleteOnIdleField;
            }
            set
            {
                this.autoDeleteOnIdleField = value;
            }
        }

        /// <remarks/>
        public string EntityAvailabilityStatus
        {
            get
            {
                return this.entityAvailabilityStatusField;
            }
            set
            {
                this.entityAvailabilityStatusField = value;
            }
        }
    }


}
