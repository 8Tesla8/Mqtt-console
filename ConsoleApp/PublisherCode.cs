using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Connection;
using Infrastructure;
using Infrastructure.Mqtt;

namespace ConsoleApp
{
    public class PublisherCode
    {

        public static void Start()
        {
            try
            {
                Console.WriteLine("Starting Publisher....");
                var connectionOption = new ConnectionOptions("127.0.0.1", 1883);
                //var connectionOption = new ConnectionOptions("127.0.0.1", 1884);

                var credential = new Credentials("PublisherFromPublisherCode", "", "");


                var publisher = new Publisher(new ConsoleLogger());
                publisher.Create(connectionOption, credential);

                publisher.OnConnected += eventArgs =>
                {
                    Console.WriteLine("Connected successfully with MQTT Brokers.");
                };
                publisher.OnDisconnected += eventArgs =>
                {
                    Console.WriteLine("Disconnected from MQTT Brokers.");
                };
                publisher.OnMessageReceived += eventArgs =>
                {
                    try
                    {
                        string topic = eventArgs.ApplicationMessage.Topic;
                        if (string.IsNullOrWhiteSpace(topic) == false)
                        {
                            string payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                            Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message, ex);
                    }
                };


                publisher.Connect();
               

                PublishMessage(publisher);


                //To keep the app running in container
                //https://stackoverflow.com/questions/38549006/docker-container-exits-immediately-even-with-console-readline-in-a-net-core-c
                Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();

                publisher.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void PublishMessage(Publisher publisher)
        {
            Thread.Sleep(1000);
            Console.WriteLine("Publishing messages.");

            var counter = 0;
            while (counter < 10)
            {
                counter++;

                var msg = new Message("MyTopic", counter.ToString(), true);
                publisher.Publish(msg);

                Thread.Sleep(500);
            }

            Console.WriteLine("Publish ended!");
        }
    }
}
