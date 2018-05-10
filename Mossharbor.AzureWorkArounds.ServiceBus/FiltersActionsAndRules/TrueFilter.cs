using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Filter", DataType = "TrueFilter")]
    public class TrueFilter : SqlFilter
    {
        internal readonly static TrueFilter Default = new TrueFilter();

        public TrueFilter() : base("1=1")
        {
        }

    }
}
