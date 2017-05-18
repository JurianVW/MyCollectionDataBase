using System;
using System.Data;
using Models;
using System.Data.SqlClient;

namespace Repository.Data
{
    public class UserSQLContext : IUserContext
    {
        private static string connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;" +
            @"AttachDbFilename=D:\OneDrive - Office 365 Fontys\Documenten\ICT\SE2\BP4 Killer App\MyCollectionDataBase\MyCollectionDB.mdf";

        public static SqlConnection Connection
        {
            get
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
        }

        public bool CheckUserLogin(User user)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM [User] WHERE [Username] = @Username AND [Password] = @Password";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public List GetAccount()
        {
            throw new NotImplementedException();
        }

        public void RespondFriendRequest(User sender, User reciever, bool repsonse)
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public void SendFriendRequest(User sender, User reciever)
        {
            throw new NotImplementedException();
        }
    }
}