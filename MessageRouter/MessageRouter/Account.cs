using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageRouter
{
    public class Account
    {
        public string UserName { get; set; }

        public string Gender { get; set; }


        public override string ToString()
        {
            return String.Format("Name: {0}... Gender: {1}", UserName, Gender);
        }
    }
}
