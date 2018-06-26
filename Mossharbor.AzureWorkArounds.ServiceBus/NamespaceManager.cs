﻿using System;
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
    [Serializable]
    public class NamespaceManager
    {
        static readonly int MAXPATHLENGTH = 260;
        static readonly int MAXNAMELENGTH = 50;

        private IEnumerable<Uri> EndpointAddresses;

        private SharedAccessSignatureTokenProvider provider;

        internal NamespaceManager(IEnumerable<Uri> endpointAddresses, SharedAccessSignatureTokenProvider provider)
        {
            this.EndpointAddresses = endpointAddresses;
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
            string rootUri = EndpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/?api-version=2017-04";

            if (changeQuestionmark)
                saddress = saddress.Replace("/?", "?");
        }

        private void GetAddressesNeeded(string path, string subscription, out string address, out string saddress, bool changeQuestionmark = false)
        {
            string rootUri = EndpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/Subscriptions/" + subscription + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/Subscriptions/" + subscription + "/?api-version=2017-04";

            if (changeQuestionmark)
                saddress = saddress.Replace("/?", "?");
        }

        private void GetTopicFeedQueryAddresses(string path, string subscription, out string address, out string saddress)
        {
            string rootUri = EndpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            bool includeRules = (null != subscription);

            if (!includeRules)
            {
                address = @"http://" + rootUri + path + "/Subscriptions/?$skip=0&$top=2147483647&api-version=2017-04";
                saddress = @"https://" + rootUri + path + "/Subscriptions/?$skip=0&$top=2147483647&api-version=2017-04";
            }
            else
            {
                address = @"http://" + rootUri + path + "/Subscriptions/" + subscription + "/Rules/?$skip=0&$top=2147483647&api-version=2017-04";
                saddress = @"https://" + rootUri + path + "/Subscriptions/" + subscription + "/Rules/?$skip=0&$top=2147483647&api-version=2017-04";
            }
        }

        private void GetConsumerGroupAddressNeeded(string path, string consumerGroup, out string address, out string saddress)
        {
            string rootUri = EndpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/?api-version=2017-04";
        }

        private void GetConsumerGroupAddressNeeded(string path, string consumerGroup, string partition, out string address, out string saddress)
        {
            string rootUri = EndpointAddresses.First().AbsoluteUri.Replace("sb://", "");
            address = @"http://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/Partitions/" + partition + "/?api-version=2017-04";
            saddress = @"https://" + rootUri + path + "/ConsumerGroups/" + consumerGroup + "/Partitions/" + partition + "/?api-version=2017-04";
        }

        private IEnumerable<T> GetFeedItems<T, TXml>(string address, string saddress, Func<string, TXml, T> entryCreator, Func<feedEntryContent, TXml> xmlSelector)
        {
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadString(saddress);
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(feed));
                var feed = (feed)xs.Deserialize(new StringReader(t));

                if (feed?.entry == null || 0 == feed.entry.Length)
                    return new T[0];

                T[] items = new T[feed.entry.Length];
                for (int i = 0; i < items.Length; ++i)
                {
                    var entry = feed.entry[i];
                    items[i] = entryCreator(entry.title.Value, xmlSelector(entry.content)); 
                }
                return items;
            }
        }

        /// <summary>Retrieves an enumerable collection of all queues in the service namespace.</summary>
        /// <returns>An object that represents the collection of all queues in the service namespace or returns an empty collection if no queue exists.</returns>
        public IEnumerable<QueueDescription> GetQueues()
        {
            string address, saddress;
            GetAddressesNeeded("$Resources/Queues", out address, out saddress, true);
            return GetFeedItems(address, saddress, (p, x) => new QueueDescription(p, x), c => c.QueueDescription);
        }

        /// <summary>Retrieves an enumerable collection of topics in a service namespace.</summary>
        /// <returns>An object that represents the collection of topics under the current namespaces or returns an empty collection if no topic exists.</returns>
        public IEnumerable<TopicDescription> GetTopics()
        {
            string address, saddress;
            GetAddressesNeeded("$Resources/Topics", out address, out saddress, true);
            return GetFeedItems(address, saddress, (p, x) => new TopicDescription(p, x), c => c.TopicDescription);
        }

        /// <summary>Determines whether a queue exists in the service namespace.</summary>
        /// <param name="path">The path of the queue relative to the service namespace base address.</param>
        /// <returns>true if a queue exists in the service namespace; otherwise, false.</returns>
        public bool QueueExists(string queueName, out QueueDescription qd)
        {
            CheckNameLength(queueName, MAXPATHLENGTH, "description.Path");
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
            CheckNameLength(queueName, MAXPATHLENGTH, "description.Path");
            QueueDescription qd;
            QueueExists(queueName, out qd);
            return qd;
        }

        /// <summary>Creates a new queue in the service namespace with the given path.</summary>
        /// <param name="queueName">The path of the queue relative to the service namespace base address.</param>
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.QueueDescription" /> of the newly created queue.</returns>
        public QueueDescription CreateQueue(string queueName)
        {
            CheckNameLength(queueName, MAXPATHLENGTH, "description.Path");
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
            CheckNameLength(description.Path, MAXPATHLENGTH, "description.Path");
            string queueName = description.Path;
            string address, saddress;
            GetAddressesNeeded(queueName, out address, out saddress);
            entry creationEntry = entry.Build(EndpointAddresses.First(), queueName, saddress);
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

            CheckNameLength(description.Path, MAXPATHLENGTH, "description.Path");

            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress, true);


            entry toXml = entry.Build(EndpointAddresses.First(), description.Path, saddress);
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
            CheckNameLength(queueName, MAXPATHLENGTH, "description.Path");
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
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
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

        /// <summary>Determines whether a topic exists in the service namespace.</summary>
        /// <param name="path">The path of the topic relative to the service namespace base address.</param>
        /// <returns>true if a topic exists in the service namespace; otherwise, false.</returns>
        public bool TopicExists(string topicName)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            TopicDescription td = null;
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
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            TopicDescription qd;
            TopicExists(topicName, out qd);
            return qd;
        }

        /// <summary>Asynchronously creates a new topic inside the service namespace with the given service namespace path.</summary>
        /// <param name="topicName">The path of the topic relative to the service namespace base address.</param>
        /// <returns>The asynchronous operation.</returns>
        public TopicDescription CreateTopic(string topicName)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetAddressesNeeded(topicName, out address, out saddress);
            string defaultTopicDescriptionXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:" + Guid.NewGuid().ToString() + ";id=1</id><title type=\"text\"></title><updated>2018-05-02T06:10:07Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><TopicDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\" /></content></entry>";
            return new TopicDescription(topicName, Create(defaultTopicDescriptionXml, address, saddress)?.TopicDescription);
        }

        /// <summary>Creates a new topic inside the service namespace with the specified topic description.</summary>
        /// <param name="topicDescription">A 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.TopicDescription" /> object describing the attributes with which the new topic will be created.</param> 
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.TopicDescription" /> of the newly created topic.</returns>
        public TopicDescription CreateTopic(TopicDescription topicDescription)
        {
            CheckNameLength(topicDescription.Path, MAXPATHLENGTH, "description.Path");
            string topicName = topicDescription.Path;
            string address, saddress;
            GetAddressesNeeded(topicName, out address, out saddress);
            entry creationEntry = entry.Build(EndpointAddresses.First(), topicName, saddress);
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

        private entryContent Create(string xml, string address, string saddress, string qaddress = null)
        {
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, qaddress: qaddress);
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
            
            CheckNameLength(description.Path, MAXPATHLENGTH, "description.Path");

            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress, true);

            entry toXml = entry.Build(EndpointAddresses.First(), description.Path, saddress);
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
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
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
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
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
                catch (WebException we)
                {
                    if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        return false;
                }
            }
            // build up the url like this:
            return (sd.xml != null);
        }


        /// <summary>Determines whether a subscription exists in the service namespace.</summary>
        /// <param name="topicPath">The path of the topic relative to the service namespace base address.</param>
        /// <param name="name">The name of the subscription.</param>
        /// <returns>true if a subscription exists in the service namespace; otherwise, false.</returns>
        public bool SubscriptionExists(string topicName, string subscriptionName)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
            SubscriptionDescription sd = null;
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
                catch (WebException we)
                {
                    if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        return false;
                }
            }
            // build up the url like this:
            return (sd.xml != null);
        }

        private void CheckNameLength(string name, int maxLength, string nameType)
        {
            if  (name.Length > maxLength)
                throw new ArgumentOutOfRangeException(nameType, name, "'The entity path/name '" + name + "' exceeds the '"+ maxLength+"' character limit.");
        }

        public SubscriptionDescription GetSubscription(string topicName, string subscriptionName)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
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
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
            string defaultSubscriptionDescription = "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>uuid:3b6fad82-0e12-4b56-93c4-fc03a5502765;id=1</id><title type=\"text\"></title><updated>2018-05-02T06:28:58Z</updated><content type=\"application/atom+xml;type=entry;charset=utf-8\"><SubscriptionDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\"><DefaultRuleDescription><Filter i:type=\"TrueFilter\"><SqlExpression>1=1</SqlExpression><CompatibilityLevel>20</CompatibilityLevel></Filter><Action i:type=\"EmptyRuleAction\" /><Name>$Default</Name></DefaultRuleDescription></SubscriptionDescription></content></entry>";
            string address, saddress;
            GetAddressesNeeded(topicName, subscriptionName, out address, out saddress);
            return new SubscriptionDescription(topicName, subscriptionName, Create(defaultSubscriptionDescription, address, saddress)?.SubscriptionDescription);
        }

        public SubscriptionDescription CreateSubscription(string topicName, string subscriptionName, Filter filter)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
            SubscriptionDescription description = new SubscriptionDescription(topicName, subscriptionName);
            description.xml.DefaultRuleDescription = new RuleDescription();
            description.xml.DefaultRuleDescription.Filter = filter;
            return CreateSubscription(description);
        }

        /// <summary>Creates a new subscription in the service namespace with the specified topic path, subscription name, and rule description.</summary>
        /// <param name="topicName">The topic path relative to the service namespace base address.</param>
        /// <param name="subscriptionName">The name of the subscription.</param>
        /// <param name="ruleDescription">A 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.RuleDescription" /> object describing the attributes with which the messages are matched and acted upon.</param> 
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.SubscriptionDescription" /> of the newly created subscription.</returns>
        /// <remarks> A default rule will be created using data from <paramref name="ruleDescription" />.
        /// If <see cref="P:Microsoft.ServiceBus.Messaging.RuleDescription.Name" /> is null or white space, then the name of the rule
        /// created will be <see cref="F:Microsoft.ServiceBus.Messaging.RuleDescription.DefaultRuleName" />. </remarks>
        public SubscriptionDescription CreateSubscription(string topicName, string subscriptionName, RuleDescription ruleDescription)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
            SubscriptionDescription description = new SubscriptionDescription(topicName, subscriptionName);
            description.xml.DefaultRuleDescription = ruleDescription;
            return CreateSubscription(description);
        }

        public SubscriptionDescription CreateSubscription(SubscriptionDescription description)
        {
            CheckNameLength(description.TopicPath, MAXPATHLENGTH, "description.Path");
            CheckNameLength(description.Name, MAXNAMELENGTH, "description.Name");
            string address, saddress;
            GetAddressesNeeded(description.TopicPath, description.Name, out address, out saddress);
            entry creationEntry = entry.Build(EndpointAddresses.First(), description.Name, saddress);
            creationEntry.content.SubscriptionDescription = description.xml;
            string xml = creationEntry.ToXml();
            var content = Create(xml, address, saddress, description.ForwardTo);
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


            CheckNameLength(description.TopicPath, MAXPATHLENGTH, "description.Path");
            CheckNameLength(description.Name, MAXNAMELENGTH, "description.Name");

            string address, saddress;
            GetAddressesNeeded(description.TopicPath, description.Name, out address, out saddress, true);

            entry toXml = entry.Build(EndpointAddresses.First(), description.Name, saddress);
            toXml.content.SubscriptionDescription = description.xml;

            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, true, description.ForwardTo);
                var t = request.UploadEntryXml(saddress, toXml);
                return new SubscriptionDescription(description.TopicPath, description.Name, t?.content?.SubscriptionDescription);
            }

        }

        /// <summary>Creates a new subscription in the service namespace with the specified subscription description and rule description.</summary>
        /// <param name="description">A 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.SubscriptionDescription" /> object describing the attributes with which the new subscription will be created.</param> 
        /// <param name="ruleDescription">A 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.RuleDescription" /> object describing the attributes with which the messages are matched and acted upon.</param> 
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.SubscriptionDescription" /> of the newly created subscription.</returns>
        /// <remarks> A default rule will be created using data from <paramref name="ruleDescription" />.
        /// If <see cref="P:Microsoft.ServiceBus.Messaging.RuleDescription.Name" /> is null or white space, then the name of the rule
        /// created will be <see cref="F:Microsoft.ServiceBus.Messaging.RuleDescription.DefaultRuleName" />. </remarks>
        public SubscriptionDescription CreateSubscription(SubscriptionDescription description, RuleDescription ruleDescription)
        {
            description.xml.DefaultRuleDescription = ruleDescription;
            return CreateSubscription(description);
        }

        /// <summary>Deletes the subscription with the specified topic path and subscription name.</summary>
        /// <param name="topicName">The topic path relative to the service namespace base address.</param>
        /// <param name="subscriptionName">The name of the subscription to delete.</param>
        public void DeleteSubscription(string topicName, string subscriptionName)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
            string address, saddress;
            GetAddressesNeeded(topicName, subscriptionName, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, false);
                request.UploadValues(saddress, "DELETE", new NameValueCollection());
            }
        }

        /// <summary>Retrieves an enumerable collection of all rules in the service namespace.</summary>
		/// <param name="topicName">The path of the topic relative to the service namespace base address.</param>
		/// <param name="subscriptionName">The name of the subscription.</param>
		/// <returns>An 
		/// <see cref="T:System.Collections.Generic.IEnumerable`1" /> object that represents the collection of all rules in the service namespace or returns an empty collection if no rule exists.</returns> 
		public IEnumerable<RuleDescription> GetRules(string topicName, string subscriptionName)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(subscriptionName, MAXNAMELENGTH, "description.Name");
            string address, saddress;
            GetTopicFeedQueryAddresses(topicName, subscriptionName, out address, out saddress);
            return GetFeedItems(address, saddress, (p, x) => x, c => c.RuleDescription);
        }

        /// <summary>Retrieves an enumerable collection of all subscriptions in the service namespace.</summary>
        /// <param name="topicName">The path of the topic relative to the service namespace base address.</param>
        /// <returns>An 
        /// <see cref="T:System.Collections.Generic.IEnumerable`1" /> object that represents the collection of all subscriptions in the service namespace or returns an empty collection if no subscription exists.</returns> 
        public IEnumerable<SubscriptionDescription> GetSubscriptions(string topicName)
        {
            CheckNameLength(topicName, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetTopicFeedQueryAddresses(topicName, null, out address, out saddress);
            return GetFeedItems(address, saddress, (p, x) => new SubscriptionDescription(topicName, p, x), c => c.SubscriptionDescription);
        }

        /// <summary>Retrieves an enumerable collection of all rules in the 
        /// service namespace with specified topic path, subscription name and filter.</summary> 
        /// <param name="topicPath">The topic path relative to the service namespace base address.</param>
        /// <param name="subscriptionName">The name of the subscription.</param>
        /// <param name="filter">The string used to filter the rules to be retrieved.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> object that represents the collection of 
        /// all rules in the service namespace or returns an empty collection if no rule exists.</returns>
        /// <remarks>
        /// Filter expression format:   {Propery} {Logical Operator} {Value} {Filter expression}
        /// -----------------------------------------------------------------------------------------
        /// Available properties:       ModifiedAt | AccessedAt | CreatedAt
        /// Logical operators:          Eq | Ne | Gt | Ge | Lt | Le  
        /// Value:                      A value of the corresponding property type
        /// </remarks>
        /// <example>
        /// <code>
        /// var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5).ToString("M/dd/yyyy hh:mm:ss");
        /// var rulesInTheLast5Minutes = namespaceManager.GetRules(topicName, subscriptionName, $"createdAt gt '{fiveMinutesAgo}'");
        /// </code>
        /// </example>
        public IEnumerable<RuleDescription> GetRules(string topicPath, string subscriptionName, string filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>Creates a new Event Hub using default values, for the given input path.</summary>
        /// <param name="eventHubName">The path to the Event Hub.</param>
        public EventHubDescription CreateEventHub(string eventHubName)
        {
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
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

        /// <summary>Indicates whether or not an Event Hub exists.</summary>
        /// <param name="eventHubName">The path to the Event Hub.</param>
        /// <returns>Returns true if the Event Hub exists; otherwise, false.</returns>
        public bool EventHubExists(string eventHubName)
        {
            EventHubDescription qd = null;
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
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            EventHubDescription qd;
            EventHubExists(eventHubName, out qd);
            return qd;
        }

        /// <summary>Deletes an Event Hub.</summary>
        /// <param name="eventHubName">The path to the Event Hub.</param>
        public void DeleteEventHub(string eventHubName)
        {
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
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
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(consumerGroup, MAXNAMELENGTH, "description.Name");
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

        public bool ConsumerGroupExists(string eventHubName, string consumerGroupName)
        {
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(consumerGroupName, MAXNAMELENGTH, "description.Name");
            ConsumerGroupDescription cgd = null;
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

        public bool ConsumerGroupExists(string eventHubName, string consumerGroupName, out ConsumerGroupDescription cgd)
        {
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(consumerGroupName, MAXNAMELENGTH, "description.Name");
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
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(consumerGroupName, MAXNAMELENGTH, "description.Name");
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
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(consumerGroupName, MAXNAMELENGTH, "description.Name");
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
        public PartitionDescription GetEventHubPartition(string eventHubName, string consumerGroupName, string partitionId)
        {
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(consumerGroupName, MAXNAMELENGTH, "description.Name");
            PartitionDescription pd;
            PartitionExists(eventHubName, consumerGroupName, partitionId, out pd);
            return pd;
        }

        public bool PartitionExists(string eventHubName, string consumerGroupName, string partitionId, out PartitionDescription pd)
        {
            CheckNameLength(eventHubName, MAXPATHLENGTH, "description.Path");
            CheckNameLength(consumerGroupName, MAXNAMELENGTH, "description.Name");
            string address, saddress;
            GetConsumerGroupAddressNeeded(eventHubName, consumerGroupName, partitionId, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadEntryXml(saddress);
                pd = t?.content?.PartitionDescription;
            }
            return (pd != null);
        }

        /// <summary>Creates a new relay in the service namespace with the given path and type.</summary>
		/// <param name="path">The path of the queue relative to the service namespace base address.</param>
		/// <param name="type">The relay type.</param>
		/// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> object for the newly created relay.</returns>
		public RelayDescription CreateRelay(string path, RelayType type)
        {
            CheckNameLength(path, MAXPATHLENGTH, "description.Path");
            RelayDescription desc = new RelayDescription(path, type);
            return CreateRelay(desc);
        }

        /// <summary>Creates a new relay in the service namespace with the specified relay description.</summary>
        /// <param name="description">The description object describing the attributes with which the new relay will be created.</param>
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> object for the newly created relay.</returns>
        public RelayDescription CreateRelay(RelayDescription description)
        {
            CheckNameLength(description.Path, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress);
            entry creationEntry = entry.Build(EndpointAddresses.First(), description.Path, saddress);
            creationEntry.content.RelayDescription = description.xml;
            var content = Create(creationEntry.ToXml(), address, saddress);
            var relayDesc = new RelayDescription(description.Path, content?.RelayDescription);
            if (null != relayDesc.xml)
            {
                relayDesc.xml.ResetSerialization();
                relayDesc.xml.Path = description.Path;
            }
            return relayDesc;
        }


        /// <summary>Determines whether a relay exists in the service namespace.</summary>
        /// <param name="path">The path of the relay relative to the service namespace base address.</param>
        public bool RelayExists(string path,out RelayDescription rd)
        {
            CheckNameLength(path, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetAddressesNeeded(path, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadEntryXml(saddress);
                rd = new RelayDescription(path, t?.content?.RelayDescription);
            }
            return (rd.xml != null);
        }

        /// <summary>Determines whether a relay exists in the service namespace.</summary>
        /// <param name="path">The path of the relay relative to the service namespace base address.</param>
        public bool RelayExists(string path)
        {
            RelayDescription rd;
            return RelayExists(path, out rd);
        }

        /// <summary>Upddates a relay endpoint.</summary>
		/// <param name="description">A <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> object describing the updated relay.</param>
		public RelayDescription UpdateRelay(RelayDescription description)
        {
            if (String.IsNullOrWhiteSpace(description.Path))
                throw new NullReferenceException("Relay Path was null or empty");

            CheckNameLength(description.Path, MAXPATHLENGTH, "description.Path");

            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress, true);

            entry toXml = entry.Build(EndpointAddresses.First(), description.Path, saddress);
            toXml.content.RelayDescription = description.xml;

            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, true);
                var t = request.UploadEntryXml(saddress, toXml);
                return new RelayDescription(description.Path, t?.content?.RelayDescription);
            }
        }

        /// <summary>Deletes the relay described by the path relative to the service namespace base address.</summary>
		/// <param name="path">The path of the relay relative to the service namespace base address.</param>
		public void DeleteRelay(string path)
        {
            CheckNameLength(path, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetAddressesNeeded(path, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, false);
                request.UploadValues(saddress, "DELETE", new NameValueCollection());
            }
        }

        /// <summary>Retrieves the details of a given relay endpoint.</summary>
		/// <param name="path">The relay path.</param>
		public RelayDescription GetRelay(string path)
        {
            RelayDescription rd;
            RelayExists(path, out rd);
            return rd;
        }

        /// <summary>Retrieves a collection of all relays in the service namespace.</summary>
        public IEnumerable<RelayDescription> GetRelays()
        {
            throw new NotImplementedException();
        }

        /// <summary>Creates a new relay in the service namespace with the given path and type.</summary>
        /// <param name="path">The path of the queue relative to the service namespace base address.</param>
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> object for the newly created relay.</returns>
        public HybridConnectionDescription CreateHybridConnection(string path)
        {
            CheckNameLength(path, MAXPATHLENGTH, "description.Path");
            HybridConnectionDescription desc = new HybridConnectionDescription(path);
            return CreateHybridConnection(desc);
        }

        /// <summary>Creates a new relay in the service namespace with the specified relay description.</summary>
        /// <param name="description">The description object describing the attributes with which the new relay will be created.</param>
        /// <returns>The <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> object for the newly created relay.</returns>
        public HybridConnectionDescription CreateHybridConnection(HybridConnectionDescription description)
        {
            CheckNameLength(description.Path, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress);
            entry creationEntry = entry.Build(EndpointAddresses.First(), description.Path, saddress);
            creationEntry.content.HybridConnectionDescription = description.xml;
            var content = Create(creationEntry.ToXml(), address, saddress);
            var relayDesc = new HybridConnectionDescription(description.Path, content?.HybridConnectionDescription);
            if (null != relayDesc.xml)
            {
                //relayDesc.xml.ResetSerialization();
                relayDesc.xml.Path = description.Path;
            }
            return relayDesc;
        }

        /// <summary>Determines whether a relay exists in the service namespace.</summary>
        /// <param name="path">The path of the relay relative to the service namespace base address.</param>
        public bool HybridConnectionExists(string path, out HybridConnectionDescription rd)
        {
            CheckNameLength(path, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetAddressesNeeded(path, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address);
                var t = request.DownloadEntryXml(saddress);
                rd = new HybridConnectionDescription(path, t?.content?.HybridConnectionDescription);
            }
            return (rd.xml != null);
        }

        /// <summary>Determines whether a relay exists in the service namespace.</summary>
        /// <param name="path">The path of the relay relative to the service namespace base address.</param>
        public bool HybridConnectionExists(string path)
        {
            HybridConnectionDescription rd;
            return HybridConnectionExists(path, out rd);
        }

        /// <summary>Updates a relay endpoint.</summary>
		/// <param name="description">A <see cref="T:Microsoft.ServiceBus.Messaging.RelayDescription" /> object describing the updated relay.</param>
		public HybridConnectionDescription UpdateHybridConnection(HybridConnectionDescription description)
        {
            if (String.IsNullOrWhiteSpace(description.Path))
                throw new NullReferenceException("Relay Path was null or empty");

            CheckNameLength(description.Path, MAXPATHLENGTH, "description.Path");

            string address, saddress;
            GetAddressesNeeded(description.Path, out address, out saddress, true);

            entry toXml = entry.Build(EndpointAddresses.First(), description.Path, saddress);
            toXml.content.HybridConnectionDescription = description.xml;

            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, true, true, true);
                var t = request.UploadEntryXml(saddress, toXml);
                return new HybridConnectionDescription(description.Path, t?.content?.HybridConnectionDescription);
            }
        }

        /// <summary>Deletes the relay described by the path relative to the service namespace base address.</summary>
		/// <param name="path">The path of the relay relative to the service namespace base address.</param>
		public void DeleteHybridConnection(string path)
        {
            CheckNameLength(path, MAXPATHLENGTH, "description.Path");
            string address, saddress;
            GetAddressesNeeded(path, out address, out saddress);
            using (System.Net.WebClient request = new WebClient())
            {
                request.AddCommmonHeaders(provider, address, false);
                request.UploadValues(saddress, "DELETE", new NameValueCollection());
            }
        }

        /// <summary>Retrieves the details of a given relay endpoint.</summary>
		/// <param name="path">The relay path.</param>
		public HybridConnectionDescription GetHybridConnection(string path)
        {
            HybridConnectionDescription rd;
            HybridConnectionExists(path, out rd);
            return rd;
        }

        /// <summary>Retrieves a collection of all relays in the service namespace.</summary>
        public IEnumerable<HybridConnectionDescription> GetHybridConnections()
        {
            throw new NotImplementedException();
        }
    }
}
