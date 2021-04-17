using System.Net;
using ClassLibrary.Connection;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace ClassLibrary.Mqtt
{
    public class Broker
    {
        private readonly ICustomLogger _logger;
        private IMqttServer _mqttServer;

        public Broker(ICustomLogger logger)
        {
            _logger = logger;
        }
        
        public void Start(ConnectionOptions options)
        {
            //configure options
            var optionsBuilder = new MqttServerOptionsBuilder()
                    .WithConnectionValidator(context =>
                    {
                        _logger?.Log($"{context.ClientId} connection validator for c.Endpoint: {context.Endpoint}");
                        context.ReasonCode = MqttConnectReasonCode.Success;
                    })
                    .WithApplicationMessageInterceptor(context =>
                    {
                        //Console.WriteLine("WithApplicationMessageInterceptor block merging data");
                        //var newData = Encoding.UTF8.GetBytes(DateTime.Now.ToString("O"));
                        //var oldData = context.ApplicationMessage.Payload;
                        //var mergedData = newData.Concat(oldData).ToArray();
                        //context.ApplicationMessage.Payload = mergedData;
                    })
                    .WithConnectionBacklog(100)
                    .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(options.IpAddress))
                    .WithDefaultEndpointPort(options.Port)
                ;

            
            //start server
            _mqttServer = new MqttFactory().CreateMqttServer();
            _mqttServer.StartAsync(optionsBuilder.Build()).Wait();
        }

        public void Stop()
        {
            _mqttServer.StopAsync().Wait();
        }

    }
}