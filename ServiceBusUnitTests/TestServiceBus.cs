using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceBusUnitTests
{
    using Mossharbor.AzureWorkArounds.ServiceBus;

    [TestClass]
    public class TestServiceBus
    {
        static string serviceBusConnectionString = @"Endpoint=...YOUR CONNECTION STRING HERE!!";
        static string eventHubConnectionString = @"Endpoint=..YOUR CONNECTION STRING HERE!!";
        
        [TestMethod]
        public void TestQueue()
        {
            string name = "testQueue";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            QueueDescription description = ns.CreateQueue("testQueue");
            Assert.IsTrue(null != description);

            if (!ns.QueueExists(name, out description))
                Assert.Fail("Queue did not exist");
            else
            {
                Assert.IsTrue(null != description);
                ns.DeleteQueue(name);
                if (ns.QueueExists(name, out description))
                    Assert.Fail("Queue was not deleted");
            }
        }

        [TestMethod]
        public void TestTopic()
        {
            string name = "testTopic";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            TopicDescription description = ns.CreateTopic("testTopic");
            Assert.IsTrue(null != description);

            if (!ns.TopicExists(name, out description))
                Assert.Fail("Topic did not exist");
            else
            {
                Assert.IsTrue(null != description);
                ns.DeleteTopic(name);
                if (ns.TopicExists(name, out description))
                    Assert.Fail("Topic was not deleted");
            }
        }

        [TestMethod]
        public void TestSubscription()
        {
            string name = "testSubscription";
            string topicName = "testTopicSubscription";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            TopicDescription tdescription = ns.CreateTopic(topicName);
            Assert.IsTrue(null != tdescription);
            SubscriptionDescription sdescription = ns.CreateSubscription(topicName, "testSubscription");
            Assert.IsTrue(null != sdescription);

            if (!ns.SubscriptionExists(topicName, name, out sdescription))
                Assert.Fail("Subscription did not exist");
            else
            {
                Assert.IsTrue(null != sdescription);
                ns.DeleteSubscription(topicName, name);
                if (ns.SubscriptionExists(topicName, name, out sdescription))
                    Assert.Fail("Subscription was not deleted");

                ns.DeleteTopic(topicName);
                if (ns.TopicExists(name, out tdescription))
                    Assert.Fail("Topic was not deleted");
            }
        }


        [TestMethod]
        public void TestEventHub()
        {
            string name = "testEventHub";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(eventHubConnectionString);
            EventHubDescription description = ns.CreateEventHub("testEventHub");
            Assert.IsTrue(null != description);

            if (!ns.EventHubExists(name, out description))
                Assert.Fail("EventHub did not exist");
            else
            {
                Assert.IsTrue(null != description);
                ns.DeleteEventHub(name);
                if (ns.EventHubExists(name, out description))
                    Assert.Fail("EventHub was not deleted");
            }
        }
    }
}
