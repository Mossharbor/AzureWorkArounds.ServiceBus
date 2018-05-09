using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    internal class Constants
    {
        public readonly static TimeSpan DefaultAllowedTimeToLive = TimeSpan.MaxValue;
        public readonly static TimeSpan MinimumAllowedTimeToLive = TimeSpan.FromSeconds(1);
        public readonly static TimeSpan MaximumAllowedTimeToLive = TimeSpan.MaxValue;
        public readonly static TimeSpan AutoDeleteOnIdleDefaultValue = Constants.DefaultAllowedTimeToLive;
        public readonly static TimeSpan MaximumDuplicateDetectionHistoryTimeWindow = TimeSpan.FromDays(7);
        public readonly static TimeSpan MinimumDuplicateDetectionHistoryTimeWindow = TimeSpan.FromSeconds(20);
        public readonly static TimeSpan DefaultDuplicateDetectionHistoryExpiryDuration = TimeSpan.FromMinutes(10);
        public readonly static TimeSpan DefaultLockDuration = TimeSpan.FromSeconds(60);
        public readonly static int DefaultMaxDeliveryCount = 10;
        public readonly static int MinAllowedMaxDeliveryCount = 1;
        public readonly static int MaxAllowedMaxDeliveryCount = 2147483647;
        public readonly static long DefaultMaxSizeInMegabytes = 1024;
        public readonly static long MaxSizeInMegabytesMinValue = 0;
        public readonly static long MaxSizeInMegabytesMaxValue = 8796093022207L;
    }
}
