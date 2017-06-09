using System;
using System.Collections.Generic;
using System.Data;
using Models;
using System.Data.SqlClient;

// ReSharper disable All
namespace Repository.Data
{
    public class UserMemoryContext : IUserContext
    {
        private DatabaseConnection con = new DatabaseConnection();
        private Encrypt encrypt = new Encrypt();

        public List<User> users = new List<User>();

        public bool CheckUserLogin(User user)
        {
            bool valid = false;
            List<User> users = new List<User>();
            users.Add(new User() { Username = "Tester1", Password = "Test1" });
            users.Add(new User() { Username = "Tester2", Password = "Test2" });
            foreach (User u in users)
            {
                if (u.Username == user.Username && u.Password == user.Password)
                {
                    valid = true;
                }
            }
            return valid;
        }

        public bool CheckUsernameAvailability(User user)
        {
            bool valid = true;
            List<string> usernames = new List<string>() { "Jurian", "Tester2" };
            if (usernames.Contains(user.Username))
            {
                valid = false;
            }
            return valid;
        }

        public bool CheckEmailAvailability(User user)
        {
            bool valid = true;
            List<string> usernames = new List<string>() { "Tester1@home.com", "Tester5@home.com" };
            if (usernames.Contains(user.Username))
            {
                valid = false;
            }
            return valid;
        }

        public User GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            users.Add(user);
        }

        public List<UserFriend> GetUserFriends(string username)
        {
            throw new NotImplementedException();
        }

        public void SendFriendRequest(int senderID, int recieverID)
        {
            throw new NotImplementedException();
        }

        public void RespondFriendRequest(int senderID, int recieverID, bool response)
        {
            throw new NotImplementedException();
        }

        public void Unfriend(int senderID, int recieverID)
        {
            throw new NotImplementedException();
        }

        public List<string> GetUsernames()
        {
            return new List<string> { "Hello", "This", "Is", "Dumb" };
        }
    }
}