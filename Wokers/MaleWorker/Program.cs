﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Model;

namespace MaleWorker
{
    class Program
    {
        private static readonly string _malePath = @".\Private$\MaleQueue";
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task.Run(() =>
            {
                while (true)
                {
                    if (MessageQueue.Exists(_malePath))
                    {
                        Stopwatch timer = new Stopwatch();
                        
                        MessageQueue messageQueue = new MessageQueue(_malePath);
                        messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Account) });
                        
                        Message receive = messageQueue.Receive();
                        timer.Start();
                        if (receive != null)
                        {
                            Account account = (Account) receive.Body;
                            Console.WriteLine("Processing...");
                            Thread.Sleep(10000);
                            timer.Stop();
                            Console.WriteLine("Message processed: " + account + ", in time: " + timer.Elapsed.TotalSeconds + "s");
                            timer.Reset();
                        }

                        messageQueue.Close();

                        if (token.IsCancellationRequested)
                            break;
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
