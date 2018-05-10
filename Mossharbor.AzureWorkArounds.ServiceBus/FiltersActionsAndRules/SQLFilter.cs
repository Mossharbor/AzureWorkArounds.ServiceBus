using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    using System.Xml.Serialization;
    
    [XmlRoot(ElementName ="Filter", DataType ="SqlFilter")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    public class SqlFilter : Filter
    {
        public SqlFilter()
        {
        }

        /// <summary>Initializes a new instance of the 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.SqlFilter" /> class using the specified SQL expression.</summary> 
        /// <param name="sqlExpression">The SQL expression.</param>
        public SqlFilter(string sqlExpression) : this(sqlExpression, 20)
        {
        }

        private SqlFilter(string sqlExpression, int compatibilityLevel)
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
            this.Type = "SqlFilter";
        }

        [XmlAttribute("type",Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Type { get; set; }

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
        public int CompatibilityLevel { get; set; }

    }
}
