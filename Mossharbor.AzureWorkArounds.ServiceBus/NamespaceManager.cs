using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <summary>Represents an anchor class used in managing entities, such as queues, topics, subscriptions, and rules, in your 
	/// service namespace. You must provide service namespace address and access credentials in order to manage your service namespace.</summary> 
	/// <example>
	/// <code>
	///  NamespaceManagerSettings nsSettings = new NamespaceManagerSettings(); // with credentials and operation timeout
	///  NamespaceManager manager = new NamespaceManager(new Uri("baseUri"), nsSettings);
	/// </code>
	/// </example>
    public class NamespaceManager
    {
        private IEnumerable<Uri> endpointAddresses;

        private SharedAccessSignatureTokenProvider provider;

        internal NamespaceManager(IEnumerable<Uri> endpointAddresses, SharedAccessSignatureTokenProvider provider)
        {
            this.endpointAddresses = endpointAddresses;
            this.provider = provider;
        }

        /// <summary>
        /// Create an instance of the NamespaceManager
        /// </summary>
        /// <param name="connectionString">The connection string for the service bus</param>
        /// <returns>a new instance of a namespace manager</returns>
        public static NamespaceManager CreateFromConnectionString(string connectionString)
        {
            KeyValueConfigurationManager keyValueConfigurationManager = new KeyValueConfigurationManager(connectionString);
            if (!string.IsNullOrWhiteSpace(keyValueConfigurationManager["EntityPath"]))
            {
                throw new ArgumentException("connectionString", "Unsupported Connection String");
            }
            return keyValueConfigurationManager.CreateNamespaceManager();
        }

        private void GetAddressesNeeded(string path, out string address, out string saddress, bool changeQuestionmark = false)
        {
            string rootUri = endpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/?api-version=2017-04";

            if (changeQuestionmark)
                saddress = saddress.Replace("/?", "?");
        }

        private void GetAddressesNeeded(string path,  string subscription, out string address, out string saddress, bool changeQuestionmark = false)
        {
            string rootUri = endpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address  =  @"http://" + rootUri + path + "/Subscriptions/" + subscription + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/Subscriptions/" + subscription + "/?api-version=2017-04";

            if (changeQuestionmark)
                saddress = saddress.Replace("/?", "?");
        }
        
        private void GetConsumerGroupAddressNeeded(string path, string consumerGroup, out string address, out string saddress)
        {
            string rootUri = endpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/?api-version=2017-04";
        }

        private void GetConsumerGroupAddressNeeded(string path, string consumerGroup, string partition, out string address, out string saddress)
        {
            string rootUri = endpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/Partitions/" + partition + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/Partitions/" + partition + "/?api-version=2017-04";
        }

        /// <summary>Determines whether a queue exists in the service namespace.</summary>
        /// <param name="path">The path of the queue relative to the service namespace base address.</param>
        /// <returns>true if a queue exists in the service namespace; otherwise, false.</returns>
        public bool QueueExists(string queueName, out QueueDescription qd)
        {
            string address, saddress;
            GetAddressesNeeded(queueName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadEntryXml(saddress);
                qd = new QueueDescription(queueName, t?.content?.QueueDescription);
            }
            return (qd.xml != null);
        }

        //
        // Summary:
        //     Retrieves a queue from the service namespace.
        //
        // Parameters:
        //   path:
        //     The path of the queue relative to the service namespace base address.
        //
        // Returns:
        //     A Microsoft.ServiceBus.Messaging.QueueDescription handle to the queue, or a Microsoft.ServiceBus.Messaging.MessagingEntityNotFoundException
        //     exception if the queue does not exist in the service namespace.
        public QueueDescription GetQueue(string queueName)
        {
            QueueDescription qd;
            QueueExists(queueName, out qd);
            return qd;
        }

        /// <summary>Creates a new queue in the service namespace with the given path.</summary>
        /// <param name="queueName">The path of the queue relative to the service namespace base address.</param>
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> of the newly created queue.</returns>
        public QueueDescription CreateQueue(string queueName)
        {
            string defaultQueueDescriptionXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:1da6bcce-2d91-4f3c-9b08-2d9316089931;id=1</id><title type=\"text\"></title><updated>2018-05-02T05:34:42Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><QueueDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\" /></content></entry>";
            string address, saddress;
            GetAddressesNeeded(queueName, out address, out saddress);
            return new QueueDescription(queueName, Create(defaultQueueDescriptionXml, address, saddress)?.QueueDescription);
        }

        /// <summary>Creates a new queue in the service namespace with the specified queue description.</summary>
        /// <param name="description">A 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> object describing the attributes with which the new queue will be created.</param> 
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> of the newly created queue.</returns>
        public QueueDescription CreateQueue(QueueDescription description)
        {
            string queueName = description.Path;
            string address, saddress;
            GetAddressesNeeded(queueName, out address, out saddress);
            entry creationEntry = entry.Build(endpointAddresses.First(), queueName, saddress);
            creationEntry.content.QueueDescription = description.xml;
            var content = Create(creationEntry.ToXml(), address, saddress);
            var queueDesc = new QueueDescription(description.Path, content?.QueueDescription);
            if (null != queueDesc.xml)
            {
                queueDesc.xml.ResetSerialization();
                queueDesc.xml.Path = queueName;
            }
            return queueDesc;
        }

        /// <summary>Enables you to update the queue.</summary>
		/// <param name="description">A <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> object describing the queue to be updated.</param>
		/// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> of the updated queue.</returns>
		public QueueDescription UpdateQueue(QueueDescription description)
        {
            if (String.IsNullOrWhiteSpace(description.Path))
                throw new NullReferenceException("Queue Path was null or empty");

            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress, true);

            
            entry toXml = entry.Build(endpointAddresses.First(), description.Path, saddress);
            toXml.content.QueueDescription = description.xml;

            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, true);
                var t = request.UploadEntryXml(saddress, toXml);
                return new QueueDescription(description.Path, t?.content?.QueueDescription);
            }
        }

        /// <summary>Deletes the queue described by the path relative to the service namespace base address.</summary>
		/// <param name="path">The path of the queue relative to the service namespace base address.</param>
        public void DeleteQueue(string queueName)
        {
            string address, saddress;
            GetAddressesNeeded(queueName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, false);
                request.UploadValues(saddress, "DELETE", new NameValueCollection());
            }
        }

        /// <summary>Determines whether a topic exists in the service namespace.</summary>
        /// <param name="path">The path of the topic relative to the service namespace base address.</param>
        /// <returns>true if a topic exists in the service namespace; otherwise, false.</returns>
        public bool TopicExists(string topicName, out TopicDescription td)
        {
            td = null;
            string address, saddress;
            GetAddressesNeeded(topicName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadEntryXml(saddress);
                td = new TopicDescription(topicName, t?.content?.TopicDescription);
            }
            // build up the url like this:
            return (td.xml != null);
        }

        public TopicDescription GetTopic(string topicName)
        {
            TopicDescription qd;
            TopicExists(topicName, out qd);
            return qd;
        }

        /// <summary>Asynchronously creates a new topic inside the service namespace with the given service namespace path.</summary>
        /// <param name="topicName">The path of the topic relative to the service namespace base address.</param>
        /// <returns>The asynchronous operation.</returns>
        public TopicDescription CreateTopic(string topicName)
        {
            string address, saddress;
            GetAddressesNeeded(topicName, out address, out saddress);
            string defaultTopicDescriptionXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:"+Guid.NewGuid().ToString()+";id=1</id><title type=\"text\"></title><updated>2018-05-02T06:10:07Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><TopicDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\" /></content></entry>";
            return new TopicDescription(topicName, Create(defaultTopicDescriptionXml, address, saddress)?.TopicDescription);
        }

        /// <summary>Creates a new topic inside the service namespace with the specified topic description.</summary>
        /// <param name="topicDescription">A 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.TopicDescription" /> object describing the attributes with which the new topic will be created.</param> 
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.TopicDescription" /> of the newly created topic.</returns>
        public TopicDescription CreateTopic(TopicDescription topicDescription)
        {
            string topicName = topicDescription.Path;
            string address, saddress;
            GetAddressesNeeded(topicName, out address, out saddress);
            entry creationEntry = entry.Build(endpointAddresses.First(), topicName, saddress);
            creationEntry.content.TopicDescription = topicDescription.xml;

            var content = Create(creationEntry.ToXml(), address, saddress);
            var topicDesc = new TopicDescription(topicDescription.Path, content?.TopicDescription);
            if (null != topicDesc.xml)
            {
                topicDesc.xml.ResetSerialization();
                topicDesc.xml.Path = topicName;
            }
            return topicDesc;
        }

        private entryContent Create(string xml, string address, string saddress)
        {
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true);
                var t = request.UploadEntryXml(saddress, xml);
                return t?.content;
            }
        }
        
        /// <summary>Enables you to update the queue.</summary>
        /// <param name="description">A <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> object describing the queue to be updated.</param>
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> of the updated queue.</returns>
        public TopicDescription UpdateTopic(TopicDescription description)
        {
            if (String.IsNullOrWhiteSpace(description.Path))
                throw new NullReferenceException("Topic Path was null or empty");

            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress, true);

            entry toXml = entry.Build(endpointAddresses.First(), description.Path, saddress);
            toXml.content.TopicDescription = description.xml;

            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, true);
                var t = request.UploadEntryXml(saddress, toXml);
                return new TopicDescription(description.Path, t?.content?.TopicDescription);
            }
        }

        /// <summary>Deletes the topic described by path relative to the service namespace base address.</summary>
        /// <param name="topicName">The path of the topic relative to the service namespace base address.</param>
        public void DeleteTopic(string topicName)
        {
            string address, saddress;
            GetAddressesNeeded(topicName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, false);
                request.UploadValues(saddress, "DELETE", new NameValueCollection());
            }
        }

        /// <summary>Determines whether a subscription exists in the service namespace.</summary>
        /// <param name="topicPath">The path of the topic relative to the service namespace base address.</param>
        /// <param name="name">The name of the subscription.</param>
        /// <returns>true if a subscription exists in the service namespace; otherwise, false.</returns>
        public bool SubscriptionExists(string topicName, string subscriptionName, out SubscriptionDescription sd)
        {
            sd = null;
            string address, saddress;
            GetAddressesNeeded(topicName, subscriptionName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                try
                {
                    var t = request.DownloadEntryXml(saddress);
                    sd = new SubscriptionDescription(topicName, subscriptionName, t?.content?.SubscriptionDescription);
                }
                catch(WebException we)
                {
                    if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        return false;
                }
            }
            // build up the url like this:
            return (sd.xml != null);
        }

        public SubscriptionDescription GetSubscription(string topicName, string subscriptionName)
        {
            SubscriptionDescription qd;
            SubscriptionExists(topicName, subscriptionName, out qd);
            return qd;
        }

        /// <summary>Creates a new subscription in the service namespace with the specified topic path and subscription name.</summary>
        /// <param name="topicName">The topic path relative to the service namespace base address.</param>
        /// <param name="subscriptionName">The name of the subscription.</param>
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.SubscriptionDescription" /> of the newly created subscription.</returns>
        public SubscriptionDescription CreateSubscription(string topicName, string subscriptionName)
        {
            string defaultSubscriptionDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:3b6fad82-0e12-4b56-93c4-fc03a5502765;id=1</id><title type=\"text\"></title><updated>2018-05-02T06:28:58Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><SubscriptionDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\"><DefaultRuleDescription><Filter i:type=\"TrueFilter\"><SqlExpression>1=1</SqlExpression><CompatibilityLevel>20</CompatibilityLevel></Filter><Action i:type=\"EmptyRuleAction\" /><Name>$Default</Name></DefaultRuleDescription></SubscriptionDescription></content></entry>";
            string address, saddress;
            GetAddressesNeeded(topicName, subscriptionName,  out address, out saddress);
            return new SubscriptionDescription(topicName, subscriptionName, Create(defaultSubscriptionDescription, address, saddress)?.SubscriptionDescription);
        }
        
        public SubscriptionDescription CreateSubscription(string topicName, string subscriptionName, SqlFilter filter)
        {
            SubscriptionDescription description = new SubscriptionDescription(topicName, subscriptionName);
            description.xml.DefaultRuleDescription = new RuleDescription();
            description.xml.DefaultRuleDescription.Filter = filter;
            return CreateSubscription(description);
        }

        public SubscriptionDescription CreateSubscription(SubscriptionDescription description)
        {
            string address, saddress;
            GetAddressesNeeded(description.TopicPath, description.Name, out address, out saddress);
            entry creationEntry = entry.Build(endpointAddresses.First(), description.Name, saddress);
            creationEntry.content.SubscriptionDescription = description.xml;
            string xml = creationEntry.ToXml();
            var content = Create(xml, address, saddress);
            var subDesc = new SubscriptionDescription(description.TopicPath, description.Name, content?.SubscriptionDescription);
            if (null != subDesc.xml)
            {
                subDesc.xml.ResetSerialization();
                subDesc.xml.TopicPath = description.TopicPath;
                subDesc.xml.Name = description.Name;
            }
            return subDesc;
        }

        public SubscriptionDescription UpdateSubscription(SubscriptionDescription description)
        {
            if (String.IsNullOrWhiteSpace(description.TopicPath))
                throw new NullReferenceException("Topic Path was null or empty");
            
            if (String.IsNullOrWhiteSpace(description.Name))
                throw new NullReferenceException("Name was null or empty");

            string address, saddress;
            GetAddressesNeeded(description.TopicPath, description.Name, out address, out saddress, true);

            entry toXml = entry.Build(endpointAddresses.First(), description.Name, saddress);
            toXml.content.SubscriptionDescription = description.xml;

            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, true);
                var t = request.UploadEntryXml(saddress, toXml);
                return new SubscriptionDescription(description.TopicPath, description.Name, t?.content?.SubscriptionDescription);
            }

        }

        /// <summary>Deletes the subscription with the specified topic path and subscription name.</summary>
        /// <param name="topicName">The topic path relative to the service namespace base address.</param>
        /// <param name="subscriptionName">The name of the subscription to delete.</param>
        public void DeleteSubscription(string topicName, string subscriptionName)
        {
            string address, saddress;
            GetAddressesNeeded(topicName, subscriptionName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, false);
                request.UploadValues(saddress, "DELETE", new NameValueCollection());
            }
        }

        /// <summary>Creates a new Event Hub using default values, for the given input path.</summary>
        /// <param name="eventHubName">The path to the Event Hub.</param>
        public EventHubDescription CreateEventHub(string eventHubName)
        {
            string defaultEventHubDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:84922a04-fd86-4062-be51-7d9732be9d4b;id=1</id><title type=\"text\"></title><updated>2018-05-02T07:10:21Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><EventHubDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\" /></content></entry>";
            string address, saddress;
            GetAddressesNeeded(eventHubName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true);
                var t = request.UploadEntryXml(saddress, defaultEventHubDescription);
                return t?.content?.EventHubDescription;
            }
        }
        
        /// <summary>Indicates whether or not an Event Hub exists.</summary>
        /// <param name="eventHubName">The path to the Event Hub.</param>
        /// <returns>Returns true if the Event Hub exists; otherwise, false.</returns>
        public bool EventHubExists(string eventHubName, out EventHubDescription qd)
        {
            string address, saddress;
            GetAddressesNeeded(eventHubName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadEntryXml(saddress);
                qd = t?.content?.EventHubDescription;
            }
            return (qd != null);
        }

        public EventHubDescription GetEventHub(string eventHubName)
        {
            EventHubDescription qd;
            EventHubExists(eventHubName, out qd);
            return qd;
        }

        /// <summary>Deletes an Event Hub.</summary>
        /// <param name="eventHubName">The path to the Event Hub.</param>
        public void DeleteEventHub(string eventHubName)
        {
            DeleteQueue(eventHubName);
        }

        //
        // Summary:
        //     Creates an Event Hubs consumer group using default values, with the specified
        //     Event Hubs path and a name for the consumer group.
        //
        // Parameters:
        //   eventHubPath:
        //     The path to the Event Hub.
        //
        //   name:
        //     The name of the consumer group.
        //
        // Returns:
        //     Returns Microsoft.ServiceBus.Messaging.ConsumerGroupDescription.
        public ConsumerGroupDescription CreateConsumerGroup(string eventHubName, string consumerGroup)
        {
            string defaultConsumerGroupDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:271cb9d0-4bfa-427c-b474-b6172e46a0e2;id=2</id><title type=\"text\"></title><updated>2018-05-08T01:58:43Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><ConsumerGroupDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\" /></content></entry>";
            string address, saddress;
            GetConsumerGroupAddressNeeded(eventHubName, consumerGroup, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true);
                var t = request.UploadEntryXml(saddress, defaultConsumerGroupDescription);
                return t?.content?.ConsumerGroupDescription;
            }
        }

        public bool ConsumerGroupExists(string eventHubName, string consumerGroupName, out ConsumerGroupDescription cgd)
        {
            cgd = null;
            string address, saddress;
            GetConsumerGroupAddressNeeded(eventHubName, consumerGroupName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                try
                {
                    request.AddCommmonHeaders(provider, address);
                    var t = request.DownloadEntryXml(saddress);
                    cgd = t?.content?.ConsumerGroupDescription;
                }
                catch (System.Net.WebException we)
                {
                    if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        return false;
                }
            }
            return (cgd != null);

        }

        public ConsumerGroupDescription GetConsumerGroup(string eventHubName, string consumerGroupName)
        {
            ConsumerGroupDescription qd;
            ConsumerGroupExists(eventHubName, consumerGroupName, out qd);
            return qd;
        }
        
        //
        // Summary:
        //     Deletes a consumer group.
        //
        // Parameters:
        //   eventHubPath:
        //     The path to the Event Hub.
        //
        //   name:
        //     The name of the consumer group to delete.
        public void DeleteConsumerGroup(string eventHubName, string consumerGroupName)
        {
            string address, saddress;
            GetConsumerGroupAddressNeeded(eventHubName, consumerGroupName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, false);
                request.UploadValues(saddress, "DELETE", new NameValueCollection());
            }
        }

        //
        // Parameters:
        //   eventHubPath:
        //
        //   consumerGroupName:
        //
        //   name:
        public PartitionDescription GetEventHubPartition(string eventHubName, string consumerGroup, string partitionId)
        {
            PartitionDescription pd;
            PartitionExists(eventHubName, consumerGroup, partitionId, out pd);
            return pd;
        }

        public bool PartitionExists(string eventHubName, string consumerGroup, string partitionId, out PartitionDescription pd)
        {
            string address, saddress;
            GetConsumerGroupAddressNeeded(eventHubName, consumerGroup, partitionId, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadEntryXml(saddress);
                pd = t?.content?.PartitionDescription;
            }
            return (pd != null);
        }
    }
}
