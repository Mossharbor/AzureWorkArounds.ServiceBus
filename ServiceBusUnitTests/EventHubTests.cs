using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusUnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mossharbor.AzureWorkArounds.ServiceBus;

    [TestClass]
    public class EventHubTests
    {
        static string eventHubConnectionString = @"Endpoint=...YOUR CONNECTION STRING HERE!!";
        
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

        [TestMethod]
        public void TestConsumerGroup()
        {
            string name = "testConsumerGroupEventHub";
            string consumerGroupName = "consumergroup1";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(eventHubConnectionString);
            EventHubDescription description = ns.CreateEventHub(name);
            Assert.IsTrue(null != description);
            ConsumerGroupDescription cgDescription = ns.CreateConsumerGroup(name, consumerGroupName);

            if (!ns.ConsumerGroupExists(name, consumerGroupName, out cgDescription))
                Assert.Fail("Consumer Group did not exist");
            else
            {
                Assert.IsTrue(null != cgDescription);
                ns.DeleteConsumerGroup(name, consumerGroupName);
                if (ns.ConsumerGroupExists(name, consumerGroupName, out cgDescription))
                    Assert.Fail("Consumer Group was not deleted");

                ns.DeleteEventHub(name);
                if (ns.EventHubExists(name, out description))
                    Assert.Fail("EventHub was not deleted");
            }
        }


        [TestMethod]
        public void TestParition()
        {
            string name = "testpartitionEventHub";
            string consumerGroupName = "consumergroup1";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(eventHubConnectionString);
            EventHubDescription description = ns.CreateEventHub(name);
            Assert.IsTrue(null != description);
            ConsumerGroupDescription cgDescription = ns.CreateConsumerGroup(name, consumerGroupName);
            PartitionDescription pd = ns.GetEventHubPartition(name, consumerGroupName, "1");
            Assert.IsTrue(null != pd);
            
            ns.DeleteEventHub(name);
        }
    }
}
