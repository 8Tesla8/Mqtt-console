using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Connection;
using Infrastructure;
using Infrastructure.Mqtt;

namespace ConsoleApp
{
    public class SubsriberCode
    {

        public static void Start()
        {
            try
            {
                Console.WriteLine("Starting Subsriber....");

                var credential = new Credentials("SubscriberFromSubsriberCode", "", "");

                //var connectionOption = new ConnectionOptions("127.0.0.1", 1884);
                var connectionOption = new ConnectionOptions("127.0.0.1", 1883);


                var subscriber = new Client(new ConsoleLogger());
                subscriber.Create(connectionOption, credential, "MyTopic");
                subscriber.OnConnected += eventArgs =>
                {
                    Console.WriteLine("Connected successfully with MQTT Brokers.");
                };
                subscriber.OnDisconnected += eventArgs =>
                {
                    Console.WriteLine("Disconnected from MQTT Brokers.");
                };
                subscriber.OnMessageReceived += eventArgs =>
                {
                    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                    Console.WriteLine($"+ Topic = {eventArgs.ApplicationMessage.Topic}");
                    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload)}");
                    //Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel.ToString()}");
                    Console.WriteLine($"+ Retain = {eventArgs.ApplicationMessage.Retain.ToString()}");
                    Console.WriteLine();
                };

                subscriber.Connect();

                Console.WriteLine("Press key to exit");
                Console.ReadLine();


                Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();

                subscriber.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
