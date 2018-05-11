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

        /// <summary>Initializes a new instance of the 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.SqlRuleAction" /> class with the specified SQL expression.</summary> 
        /// <param name="sqlExpression">The SQL expression.</param>
        public SqlRuleAction(string sqlExpression) : this(sqlExpression, 20)
        {
        }

        /// <summary>Initializes a new instance of the 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.SqlRuleAction" /> class with the specified SQL expression and compatibility level.</summary> 
        /// <param name="sqlExpression">The SQL expression.</param>
        /// <param name="compatibilityLevel">Reserved for future use. An integer value showing compatibility level. Currently hard-coded to 20.</param>
        private SqlRuleAction(string sqlExpression, int compatibilityLevel)
        {
            if (string.IsNullOrEmpty(sqlExpression))
            {
                throw new ArgumentNullException(sqlExpression);
            }
            if (sqlExpression.Length > 1024)
            {
                throw new ArgumentOutOfRangeException(sqlExpression);
            }
            this.SqlExpression = sqlExpression;
            this.CompatibilityLevel = compatibilityLevel;
            this.Type = "SqlRuleAction";
        }

        [XmlAttribute("type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Type { get; set; }

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
