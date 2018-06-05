using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mossharbor.AzureWorkArounds.ServiceBus;

namespace ServiceBusUnitTests
{
    [TestClass]
    public class TestDescriptionsOperations
    {

        [TestMethod]
        public void GetHybridConnection()
        {
            string relayName = "GetHybridConnection";
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
            try
            {
                ns.CreateHybridConnection(relayName);
                HybridConnectionDescription initialDesc = ns.GetHybridConnection(relayName);
            }
            finally
            {
                ns.DeleteRelay(relayName);
            }
        }

        [TestMethod]
        public void HybridConnectionCreation()
        {
            string relayName = "GetHybridConnection";
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
            try
            {
                ns.CreateHybridConnection(relayName);
                Assert.IsTrue(ns.HybridConnectionExists(relayName));
            }
            finally
            {
                ns.DeleteRelay(relayName);
            }
        }

        [TestMethod]
        public void HybridConnectionWithAuthorization()
        {
            string relayName = "HybridConnectionWithAuthorization";
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
            try
            {
                var relayDescription = new HybridConnectionDescription(relayName)
                {
                    RequiresClientAuthorization = true
                };

                var sendKey = SharedAccessAuthorizationRule.GenerateRandomKey();
                var sendKeyName = "SendAccessKey";
                var listenKey = SharedAccessAuthorizationRule.GenerateRandomKey();
                var listenKeyName = "ListenAccessKey";

                relayDescription.Authorization.Add(new SharedAccessAuthorizationRule(listenKeyName, listenKey,
                new List<AccessRights> { AccessRights.Listen }));
                relayDescription.Authorization.Add(new SharedAccessAuthorizationRule(sendKeyName, sendKey,
                new List<AccessRights> { AccessRights.Send }));

                ns.CreateHybridConnection(relayDescription);
                Assert.IsTrue(ns.HybridConnectionExists(relayName));
            }
            finally
            {
                ns.DeleteHybridConnection(relayName);
            }
        }

        [TestMethod]
        public void RelayHttpCreation()
        {
            string relayName = "RelayHttpCreation";
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
            try
            {
                RelayDescription initialDesc = ns.CreateRelay(relayName,RelayType.Http);
            }
            finally
            {
                ns.DeleteRelay(relayName);
            }
        }

        [TestMethod]
        public void RelayTcpCreation()
        {
            string relayName = "RelayTcpCreation";
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
            try
            {
                RelayDescription initialDesc = ns.CreateRelay(relayName, RelayType.NetTcp);
            }
            finally
            {
                ns.DeleteRelay(relayName);
            }
        }

        [TestMethod]
        public void RelayWithAuthorization()
        {
            string relayName = "RelayWithAuthorization";
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
            try
            {
                var relayDescription = new RelayDescription(relayName, RelayType.Http)
                {
                    RequiresClientAuthorization = true,
                    RequiresTransportSecurity = true
                };

                var sendKey = SharedAccessAuthorizationRule.GenerateRandomKey();
                var sendKeyName = "SendAccessKey";
                var listenKey = SharedAccessAuthorizationRule.GenerateRandomKey();
                var listenKeyName = "ListenAccessKey";

                relayDescription.Authorization.Add(new SharedAccessAuthorizationRule(listenKeyName, listenKey,
                new List<AccessRights> { AccessRights.Listen }));
                relayDescription.Authorization.Add(new SharedAccessAuthorizationRule(sendKeyName, sendKey,
                new List<AccessRights> { AccessRights.Send }));

                ns.CreateRelay(relayDescription);
                Assert.IsTrue(ns.RelayExists(relayName));
            }
            finally
            {
                ns.DeleteRelay(relayName);
            }
        }

        [TestMethod]
        public void GetRelay()
        {
            string RelayName = "GetRelay".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
            try
            {
                RelayDescription initialDesc = ns.CreateRelay(RelayName, RelayType.Http);
                RelayDescription getDesc = ns.GetRelay(RelayName);
                Assert.IsTrue(getDesc.RequiresTransportSecurity == initialDesc.RequiresTransportSecurity);
            }
            finally
            {
                ns.DeleteRelay(RelayName);
            }
        }

        //[TestMethod]
        //public void RelayModification()
        //{
        //    string RelayName = "RelayModification".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
        //    var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.relayConnectionString);
        //    try
        //    {
        //        RelayDescription initialDesc = ns.CreateRelay(RelayName,RelayType.Http);
        //        initialDesc.RequiresTransportSecurity = true;
        //        RelayDescription retDesc = ns.UpdateRelay(initialDesc);
        //        RelayDescription getDesc = ns.GetRelay(RelayName);
        //        Assert.IsTrue(getDesc.RequiresTransportSecurity == retDesc.RequiresTransportSecurity);
        //    }
        //    finally
        //    {
        //        ns.DeleteRelay(RelayName);
        //    }
        //}

