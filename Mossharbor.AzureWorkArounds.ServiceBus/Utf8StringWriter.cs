using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    internal sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
