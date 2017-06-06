using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Models
{
    public class UserFriend : User
    {
        public override int ID { get; set; }

        public override string Username { get; set; }

        public bool Status { get; set; }

        public bool Sender { get; set; }
    }
}