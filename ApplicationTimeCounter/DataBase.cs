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
        static public MySqlConnection Connection { get; set; }
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
                "server=localhost;user=" + nameUser + ";database=applicationtimecounter;password=" + password;

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

        /// <summary>
        /// Pobieranie połącznienia do bazy MySql oraz tworzenie tabel jeśli nie istnieją.
        /// </summary>
        static private void GetMySqlConnectionAndTryCreateDataBaseAndTableIfNotExist()
        {
            string myConnectionString = "server=localhost;user=" + DataBase.nameUser + ";password=" + DataBase.password;
            MySqlConnection testConnection = new MySqlConnection(myConnectionString);
            string stringCommand = "CREATE DATABASE IF NOT EXISTS `applicationtimecounter`";
            MySqlCommand command = new MySqlCommand(stringCommand, testConnection);
            bool addnameActivity = true;
            bool addDefaultActivityApplication = true;
            bool addDateFirstStartApplication = true;
            testConnection.Open();
            ExecuteNonQuery(command);
            testConnection.Close();

            GetMySqlConnection();
            ConnectToDataBase();

            addDateFirstStartApplication = CheckIfExistTable("alldate", command);

            stringCommand = @"CREATE TABLE IF NOT EXISTS `alldate`(
                `Id` int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                `Date` date NULL,
                `IdTitle` int(11) UNSIGNED NOT NULL,
                `ActivityTime` int(11) UNSIGNED) CHARACTER SET utf8 COLLATE utf8_polish_ci";
            command = new MySqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            stringCommand = @"CREATE TABLE IF NOT EXISTS `dailyuseofapplication`(
                `Id` int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                `IdTitle` int(11) UNSIGNED NOT NULL,
                `ActivityTime` int(11) UNSIGNED) CHARACTER SET utf8 COLLATE utf8_polish_ci";
            command = new MySqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            addDefaultActivityApplication = CheckIfExistTable("activeapplications", command);

            stringCommand = @"CREATE TABLE IF NOT EXISTS `activeapplications`(
                `Id` int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                `Title` varchar(256) CHARACTER SET utf8 COLLATE utf8_polish_ci NULL,
                `IdNameActivity` int(11)) CHARACTER SET utf8 COLLATE utf8_polish_ci";
            command = new MySqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            addnameActivity = CheckIfExistTable("nameactivity", command);

            stringCommand = @"CREATE TABLE IF NOT EXISTS `nameactivity`(
                `Id` int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
                `NameActivity` varchar(256) CHARACTER SET utf8 COLLATE utf8_polish_ci NULL)
                CHARACTER SET utf8 COLLATE utf8_polish_ci";
            command = new MySqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            if (addnameActivity)
            {
                stringCommand = "INSERT INTO nameactivity (NameActivity) VALUES ('Brak') , ('Programowanie') , ('Inne')";
                command = new MySqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);
            }

            if (addDefaultActivityApplication)
            {
                stringCommand = "INSERT INTO activeapplications (Title , idNameActivity) VALUES ('Wył. komputer', -2),('Brak Aktyw.', -1)";
                command = new MySqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);
            }

            if (addDateFirstStartApplication)
            {
                stringCommand = "INSERT INTO alldate (Date , IdTitle , ActivityTime) VALUES (CURDATE()- INTERVAL 1 DAY , 0, 0)";
                command = new MySqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);

                stringCommand = "INSERT INTO dailyuseofapplication (IdTitle, ActivityTime) VALUES (1, 0), (2, 0)";
                command = new MySqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);
            }

            CloseConnection();
        }


        private static bool CheckIfExistTable(string nameTable, MySqlCommand command)
        {
            bool returnValue = true;
            string stringCommand = @"SHOW TABLES LIKE '" + nameTable + "'";
            command = new MySqlCommand(stringCommand, Connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) returnValue = false;
            reader.Close();
            return returnValue;
        }


    }
}
