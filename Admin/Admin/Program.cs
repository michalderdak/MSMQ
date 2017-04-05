using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Admin
{
    class Program
    {
        private static string _path = @".\Private$\MyQueue";

        static void Main(string[] args)
        {
            Console.WriteLine("Bello! Papagela oma papaja.");
            Console.WriteLine("Initializing Queue");

            if (MessageQueue.Exists(_path))
            {
                Console.WriteLine("Queue exists on path: " + _path);
            }
            else
            {
                try
                {
                    MessageQueue.Create(_path);
                    Console.WriteLine("Queue created on path: " + _path);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error occured during the process");
                    Console.WriteLine(exception.Message);
                }
            }
            
            while (true)
            {
                Console.WriteLine("How many messages?");
                int i = Convert.ToInt32(Console.ReadLine());

                for (int j = 0; j < i; j++)
                {
                    SendMessage(_path, "Awesome Message");
                    Thread.Sleep(1000);
                }   
            }
        }

        private static void SendMessage(string path, string message)
        {
            using (MessageQueue messageQueue = new MessageQueue(path))
            {
                messageQueue.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                messageQueue.Send(message);
            }
        }
    }
}
