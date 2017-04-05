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
        private static readonly string _malePath = @".\Private$\MaleQueue";
        private static readonly string _femalePath = @".\Private$\FemaleQueue";

        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task.Run(() =>
            {
                while (true)
                {
                    Listen();
                    if (token.IsCancellationRequested)
                        break;
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

        private static void Listen()
        {
            MessageQueue messageQueue = new MessageQueue(_path);
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Account) });

            Message receive = messageQueue.Receive();
            if (receive != null)
            {
                Account account = (Account) receive.Body;

                if (account.Gender.Equals("Male"))
                {
                    SendMessage(_malePath, account);
                    Console.WriteLine("Message sent: " + account);
                }
                else if (account.Gender.Equals("Female"))
                {
                    SendMessage(_femalePath, account);
                    Console.WriteLine("Message sent: " + account);
                }
            }
            messageQueue.Close();
        }

        private static void SendMessage(string path, Account message)
        {
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path);
                Console.WriteLine("Queue created on path: " + path);
            }
            using (MessageQueue messageQueue = new MessageQueue(path))
            {
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Account) });
                messageQueue.Send(message);
            }
        }
    }
}
