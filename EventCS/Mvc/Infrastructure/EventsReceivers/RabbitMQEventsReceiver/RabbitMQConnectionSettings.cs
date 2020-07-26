using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver
{
    public class RabbitMQConnectionSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public bool UseSsl { get; set; }
        public int ConnectionRetryCount { get; set; } = 5;
    }
}
