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
    public class RabbitEventsReceiver : IEventsReceiver, IDisposable
    {
        private const string _exchangeName = "PrO_events";
        private const string _queueName = "EventCS_Listener";

        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;        
        private readonly IServiceProvider _serviceProvider;        

        private IModel _consumerChannel;

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

                _consumerChannel.QueueDeclare( 
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null );

                _consumerChannel.QueueBind(
                    queue: _queueName,
                    exchange: _exchangeName,
                    routingKey: "#" );
                EventingBasicConsumer consumer = new EventingBasicConsumer( _consumerChannel );

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: true,
                    consumer: consumer );

                consumer.Received += ParseEventOnHandle;                
            }                            
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }

        private void ParseEventOnHandle( object model, BasicDeliverEventArgs ea )
        {
            byte[] body = ea.Body.ToArray();
            string message = Encoding.UTF8.GetString( body );
            string routingKey = ea.RoutingKey;
            using IServiceScope scope = _serviceProvider.CreateScope();
            JsonEventParser jsonEventParser = new JsonEventParser();
            IEventRepository eventRepository = scope.ServiceProvider.GetService<IEventRepository>();

            Event parsedEvent = jsonEventParser.Parse( routingKey, message );

            string eventFromLogKey = parsedEvent.EventKey;
            Event existingEvent = eventRepository.GetEvent( eventFromLogKey );
            bool doesEventExists = existingEvent != null;

            if ( parsedEvent.JsonPropertiesMetaValue == null )
                return;

            if (doesEventExists)
            {
                eventRepository.Update( parsedEvent );
            }
            else
            {
                eventRepository.Add( parsedEvent );
            }
        }        
    }
}
