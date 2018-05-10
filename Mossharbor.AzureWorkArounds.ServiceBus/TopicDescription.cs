using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    public class TopicDescription
    {
        internal TopicDescriptionXml xml = null;
        internal TopicDescription(TopicDescriptionXml xml)
        {
            this.xml = xml;
        }

        /// <summary>Initializes a new instance of the 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.TopicDescription" /> class with the specified relative path.</summary> 
        /// <param name="path">Path of the topic relative to the namespace base address.</param>
        public TopicDescription(string path)
        {
            this.xml = new TopicDescriptionXml(path);
        }
        
        /// <summary>Gets or sets the maximum size of the topic in megabytes, which is the size of memory allocated for the topic.</summary>
		/// <value>The maximum size of the topic in megabytes.</value>
        public long MaxSizeInMegabytes
        {
            get { return xml.MaxSizeInMegabytes; }
            set { xml.MaxSizeInMegabytes = value; }
        }

        /// <summary>Gets or sets the value indicating if this topic requires duplicate detection.</summary>
		/// <value>true if this topic requires duplicate detection; otherwise, false.</value>
        public bool RequiresDuplicateDetection
        {
            get { return xml.RequiresDuplicateDetection; }
            set { xml.RequiresDuplicateDetection = value; }
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
        
        /// <summary>Gets or sets a value that indicates whether server-side batched operations are enabled.</summary>
        /// <value>true if the batched operations are enabled; otherwise, false.</value>
        public bool EnableBatchedOperations
        {
            get { return xml.EnableBatchedOperations; }
            set { xml.EnableBatchedOperations = value; }
        }

        /// <remarks/>
        public long SizeInBytes
        {
            get { return xml.SizeInBytes; }
        }

        /// <summary>Gets or sets whether messages should be filtered before publishing.</summary>
        /// <value>true if message filtering is enabled before publishing; otherwise, false.</value>
        /// <remarks> This feature is recommended to be used only for development and testing purposes.  
        /// For example, when  new Rules or Filters are being added to the topic, this feature can 
        /// be used to verify that the new filter expression is working as expected. Once tested 
        /// and working fine, the feature should be turned off in production. </remarks>
        /// <exception cref="T:Microsoft.ServiceBus.Messaging.NoMatchingSubscriptionException">Thrown if the subscriptions do not match.</exception>
        public bool EnableFilteringMessagesBeforePublishing
        {
            get
            {
                return this.xml.FilteringMessagesBeforePublishing;
            }
            set
            {
                this.xml.FilteringMessagesBeforePublishing = value;
            }
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

        /// <summary>Gets or sets the current status of the topic (enabled or 
		/// disabled). When an entity is disabled, that entity cannot send or receive messages.</summary> 
		/// <value>The current status of the topic.</value>
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
        
        /// <summary>Gets or sets a value that indicates whether the topic supports ordering.</summary>
		/// <value>true if the topic supports ordering; otherwise, false.</value>
        public bool SupportOrdering
        {
            get { return xml.SupportOrdering; }
            set { xml.SupportOrdering = value; }
        }
        
        /// <summary>Gets or sets the 
        /// <see cref="T:System.TimeSpan" /> idle interval after which the topic is automatically deleted. The minimum duration is 5 minutes.</summary> 
        /// <value>The auto delete on idle time span for the topic.</value>
        public TimeSpan AutoDeleteOnIdle
        {
            get { return xml.AutoDeleteOnIdle; }
            set { xml.AutoDeleteOnIdle = value; }
        }

        /// <summary>Gets or sets a value that indicates whether the topic to be partitioned across multiple message brokers is enabled. </summary>
		/// <value>true if the topic to be partitioned across multiple message brokers is enabled; otherwise, false.</value>
        public bool EnablePartitioning
        {
            get { return xml.EnablePartitioning; }
            set { xml.EnablePartitioning = value; }
        }

        /// <summary> Gets or sets whether partitioning is enabled or disabled. </summary>
        public bool EnableSubscriptionPartitioning
        {
            get { return xml.EnableSubscriptionPartitioning; }
            set { xml.EnableSubscriptionPartitioning = value; }
        }

        /// <summary>Gets or sets the name of the topic.</summary>
		/// <value>The name of the topic.</value>
		/// <remarks>
		///   This is a relative path to the <see cref="P:Microsoft.ServiceBus.NamespaceManager.Address" />.
		/// </remarks>
        public string Path
        {
            get { return xml.Path; }
            set { xml.Path = value; }
        }
        
        /// <remarks/>
        public string EntityAvailabilityStatus
        {
            get { return xml.EntityAvailabilityStatus; }
        }

        /// <summary>Gets or sets a value that indicates whether Express Entities are enabled. An 
		/// express topic holds a message in memory temporarily before writing it to persistent storage.</summary> 
		/// <value>true if Express Entities are enabled; otherwise, false.</value>
        public bool EnableExpress
        {
            get { return xml.EnableExpress; }
            set { xml.EnableExpress = value; }
        }
    }
}
