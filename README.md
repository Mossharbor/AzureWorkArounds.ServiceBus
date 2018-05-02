# Mossharbor.AzureWorkArounds.ServiceBus
The lasted incarnation of the Azure Service Bus Api's (Microsoft.Azure.ServiceBus) are missing functionality that used to exist in the non-dotnet core libaries (mainly WindowsAzure.ServiceBus).  
This functionality that is missing involves the CRUD operations on Azure Queue's/Topics/Subscriptions and Event Hubs.  

This library adds those operations back again in a donet standard 2.0 compatible library.

you can see a summary of the issue [here](https://github.com/Azure/azure-service-bus-dotnet/issues/65)

*Example:*
```
		    string name = "testSubscription";
            string topicName = "testTopicSubscription";
            NamespaceManager ns = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            TopicDescription tdescription = ns.CreateTopic(topicName);
            SubscriptionDescription sdescription = ns.CreateSubscription(topicName, "testSubscription");

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
```