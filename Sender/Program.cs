using RabbitMQ.Client;
using System;
using System.Text;
namespace Sender
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // If your broker resides on a different machine, you can specify the name or the IP address.
            using (var connection = factory.CreateConnection()) // Creates the RabbitMQ connection
            using (var channel = connection.CreateModel()) // Creates a channel, which is where most of the API for getting things done resides.
            {
                //Declares the queue
                channel.QueueDeclare(queue: "hello", // The name of the queue
                    durable: false, // true if we are declaring a durable queue(the queue will survive a server restart)
                    exclusive: false, // true if we are declaring an exclusive queue (restricted to this connection)
                    autoDelete: false, // true if we are declaring an auto delete queue (server will delete it when no longer in use)
                    arguments: null); // other properties for the queue

                Console.WriteLine("Please enter your message. Type 'exit' to exit.");
                while (true)
                {
                    //Converts message to byte array
                    string message = Console.ReadLine();
                    if (message?.ToUpper() == "EXIT")
                    {
                        break;
                    }

                    var body = Encoding.UTF8.GetBytes(message);

                    //Publish message
                    channel.BasicPublish(exchange: "", // the exchange to publish the message to

                                         routingKey: "hello", // the routing key
                                         basicProperties: null, // other properties for the message
                                         body: body); // the message body

                    Console.WriteLine("Sent: {0}", message);
                }
            }
        }
    }
}
