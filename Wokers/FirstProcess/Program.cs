using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace FirstProcess
{
    class Program
    {
        private static readonly string _firstPath = @".\Private$\FirstQueue";
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task.Run(() =>
            {
                while (true)
                {
                    if (MessageQueue.Exists(_firstPath))
                    {
                        Stopwatch timer = new Stopwatch();

                        MessageQueue messageQueue = new MessageQueue(_firstPath);
                        messageQueue.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });

                        Message receive = messageQueue.Receive();
                        timer.Start();
                        if (receive != null)
                        {
                            string message = receive.Body.ToString();
                            Console.WriteLine("Processing...");
                            Thread.Sleep(5000);
                            timer.Stop();
                            Console.WriteLine("Message processed: " + message + ", in time: " + timer.Elapsed.TotalSeconds + "s");
                            timer.Reset();
                        }

                        messageQueue.Close();

                        if (token.IsCancellationRequested)
                        {
                            messageQueue.Dispose();
                            messageQueue.Close();
                            break;
                        }

                    }
                }
            }, token);

            while (true)
            {
                string msg = Console.ReadLine();
                if (msg.Equals("exit"))
                {
                    cancellationTokenSource.Cancel();
                    break;
                }
                Console.WriteLine("If you wist to exit type: exit");
            }
        }
    }
}
