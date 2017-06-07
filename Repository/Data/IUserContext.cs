using System.Collections.Generic;
using Models;

namespace Repository.Data
{
    public interface IUserContext
    {
        void SaveUser(User user);

        User GetUser(string username);

        List<UserFriend> GetUserFriends(string username);

        bool CheckUserLogin(User user);

        bool CheckUsernameAvailability(User user);

        bool CheckEmailAvailability(User user);

        void SendFriendRequest(int senderID, int recieverID);

        void RespondFriendRequest(int senderID, int recieverID, bool response);

        void Unfriend(int senderID, int recieverID);

        List<string> GetUsernames();
    }
}