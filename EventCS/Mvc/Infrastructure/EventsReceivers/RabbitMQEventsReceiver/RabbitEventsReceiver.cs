using EventToMetaValueDeconstructor;
using Mvc.Application.EventsHandler;
using Mvc.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver
{
    public class RabbitEventsReceiver : IEventsReceiver//можно добавить настройки для ресивера, чтобы имя exchange и очереди вынести
    {
        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly IEventsManager _eventsManager;
        private readonly string _exchangeName = "PrO_events";
        private readonly string _queueName = "EventCS_Listener";

        public RabbitEventsReceiver( IRabbitMQPersistentConnection rabbitMQPersistentConnection, IEventsManager eventManager )
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            _eventsManager = eventManager;
        }

        public void Receive()
        {

            List<Event> listOfEvents = new List<Event>();            

            if ( _rabbitMQPersistentConnection.TryConnect() )
            {
                JsonEventParser jsonEventParser = new JsonEventParser();

                using ( var model = _rabbitMQPersistentConnection.CreateModel() )
                {                    
                    model.ExchangeDeclare( exchange: _exchangeName, type: ExchangeType.Topic, durable: true );
                    
                    model.QueueDeclare(queue: _queueName,
                                       durable: true,
                                       exclusive: false,
                                       autoDelete: false,
                                       arguments: null);

                    model.QueueBind(queue: _queueName,
                                    exchange: _exchangeName,
                                    routingKey: "#");

                        

                    var consumer = new EventingBasicConsumer( model );

                    consumer.Received += ( model, ea ) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;

                        Event parsedEvent = jsonEventParser.Parse(routingKey, message);
                        if ( !( parsedEvent.JsonPropertiesMetaValue == null ) )
                        { 
                            listOfEvents.Add( parsedEvent );
                        }
                    };

                    model.BasicConsume(queue: _queueName,
                                       autoAck: true,
                                       consumer: consumer);                    

                    _rabbitMQPersistentConnection.Dispose();
                }
            }                
            _eventsManager.Add( listOfEvents );
        }            
    }
}
