namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <remarks/>
    [System.SerializableAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    public class CountDetailsXml
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long ActiveMessageCount { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long DeadLetterMessageCount { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long ScheduledMessageCount { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long TransferMessageCount { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus")]
        public long TransferDeadLetterMessageCount { get; set; }
    }
}