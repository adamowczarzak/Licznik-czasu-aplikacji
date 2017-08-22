using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;

namespace ApplicationTimeCounter
{
    public class DataBase
    {
        public MySqlConnection Connection { get; set; }

        
        public DataBase()
        {

        }

        public void ConnectToDataBase()
        {   
            GetMySqlConnection();     
            try
            {
                Connection.Open();
                if (Connection.State.ToString() != "Open")
                {
                    MessageBox.Show("Application could not connect to server ", "Connection Failed ", MessageBoxButton.OK, MessageBoxImage.Error);
                }              
            }
            catch (Exception _ex)
            {
                MessageBox.Show(_ex.ToString());
            }
        }

        public void GetMySqlConnection()
        {
            string password = "1994aaxd";
            string connStr =
                "server=localhost;user=root;database=applicationtimecounter;port=3306;password="
                + password;
            Connection = new MySqlConnection(connStr);
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        
    }
}
