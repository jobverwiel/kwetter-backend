using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Messaging
{
    public class MessageConsumer
    {
        public MessageConsumer()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:7101")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("email-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };

            channel.BasicConsume("email-queue", true, consumer);
        }
    }
}
