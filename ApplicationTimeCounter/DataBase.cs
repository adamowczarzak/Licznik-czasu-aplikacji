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

        public bool ConnectToDataBase()
        {   
            GetMySqlConnection();     
            try
            {
                Connection.Open();            
            }
            catch (Exception _ex)
            {
                // tu dodaj błąd łączenia się z bazą
                MessageBox.Show(_ex.ToString());
            }
            return true;
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

        public bool CheckDataBase()
        {
            // sprawdzenie czy istnieje
            // jeśli nie podjecie próby utworznia
            // jeśli nie tyświetlić że się nie da i zrzucić loga
            //s0 = "CREATE DATABASE IF NOT EXISTS `hello`;";
            GetDataBaseWindow getDataBaseWindow = new GetDataBaseWindow();
            getDataBaseWindow.ShowDialog();
            
            string myConnectionString = "server=localhost;user=root;password=1994aaxd";
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SHOW DATABASES";
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    row += Reader.GetValue(i).ToString() + ", ";
                ApplicationLog.LogService.AddRaportTest(row);
            }
            connection.Close();

            return true;
        }

        
    }
}
