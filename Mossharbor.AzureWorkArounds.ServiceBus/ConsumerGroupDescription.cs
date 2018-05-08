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
    public partial class ConsumerGroupDescription
    {

        private System.DateTime createdAtField;

        private System.DateTime updatedAtField;

        /// <remarks/>
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

        /// <remarks/>
        public System.DateTime UpdatedAt
        {
            get
            {
                return this.updatedAtField;
            }
            set
            {
                this.updatedAtField = value;
            }
        }
    }


}
