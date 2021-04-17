using System;
using System.Globalization;
using System.Text;
using ClassLibrary.Connection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Infrastructure.Mqtt
{
    public class Publisher : Client
    {
        public Publisher(ICustomLogger logger) : base(logger)
        {
        }

        public void Publish(Message msg)
        {
            if (_client == null)
            {
                _logger.Log(nameof(Publisher), "Client is absent");
                return;
            }
            else if(!_client.IsConnected)
            {
                _logger.Log(nameof(Publisher), "Client is not connected");
                return;
            }

            var testMessage = new MqttApplicationMessageBuilder()
                .WithTopic(msg.Topic)
                .WithPayload(msg.Payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag(msg.Retain)
                .Build();

            _logger.Log(nameof(Publisher), $"Publishing topic: {msg.Topic}, at: {DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");    
            _client.PublishAsync(testMessage);
        }
        

    }
}