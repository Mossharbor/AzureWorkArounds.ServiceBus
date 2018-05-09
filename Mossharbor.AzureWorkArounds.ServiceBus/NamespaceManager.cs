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

        private void GetAddressesNeeded(string path, out string address, out string saddress)
        {
            string rootUri = endpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/?api-version=2017-04";
        }

        private void GetAddressesNeeded(string path,  string subscription, out string address, out string saddress)
        {
            string rootUri = endpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address  =  @"http://" + rootUri + path + "/Subscriptions/" + subscription + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/Subscriptions/" + subscription + "/?api-version=2017-04";
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
                qd = new QueueDescription(t?.content?.QueueDescription);
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
            string defaultQueueDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:1da6bcce-2d91-4f3c-9b08-2d9316089931;id=1</id><title type=\"text\"></title><updated>2018-05-02T05:34:42Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><QueueDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\" /></content></entry>";
            string address, saddress;
            GetAddressesNeeded(queueName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true);
                var t = request.UploadEntryXml(saddress, defaultQueueDescription);
                if (null != t && null != t.content && null != t.content.QueueDescription)
                {
                    t.content.QueueDescription.ResetSerialization();
                    t.content.QueueDescription.Path = queueName;
                }
                if (null == t?.content?.QueueDescription)
                    return null;

                return new QueueDescription(t?.content?.QueueDescription);
            }
        }

        /// <summary>Enables you to update the queue.</summary>
		/// <param name="description">A <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> object describing the queue to be updated.</param>
		/// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> of the updated queue.</returns>
		public QueueDescription UpdateQueue(QueueDescription description)
        {
            if (String.IsNullOrWhiteSpace(description.Path))
                throw new NullReferenceException("Queue Path was snull or empty");

            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress);

            saddress = saddress.Replace("/?", "?");

            entry toXml = new entry();
            toXml.id = "uuid:e"+Guid.NewGuid().ToString()+";id=1";
            toXml.author = new entryAuthor();
            toXml.author.name = endpointAddresses.First().Host.Split('.').First();
            toXml.title = new entryTitle() { type = "text", Value = description.Path };
            toXml.updated = DateTime.UtcNow;
            toXml.link = new entryLink() { rel = "self", href = saddress };
            toXml.content = new entryContent();
            toXml.content.QueueDescription = description.xml;

            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, true);
                var t = request.UploadEntryXml<entry>(saddress, toXml);
                return new QueueDescription(t?.content?.QueueDescription);
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
                td = t?.content?.TopicDescription;
            }
            // build up the url like this:
            return (td != null);
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
            string defaultTopicDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:bf6e6ef3-d7c5-41a2-a409-7daa5affef08;id=1</id><title type=\"text\"></title><updated>2018-05-02T06:10:07Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><TopicDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\" /></content></entry>";
            string address, saddress;
            GetAddressesNeeded(topicName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true);
                var t = request.UploadEntryXml(saddress, defaultTopicDescription);
                return t?.content?.TopicDescription;
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
                    sd = t?.content?.SubscriptionDescription;
                }
                catch(WebException we)
                {
                    if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        return false;
                }
            }
            // build up the url like this:
            return (sd != null);
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
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true);
                var t = request.UploadEntryXml(saddress, defaultSubscriptionDescription);
                return t?.content?.SubscriptionDescription;
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
