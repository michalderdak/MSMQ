using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
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

            string command;
            while (true)
            {
                command = "";
                command = Console.ReadLine();
                if (command.ToLower().Equals("send"))
                {
                    Account account = new Account();

                    Console.WriteLine("Write your user name");
                    account.UserName = Console.ReadLine();

                    Console.WriteLine("Gender: M/F");
                    string gender = Console.ReadLine();
                    if (gender.ToLower().Equals("m"))
                    {
                        account.Gender = "Male";
                    }
                    else if (gender.ToLower().Equals("f"))
                    {
                        account.Gender = "Female";
                    }

                    try
                    {
                        SendMessage(_path, account);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Something went wrong. Try again");
                        Console.WriteLine(exception.Message);
                    }
                }
                else if (command.ToLower().Equals("get all"))
                {
                    try
                    {
                        List<Account> messageList = ReadQueue(_path);
                        foreach (Account account in messageList)
                        {
                            Console.WriteLine(account);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Something went wrong. Try again");
                        Console.WriteLine(exception.Message);
                    }
                }
                else if (command.ToLower().Equals("exit"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid command");
                    Console.WriteLine("---HELP--- \n send = command for sending a message \n get all = gets list of messages from queue \n exit = exits admin");
                }
            }
        }

        private static void SendMessage(string path, Account message)
        {
            using (MessageQueue messageQueue = new MessageQueue(path))
            {
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] {typeof(Account)});
                messageQueue.Send(message);
            }
        }

        private static List<Account> ReadQueue(string path)
        {
            List<Account> accountList = new List<Account>();

            using (MessageQueue messageQueue = new MessageQueue(path))
            {
                Message[] messages = messageQueue.GetAllMessages();

                foreach (Message message in messages)
                {
                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(Account) });
                    Account account = (Account) message.Body;
                    accountList.Add(account);
                }
            }
            return accountList;
        }
    }
}
