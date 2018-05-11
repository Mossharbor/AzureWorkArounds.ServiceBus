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
                IEnumerable<RuleDescription> rules = ns.GetRules(topicName, SubscriptionName);
                Assert.IsTrue((rules.First().Filter as SqlFilter).SqlExpression.Equals("From LIKE '%Smith'"));
            }
            finally
            {
                ns.DeleteTopic(topicName);
            }
        }

        [TestMethod]
        public void TestCorrelationFilter()
        {
            string topicName = "TestCorrelationFilter";
            string SubscriptionName = "TestCorrelationFilter".ToLower() + Guid.NewGuid().ToString().Substring(0, 5);
            var ns = NamespaceManager.CreateFromConnectionString(TestServiceBus.serviceBusConnectionString);
            try
            {
                ns.CreateTopic(topicName);
                var filter = new CorrelationFilter();
                filter.Label = "blah";
                filter.ReplyTo = "x";
                filter.Properties["prop1"] = "abc"; //from message.Properties["prop1"] = "abc";
                filter.Properties["prop2"] = "xyz";
                SubscriptionDescription initialDesc = ns.CreateSubscription(topicName, SubscriptionName, filter);
                IEnumerable<RuleDescription> rules = ns.GetRules(topicName, SubscriptionName);
                Assert.IsTrue((rules.First().Filter as CorrelationFilter).Label.Equals("blah"));
                Assert.IsTrue((rules.First().Filter as CorrelationFilter).ReplyTo.Equals("x"));
                //TODO properties dont serialize correctly yet!!
            }
            finally
            {
                ns.DeleteTopic(topicName);
            }

        }
    }
}
