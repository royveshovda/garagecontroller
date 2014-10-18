using System;

namespace GarageController
{
    public class ConsoleLogger : ILog
    {
        public void Debug(string message)
        {
            Console.WriteLine("Debug: {0}", message);
        }

        public void Debug(string message, Exception ex)
        {
            Console.WriteLine("Debug: {0}. Exception: {1}", message, ex);
        }

        public void Error(string message)
        {
            Console.WriteLine("Error: {0}", message);
        }

        public void Error(string message, Exception ex)
        {
            Console.WriteLine("Error: {0}. Exception: {1}", message, ex);
        }

        public void Info(string message)
        {
            Console.WriteLine("Info: {0}", message);
        }

        public void Info(string message, Exception ex)
        {
            Console.WriteLine("Info: {0}. Exception: {1}", message, ex);
        }

        public void Warn(string message)
        {
            Console.WriteLine("Warning: {0}", message);
        }

        public void Warn(string message, Exception ex)
        {
            Console.WriteLine("Warning: {0}. Exception: {1}", message, ex);
        }
    }
}
