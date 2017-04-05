using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageRouter
{
    class Program
    {
        private static readonly string _path = @".\Private$\MyQueue";
        private static readonly string _firstPath = @".\Private$\FirstQueue";
        private static readonly string _secondPath = @".\Private$\SecondQueue";

        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            if (!MessageQueue.Exists(_path))
                MessageQueue.Create(_path);
            if (!MessageQueue.Exists(_firstPath))
                MessageQueue.Create(_firstPath);
            if (!MessageQueue.Exists(_secondPath))
                MessageQueue.Create(_secondPath);

            MessageRouter router = new MessageRouter(new MessageQueue(_path), new MessageQueue(_firstPath), new MessageQueue(_secondPath));

            Task.Run(() =>
            {
                while (true)
                {
                    router.Listen();

                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        router.Dispose();
                        break;
                    }
                }
               
            }, token);

            while (true)
            {
                if (Console.ReadLine().Equals("exit"))
                {
                    cancellationTokenSource.Cancel();
                    break;
                }
            }
        }
    }
}
