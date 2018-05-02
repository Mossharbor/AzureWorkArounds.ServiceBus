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
    public partial class QueueDescription
    {

        private string lockDurationField;

        private long maxSizeInMegabytesField;

        private bool requiresDuplicateDetectionField;

        private bool requiresSessionField;

        private string defaultMessageTimeToLiveField;

        private bool deadLetteringOnMessageExpirationField;

        private string duplicateDetectionHistoryTimeWindowField;

        private int maxDeliveryCountField;

        private bool enableBatchedOperationsField;

        private long sizeInBytesField;

        private long messageCountField;

        private bool isAnonymousAccessibleField;

        private object authorizationRulesField;

        private string statusField;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private System.DateTime accessedAtField;

        private bool supportOrderingField;

        private QueueDescriptionCountDetails countDetailsField;

        private string autoDeleteOnIdleField;

        private bool enablePartitioningField;

        private string entityAvailabilityStatusField;

        private bool enableExpressField;

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
        public long MaxSizeInMegabytes
        {
            get
            {
                return this.maxSizeInMegabytesField;
            }
            set
            {
                this.maxSizeInMegabytesField = value;
            }
        }

        /// <remarks/>
        public bool RequiresDuplicateDetection
        {
            get
            {
                return this.requiresDuplicateDetectionField;
            }
            set
            {
                this.requiresDuplicateDetectionField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string DuplicateDetectionHistoryTimeWindow
        {
            get
            {
                return this.duplicateDetectionHistoryTimeWindowField;
            }
            set
            {
                this.duplicateDetectionHistoryTimeWindowField = value;
            }
        }

        /// <remarks/>
        public int MaxDeliveryCount
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
        public long SizeInBytes
        {
            get
            {
                return this.sizeInBytesField;
            }
            set
            {
                this.sizeInBytesField = value;
            }
        }

        /// <remarks/>
        public long MessageCount
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
        public bool IsAnonymousAccessible
        {
            get
            {
                return this.isAnonymousAccessibleField;
            }
            set
            {
                this.isAnonymousAccessibleField = value;
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
        public bool SupportOrdering
        {
            get
            {
                return this.supportOrderingField;
            }
            set
            {
                this.supportOrderingField = value;
            }
        }

        /// <remarks/>
        public QueueDescriptionCountDetails CountDetails
        {
            get
            {
                return this.countDetailsField;
            }
            set
            {
                this.countDetailsField = value;
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
        public bool EnablePartitioning
        {
            get
            {
                return this.enablePartitioningField;
            }
            set
            {
                this.enablePartitioningField = value;
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

        /// <remarks/>
        public bool EnableExpress
        {
            get
            {
                return this.enableExpressField;
            }
            set
            {
                this.enableExpressField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    public partial class QueueDescriptionCountDetails
    {

        private long activeMessageCountField;

        private long deadLetterMessageCountField;

        private long scheduledMessageCountField;

        private long transferMessageCountField;

        private long transferDeadLetterMessageCountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long ActiveMessageCount
        {
            get
            {
                return this.activeMessageCountField;
            }
            set
            {
                this.activeMessageCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long DeadLetterMessageCount
        {
            get
            {
                return this.deadLetterMessageCountField;
            }
            set
            {
                this.deadLetterMessageCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long ScheduledMessageCount
        {
            get
            {
                return this.scheduledMessageCountField;
            }
            set
            {
                this.scheduledMessageCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long TransferMessageCount
        {
            get
            {
                return this.transferMessageCountField;
            }
            set
            {
                this.transferMessageCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long TransferDeadLetterMessageCount
        {
            get
            {
                return this.transferDeadLetterMessageCountField;
            }
            set
            {
                this.transferDeadLetterMessageCountField = value;
            }
        }
    }
}
