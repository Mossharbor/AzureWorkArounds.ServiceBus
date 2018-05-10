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
                ns.DeleteTopic(path);
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
                ns.DeleteTopic(path);
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
    }
}