        [TestMethod]
        public void QueueCreation()
        {
            string queueName = "TestQueueCreation".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                QueueDescription initialDesc = ns.CreateQueue(queueName);
            }
            finally
            {
                ns.DeleteQueue(queueName);
            }
        }

        [TestMethod]
        public void QueueCreationCustom()
        {
            string path = "QueueCreationCustom".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                QueueDescription desc = new QueueDescription(path);
                QueueDescription initialDesc = ns.CreateQueue(desc);
                QueueDescription exists = null;
                Assert.IsTrue(ns.QueueExists(path, out exists));
            }
            finally
            {
                ns.DeleteQueue(path);
            }
        }

        [TestMethod]
        public void QueueCreationCustomDuplicateDetectionHistoryTimeWindow()
        {
            string path = "QueueCreationCustomDuplicateDetectionHistoryTimeWindow".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                QueueDescription desc = new QueueDescription(path);
                desc.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(3);
                QueueDescription initialDesc = ns.CreateQueue(desc);
                QueueDescription exists = null;
                Assert.IsTrue(ns.QueueExists(path, out exists));
                Assert.IsTrue(exists.DuplicateDetectionHistoryTimeWindow.TotalMinutes == 3);
            }
            finally
            {
                ns.DeleteQueue(path);
            }
        }

        [TestMethod]
        public void QueueModification()
        {
            string queueName = "QueueModification".ToLower()+Guid.NewGuid().ToString().Substring(0,5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                QueueDescription initialDesc = ns.CreateQueue(queueName);
                initialDesc.LockDuration = TimeSpan.FromMinutes(5);
                QueueDescription retDesc = ns.UpdateQueue(initialDesc);
                QueueDescription getDesc = ns.GetQueue(queueName);
                Assert.IsTrue(getDesc.LockDuration == retDesc.LockDuration);
            }
            finally
            {
                ns.DeleteQueue(queueName);
            }
        }

        [TestMethod]
        public void QueueModification2()
        {
            string queueName = "QueueModification2".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                QueueDescription initialDesc = ns.CreateQueue(queueName);
                QueueDescription newDesc = new QueueDescription(queueName);
                newDesc.LockDuration = TimeSpan.FromMinutes(5);
                QueueDescription retDesc = ns.UpdateQueue(newDesc);
                QueueDescription getDesc = ns.GetQueue(queueName);
                Assert.IsTrue(getDesc.LockDuration == retDesc.LockDuration);
                Assert.IsTrue(getDesc.LockDuration != initialDesc.LockDuration);
                Assert.IsTrue(getDesc.LockDuration == newDesc.LockDuration);
            }
            finally
            {
                ns.DeleteQueue(queueName);
            }
        }

        [TestMethod]
        public void TopicCreation()
        {
            string TopicName = "TestTopicCreation".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                TopicDescription initialDesc = ns.CreateTopic(TopicName);
            }
            finally
            {
                ns.DeleteTopic(TopicName);
            }
        }

        [TestMethod]
        public void TopicCreationCustom()
        {
            string TopicName = "TopicCreationCustom".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                TopicDescription desc = new TopicDescription(TopicName);
                TopicDescription initialDesc = ns.CreateTopic(desc);
                TopicDescription exists = null;
                Assert.IsTrue(ns.TopicExists(TopicName, out exists));
            }
            finally
            {
                ns.DeleteTopic(TopicName);
            }
        }

        [TestMethod]
        public void TopicCreationCustomSupportOrdering()
        {
            string TopicName = "TopicCreationCustomSupportOrdering".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                TopicDescription desc = new TopicDescription(TopicName);
                desc.SupportOrdering = true;
                TopicDescription initialDesc = ns.CreateTopic(desc);
                TopicDescription exists = null;
                Assert.IsTrue(ns.TopicExists(TopicName, out exists));
                Assert.IsTrue(exists.SupportOrdering);
            }
            finally
            {
                ns.DeleteTopic(TopicName);
            }
        }

        [TestMethod]
        public void TopicModification()
        {
            string TopicName = "TopicModification".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                TopicDescription initialDesc = ns.CreateTopic(TopicName);
                initialDesc.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(5);
                TopicDescription retDesc = ns.UpdateTopic(initialDesc);
                TopicDescription getDesc = ns.GetTopic(TopicName);
                Assert.IsTrue(getDesc.DuplicateDetectionHistoryTimeWindow == retDesc.DuplicateDetectionHistoryTimeWindow);
            }
            finally
            {
                ns.DeleteTopic(TopicName);
            }
        }

        [TestMethod]
        public void TopicModification2()
        {
            string TopicName = "TopicModification2".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                TopicDescription initialDesc = ns.CreateTopic(TopicName);
                TopicDescription newDesc = new TopicDescription(TopicName);
                newDesc.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(5);
                TopicDescription retDesc = ns.UpdateTopic(newDesc);
                TopicDescription getDesc = ns.GetTopic(TopicName);
                Assert.IsTrue(getDesc.DuplicateDetectionHistoryTimeWindow == retDesc.DuplicateDetectionHistoryTimeWindow);
                Assert.IsTrue(getDesc.DuplicateDetectionHistoryTimeWindow != initialDesc.DuplicateDetectionHistoryTimeWindow);
                Assert.IsTrue(getDesc.DuplicateDetectionHistoryTimeWindow == newDesc.DuplicateDetectionHistoryTimeWindow);
            }
            finally
            {
                ns.DeleteTopic(TopicName);
            }
        }

        [TestMethod]
        public void SubscriptionCreation()
        {
            string topicName = "SubscriptionCreation";
            string SubscriptionName = "TestSubscriptionCreation".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                SubscriptionDescription initialDesc = ns.CreateSubscription(topicName, SubscriptionName);
            }
            finally
            {
                ns.DeleteSubscription(topicName, SubscriptionName);
                ns.DeleteTopic(topicName);
            }
        }

        [TestMethod]
        public void SubscriptionCreationCustom()
        {
            string topicName = "SubscriptionCreationCustom";
            string path = "SubscriptionCreationCustom".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                SubscriptionDescription desc = new SubscriptionDescription(topicName, path);
                SubscriptionDescription initialDesc = ns.CreateSubscription(desc);
                SubscriptionDescription exists = null;
                Assert.IsTrue(ns.SubscriptionExists(topicName, path, out exists));
            }
            finally
            {
                ns.DeleteSubscription(topicName, path);
                ns.DeleteTopic(topicName);
            }
        }

        [TestMethod]
        public void SubscriptionCreationAutoDeleteOnIdle()
        {
            string topicName = "SubscriptionCreationAutoDeleteOnIdle";
            string path = "SubscriptionCreationAutoDeleteOnIdle".ToLower();
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                SubscriptionDescription desc = new SubscriptionDescription(topicName, path);
                desc.AutoDeleteOnIdle = TimeSpan.FromMinutes(6); // min is 5 min
                SubscriptionDescription initialDesc = ns.CreateSubscription(desc);
                SubscriptionDescription exists = null;
                Assert.IsTrue(ns.SubscriptionExists(topicName, path, out exists));
                Assert.IsTrue(exists.AutoDeleteOnIdle.TotalMinutes == 6);
            }
            finally
            {
                ns.DeleteTopic(topicName);
            }
        }

        [TestMethod]
        public void SubscriptionModification()
        {
            string topicName = "SubscriptionModification";
            string SubscriptionName = "SubscriptionModification".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                SubscriptionDescription initialDesc = ns.CreateSubscription(topicName, SubscriptionName);
                initialDesc.MaxDeliveryCount = 3;
                SubscriptionDescription retDesc = ns.UpdateSubscription(initialDesc);
                SubscriptionDescription getDesc = ns.GetSubscription(topicName, SubscriptionName);
                Assert.IsTrue(getDesc.MaxDeliveryCount == retDesc.MaxDeliveryCount);
            }
            finally
            {
                ns.DeleteTopic(topicName);
            }
        }

        [TestMethod]
        public void SubscriptionModification2()
        {
            string topicName = "SubscriptionModification2";
            string SubscriptionName = "SubscriptionModification2".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                SubscriptionDescription initialDesc = ns.CreateSubscription(topicName, SubscriptionName);
                SubscriptionDescription newDesc = new SubscriptionDescription(topicName, SubscriptionName);
                initialDesc.MaxDeliveryCount = 3;
                SubscriptionDescription retDesc = ns.UpdateSubscription(newDesc);
                SubscriptionDescription getDesc = ns.GetSubscription(topicName, SubscriptionName);
                Assert.IsTrue(getDesc.MaxDeliveryCount == retDesc.MaxDeliveryCount);
                Assert.IsTrue(getDesc.MaxDeliveryCount != initialDesc.MaxDeliveryCount);
                Assert.IsTrue(getDesc.MaxDeliveryCount == newDesc.MaxDeliveryCount);
            }
            finally
            {
                ns.DeleteSubscription(topicName, SubscriptionName);
                ns.DeleteTopic(topicName);
            }
        }
    }
}
