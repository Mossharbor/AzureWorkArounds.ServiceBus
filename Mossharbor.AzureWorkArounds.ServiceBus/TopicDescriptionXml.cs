using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect", IsNullable = false)]
    public partial class TopicDescriptionXml
    {
        /// <summary>
        ///   The message time to live default value in bytes
        /// </summary>
        public readonly static TimeSpan MessageTimeToLiveDefaultValue = Constants.DefaultAllowedTimeToLive;

        private TimeSpan? defaultMessageTimeToLiveField;

        private long maxSizeInMegabytesField = Constants.DefaultMaxSizeInMegabytes;

        private bool requiresDuplicateDetectionField = false;

        private TimeSpan? duplicateDetectionHistoryTimeWindowField;

        private bool enableBatchedOperationsField = true;

        private long sizeInBytesField = 0;

        private bool filteringMessagesBeforePublishingField;

        private bool isAnonymousAccessibleField = false;

        private object authorizationRulesField;

        private EntityStatus statusField = EntityStatus.Active;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private bool? supportOrderingField;

        private TimeSpan? autoDeleteOnIdleField;

        private bool enablePartitioningField = false;

        private bool isExpressField;

        private string entityAvailabilityStatusField;

        private bool enableSubscriptionPartitioningField;

        private bool enableExpressField = false;

        [System.Xml.Serialization.XmlElementAttribute("DefaultMessageTimeToLive", DataType = "duration")]
        public string DefaultMessageTimeToLiveTimeSpanString
        {
            get
            {
                if (!defaultMessageTimeToLiveField.HasValue)
                    return String.Empty;

                return XmlConvert.ToString(defaultMessageTimeToLiveField.Value);
            }
            set
            {
                this.DefaultMessageTimeToLiveTimeSpanStringSpecified = true;
                this.defaultMessageTimeToLiveField = XmlConvert.ToTimeSpan(value);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool DefaultMessageTimeToLiveTimeSpanStringSpecified { get; set; }

        /// <summary>Gets or sets the default message time to live value. This is the duration after which the message expires, starting from when the message is sent to Service Bus. This is the default value used when 
        /// <see cref="P:Microsoft.ServiceBus.Messaging.BrokeredMessage.TimeToLive" /> is not set on a message itself.Messages older than their TimeToLive value will expire and no longer be retained in the message store. Subscribers will be unable to receive expired messages.A message can have a lower TimeToLive value than that specified here, but by default TimeToLive is set to 
        /// <see cref="F:System.TimeSpan.MaxValue" />. Therefore, this property becomes the default time to live value applied to messages.</summary> 
        /// <value>The default message time to live value.</value>
        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan DefaultMessageTimeToLive
        {
            get
            {
                if (!this.defaultMessageTimeToLiveField.HasValue)
                {
                    return TopicDescriptionXml.MessageTimeToLiveDefaultValue;
                }
                return this.defaultMessageTimeToLiveField.GetValueOrDefault();
            }
            set
            {
                if (value < Constants.MinimumAllowedTimeToLive || value > Constants.MaximumAllowedTimeToLive)
                {
                    throw new ArgumentOutOfRangeException("DefaultMessageTimeToLive");
                }
                this.DefaultMessageTimeToLiveTimeSpanStringSpecified = true;
                this.defaultMessageTimeToLiveField = new TimeSpan?(value);
            }
        }

        /// <summary>Gets or sets the maximum size of the topic in megabytes, which is the size of memory allocated for the queue.</summary>
        /// <value>The maximum size of the queue in megabytes.</value>
        public long MaxSizeInMegabytes
        {
            get
            {
                return this.maxSizeInMegabytesField;
            }
            set
            {
                if (value < Constants.MaxSizeInMegabytesMinValue || value > Constants.MaxSizeInMegabytesMaxValue)
                {
                    throw new ArgumentOutOfRangeException("MaxSizeInMegabytes");
                }
                this.MaxSizeInMegabytesSpecified = true;
                this.maxSizeInMegabytesField = value;
            }
        }

        public bool MaxSizeInMegabytesSpecified { get; set; }

        /// <summary>Gets or sets the value indicating if this queue requires duplicate detection.</summary>
        /// <value>true if this queue requires duplicate detection; otherwise, false.</value>
        public bool RequiresDuplicateDetection
        {
            get
            {
                return this.requiresDuplicateDetectionField;
            }
            set
            {
                RequiresDuplicateDetectionSpecified = true;
                this.requiresDuplicateDetectionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool RequiresDuplicateDetectionSpecified { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("DuplicateDetectionHistoryTimeWindow", DataType = "duration")]
        public string DuplicateDetectionHistoryTimeWindowTimeSpanString
        {
            get
            {
                if (!duplicateDetectionHistoryTimeWindowField.HasValue)
                    return String.Empty;

                return XmlConvert.ToString(duplicateDetectionHistoryTimeWindowField.Value);
            }
            set
            {
                DuplicateDetectionHistoryTimeWindowTimeSpanStringSpecified = true;
                this.duplicateDetectionHistoryTimeWindowField = XmlConvert.ToTimeSpan(value);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool DuplicateDetectionHistoryTimeWindowTimeSpanStringSpecified { get; set; }

        /// <summary>Gets or sets the 
        /// <see cref="T:System.TimeSpan" /> structure that defines the duration of the duplicate detection history. The default value is 10 minutes.</summary> 
        /// <value>The <see cref="T:System.TimeSpan" /> structure that represents the time windows for duplication detection history.</value>
        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan DuplicateDetectionHistoryTimeWindow
        {
            get
            {
                if (!duplicateDetectionHistoryTimeWindowField.HasValue)
                {
                    return Constants.DefaultDuplicateDetectionHistoryExpiryDuration;
                }
                return this.duplicateDetectionHistoryTimeWindowField.GetValueOrDefault();
            }
            set
            {
                if (value > Constants.MaximumDuplicateDetectionHistoryTimeWindow)
                {
                    throw new ArgumentOutOfRangeException("DuplicateDetectionHistoryTimeWindow");
                }
                if (value < Constants.MinimumDuplicateDetectionHistoryTimeWindow)
                {
                    this.duplicateDetectionHistoryTimeWindowField = new TimeSpan?(Constants.MinimumDuplicateDetectionHistoryTimeWindow);
                    return;
                }
                DuplicateDetectionHistoryTimeWindowTimeSpanStringSpecified = true;
                this.duplicateDetectionHistoryTimeWindowField = new TimeSpan?(value);
            }
        }

        /// <summary>Gets or sets a value that indicates whether server-side batched operations are enabled.</summary>
        /// <value>true if the batched operations are enabled; otherwise, false.</value>
        public bool EnableBatchedOperations
        {
            get
            {
                return this.enableBatchedOperationsField;
            }
            set
            {
                this.EnableBatchedOperationsSpecified = true;
                this.enableBatchedOperationsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool EnableBatchedOperationsSpecified { get; set; }

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

        [System.Xml.Serialization.XmlIgnore]
        public bool SizeInBytesSpecified { get { return false; } }

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

        /// <summary>Gets or sets a value that indicates whether the message is anonymous accessible.</summary>
        /// <value>true if the message is anonymous accessible; otherwise, false.</value>
        public bool IsAnonymousAccessible
        {
            get
            {
                return this.isAnonymousAccessibleField;
            }
            set
            {
                this.IsAnonymousAccessibleSpecified = true;
                this.isAnonymousAccessibleField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsAnonymousAccessibleSpecified { get; set; }

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

        /// <summary>Gets or sets the current status of the queue (enabled or 
        /// disabled). When an entity is disabled, that entity cannot send or receive messages.</summary> 
        /// <value>The current status of the queue.</value>
        public EntityStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.StatusSpecified = true;
                this.statusField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool StatusSpecified { get; set; }

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

        [System.Xml.Serialization.XmlIgnore]
        public bool CreatedAtSpecified { get { return false; } }

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

        [System.Xml.Serialization.XmlIgnore]
        public bool UpdatedAtSpecified { get { return false; } }

        /// <summary>Gets or sets a value that indicates whether the queue supports ordering.</summary>
        /// <value>true if the queue supports ordering; otherwise, false.</value>
        public bool SupportOrdering
        {
            get
            {
                if (supportOrderingField.HasValue)
                    return supportOrderingField.Value;
                if (!this.EnablePartitioning)
                {
                    return true;
                }
                return false;
            }
            set
            {
                this.SupportOrderingSpecified = true;
                this.supportOrderingField = new bool?(value);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool SupportOrderingSpecified { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("AutoDeleteOnIdle", DataType = "duration")]
        public string AutoDeleteOnIdleTimeSpanString
        {
            get
            {
                if (!autoDeleteOnIdleField.HasValue)
                    return String.Empty;

                return XmlConvert.ToString(autoDeleteOnIdleField.Value);
            }
            set
            {
                AutoDeleteOnIdleTimeSpanStringSpecified = true;
                this.autoDeleteOnIdleField = XmlConvert.ToTimeSpan(value);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool AutoDeleteOnIdleTimeSpanStringSpecified { get; set; }

        /// <summary>Gets or sets the 
        /// <see cref="T:System.TimeSpan" /> idle interval after which the queue is automatically deleted. The minimum duration is 5 minutes.</summary> 
        /// <value>The auto delete on idle time span for the queue.</value>
        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan AutoDeleteOnIdle
        {
            get
            {
                if (!this.autoDeleteOnIdleField.HasValue)
                {
                    return Constants.AutoDeleteOnIdleDefaultValue;
                }
                return this.autoDeleteOnIdleField.GetValueOrDefault();
            }
            set
            {
                AutoDeleteOnIdleTimeSpanStringSpecified = true;
                this.autoDeleteOnIdleField = new TimeSpan?(value);
            }
        }

        /// <summary>Gets or sets a value that indicates whether the queue to be partitioned across multiple message brokers is enabled. </summary>
        /// <value>true if the queue to be partitioned across multiple message brokers is enabled; otherwise, false.</value>
        public bool EnablePartitioning
        {
            get
            {
                return this.enablePartitioningField;
            }
            set
            {
                this.EnablePartitioningSpecified = true;
                this.enablePartitioningField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnore]
        public bool EnablePartitioningSpecified { get; set; }

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

        [System.Xml.Serialization.XmlIgnore]
        public bool EntityAvailabilityStatusSpecified { get { return false; } }

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

        /// <summary>Gets or sets a value that indicates whether Express Entities are enabled. An 
        /// express queue holds a message in memory temporarily before writing it to persistent storage.</summary> 
        /// <value>true if Express Entities are enabled; otherwise, false.</value>
        public bool EnableExpress
        {
            get
            {
                return this.enableExpressField;
            }
            set
            {
                this.EnableExpressSpecified = false;
                this.enableExpressField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool EnableExpressSpecified { get; set; }
    }


}
