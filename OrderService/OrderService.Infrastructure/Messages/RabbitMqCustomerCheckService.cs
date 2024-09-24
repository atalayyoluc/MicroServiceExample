using OrderService.Application.Common.Interface;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrderService.Infrastructure.Messages;

public class RabbitMqCustomerCheckService : IRabbitMqCustomerCheckService
{

    public async Task<bool> PlaceOrder(int customerId)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var replyQueueName = channel.QueueDeclare().QueueName;
        var consumer = new EventingBasicConsumer(channel);
        var correlationId = Guid.NewGuid().ToString();

        var tcs = new TaskCompletionSource<string>();

        consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                tcs.SetResult(response);
            }
        };

        channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);

        var props = channel.CreateBasicProperties();
        props.ReplyTo = replyQueueName;
        props.CorrelationId = correlationId;

        var customerRequest = new CustomerRequest { CustomerId = customerId };
        var message = JsonConvert.SerializeObject(customerRequest);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", routingKey: "customer_queue", basicProperties: props, body: messageBytes);

        var timeoutTask = Task.Delay(5000);
        var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

        if (completedTask == timeoutTask)
        {
            Console.WriteLine("Zaman aþýmý. RabbitMQ'dan cevap alýnamadý.");
            return false;
        }

        var result = await tcs.Task;

        if (result == "Müþteri mevcut deðil")
        {
            Console.WriteLine("Sipariþ iptal edildi. Müþteri mevcut deðil.");
            return false;
        }

        return true; 
    }
}

public class CustomerRequest
{
    public int CustomerId { get; set; }
}
