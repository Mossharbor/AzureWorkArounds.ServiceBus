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
    public partial class TopicDescription
    {

        private string defaultMessageTimeToLiveField;

        private long maxSizeInMegabytesField;

        private bool requiresDuplicateDetectionField;

        private string duplicateDetectionHistoryTimeWindowField;

        private bool enableBatchedOperationsField;

        private long sizeInBytesField;

        private bool filteringMessagesBeforePublishingField;

        private bool isAnonymousAccessibleField;

        private object authorizationRulesField;

        private string statusField;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private bool supportOrderingField;

        private string autoDeleteOnIdleField;

        private bool enablePartitioningField;

        private bool isExpressField;

        private string entityAvailabilityStatusField;

        private bool enableSubscriptionPartitioningField;

        private bool enableExpressField;

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
        public bool FilteringMessagesBeforePublishing
        {
            get
            {
                return this.filteringMessagesBeforePublishingField;
            }
            set
            {
                this.filteringMessagesBeforePublishingField = value;
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
        public bool IsExpress
        {
            get
            {
                return this.isExpressField;
            }
            set
            {
                this.isExpressField = value;
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
        public bool EnableSubscriptionPartitioning
        {
            get
            {
                return this.enableSubscriptionPartitioningField;
            }
            set
            {
                this.enableSubscriptionPartitioningField = value;
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


}
