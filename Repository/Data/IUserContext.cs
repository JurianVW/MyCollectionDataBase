using System.Collections.Generic;
using Models;

namespace Repository.Data
{
    public interface IUserContext
    {
        void SaveUser(User user);

        List GetAccount();

        bool CheckUserLogin(User user);

        void SendFriendRequest(User sender, User reciever);

        void RespondFriendRequest(User sender, User reciever, bool repsonse);
    }
}