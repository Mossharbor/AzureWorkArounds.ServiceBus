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
    }
}
