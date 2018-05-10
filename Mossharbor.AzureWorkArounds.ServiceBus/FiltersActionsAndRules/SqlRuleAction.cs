using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    [XmlRoot(ElementName = "Action", DataType = "SqlRuleAction")]
    public class SqlRuleAction : RuleAction
    {
        public SqlRuleAction()
        { }

        private int compatibilityField = 20;

        /// <summary>Gets the SQL expression.</summary>
		/// <value>The SQL expression.</value>
        public string SqlExpression
        {
            get;
            set;
        }

        /// <summary>This property is reserved for future use. An integer value showing the compatibility level, currently hard-coded to 20.</summary>
        /// <value>An integer value showing the compatibility level</value>
        /// <remarks>This property is reserved for future use.</remarks>
        public int CompatibilityLevel
        {
            get
            {
                if (0 == compatibilityField)
                    compatibilityField = 20;
                return compatibilityField;
            }
            set
            {
                compatibilityField = value;
            }
        }
    }
}
