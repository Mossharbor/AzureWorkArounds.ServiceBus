using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <summary>Represents the metadata description of the queue.</summary>
    public partial class QueueDescription
    {
        internal QueueDescriptionXml xml = null;
        internal QueueDescription(string path, QueueDescriptionXml xml)
        {
            this.xml = xml;
            if (null != xml)
                this.xml.Path = path;
        }

        /// <summary>Initializes a new instance of the 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> class with the specified relative path.</summary> 
        /// <param name="path">Path of the queue relative to the namespace base address.</param>
        public QueueDescription(string path)
        {
            this.xml = new QueueDescriptionXml(path);
        }
        
        
        /// <summary>Gets or sets the duration of a peek lock; that is, the amount of time that the message is locked for other receivers. The maximum value for 
		/// <see cref="P:Microsoft.ServiceBus.Messaging.QueueDescription.LockDuration" /> is 5 minutes; the default value is 1 minute.</summary> 
		/// <value>The duration of the lock.</value>
        public TimeSpan LockDuration
        {
            get { return xml.LockDuration; }
            set { xml.LockDuration = value; }
        }

        /// <summary>Gets or sets the maximum size of the queue in megabytes, which is the size of memory allocated for the queue.</summary>
		/// <value>The maximum size of the queue in megabytes.</value>
        public long MaxSizeInMegabytes
        {
            get { return xml.MaxSizeInMegabytes; }
            set { xml.MaxSizeInMegabytes = value; }
        }

        /// <summary>Gets or sets the value indicating if this queue requires duplicate detection.</summary>
		/// <value>true if this queue requires duplicate detection; otherwise, false.</value>
        public bool RequiresDuplicateDetection
        {
            get { return xml.RequiresDuplicateDetection; }
            set { xml.RequiresDuplicateDetection = value; }
        }

        /// <summary>Gets or sets a value that indicates whether the queue supports the concept of session.</summary>
		/// <value>true if the receiver application can only receive from the queue through a 
		/// <see cref="T:Microsoft.ServiceBus.Messaging.MessageSession" />; false if a queue cannot receive using 
		/// <see cref="T:Microsoft.ServiceBus.Messaging.MessageSession" />.</value> 
        public bool RequiresSession
        {
            get { return xml.RequiresSession; }
            set { xml.RequiresSession = value; }
        }
        
        /// <summary>Gets or sets the default message time to live value. This is the duration after which the message expires, starting from when the message is sent to Service Bus. This is the default value used when 
		/// <see cref="P:Microsoft.ServiceBus.Messaging.BrokeredMessage.TimeToLive" /> is not set on a message itself.Messages older than their TimeToLive value will expire and no longer be retained in the message store. Subscribers will be unable to receive expired messages.A message can have a lower TimeToLive value than that specified here, but by default TimeToLive is set to 
		/// <see cref="F:System.TimeSpan.MaxValue" />. Therefore, this property becomes the default time to live value applied to messages.</summary> 
		/// <value>The default message time to live value.</value>
        public TimeSpan DefaultMessageTimeToLive
        {
            get { return xml.DefaultMessageTimeToLive; }
            set { xml.DefaultMessageTimeToLive = value; }
        }

        public bool DeadLetteringOnMessageExpiration
        {
            get { return xml.DeadLetteringOnMessageExpiration; }
        }

        public string DuplicateDetectionHistoryTimeWindowTimeSpanString
        {
            get { return xml.DuplicateDetectionHistoryTimeWindowTimeSpanString; }
            set { xml.DuplicateDetectionHistoryTimeWindowTimeSpanString = value; }
        }

        /// <summary>Gets or sets the 
		/// <see cref="T:System.TimeSpan" /> structure that defines the duration of the duplicate detection history. The default value is 10 minutes.</summary> 
		/// <value>The <see cref="T:System.TimeSpan" /> structure that represents the time windows for duplication detection history.</value>
        /// TODO should be TimeSpan
        public TimeSpan DuplicateDetectionHistoryTimeWindow
        {
            get { return xml.DuplicateDetectionHistoryTimeWindow; }
            set { xml.DuplicateDetectionHistoryTimeWindow = value; }
        }

        /// <summary>Gets or sets the maximum delivery count. A message is automatically deadlettered after this number of deliveries.</summary>
		/// <value>The number of maximum deliveries.</value>
		/// The default value is 10.
        public int MaxDeliveryCount
        {
            get { return xml.MaxDeliveryCount; }
            set { xml.MaxDeliveryCount = value; }
        }

        /// <summary>Gets or sets a value that indicates whether server-side batched operations are enabled.</summary>
        /// <value>true if the batched operations are enabled; otherwise, false.</value>
        public bool EnableBatchedOperations
        {
            get { return xml.EnableBatchedOperations; }
            set { xml.EnableBatchedOperations = value; }
        }

        /// <summary>Gets or sets a value that indicates whether this queue has dead letter support when a message expires.</summary>
		/// <value>true if the queue has a dead letter support when a message expires; otherwise, false.</value>
		public bool EnableDeadLetteringOnMessageExpiration
        {
            get { return xml.EnableDeadLetteringOnMessageExpiration; }
            set { xml.EnableDeadLetteringOnMessageExpiration = value; }
        }

        /// <remarks/>
        public long SizeInBytes
        {
            get { return xml.SizeInBytes; }
        }

        /// <remarks/>
        public long MessageCount
        {
            get { return xml.MessageCount; }
        }

        /// <summary>Gets or sets a value that indicates whether the message is anonymous accessible.</summary>
		/// <value>true if the message is anonymous accessible; otherwise, false.</value>
        public bool IsAnonymousAccessible
        {
            get { return xml.IsAnonymousAccessible; }
            set { xml.IsAnonymousAccessible = value; }
        }

        /// <remarks/>
        public object AuthorizationRules
        {
            get { return xml.AuthorizationRules; }
        }

        /// <summary>Gets or sets the current status of the queue (enabled or 
		/// disabled). When an entity is disabled, that entity cannot send or receive messages.</summary> 
		/// <value>The current status of the queue.</value>
        public EntityStatus Status
        {
            get { return xml.Status; }
            set { xml.Status = value; }
        }

        /// <remarks/>
        public System.DateTime CreatedAt
        {
            get { return xml.CreatedAt; }
        }

        /// <remarks/>
        public System.DateTime UpdatedAt
        {
            get { return xml.UpdatedAt; }
        }

        /// <remarks/>
        public System.DateTime AccessedAt
        {
            get { return xml.AccessedAt; }
        }

        /// <summary>Gets or sets a value that indicates whether the queue supports ordering.</summary>
		/// <value>true if the queue supports ordering; otherwise, false.</value>
        public bool SupportOrdering
        {
            get { return xml.SupportOrdering; }
            set { xml.SupportOrdering = value; }
        }

        /// <summary>Gets or sets the user metadata.</summary>
		/// <value>The user metadata.</value>
		public string UserMetadata
        {
            get { return xml.UserMetadata; }
            set { xml.UserMetadata = value; }
        }

        /// <remarks/>
        public QueueDescriptionCountDetails CountDetails
        {
            get { return new QueueDescriptionCountDetails(xml.CountDetails); }
        }
        
        /// <summary>Gets or sets the 
		/// <see cref="T:System.TimeSpan" /> idle interval after which the queue is automatically deleted. The minimum duration is 5 minutes.</summary> 
		/// <value>The auto delete on idle time span for the queue.</value>
        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan AutoDeleteOnIdle
        {
            get { return xml.AutoDeleteOnIdle; }
            set { xml.AutoDeleteOnIdle = value; }
        }

        /// <summary>Gets or sets a value that indicates whether the queue to be partitioned across multiple message brokers is enabled. </summary>
		/// <value>true if the queue to be partitioned across multiple message brokers is enabled; otherwise, false.</value>
        public bool EnablePartitioning
        {
            get { return xml.EnablePartitioning; }
            set { xml.EnablePartitioning = value; }
        }

        /// <summary>Gets or sets the name of the queue.</summary>
		/// <value>The name of the queue.</value>
		/// <remarks>
		///   This is a relative path to the <see cref="P:Microsoft.ServiceBus.NamespaceManager.Address" />.
		/// </remarks>
        public string Path
        {
            get { return xml.Path; }
            set { xml.Path = value; }
        }

        /// <summary>Gets or sets the path to the recipient to which the dead lettered message is forwarded.</summary>
		/// <value>The path to the recipient to which the dead lettered message is forwarded.</value>
		public string ForwardDeadLetteredMessagesTo
        {
            get { return xml.ForwardDeadLetteredMessagesTo; }
            set { xml.ForwardDeadLetteredMessagesTo = value; }
        }

        /// <summary>Gets or sets the path to the recipient to which the message is forwarded.</summary>
		/// <value>The path to the recipient to which the message is forwarded.</value>
		public string ForwardTo
        {
            get { return xml.ForwardTo; }
            set { xml.ForwardTo = value; }
        }

        /// <remarks/>
        public string EntityAvailabilityStatus
        {
            get { return xml.EntityAvailabilityStatus; }
        }

        /// <summary>Gets or sets a value that indicates whether Express Entities are enabled. An 
		/// express queue holds a message in memory temporarily before writing it to persistent storage.</summary> 
		/// <value>true if Express Entities are enabled; otherwise, false.</value>
        public bool EnableExpress
        {
            get { return xml.EnableExpress; }
            set { xml.EnableExpress = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    public partial class QueueDescriptionCountDetails
    {
        QueueDescriptionCountDetailsXml xml;
        internal QueueDescriptionCountDetails(QueueDescriptionCountDetailsXml xml)
        {
            this.xml = xml;
        }

        /// <remarks/>
        public long ActiveMessageCount
        {
            get { return xml.ActiveMessageCount; }
        }

        /// <remarks/>
        public long DeadLetterMessageCount
        {
            get { return xml.DeadLetterMessageCount; }
        }

        /// <remarks/>
        public long ScheduledMessageCount
        {
            get { return xml.ScheduledMessageCount; }
        }

        /// <remarks/>
        public long TransferMessageCount
        {
            get { return xml.ScheduledMessageCount; }
        }

        /// <remarks/>
        public long TransferDeadLetterMessageCount
        {
            get { return xml.ScheduledMessageCount; }
        }
    }
}
