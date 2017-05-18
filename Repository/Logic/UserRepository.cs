using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Repository.Data;

namespace Repository.Logic
{
    public class UserRepository
    {
        private IUserContext context;

        public UserRepository(IUserContext context)
        {
            this.context = context;
        }

        public bool CheckUserLogin(User user)
        {
            return this.context.CheckUserLogin(user);
        }

        public List GetAccount()
        {
            return this.context.GetAccount();
        }

        public void RespondFriendRequest(User sender, User reciever, bool repsonse)
        {
            this.context.RespondFriendRequest(sender, reciever, repsonse);
        }

        public void SaveUser(User user)
        {
            this.context.SaveUser(user);
        }

        public void SendFriendRequest(User sender, User reciever)
        {
            this.context.SendFriendRequest(sender, reciever);
        }
    }
}