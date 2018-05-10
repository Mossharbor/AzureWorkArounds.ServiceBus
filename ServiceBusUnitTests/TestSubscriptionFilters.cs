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
    public class TestSubscriptionFilters
    {
        [TestMethod]
        public void TestSqlFilter()
        {
            string topicName = "TestSqlFilter";
            string SubscriptionName = "TestSqlFilter".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                var filter = new SqlFilter("From LIKE '%Smith'");
                SubscriptionDescription initialDesc = ns.CreateSubscription(topicName, SubscriptionName, filter);
            }
            finally
            {
                ns.DeleteTopic(topicName);
            }
        }
    }
}
