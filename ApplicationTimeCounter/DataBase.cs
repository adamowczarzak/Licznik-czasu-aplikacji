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
    static public class DataBase
    {
        static public MySqlConnection Connection {get;set;}
        static public MySqlConnection AdditionalConnection { get; set; }
        static private string nameUser;
        static private string password;
        static private bool isOpenConnection = false;


        static public bool ConnectToDataBase()
        {
            bool connectToLocalhost = true;

            if (isOpenConnection == false)
            {
                try
                {
                    Connection.Open();
                    isOpenConnection = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                    connectToLocalhost = false;
                    ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się otworzyć połącznia (Connection).", exception.ToString());
                }
            }
            return connectToLocalhost;
        }

        static public bool AdditionalConnectToDataBase()
        {
            bool connectToLocalhost = true;
            try
            {
                AdditionalConnection.Open();
            }
            catch (Exception exception)
            {
                ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się otworzyć połącznia (AdditionalConnection).", exception.ToString());
                connectToLocalhost = false;
            }

            return connectToLocalhost;
        }

        static public bool ExecuteNonQuery(MySqlCommand command)
        {
            bool executeNonQuery = true;

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się wykonać zapytania (ExecuteNonQuery).", exception.ToString());
                executeNonQuery = false;
            }

            return executeNonQuery;
        }

        static private void GetMySqlConnection()
        {
            string myConnectionString =
                "server=localhost;user=" + nameUser + ";database=applicationtimecounter;password="+ password;
                
            Connection = new MySqlConnection(myConnectionString);
            AdditionalConnection = new MySqlConnection(myConnectionString);
        }

        static public void CloseConnection()
        {
            if (isOpenConnection == true)
            {
                Connection.Close();
                isOpenConnection = false;
            }
        }

        static public bool CheckIsOpenConnection()
        {
            return isOpenConnection;
        }

        /// <summary>
        /// Metoda sprawdza czy nazwa użytkownika i hasło poznawają połączyć się z mysql.
        /// </summary>
        /// <param name="userName"> Nazwa użytkownika.</param>
        /// <param name="password"> Hasło użytkownika.</param>
        /// <returns></returns>
        static public bool TryConnectToMySql(string userName, string password)
        {
            // sprawdzenie czy istnieje
            // jeśli nie podjecie próby utworznia
            // jeśli nie tyświetlić że się nie da i zrzucić loga
            //s0 = "CREATE DATABASE IF NOT EXISTS `hello`;";
            DataBase.nameUser = userName;
            DataBase.password = password;

            string myConnectionString = "server=localhost;user=" + DataBase.nameUser + ";password=" + DataBase.password;
            MySqlConnection testConnection = new MySqlConnection(myConnectionString);
            bool isConnection = TryConnectToDataBase(testConnection);
            if (isConnection)
                GetMySqlConnection();


          //  MySqlCommand command = connection.CreateCommand();
           // command.CommandText = "SHOW DATABASES";
          //  MySqlDataReader Reader;
           
            /*
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    row += Reader.GetValue(i).ToString() + ", ";
                ApplicationLog.LogService.AddRaportTest(row);
            }*/
         //   testConnection.Close();
            //openConnection = false;

            return isConnection;
        }

        static private bool TryConnectToDataBase(MySqlConnection connection)
        {
            bool connectToLocalhost = true;
            try
            {
                connection.Open();
            }
            catch (Exception _ex)
            {
                // tu dodaj błąd łączenia się z bazą
                MessageBox.Show(_ex.ToString());
            }
            return connectToLocalhost;
        }

        
    }
}
