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
            return context.CheckUserLogin(user);
        }

        public bool CheckUsernameAvailability(User user)
        {
            return context.CheckUsernameAvailability(user);
        }

        public bool CheckEmailAvailability(User user)
        {
            return context.CheckEmailAvailability(user);
        }

        public User GetUser(string username)
        {
            return context.GetUser(username);
        }

        public List<UserFriend> GetUserFriends(string username)
        {
            return context.GetUserFriends(username);
        }

        public void RespondFriendRequest(int senderID, int recieverID, bool response)
        {
            context.RespondFriendRequest(senderID, recieverID, response);
        }

        public void Unfriend(int senderID, int recieverID)
        {
            context.Unfriend(senderID, recieverID);
        }

        public void SaveUser(User user)
        {
            context.SaveUser(user);
        }

        public void SendFriendRequest(int senderID, string recieverUsername, string username)
        {
            bool valid = true;
            int recieverID = GetUser(recieverUsername).ID;
            foreach (UserFriend uf in context.GetUserFriends(username))
            {
                if (uf.ID == recieverID || senderID == recieverID)
                {
                    valid = false;
                }
            }
            if (valid)
            {
                context.SendFriendRequest(senderID, recieverID);
            }
        }

        public List<string> GetUserFriendUsernames(string username)
        {
            List<string> usernames = new List<string>();
            foreach (UserFriend uf in context.GetUserFriends(username))
            {
                if (uf.Status)
                {
                    usernames.Add(uf.Username);
                }
            }
            return usernames;
        }
    }
}