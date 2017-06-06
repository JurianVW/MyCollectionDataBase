using System;
using System.Collections.Generic;
using System.Data;
using Models;
using System.Data.SqlClient;

// ReSharper disable All
namespace Repository.Data
{
    public class UserSQLContext : IUserContext
    {
        private DatabaseConnection con = new DatabaseConnection();
        private Encrypt encrypt = new Encrypt();

        public bool CheckUserLogin(User user)
        {
            string query = "SELECT * FROM [User] WHERE [Username] = @Username AND [Password] = @Password";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", encrypt.EncryptPassword(user.Password));

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        user.ID = dataReader.GetInt32(dataReader.GetOrdinal("ID"));
                        return true;
                    }
                    return false;
                }
            }
        }

        public bool CheckUsernameAvailability(User user)
        {
            string query = "SELECT * FROM [User] WHERE [Username] = @Username";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Username", user.Username);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        public bool CheckEmailAvailability(User user)
        {
            string query = "SELECT * FROM [User] WHERE [Email_Address] = @EmailAddress";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        public User GetUser(string username)
        {
            string query = "SELECT * FROM [User] WHERE [Username] = @Username";
            User user = new User();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        user = new User
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("ID")),
                            Username = dataReader.GetString(dataReader.GetOrdinal("Username")),
                            EmailAddress = dataReader.GetString(dataReader.GetOrdinal("Email_Address")),
                        };
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("ProfilePicture")))
                        {
                            user.ProfilePicture = dataReader.GetString(dataReader.GetOrdinal("ProfilePicture"));
                        }
                    }
                }
            }
            return user;
        }

        public List<UserFriend> GetUserFriends(string username)
        {
            string query =
                "SELECT f.User_ID_1 AS Sender_ID, CASE WHEN f.User_ID_1 = u.ID THEN f.User_ID_2 ELSE f.User_ID_1 END AS Friend_ID, " +
                "CASE WHEN f.User_ID_1 = u.ID THEN (SELECT u1.Username FROM [User] u1 WHERE u1.ID = f.User_ID_2) ELSE (SELECT u1.Username FROM [User] u1 WHERE u1.ID = f.User_ID_1) END AS Friend_Username, " +
                "f.Status FROM [User] u JOIN Friendship f ON f.User_ID_1 = u.ID OR f.User_ID_2 = u.ID WHERE u.Username = @Username";
            List<UserFriend> friends = new List<UserFriend>();
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        UserFriend uf = new UserFriend
                        {
                            ID = dataReader.GetInt32(dataReader.GetOrdinal("Friend_ID")),
                            Username = dataReader.GetString(dataReader.GetOrdinal("Friend_Username")),
                            Status = dataReader.GetBoolean(dataReader.GetOrdinal("Status")),
                            Sender = false
                        };
                        if (uf.ID != dataReader.GetInt32(dataReader.GetOrdinal("Sender_ID")))
                        {
                            uf.Sender = true;
                        }
                        friends.Add(uf);
                    }
                }
            }
            return friends;
        }

        public void RespondFriendRequest(int senderID, int recieverID, bool response)
        {
            string query = response
                ? "UPDATE [Friendship] SET Status=@Status " +
                  "WHERE (User_ID_1=@SenderID AND User_ID_2=@RecieverID) OR (User_ID_2=@SenderID AND User_ID_1=@RecieverID)"
                : "DELETE FROM [Friendship] " +
                  "WHERE (User_ID_1=@SenderID AND User_ID_2=@RecieverID) OR (User_ID_2=@SenderID AND User_ID_1=@RecieverID)";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SenderID", senderID);
                command.Parameters.AddWithValue("@RecieverID", recieverID);
                if (response)
                {
                    command.Parameters.AddWithValue("@Status", response);
                }
                command.ExecuteNonQuery();
            }
        }

        public void Unfriend(int senderID, int recieverID)
        {
            string query = "DELETE FROM [Friendship] " +
                           "WHERE (User_ID_1=@SenderID AND User_ID_2=@RecieverID) OR (User_ID_2=@SenderID AND User_ID_1=@RecieverID)";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SenderID", senderID);
                command.Parameters.AddWithValue("@RecieverID", recieverID);
                command.ExecuteNonQuery();
            }
        }

        public void SaveUser(User user)
        {
            string query = user.ID == 0
                ? "INSERT INTO [User] (Username, Password, Email_Address)" +
                  " VALUES (@Username, @Password, @Email_Address)"
                : "UPDATE [User] SET Username=@Username, Password=@Password, Email_Address=@Email_Address, ProfilePicture=@ProfilePicture" +
                  " WHERE ID=@ID";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", encrypt.EncryptPassword(user.Password));
                command.Parameters.AddWithValue("@Email_Address", user.EmailAddress);

                if (user.ID != 0)
                {
                    command.Parameters.AddWithValue("@ID", user.ID);
                    if (user.ProfilePicture != null)
                    {
                        command.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ProfilePicture", DBNull.Value);
                    }
                }
                command.ExecuteNonQuery();

                if (user.ID == 0)
                {
                    user.ID = GetUserID(user.Username);
                }
            }
        }

        public int GetUserID(string username)
        {
            string query = "SELECT * FROM [User] WHERE Username = @Username";
            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        return dataReader.GetInt32(dataReader.GetOrdinal("ID"));
                    }
                }
            }
            return 0;
        }

        public void SendFriendRequest(int senderID, int recieverID)
        {
            string query =
                "INSERT INTO [Friendship] (User_ID_1, User_ID_2, Status)" +
                " VALUES (@UserID1, @UserID2, @Status)";

            using (SqlCommand command = con.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@UserID1", senderID);
                command.Parameters.AddWithValue("@UserID2", recieverID);
                command.Parameters.AddWithValue("@Status", false);
                command.ExecuteNonQuery();
            }
        }
    }
}