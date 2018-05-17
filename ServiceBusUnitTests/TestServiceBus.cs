using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceBusUnitTests
{
    using Mossharbor.AzureWorkArounds.ServiceBus;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class TestServiceBus
    {
        public static string serviceBusConnectionString = @"Endpoint=.. TYPE YOUR CONNECTION STRING HERE!!!";


        [TestMethod]
        public void TestLongTopicName()
        {
            string topicName = "TestLongTopicName";
            while (topicName.Length < 261)
                topicName += "a";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            try
            {
                TopicDescription description = ns.CreateTopic(topicName);
                Assert.IsFalse(true);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.IsTrue(true);
            }
            finally
            {
            }
        }

        [TestMethod]
        public void TestLongSubscriptionName()
        {
            string topicName = "TestLogSubscriptionName";
            string tooLongSubName = "ServiceBusTest-f7b6c694-b264-40e1-bacb-0e30ca8b33e9";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

            TopicDescription description = ns.CreateTopic(topicName);
            Assert.IsTrue(null != description);
            try
            {
                ns.CreateSubscription(topicName, tooLongSubName);
                Assert.IsFalse(true);
            }
            catch(ArgumentOutOfRangeException)
            {
                Assert.IsTrue(true);
            }
            finally
            {
                ns.DeleteTopic(topicName);
            }
        }

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
            string name = "testTopic2";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            TopicDescription description = ns.CreateTopic(name);
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
        public void TestGetSubscriptions()
        {
            string name = "testSubscription";
            string topicName = "TestGetSubscriptions";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            try
            {
                TopicDescription tdescription = ns.CreateTopic(topicName);
                Assert.IsTrue(null != tdescription);
                SubscriptionDescription sdescription = ns.CreateSubscription(topicName, name);
                Assert.IsTrue(null != sdescription);

                IEnumerable<SubscriptionDescription> suscriptions = ns.GetSubscriptions(topicName);
                Assert.IsTrue(suscriptions.First().Name.Equals(name));
            }
            finally
            {
                ns.DeleteTopic(topicName);
            }
        }

        [TestMethod]
        public void TestGetRules()
        {
            string topicName = "TestGetRules";
            string SubscriptionName = "TestGetSubscriptions";
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                var filter = new SqlFilter("From LIKE '%Smith'");
                SubscriptionDescription initialDesc = ns.CreateSubscription(topicName, SubscriptionName, filter);
                IEnumerable<RuleDescription> rules = ns.GetRules(topicName, SubscriptionName);
                Assert.IsTrue((rules.First().Filter as SqlFilter).SqlExpression.Equals("From LIKE '%Smith'"));
            }
            finally
            {
                ns.DeleteTopic(topicName);
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
            SubscriptionDescription sdescription = ns.CreateSubscription(topicName, name);
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
    }
}
