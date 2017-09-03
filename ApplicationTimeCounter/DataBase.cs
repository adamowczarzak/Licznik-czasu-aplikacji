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
                ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się otworzyć połącznia (AdditionalConnection).", exception);
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
                ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się wykonać zapytania (ExecuteNonQuery).", exception);
                executeNonQuery = false;
            }

            return executeNonQuery;
        }

        static private void GetMySqlConnection()
        {
            string myConnectionString =
                "server=localhost;user=" + nameUser + ";database=applicationtimecounter2;password="+ password;
                
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
            DataBase.nameUser = userName;
            DataBase.password = password;

            string myConnectionString = "server=localhost;user=" + DataBase.nameUser + ";password=" + DataBase.password;
            MySqlConnection testConnection = new MySqlConnection(myConnectionString);
            bool isConnection = TryConnectToDataBase(testConnection);

            if (isConnection)
            {
                testConnection.Close();
                isOpenConnection = false;
                GetMySqlConnectionAndTryCreateDataBaseAndTableIfNotExist();
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
                    ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się otworzyć połącznia (Connection).", exception);
                }
            }
            return connectToLocalhost;
        }



        static private void GetMySqlConnectionAndTryCreateDataBaseAndTableIfNotExist()
        {
            string myConnectionString = "server=localhost;user=" + DataBase.nameUser + ";password=" + DataBase.password;
            MySqlConnection testConnection = new MySqlConnection(myConnectionString);
            string stringCommand = "CREATE DATABASE IF NOT EXISTS `applicationtimecounter2`";
            MySqlCommand command = new MySqlCommand(stringCommand, testConnection);
            testConnection.Open();
            ExecuteNonQuery(command);
            testConnection.Close();

            GetMySqlConnection();
            ConnectToDataBase();
            stringCommand = @"CREATE TABLE IF NOT EXISTS `alldate`(
            `idAllDate` int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
            `Date` date NULL,
            `Title` varchar(256) CHARACTER SET utf8 COLLATE utf8_polish_ci NULL,
            `ActivityTime` int(11) UNSIGNED) CHARACTER SET utf8 COLLATE utf8_polish_ci"; 
            command = new MySqlCommand(stringCommand, Connection);         
            ExecuteNonQuery(command);

            stringCommand = @"CREATE TABLE IF NOT EXISTS `dailyuseofapplication`(
            `idTitle` int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
            `Title` varchar(256) CHARACTER SET utf8 COLLATE utf8_polish_ci NULL,
            `ActivityTime` int(11) UNSIGNED,
            `idNameDailyActivity` int(11) UNSIGNED) CHARACTER SET utf8 COLLATE utf8_polish_ci";
            command = new MySqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            // sprawdź czy istnieje tablea

            stringCommand = @"CREATE TABLE IF NOT EXISTS `namedailyactivity`(
            `idNameDailyActivity` int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
            `NameDailyActivity` varchar(256) CHARACTER SET utf8 COLLATE utf8_polish_ci NULL) CHARACTER SET utf8 COLLATE utf8_polish_ci";
            command = new MySqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);


            // jeśli nie istnieje tabela to dodaj te 2 rekordy
            stringCommand = "INSERT IGNORE INTO namedailyactivity (NameDailyActivity) VALUES ('Programowanie'),('Inne')";
            command = new MySqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            CloseConnection();
        }

        
    }
}
