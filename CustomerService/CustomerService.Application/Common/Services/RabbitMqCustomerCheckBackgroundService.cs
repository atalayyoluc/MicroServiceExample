using CustomerService.Application.Common.Interfaces.Repositories;
using CustomerService.Application.Features.Customers.Dtos;
using CustomerService.Application.Features.Customers.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CustomerService.Application.Common.Services
{
    public class RabbitMqCustomerCheckBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqCustomerCheckBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Kuyruk tanýmlamasý
            channel.QueueDeclare(queue: "customer_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            // Mesaj geldiðinde tetiklenen olay
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())  // Scoped servisler için scope oluþturuyoruz
                {
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var customerRequest = JsonConvert.DeserializeObject<CustomerCheckDto>(message);

                    var isProductAvailable = await userRepository.GetUserByIdAsync(customerRequest.CustomerId);

                    var response = isProductAvailable != null ? "Müþteri mevcut" : "Müþteri mevcut deðil";

                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: replyProps, body: responseBytes);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            channel.BasicConsume(queue: "customer_queue", autoAck: false, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
