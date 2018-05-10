using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    [XmlRoot(ElementName = "Action", DataType = "EmptyRuleAction")]
    public class EmptyRuleAction: RuleAction
    {
        internal readonly static EmptyRuleAction Default = new EmptyRuleAction();
        public EmptyRuleAction()
        {
            this.Type = "EmptyRuleAction";
        }


        [XmlAttribute("type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Type { get; set; }
    }
}
