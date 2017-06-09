using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Data
{
    public class DatabaseConnection
    {
        //From server
        //private static string connectionString = @"Data Source=S12-SERVER-01; Database=MyCollectionDB; User id=frietpan; Password=frietpan; POOLING=false";

        //From pc
        private static string connectionString = "Data Source=192.168.19.12; Database=MyCollectionDB; User id=frietpan; Password=frietpan; Connection Timeout=10; POOLING=false";

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