using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public partial class entry
    {
        internal static entry Build(Uri endpoint, string path, string saddress)
        {
            entry toXml = new entry();
            toXml.id = "uuid:e" + Guid.NewGuid().ToString() + ";id=1";
            toXml.author = new entryAuthor();
            toXml.author.name = endpoint.Host.Split('.').First();
            toXml.title = new entryTitle() { type = "text", Value = path};
            toXml.updated = DateTime.UtcNow;
            if (!String.IsNullOrEmpty(saddress))
                toXml.link = new entryLink() { rel = "self", href = saddress };
            toXml.content = new entryContent();
            return toXml;
        }

        internal string ToXml()
        {
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };

            Utf8StringWriter sw = new Utf8StringWriter();
            XmlWriter xw = XmlWriter.Create(sw, xmlWriterSettings);
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(entry));
            xs.Serialize(xw, this);
            return sw.ToString();
        }

        private string idField;

        private entryTitle titleField;

        private System.DateTime publishedField;

        private System.DateTime updatedField;

        private entryAuthor authorField;

        private entryLink linkField;

        private entryContent contentField;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public entryTitle title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public System.DateTime published
        {
            get
            {
                return this.publishedField;
            }
            set
            {
                this.publishedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool publishedSpecified { get { return false; } }

        /// <remarks/>
        public System.DateTime updated
        {
            get
            {
                return this.updatedField;
            }
            set
            {
                this.updatedField = value;
            }
        }

        /// <remarks/>
        public entryAuthor author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool authorSpecified { get { return false; } }
        
        /// <remarks/>
        public entryLink link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool linkSpecified { get { return false; } }

        /// <remarks/>
        public entryContent content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryTitle
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryAuthor
    {

        private string nameField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryLink
    {

        private string relField;

        private string hrefField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryContent
    {

        private TopicDescriptionXml topicDescriptionField;
        private QueueDescriptionXml queueDescriptionField;
        private SubscriptionDescription subscriptionDescription;
        private EventHubDescription eventHubDescriptionField;
        private ConsumerGroupDescription consumerGroupDescriptionField;
        private PartitionDescription partitionDescriptionField;

        private string typeField = "application/xml";

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="QueueDescription", Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
        public QueueDescriptionXml QueueDescription
        {
            get
            {
                return this.queueDescriptionField;
            }
            set
            {
                this.queueDescriptionField = value;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
        public TopicDescriptionXml TopicDescription
        {
            get
            {
                return this.topicDescriptionField;
            }
            set
            {
                this.topicDescriptionField = value;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
        public SubscriptionDescription SubscriptionDescription
        {
            get
            {
                return this.subscriptionDescription;
            }
            set
            {
                this.subscriptionDescription = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
        public EventHubDescription EventHubDescription
        {
            get
            {
                return this.eventHubDescriptionField;
            }
            set
            {
                this.eventHubDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
        public ConsumerGroupDescription ConsumerGroupDescription
        {
            get
            {
                return this.consumerGroupDescriptionField;
            }
            set
            {
                this.consumerGroupDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
        public PartitionDescription PartitionDescription
        {
            get
            {
                return this.partitionDescriptionField;
            }
            set
            {
                this.partitionDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

}
