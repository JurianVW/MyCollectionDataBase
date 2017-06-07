using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Data
{
    public class DatabaseConnection
    {
        //From server
        //private static string connectionString = "";

        //From pc
        private static string connectionString = "Data Source=192.168.19.12; Initial Catalog=MyCollectionDB; User id=frietpan; Password=frietpan; Connection Timeout=3";

        public SqlConnection Connection
        {
            get
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
        }
    }
}