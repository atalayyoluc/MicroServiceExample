using OrderService.Application.Common.Interface;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using OrderService.Domain.Entities;
using Mapster;

namespace OrderService.Infrastructure.Messages;
public class RabbitMqProductCheckService : IRabbitMqProductCheckService
{
    public async Task<ProductDetails?> PlaceOrder(int productId)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // ReplyTo adında geçici bir kuyruk oluşturuyoruz
        var replyQueueName = channel.QueueDeclare().QueueName;
        var consumer = new EventingBasicConsumer(channel);
        var correlationId = Guid.NewGuid().ToString();

        var tcs = new TaskCompletionSource<string>();

        // Cevap mesajını işleyen kısım
        consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                tcs.SetResult(response);
            }
        };

        // RabbitMQ'dan gelen cevabı dinlemeye başlıyoruz
        channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);

        var props = channel.CreateBasicProperties();
        props.ReplyTo = replyQueueName;
        props.CorrelationId = correlationId;

        // OrderRequest nesnesini oluşturuyoruz ve JSON formatına çeviriyoruz
        var orderRequest = new OrderRequest { ProductId = productId };
        var message = JsonConvert.SerializeObject(orderRequest);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        // Mesajı RabbitMQ'ya gönderiyoruz
        channel.BasicPublish(exchange: "", routingKey: "product_queue", basicProperties: props, body: messageBytes);

        // Cevap bekleniyor, maksimum 5 saniye bekliyoruz
        var timeoutTask = Task.Delay(5000);
        var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

        if (completedTask == timeoutTask)
        {
            Console.WriteLine("Zaman aşımı. RabbitMQ'dan cevap alınamadı.");
            return null;
        }

        var result = tcs.Task.Result;
        var product = JsonConvert.DeserializeObject<ProductDetails>(result);
        var productDetails = product.Adapt<ProductDetails>();

        if (result == null)
        {
            Console.WriteLine("Sipariş iptal edildi. Ürün mevcut değil.");
            return null;
        }

        Console.WriteLine("Sipariş başarıyla tamamlandı.");
        return productDetails;
    }
}

public class OrderRequest
{
    public int ProductId { get; set; }
}
