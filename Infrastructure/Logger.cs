using System;
namespace Infrastructure
{
    public interface ICustomLogger
    {
        public void Log(string type, string message);
    }

    public class ConsoleLogger : ICustomLogger
    {
        private void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Log(string type, string message)
        {
            Log($"Type: {type}, {message}");
        }
    }
}