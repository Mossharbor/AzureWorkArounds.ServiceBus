using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <summary>Represents a description of the subscription.</summary>
    public class SubscriptionDescription
    {
        internal SubscriptionDescriptionXml xml = null;
        internal SubscriptionDescription(string topicPath, string subscriptionName, SubscriptionDescriptionXml xml)
        {
            this.xml = xml;
            if (null != xml)
            {
                this.xml.TopicPath = topicPath;
                this.xml.Name = subscriptionName;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.SubscriptionDescription" /> class.</summary>
		/// <param name="topicPath">The topic name.</param>
		/// <param name="subscriptionName">The subscription name.</param>
        public SubscriptionDescription(string topicPath, string subscriptionName)
        {
            this.xml = new SubscriptionDescriptionXml(topicPath, subscriptionName);
        }

        /// <summary>Gets or sets the duration of a peek lock; that is, the amount of time that the message is locked for other receivers. The maximum value for 
        /// <see cref="P:Microsoft.ServiceBus.Messaging.SubscriptionDescription.LockDuration" /> is 5 minutes; the default value is 1 minute.</summary> 
        /// <value>The duration of the lock.</value>
        public TimeSpan LockDuration
        {
            get { return this.xml.LockDuration; }
            set { this.xml.LockDuration = value; }
        }


        /// <summary>Gets or sets a value that indicates whether the subscription supports the concept of session.</summary>
        /// <value>true if the receiver application can only receive from the queue through a 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.MessageSession" />; false if a queue cannot receive using 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.MessageSession" />.</value> 
        public bool RequiresSession
        {
            get
            {
                return this.xml.RequiresSession;
            }
            set
            {
                this.xml.RequiresSession = value;
            }
        }

        /// <summary>Gets or sets the value that indicates if a subscription has dead letter support on Filter evaluation exceptions.</summary>
		/// <value>true if a subscription has dead letter support on Filter evaluation exceptions; otherwise, false.</value>
        public bool DeadLetteringOnFilterEvaluationExceptions
        {
            get
            {
                return this.xml.DeadLetteringOnFilterEvaluationExceptions;
            }
            set
            {
                this.xml.DeadLetteringOnFilterEvaluationExceptions = value;
            }
        }

        /// <remarks/>
        public long MessageCount
        {
            get
            {
                return this.xml.MessageCount;
            }
        }

        /// <summary>Gets or sets the maximum delivery count. A message is automatically deadlettered after this number of deliveries.</summary>
        /// <value>The number of maximum deliveries.</value>
        /// The default value is 10.
        public int MaxDeliveryCount
        {
            get
            {
                return this.xml.MaxDeliveryCount;
            }
            set
            {
                this.xml.MaxDeliveryCount = value;
            }
        }

        /// <summary>Gets or sets the path to the recipient to which the message is forwarded.</summary>
		/// <value>The path to the recipient to which the message is forwarded.</value>
        public string ForwardTo
        {
            get
            {
                return this.xml.ForwardTo;
            }
            set
            {
                this.xml.ForwardTo = value;
            }
        }

        /// <summary>Gets or sets a value that indicates whether server-side batched operations are enabled.</summary>
        /// <value>true if the batched operations are enabled; otherwise, false.</value>
        public bool EnableBatchedOperations
        {
            get
            {
                return this.xml.EnableBatchedOperations;
            }
            set
            {
                this.xml.EnableBatchedOperations = value;
            }
        }
        /// <summary>Gets or sets the current status of the subscription (enabled or 
        /// disabled). When an entity is disabled, that entity cannot send or receive messages.</summary> 
        /// <value>The current status of the queue.</value>
        public EntityStatus Status
        {
            get
            {
                return this.xml.Status;
            }
            set
            {
                this.xml.Status = value;
            }
        }
        /// <remarks/>
        public System.DateTime CreatedAt
        {
            get
            {
                return this.xml.CreatedAt;
            }
        }
        /// <remarks/>
        public System.DateTime UpdatedAt
        {
            get
            {
                return this.xml.CreatedAt;
            }
        }
        /// <remarks/>
        public System.DateTime AccessedAt
        {
            get
            {
                return this.xml.CreatedAt;
            }
        }
        /// <remarks/>
        public EntityAvailabilityStatus EntityAvailabilityStatus
        {
            get
            {
                return this.xml.EntityAvailabilityStatus;
            }
        }

        /// <summary>Gets or sets the default message time to live value. This is the duration after which the message expires, starting from when the message is sent to Service Bus. This is the default value used when 
        /// <see cref="P:Microsoft.ServiceBus.Messaging.BrokeredMessage.TimeToLive" /> is not set on a message itself.Messages older than their TimeToLive value will expire and no longer be retained in the message store. Subscribers will be unable to receive expired messages.A message can have a lower TimeToLive value than that specified here, but by default TimeToLive is set to 
        /// <see cref="F:System.TimeSpan.MaxValue" />. Therefore, this property becomes the default time to live value applied to messages.</summary> 
        /// <value>The default message time to live value.</value>
        public TimeSpan DefaultMessageTimeToLive
        {
            get
            {
                return this.xml.DefaultMessageTimeToLive;
            }
            set
            {
                this.xml.DefaultMessageTimeToLive = value;
            }
        }

        /// <summary>Gets or sets the value that indicates if a subscription has dead letter support when a message expires.</summary>
		/// <value>true if a subscription has dead letter support when a message expires; otherwise, false.</value>
        public bool EnableDeadLetteringOnMessageExpiration
        {
            get
            {
                return this.xml.DeadLetteringOnMessageExpiration;
            }
            set
            {
                this.xml.DeadLetteringOnMessageExpiration = value;
            }
        }
        
        /// <summary>Gets or sets the value that indicates if a subscription has dead letter support on Filter evaluation exceptions.</summary>
		/// <value>true if a subscription has dead letter support on Filter evaluation exceptions; otherwise, false.</value>
		public bool EnableDeadLetteringOnFilterEvaluationExceptions
        {
            get
            {
                return this.xml.DeadLetteringOnFilterEvaluationExceptions;
            }
            set
            {
                this.xml.DeadLetteringOnFilterEvaluationExceptions = value;
            }
        }

        /// <summary>Gets or sets the name of the Topic</summary>
        /// <value>The name of the queue.</value>
        /// <remarks>
        ///   This is a relative path to the <see cref="P:Microsoft.ServiceBus.NamespaceManager.Address" />.
        /// </remarks>
        public string TopicPath
        {
            get
            {
                return this.xml.TopicPath;
            }
        }

        /// <summary>Gets or sets the name of the subscription.</summary>
        /// <value>The name of the queue.</value>
        /// <remarks>
        ///   This is a relative path to the <see cref="P:Microsoft.ServiceBus.NamespaceManager.Address" />.
        /// </remarks>
        public string Name
        {
            get
            {
                return this.xml.Name;
            }
        }
        
        /// <summary>Gets or sets the 
        /// <see cref="T:System.TimeSpan" /> idle interval after which the queue is automatically deleted. The minimum duration is 5 minutes.</summary> 
        /// <value>The auto delete on idle time span for the queue.</value>
        public TimeSpan AutoDeleteOnIdle
        {
            get
            {
                return this.xml.AutoDeleteOnIdle;
            }
            set
            {
                this.xml.AutoDeleteOnIdle = value;
            }
        }
    }
}
