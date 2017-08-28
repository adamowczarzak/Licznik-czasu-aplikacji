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
            return TryConnectToDataBase(Connection);
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
            //s0 = "CREATE DATABASE IF NOT EXISTS `hello`;";
            DataBase.nameUser = userName;
            DataBase.password = password;

            string myConnectionString = "server=localhost;user=" + DataBase.nameUser + ";password=" + DataBase.password;
            MySqlConnection testConnection = new MySqlConnection(myConnectionString);
            bool isConnection = TryConnectToDataBase(testConnection);

            if (isConnection)
            {
                testConnection.Close();
                isOpenConnection = false;
                GetMySqlConnection();
            }
                
            return isConnection;
        }

        static private bool TryConnectToDataBase(MySqlConnection connection)
        {
            bool connectToLocalhost = true;

            if (isOpenConnection == false)
            {
                try
                {
                    connection.Open();
                    isOpenConnection = true;
                }
                catch (Exception exception)
                {
                    connectToLocalhost = false;
                    ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się otworzyć połącznia (Connection).", exception.ToString());
                }
            }
            return connectToLocalhost;
        }

        
    }
}
