using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Filter", DataType = "CorrelationFilter")]
    public class CorrelationFilter : Filter
    {
        /// <summary>Initializes a new instance of the <see cref="T:Microsoft.ServiceBus.Messaging.CorrelationFilter" /> class with default values.</summary>
		public CorrelationFilter()
        {
        }

        /// <summary>Initializes a new instance of the 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.CorrelationFilter" /> class with the specified correlation identifier.</summary> 
        /// <param name="correlationId">The identifier for the correlation.</param>
        /// <exception cref="T:System.ArgumentException">Thrown when the <paramref name="correlationId" /> is null or empty.</exception>
        public CorrelationFilter(string correlationId) : this()
        {
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentNullException("correlationId");
            }
            this.CorrelationId = correlationId;
        }

        /// <summary>Gets the content type of the message. </summary>
		/// <value>The content type of the message.</value>
        public string ContentType
        {
            get;
            set;
        }

        /// <summary>Gets the identifier of the correlation.</summary>
		/// <value>The identifier of the correlation.</value>
        public string CorrelationId
        {
            get;
            set;
        }

        /// <summary>Gets the application specific label.</summary>
		/// <value>The application specific label.</value>
        public string Label
        {
            get;
            set;
        }

        /// <summary>Gets the identifier of the message.</summary>
		/// <value>The identifier of the message.</value>
        public string MessageId
        {
            get;
            set;
        }

        /// <summary>Gets the address of the queue to reply to.</summary>
		/// <value>The address of the queue to reply to.</value>
        public string ReplyTo
        {
            get;
            set;
        }

        /// <summary>Gets the session identifier to reply to.</summary>
		/// <value>The session identifier to reply to.</value>
        public string ReplyToSessionId
        {
            get;
            set;
        }

        /// <summary>Gets the session identifier.</summary>
		/// <value>The session identifier.</value>
        public string SessionId
        {
            get;
            set;
        }

        /// <summary>Gets the address to send to.</summary>
		/// <value>The address to send to.</value>
        public string To
        {
            get;
            set;
        }
    }
}
