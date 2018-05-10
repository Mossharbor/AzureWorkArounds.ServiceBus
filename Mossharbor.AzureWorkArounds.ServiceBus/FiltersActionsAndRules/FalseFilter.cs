using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Filter", DataType = "FalseFilter")]
    public class FalseFilter : SqlFilter
    {
        internal readonly static FalseFilter Default = new FalseFilter();

        public FalseFilter() : base("1=0")
        {
        }

    }
}
