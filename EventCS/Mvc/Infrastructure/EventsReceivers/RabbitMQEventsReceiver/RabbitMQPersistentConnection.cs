using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using Microsoft.Extensions.Logging;
using Mvc.Application.Interfaces;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver
{
    public class RabbitMQPersistentConnection: IRabbitMQPersistentConnection
    {
        readonly RabbitMQConnectionSettings _settings;
        readonly IConnectionFactory _connectionFactory;

        IConnection _connection;
        bool _disposed;

        object sync_root = new object();

        public RabbitMQPersistentConnection( IConnectionFactory connectionFactory, RabbitMQConnectionSettings settings )
        {
            _connectionFactory = connectionFactory;
            _settings = settings;
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if ( !IsConnected )
            {
                throw new InvalidOperationException( "No RabbitMQ connections are available to perform this action" );
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if ( _disposed ) return;

            _disposed = true;

            try
            {
                _connection?.Dispose();
            }
            catch ( IOException )
            {

            }
        }

        public bool TryConnect()
        {

            lock ( sync_root )
            {
                var policy = RetryPolicy.Handle<SocketException>().Or<BrokerUnreachableException>()
                    .WaitAndRetry( _settings.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds( Math.Pow( 2, retryAttempt ) ), ( ex, time ) =>
                    {

                    }
                );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if ( IsConnected )
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static ConnectionFactory CreateConnectionFactory(RabbitMQConnectionSettings settings)
        {
            var connectionFactory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                HostName = settings.HostName,
                UserName = settings.UserName,
                Password = settings.Password,
                VirtualHost = settings.VirtualHost
            };

            if ( settings.UseSsl )
            {
                connectionFactory.Ssl.Enabled = true;
                connectionFactory.Ssl.ServerName = string.Empty;
                connectionFactory.Ssl.CertPath = string.Empty;
                connectionFactory.Ssl.CertPassphrase = string.Empty;
                connectionFactory.Ssl.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateChainErrors | SslPolicyErrors.RemoteCertificateNameMismatch;
                connectionFactory.Ssl.Version = SslProtocols.Tls12;
            }
            return connectionFactory;
        }
    }
}
