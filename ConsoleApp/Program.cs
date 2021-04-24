using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Connection;
using Infrastructure;
using Infrastructure.Mqtt;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Task.Run( () => PublisherCode.Start() );
            Task.Run( () => SubsriberCode.Start() );

            Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();
        }

    }
}
