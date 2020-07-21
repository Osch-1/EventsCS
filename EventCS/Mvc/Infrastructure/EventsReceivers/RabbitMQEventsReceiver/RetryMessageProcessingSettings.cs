using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver
{
    public class RetryMessageProcessingSettings
    {
        public int QueueWaitingTime { get; set; }
        public int TimeProcessInQueueSeconds { get; set; }
        public int AttemptCount { get; set; }        
    }
}
