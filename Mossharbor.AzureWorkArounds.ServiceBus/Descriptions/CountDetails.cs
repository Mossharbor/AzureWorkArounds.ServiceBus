namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    /// <remarks/>
    [System.SerializableAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    public class CountDetails
    {
        private readonly CountDetailsXml _xml;
        
        internal CountDetails(CountDetailsXml xml)
        {
            _xml = xml;
        }

        /// <remarks/>
        public long ActiveMessageCount => _xml.ActiveMessageCount;

        /// <remarks/>
        public long DeadLetterMessageCount => _xml.DeadLetterMessageCount;

        /// <remarks/>
        public long ScheduledMessageCount => _xml.ScheduledMessageCount;

        /// <remarks/>
        public long TransferMessageCount => _xml.ScheduledMessageCount;

        /// <remarks/>
        public long TransferDeadLetterMessageCount => _xml.ScheduledMessageCount;
    }
}