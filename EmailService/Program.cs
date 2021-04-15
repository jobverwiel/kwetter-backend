using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace EmailService
{
    class Program
    {
        static void Main(string[] args)
        {


            var _factory = new ConnectionFactory();
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _factory.HostName = "rabbitmq";
            _factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            var _conn = _factory.CreateConnection();
            var _channel = _conn.CreateModel();
            _channel.QueueDeclare("email-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received from Rabbit: {0}", message);
            };
            _channel.BasicConsume(queue: "email-queue",
                                    autoAck: true,
                                    consumer: consumer);
            Console.ReadLine();
        }
    }
}
