using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <summary>Represents the metadata description of the queue.</summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect", IsNullable = false)]
    public class QueueDescriptionXml
    {
        public QueueDescriptionXml()
        {
        }

        /// <summary>Initializes a new instance of the 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> class with the specified relative path.</summary> 
        /// <param name="path">Path of the queue relative to the namespace base address.</param>
        public QueueDescriptionXml(string path)
        {
            this.Path = path;
        }

        internal void ResetSerialization()
        {
            LockDurationTimeSpanStringSpecified = false;
            MaxSizeInMegabytesSpecified = false;
            EnableExpressSpecified = false;
            EnablePartitioningSpecified = false;
            AutoDeleteOnIdleTimeSpanStringSpecified = false;
            SupportOrderingSpecified = false;
            StatusSpecified = false;
            IsAnonymousAccessibleSpecified = false;
            EnableDeadLetteringOnMessageExpirationSpecified = false;
            EnableBatchedOperationsSpecified = false;
            MaxDeliveryCountSpecified = false;
            RequiresSessionSpecified = false;
            RequiresDuplicateDetectionSpecified = false;
            DuplicateDetectionHistoryTimeWindowTimeSpanStringSpecified = false;
            DefaultMessageTimeToLiveTimeSpanStringSpecified = false;
        }

        /// <summary>
        ///   The message time to live default value in bytes
        /// </summary>
        public readonly static TimeSpan MessageTimeToLiveDefaultValue = Constants.DefaultAllowedTimeToLive;

        private TimeSpan? lockDurationField;

        private long maxSizeInMegabytesField = Constants.DefaultMaxSizeInMegabytes;

        private bool requiresDuplicateDetectionField = false;

        private bool requiresSessionField = false;

        private TimeSpan? defaultMessageTimeToLiveField;

        private bool deadLetteringOnMessageExpirationField = false;

        private TimeSpan? duplicateDetectionHistoryTimeWindowField;

        private int maxDeliveryCountField = Constants.DefaultMaxDeliveryCount;

        private bool enableBatchedOperationsField = true;

        private bool enableDeadLetteringOnMessageExpiration = false;

        private long sizeInBytesField = 0;

        private long messageCountField;

        private bool isAnonymousAccessibleField = false;

        private object authorizationRulesField;

        private EntityStatus statusField = EntityStatus.Active;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private System.DateTime accessedAtField;

        private bool? supportOrderingField;

        private string userMetadataField;

        private QueueDescriptionCountDetailsXml countDetailsField;

        private TimeSpan? autoDeleteOnIdleField;

        private bool enablePartitioningField = false;

        private string pathField;

        private string forwardDeadLetteredMessagesTo;

        private string forwardToField;

        private string entityAvailabilityStatusField;

        private bool enableExpressField = false;

        [System.Xml.Serialization.XmlElementAttribute("LockDuration", DataType = "duration")]
        public string LockDurationTimeSpanString
        {
            get
            {
                if (!lockDurationField.HasValue)
                    return String.Empty;

                return XmlConvert.ToString(lockDurationField.Value);
            }
            set
            {
                this.lockDurationField = XmlConvert.ToTimeSpan(value);
                LockDurationTimeSpanStringSpecified = true;
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public bool LockDurationTimeSpanStringSpecified { get; set; }

        /// <summary>Gets or sets the duration of a peek lock; that is, the amount of time that the message is locked for other receivers. The maximum value for 
        /// <see cref="P:Microsoft.ServiceBus.Messaging.QueueDescription.LockDuration" /> is 5 minutes; the default value is 1 minute.</summary> 
        /// <value>The duration of the lock.</value>
        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan LockDuration
        {
            get
            {
                if (!this.lockDurationField.HasValue)
                {
                    return Constants.DefaultLockDuration;
                }
                return this.lockDurationField.GetValueOrDefault();
            }
            set
            {
                LockDurationTimeSpanStringSpecified = true;
                this.lockDurationField = new TimeSpan?(value);
            }
        }

        /// <summary>Gets or sets the maximum size of the queue in megabytes, which is the size of memory allocated for the queue.</summary>
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

        [System.Xml.Serialization.XmlIgnore]
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

        /// <summary>Gets or sets a value that indicates whether the queue supports the concept of session.</summary>
        /// <value>true if the receiver application can only receive from the queue through a 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.MessageSession" />; false if a queue cannot receive using 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.MessageSession" />.</value> 
        public bool RequiresSession
        {
            get
            {
                return this.requiresSessionField;
            }
            set
            {
                RequiresSessionSpecified = true;
                this.requiresSessionField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnore]
        public bool RequiresSessionSpecified { get; set; }

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
                    return QueueDescriptionXml.MessageTimeToLiveDefaultValue;
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

        [System.Xml.Serialization.XmlIgnore]
        public bool DeadLetteringOnMessageExpirationSpecified { get { return false; } }

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

        /// <summary>Gets or sets the maximum delivery count. A message is automatically deadlettered after this number of deliveries.</summary>
        /// <value>The number of maximum deliveries.</value>
        /// The default value is 10.
        public int MaxDeliveryCount
        {
            get
            {
                return this.maxDeliveryCountField;
            }
            set
            {
                if (value < Constants.MinAllowedMaxDeliveryCount || value > Constants.MaxAllowedMaxDeliveryCount)
                {
                    throw new ArgumentOutOfRangeException("MaxDeliveryCount");
                }
                this.MaxDeliveryCountSpecified = true;
                this.maxDeliveryCountField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool MaxDeliveryCountSpecified { get; set; }

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

        /// <summary>Gets or sets a value that indicates whether this queue has dead letter support when a message expires.</summary>
        /// <value>true if the queue has a dead letter support when a message expires; otherwise, false.</value>
        public bool EnableDeadLetteringOnMessageExpiration
        {
            get
            {
                return this.enableDeadLetteringOnMessageExpiration;
            }
            set
            {
                EnableDeadLetteringOnMessageExpirationSpecified = true;
                this.enableBatchedOperationsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool EnableDeadLetteringOnMessageExpirationSpecified { get; set; }

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

        [System.Xml.Serialization.XmlIgnore]
        public bool MessageCountSpecified { get { return false; } }

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

        [System.Xml.Serialization.XmlIgnore]
        public bool AuthorizationRulesSpecified { get { return false; } }

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

        [System.Xml.Serialization.XmlIgnore]
        public bool AccessedAtSpecified { get { return false; } }

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

        /// <remarks/>
        public QueueDescriptionCountDetailsXml CountDetails
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
        
        [System.Xml.Serialization.XmlIgnore]
        public bool CountDetailsSpecified { get { return false; } }
        
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

        /// <summary>Gets or sets the name of the queue.</summary>
        /// <value>The name of the queue.</value>
        /// <remarks>
        ///   This is a relative path to the <see cref="P:Microsoft.ServiceBus.NamespaceManager.Address" />.
        /// </remarks>
        [System.Xml.Serialization.XmlIgnore]
        public string Path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Path");
                }
                this.pathField = value;
            }
        }

        /// <summary>Gets or sets the path to the recipient to which the dead lettered message is forwarded.</summary>
        /// <value>The path to the recipient to which the dead lettered message is forwarded.</value>
        public string ForwardDeadLetteredMessagesTo
        {
            get
            {
                return this.forwardDeadLetteredMessagesTo;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("ForwardDeadLetteredMessagesTo");
                }
                this.forwardDeadLetteredMessagesTo = value;
            }
        }

        /// <summary>Gets or sets the path to the recipient to which the message is forwarded.</summary>
        /// <value>The path to the recipient to which the message is forwarded.</value>
        public string ForwardTo
        {
            get
            {
                return this.forwardToField;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && string.Equals(this.Path, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new InvalidOperationException(value);
                }
                this.forwardToField = value;
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    public partial class QueueDescriptionCountDetailsXml
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
