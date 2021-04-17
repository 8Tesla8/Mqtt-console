namespace ClassLibrary.Connection
{
    public class ConnectionOptions
    {
        public int Port { get; }
        public string IpAddress { get; }
        
        public ConnectionOptions(string ipAddress, int  port)
        {
            Port = port;
            IpAddress = ipAddress;
        }

    }
}