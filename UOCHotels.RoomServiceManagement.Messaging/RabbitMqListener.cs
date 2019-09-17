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
        IMediator _mediator;
        RabbitMqSubscriberConfiguration _messagingConfig;

        public RabbitMqListener(IMediator mediator, RabbitMqSubscriberConfiguration messagingConfig)
        {
            _mediator = mediator;
            _messagingConfig = messagingConfig;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = _messagingConfig.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _messagingConfig.Exchange.Name,
                                        type: _messagingConfig.Exchange.Type);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    //To be refactored,
                    if (ea.RoutingKey == "OccupiedRoom")
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received '{0}':'{1}'",
                                          routingKey, message);

                        var occupiedRoomEvent = JsonConvert.DeserializeObject<RoomOccupationStarted>(message);
                        await _mediator.Publish(occupiedRoomEvent);
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

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
