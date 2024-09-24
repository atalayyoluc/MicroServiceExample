using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using MediatR;
using Newtonsoft.Json;
using ProductService.Application.Features.Queries;
using ProductService.Infrastructure.Repositories.Interfaces;
namespace ProductService.Application.Common.Services;
public class RabbitMqBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMqBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();


        channel.QueueDeclare(queue: "product_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(message);

                var product = await productRepository.GetProductByIdAsync(orderRequest.ProductId);

                var responseJson = JsonConvert.SerializeObject(product);

                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

                var responseBytes = Encoding.UTF8.GetBytes(responseJson);
                channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: replyProps, body: responseBytes);

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        };


        channel.BasicConsume(queue: "product_queue", autoAck: false, consumer: consumer);


        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}

public class OrderRequest
{
    public int ProductId { get; set; }
}