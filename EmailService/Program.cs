using KwetterMessaging.Consumer;
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
            try
            {
                MessageConsumer xd = new MessageConsumer("email-queue");
                xd.ConsumeStandardQueue();
                xd.receivedMessage += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var stringBody = Encoding.UTF8.GetString(body);
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received from Rabbit: {0}", message);
                };
                while (true);
            }
            catch (Exception e)
            {
                Console.WriteLine("XD");
                throw new ArgumentException(e.ToString());
            }
        }
        public static void messageHandler(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received from Rabbit: {0}", message);
        }
    }
}
