using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Messaging
{
    public class MessageProducer: IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        public MessageProducer()
        {
            _factory = new ConnectionFactory();
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _factory.HostName = "rabbitmq"; //change to localhost for local development
            _factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare("email-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        }
        public bool Enqueue(string messageString)
        {
            var body = Encoding.UTF8.GetBytes("server processed " + messageString);
            _channel.BasicPublish(exchange: "",
                                routingKey: "email-queue",
                                basicProperties: null,
                                body: body);
            //Console.WriteLine(" [x] Published {0} to RabbitMQ", messageString);
            return true;
        }
    }
}
