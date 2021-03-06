﻿using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver
{
    public interface IRabbitMQPersistentConnection
    {
        bool IsConnected { get; }
        bool TryConnect();
        void Dispose();
        IModel CreateModel();
    }
}
