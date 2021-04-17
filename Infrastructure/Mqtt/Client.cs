using System;
using ClassLibrary.Connection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Infrastructure.Mqtt
{
    //subscriber
    public class Client
    {
        protected IManagedMqttClient _client;
        protected ManagedMqttClientOptions _options;
        
        protected string _topic; 
        protected readonly ICustomLogger _logger;

        
        public event Action<MqttClientConnectedEventArgs> OnConnected;
        public event Action<MqttClientDisconnectedEventArgs> OnDisconnected;
        public event Action<MqttApplicationMessageReceivedEventArgs> OnMessageReceived;
        

        public Client(ICustomLogger logger)
        {
            _logger = logger;
        }

        public void Create(ConnectionOptions options, Credentials credentials)
        {
            Create(options,credentials, null);
        }

        public void Create(ConnectionOptions options, Credentials credentials, string topic)
        {
            //configure options 
            var mqttClientOptions = new MqttClientOptionsBuilder() 
                .WithClientId(credentials.UserName)
                .WithTcpServer(options.IpAddress, options.Port)
                .WithCredentials(credentials.Login, credentials.Password)
                .WithCleanSession()
                .Build();

            
            var mqttManagedMqttClientOptions = _options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(mqttClientOptions)
                .Build();

            
            // Create a new MQTT client.
            //var factory = new MqttFactory();
            //_client = factory.CreateMqttClient();

            _client = new MqttFactory().CreateManagedMqttClient();

            
            //handlers
            _client.UseConnectedHandler(e =>
            {
                OnConnected?.Invoke(e);
                
                //subscribing to a topic
                if (!string.IsNullOrEmpty(topic))
                {
                    _topic = topic;
                    _client.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build()).Wait();                
                }
            });
            _client.UseDisconnectedHandler(e =>
            {
                OnDisconnected?.Invoke(e);
            });
            _client.UseApplicationMessageReceivedHandler(e =>
            {
                OnMessageReceived?.Invoke(e);
            });
        }
        
        public void Connect()
        {
            _client.StartAsync(_options).Wait();
        }
        
        public void Disconnect()
        {
            _client.StopAsync().Wait();
        }
    }
}