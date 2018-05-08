using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect", IsNullable = false)]
    public partial class PartitionDescription
    {

        private byte sizeInBytesField;

        private sbyte beginSequenceNumberField;

        private sbyte endSequenceNumberField;

        private byte incomingBytesPerSecondField;

        private byte outgoingBytesPerSecondField;

        private sbyte lastEnqueuedOffsetField;

        private System.DateTime lastEnqueuedTimeUtcField;

        /// <remarks/>
        public byte SizeInBytes
        {
            get
            {
                return this.sizeInBytesField;
            }
            set
            {
                this.sizeInBytesField = value;
            }
        }

        /// <remarks/>
        public sbyte BeginSequenceNumber
        {
            get
            {
                return this.beginSequenceNumberField;
            }
            set
            {
                this.beginSequenceNumberField = value;
            }
        }

        /// <remarks/>
        public sbyte EndSequenceNumber
        {
            get
            {
                return this.endSequenceNumberField;
            }
            set
            {
                this.endSequenceNumberField = value;
            }
        }

        /// <remarks/>
        public byte IncomingBytesPerSecond
        {
            get
            {
                return this.incomingBytesPerSecondField;
            }
            set
            {
                this.incomingBytesPerSecondField = value;
            }
        }

        /// <remarks/>
        public byte OutgoingBytesPerSecond
        {
            get
            {
                return this.outgoingBytesPerSecondField;
            }
            set
            {
                this.outgoingBytesPerSecondField = value;
            }
        }

        /// <remarks/>
        public sbyte LastEnqueuedOffset
        {
            get
            {
                return this.lastEnqueuedOffsetField;
            }
            set
            {
                this.lastEnqueuedOffsetField = value;
            }
        }

        /// <remarks/>
        public System.DateTime LastEnqueuedTimeUtc
        {
            get
            {
                return this.lastEnqueuedTimeUtcField;
            }
            set
            {
                this.lastEnqueuedTimeUtcField = value;
            }
        }
    }


}
