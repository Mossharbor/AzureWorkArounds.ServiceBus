using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect", IsNullable = false)]
    public class HybridConnectionDescriptionXml
    {
        public HybridConnectionDescriptionXml()
        {
        }

        public HybridConnectionDescriptionXml(string path)
        {
            Path = path;
        }

        public DateTime CreatedAt { get; set; }
        public int ListenerCount { get; set; }
        public string Path { get; set; }
        public bool RequiresClientAuthorization { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserMetadata { get; set; }
        private AuthorizationRules authorization = null;
        public AuthorizationRules Authorization
        {
            get
            {
                if (null == authorization)
                    authorization = new AuthorizationRules();
                return authorization;
            }
            set { authorization = value; }
        }
    }
}
