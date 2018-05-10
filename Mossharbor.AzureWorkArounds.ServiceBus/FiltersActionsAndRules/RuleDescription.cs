using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    public sealed class RuleDescription : IXmlSerializable
    {
        /// <summary>
		/// The default name used in creating default rule when adding subscriptions
		/// to a topic. The name is "$Default".
		/// </summary>
		public const string DefaultRuleName = "$Default";

        private RuleAction actionField = EmptyRuleAction.Default;
        private Filter filterField;
        private System.DateTime createdAtField;
        private string nameField = DefaultRuleName;

        /// <summary>Initializes a new instance of the 
		/// <see cref="T:Microsoft.ServiceBus.Messaging.RuleDescription" /> class with the specified name and filter expression.</summary> 
		/// <param name="name">The name of the rule.</param>
		/// <param name="filter">The filter expression used to match messages.</param>
		public RuleDescription(string name,Filter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("Filter");
            }
            this.filterField = filter;
            this.Name = name;
        }

        /// <summary>Initializes a new instance of the 
		/// <see cref="T:Microsoft.ServiceBus.Messaging.RuleDescription" /> class with the specified filter expression.</summary> 
		/// <param name="filter">The filter expression used to match messages.</param>
		public RuleDescription(Filter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("Filter");
            }
            this.filterField = filter;
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.RuleDescription" /> class with the specified name.</summary>
		/// <param name="name">The name of the rule.</param>
		public RuleDescription(string name) : this(name, TrueFilter.Default)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.RuleDescription" /> class with default values.</summary>
		public RuleDescription() : this(TrueFilter.Default)
        {
            //if (this.filterField is TrueFilter)
            //{
            //    this.filterField = TrueFilter.Default;
            //}
            //else if (this.filterField is FalseFilter)
            //{
            //    this.filterField = FalseFilter.Default;
            //}
            //if (this.actionField == null || this.actionField is EmptyRuleAction)
            //{
            //    this.actionField = EmptyRuleAction.Default;
            //}
        }

        /// <summary>Gets or sets the action to perform if the message satisfies the filtering expression.</summary>
        /// <value>The action to perform if the message satisfies the filtering expression.</value>
        [XmlIgnore]
        public RuleAction Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <summary>Gets creation time of the rule.</summary>
		/// <value>The creation time of the rule.</value>
        public System.DateTime CreatedAt
        {
            get
            {
                return this.createdAtField;
            }
            set
            {
                this.createdAtField = value;
            }
        }

        /// <summary>Gets or sets the filter expression used to match messages.</summary>
		/// <value>The filter expression used to match messages.</value>
		/// <exception cref="T:System.ArgumentNullException">null (Nothing in Visual Basic) is assigned.</exception>
        public Filter Filter
        {
            get
            {
                return (SqlFilter)this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }

        public string Name
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

        //
        // Summary:
        //     This method is reserved and should not be used. When implementing the IXmlSerializable
        //     interface, you should return null (Nothing in Visual Basic) from this method,
        //     and instead, if specifying a custom schema is required, apply the System.Xml.Serialization.XmlSchemaProviderAttribute
        //     to the class.
        //
        // Returns:
        //     An System.Xml.Schema.XmlSchema that describes the XML representation of the object
        //     that is produced by the System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)
        //     method and consumed by the System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)
        //     method.
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Generates an object from its XML representation.
        //
        // Parameters:
        //   reader:
        //     The System.Xml.XmlReader stream from which the object is deserialized.
        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement();
            reader.ReadAttributeValue();
            string typeStr = reader.ReadContentAsString();
            //[System.Xml.Serialization.XmlInclude(typeof(SqlFilter))]
            //    [System.Xml.Serialization.XmlInclude(typeof(CorrelationFilter))]
            //    [System.Xml.Serialization.XmlInclude(typeof(FalseFilter))]
            //    [System.Xml.Serialization.XmlInclude(typeof(TrueFilter))]
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Converts an object into its XML representation.
        //
        // Parameters:
        //   writer:
        //     The System.Xml.XmlWriter stream to which the object is serialized.
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer filterSrlzr = new XmlSerializer(this.Filter.GetType(), "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect"); //, 
            filterSrlzr.Serialize(writer, this.Filter);
            XmlSerializer actionSrlzr = new XmlSerializer(this.Action.GetType(), "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect");
            actionSrlzr.Serialize(writer, this.Action);
            writer.WriteElementString("Name", this.Name);
        }
    }
}