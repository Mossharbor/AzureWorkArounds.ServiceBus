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
    public partial class SubscriptionDescriptionXml
    {
        public SubscriptionDescriptionXml()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.SubscriptionDescription" /> class.</summary>
		/// <param name="topicPath">The topic name.</param>
		/// <param name="subscriptionName">The subscription name.</param>
        public SubscriptionDescriptionXml(string topicPath, string subscriptionName)
        {
            this.TopicPath = topicPath;
            this.Name = subscriptionName;
        }

        internal void ResetSerialization()
        {
            DeadLetteringOnFilterEvaluationExceptionsSpecified = false;
            DeadLetteringOnMessageExpirationSpecified = false;
            LockDurationTimeSpanStringSpecified = false;
            AutoDeleteOnIdleTimeSpanStringSpecified = false;
            StatusSpecified = false;
            EnableBatchedOperationsSpecified = false;
            MaxDeliveryCountSpecified = false;
            RequiresSessionSpecified = false;
            DefaultMessageTimeToLiveTimeSpanStringSpecified = false;
        }

        private string topicPathField;

        private string nameField;

        private TimeSpan? lockDurationField;

        private bool requiresSessionField = false;

        private TimeSpan? defaultMessageTimeToLiveField;

        private bool deadLetteringOnMessageExpirationField = false;

        private bool deadLetteringOnFilterEvaluationExceptionsField = true;

        private long messageCountField;
        private string forwardToField;

        private int maxDeliveryCountField = Constants.DefaultMaxDeliveryCount;

        private bool enableBatchedOperationsField = true;

        private EntityStatus statusField = EntityStatus.Active;

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        private System.DateTime accessedAtField;

        private TimeSpan? autoDeleteOnIdleField;

        private EntityAvailabilityStatus entityAvailabilityStatusField = EntityAvailabilityStatus.Available;


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

        /// <summary>Gets or sets a value that indicates whether the subscription supports the concept of session.</summary>
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

        /// <summary>Gets or sets the value that indicates if a subscription has dead letter support when a message expires.</summary>
		/// <value>true if a subscription has dead letter support when a message expires; otherwise, false.</value>
        public bool DeadLetteringOnMessageExpiration
        {
            get
            {
                return this.deadLetteringOnMessageExpirationField;
            }
            set
            {
                this.DeadLetteringOnMessageExpirationSpecified = true;
                this.deadLetteringOnMessageExpirationField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool DeadLetteringOnMessageExpirationSpecified { get; set; }

        /// <summary>Gets or sets the value that indicates if a subscription has dead letter support on Filter evaluation exceptions.</summary>
		/// <value>true if a subscription has dead letter support on Filter evaluation exceptions; otherwise, false.</value>
        public bool DeadLetteringOnFilterEvaluationExceptions
        {
            get
            {
                return this.deadLetteringOnFilterEvaluationExceptionsField;
            }
            set
            {
                this.DeadLetteringOnFilterEvaluationExceptionsSpecified = true;
                this.deadLetteringOnFilterEvaluationExceptionsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool DeadLetteringOnFilterEvaluationExceptionsSpecified { get; set; }

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
        public bool ForwardToSpecified { get; set; }

        /// <summary>Fowards Subscription To Queue.</summary>
        /// <value>QueueName.</value>
        public string ForwardTo
        {
            get
            {
                return this.forwardToField;
            }
            set
            {
                this.ForwardToSpecified = true;
                this.forwardToField = value;
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

        /// <summary>Gets or sets the current status of the subscription (enabled or 
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

        /// <remarks/>
        public CountDetailsXml CountDetails { get; set; }
        
        [System.Xml.Serialization.XmlIgnore]
        public bool CountDetailsSpecified => false;

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

        public RuleDescription DefaultRuleDescription
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool DefaultRuleDescriptionSpecified
        {
            get { return null != DefaultRuleDescription; }
        }
        
        /// <remarks/>
        public EntityAvailabilityStatus EntityAvailabilityStatus
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

        /// <summary>Gets or sets the name of the Topic</summary>
        /// <value>The name of the queue.</value>
        /// <remarks>
        ///   This is a relative path to the <see cref="P:Microsoft.ServiceBus.NamespaceManager.Address" />.
        /// </remarks>
        [System.Xml.Serialization.XmlIgnore]
        public string TopicPath
        {
            get
            {
                return this.topicPathField;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("TopicPath");
                }
                this.topicPathField = value;
            }
        }

        /// <summary>Gets or sets the name of the subscription.</summary>
        /// <value>The name of the queue.</value>
        /// <remarks>
        ///   This is a relative path to the <see cref="P:Microsoft.ServiceBus.NamespaceManager.Address" />.
        /// </remarks>
        [System.Xml.Serialization.XmlIgnore]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name");
                }
                this.nameField = value;
            }
        }
    }


}
