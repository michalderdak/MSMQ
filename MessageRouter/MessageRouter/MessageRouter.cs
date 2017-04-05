using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MessageRouter
{
    public class MessageRouter : IDisposable
    {
        private MessageQueue _inMessageQueue;
        private MessageQueue _outMessageQueue1;
        private MessageQueue _outMessageQueue2;

        public MessageRouter(MessageQueue inMessageQueue, MessageQueue outMessageQueue1, MessageQueue outMessageQueue2)
        {
            _inMessageQueue = inMessageQueue;
            _outMessageQueue1 = outMessageQueue1;
            _outMessageQueue2 = outMessageQueue2;
        }

        public void Listen()
        {
            Message message = _inMessageQueue.Receive();

            if (IsFirstFull())
            {
                _outMessageQueue2.Send(message);
            }
            else
            {
                _outMessageQueue1.Send(message);
            }
        }

        private bool IsFirstFull()
        {
            if (_outMessageQueue1.GetAllMessages().Length >= 5)
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _inMessageQueue.Dispose();
            _outMessageQueue1.Dispose();
            _outMessageQueue2.Dispose();

            _inMessageQueue.Close();
            _outMessageQueue1.Close();
            _outMessageQueue2.Close();
        }
    }
}
