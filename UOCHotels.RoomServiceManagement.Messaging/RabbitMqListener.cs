using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UOCHotels.RoomServiceManagement.Application.IntegrationEvents;
using UOCHotels.RoomServiceManagement.Messaging.Configuration;

namespace UOCHotels.RoomServiceManagement.Messaging
{
    public class RabbitMqListener : IHostedService
    {
        private RabbitMqSubscriberConfiguration _messagingConfig;
        public IServiceScopeFactory _scopeFactory { get; }

        public RabbitMqListener(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _messagingConfig = scope.ServiceProvider.GetRequiredService<RabbitMqSubscriberConfiguration>();
            }

            var factory = new ConnectionFactory()
            {
                HostName = _messagingConfig.HostName,
                UserName = _messagingConfig.User,
                Password = _messagingConfig.Password,
                Port = _messagingConfig.Port
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _messagingConfig.Exchange.Name,
                                    type: _messagingConfig.Exchange.Type);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                if (ea.RoutingKey == "OccupiedRoom")
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Publish(JsonConvert.DeserializeObject<RoomOccupationStarted>(message));
                    }
                }
            };

            foreach (var queue in _messagingConfig.Exchange.Queues)
            {
                channel.QueueDeclare(
                            queue: queue,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                channel.QueueBind(
                         queue: queue,
                         exchange: _messagingConfig.Exchange.Name,
                         routingKey: "OccupiedRoom");

                channel.BasicConsume(
                                queue: queue,
                                autoAck: true,
                                consumer: consumer);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
