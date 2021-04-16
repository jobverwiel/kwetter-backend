using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLibrary.Services
{
    public class MessageProducer: IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        public MessageProducer()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithMachineName().CreateLogger();
            try
            {
                string QueueName = "email-queue";
                _factory = new ConnectionFactory();
                _factory.UserName = "guest";
                _factory.Password = "guest";
                _factory.HostName = "rabbitmq"; //change to localhost for local development
                _factory.Port = AmqpTcpEndpoint.UseDefaultPort;
                _conn = _factory.CreateConnection();
                _channel = _conn.CreateModel();
                _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                Log.Information("The following queue was created and connected to: {queueName}", QueueName);
            }
            catch (Exception ex)
            {
                Log.Error("The following exception was caught: {caughtException}", ex);
                throw;
            }
            

        }
        public bool Enqueue(string messageString)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes("server processed " + messageString);
                _channel.BasicPublish(exchange: "",
                                    routingKey: "email-queue",
                                    basicProperties: null,
                                    body: body);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("The following exception was caught: {caughtException}", ex);
                throw;
            }
            
        }
    }
}
