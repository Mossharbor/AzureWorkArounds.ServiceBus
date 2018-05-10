using System;
using System.Collections.Generic;
using System.Text;

namespace Mossharbor.AzureWorkArounds.ServiceBus
{
    public enum EntityStatus
    {
        /// <summary>The status of the messaging entity is active.</summary>
        Active = 0,
        /// <summary>The status of the messaging entity is disabled.</summary>
        Disabled = 1,
        /// <summary>Resuming the previous status of the messaging entity.</summary>
        Restoring = 2,
        /// <summary>The sending status of the messaging entity is disabled.</summary>
        SendDisabled = 3,
        /// <summary>The receiving status of the messaging entity is disabled.</summary>
        ReceiveDisabled = 4,
        /// <summary>Indicates that the resource is still being created. Any creation attempt on the same resource path will result in a 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.MessagingException" /> exception (HttpCode.Conflict 409).</summary> 
        Creating = 5,
        /// <summary>Indicates that the system is still attempting cleanup of the entity. Any additional deletion call will be allowed (the system will be notified). Any additional creation call on the same resource path will result in a 
        /// <see cref="T:Microsoft.ServiceBus.Messaging.MessagingException" /> exception (HttpCode.Conflict 409).</summary> 
        Deleting = 6,
        /// <summary>The messaging entity is being renamed.</summary>
        Renaming = 7,
        /// <summary>The status of the messaging entity is unknown.</summary>
        Unknown = 99
    }

    public enum EntityAvailabilityStatus
    {
        /// <summary>The entity is unknown.</summary>
        Unknown,
        /// <summary>The entity is available.</summary>
        Available,
        /// <summary>The entity is limited.</summary>
        Limited,
        /// <summary>The entity is being restored.</summary>
        Restoring,
        /// <summary>The entity is being renamed.</summary>
        Renaming
    }
}
