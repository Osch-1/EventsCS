using EventToMetaValueDeconstructor;
using Mvc.Application.EventsHandler;
using Mvc.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver
{
    public class RabbitEventsReceiver : IEventsReceiver, IDisposable//можно добавить настройки для ресивера, чтобы имя exchange и очереди вынести
    {
        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;        
        private readonly IServiceProvider _serviceProvider;
        private IModel _consumerChannel;
        private readonly string _exchangeName = "PrO_events";
        private readonly string _queueName = "EventCS_Listener";

        public RabbitEventsReceiver( IRabbitMQPersistentConnection rabbitMQPersistentConnection, IServiceProvider serviceProvider )
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection;            
            _serviceProvider = serviceProvider;
        }

        public void Init()
        {                                  
            if ( _rabbitMQPersistentConnection.TryConnect() )
            {
                _consumerChannel = _rabbitMQPersistentConnection.CreateModel();

                _consumerChannel.ExchangeDeclare( exchange: _exchangeName, type: ExchangeType.Topic, durable: true );

                _consumerChannel.QueueDeclare(queue: _queueName,
                                       durable: true,
                                       exclusive: false,
                                       autoDelete: false,
                                       arguments: null);

                _consumerChannel.QueueBind(queue: _queueName,
                                    exchange: _exchangeName,
                                    routingKey: "#");
                        
                var consumer = new EventingBasicConsumer( _consumerChannel );

                _consumerChannel.BasicConsume(queue: _queueName,
                                       autoAck: true,
                                       consumer: consumer);

                consumer.Received += ParseEventOnHandle;
                
            }                            
        }        
        private void ParseEventOnHandle( object model, BasicDeliverEventArgs ea )
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString( body );
            var routingKey = ea.RoutingKey;
            using ( var scope = _serviceProvider.CreateScope() )
            { 
                JsonEventParser jsonEventParser = new JsonEventParser();
                IEventsManager eventsManager = scope.ServiceProvider.GetService<IEventsManager>();

                Event parsedEvent = jsonEventParser.Parse( routingKey, message );
                if (!(parsedEvent.JsonPropertiesMetaValue == null))
                {
                    eventsManager.Add( new List<Event> { parsedEvent } );
                }
            }
        }
        public void Dispose()
        {
            if ( _consumerChannel != null )
            {
                _consumerChannel.Dispose();
            }
        }
    }
}
