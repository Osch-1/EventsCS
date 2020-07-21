using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver
{
    public class RabbitMQEventBusSettings
    {
        public string Application { get; set; } = "PrO";
        public string Service { get; set; }
        public int ConnectionRetryCount { get; set; } = 5;
        public RetryMessageProcessingSettings RetryMessageProcessingSettings { get; set; }
        public RabbitMQConnectionSettings ConnectionSettings { get; set; }        
    }
}
