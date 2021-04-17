namespace ClassLibrary.Mqtt
{
    public class Message
    {
        public string Topic { get; }
        public string Payload { get; }
        public bool Retain { get; }

        public Message(string topic, string payload): this(topic, payload, false)
        {  
        }
        
        public Message(string topic, string payload, bool retain)
        {
            Topic = topic;
            Payload = payload;
            Retain = retain;
        }
    }
}