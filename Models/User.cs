using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        public virtual int ID { get; set; }
        public virtual string Username { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string Password { get; set; }
        public virtual string ProfilePicture { get; set; }

        public List<UserFriend> friends = new List<UserFriend>();
    }
}