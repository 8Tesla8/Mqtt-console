using System;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Connection;
using Infrastructure;
using Infrastructure.Mqtt;

namespace ConsoleApp
{
    public class BrokerCode
    {

        public static void Start()
        {
            var connectionOption = new ConnectionOptions("127.0.0.1", 1883);

            var broker = new Broker(new ConsoleLogger());
            broker.Start(connectionOption);



            Console.WriteLine($"Broker is Running: Host: {broker.GetConnectionOptions().IpAddress} Port: {broker.GetConnectionOptions().Port.ToString()}");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();


            //To keep the app running in container
            //https://stackoverflow.com/questions/38549006/docker-container-exits-immediately-even-with-console-readline-in-a-net-core-c
            Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();

            broker.Stop();
        }
    }
}
