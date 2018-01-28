using System;
using System.Collections.Generic;
using ApplicationTimeCounter.Other;
using System.Data.SqlClient;

namespace ApplicationTimeCounter
{
    static public class DataBase
    {
        static public SqlConnection Connection { get; set; }
        static public SqlConnection AdditionalConnection { get; set; }
        static private string serverName;
        static private string userName;
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

        static public bool ExecuteNonQuery(SqlCommand command)
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


        static public bool ExecuteNonQuery(string contentCommand)
        {
            bool executeNonQuery = true;
            SqlCommand command = new SqlCommand();
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = contentCommand;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się wykonać zapytania (ExecuteNonQuery).", exception);
                    executeNonQuery = false;
                }
                DataBase.CloseConnection();
            }
            return executeNonQuery;
        }

        public static List<string> GetListStringFromExecuteReader(string contentCommand, string nameReturnColumn)
        {
            List<string> returnList = new List<string>();
            SqlCommand command = new SqlCommand();
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = contentCommand;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        returnList.Add(reader[nameReturnColumn].ToString());
                    }
                    catch (SqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error !!!\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return returnList;
        }

        public static Dictionary<string, string> GetDictionaryFromExecuteReader(string contentCommand, string nameKey, string nameValue)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            SqlCommand command = new SqlCommand();
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = contentCommand;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        if (!dictionary.ContainsKey(reader[nameKey].ToString()))
                            dictionary.Add(reader[nameKey].ToString(), reader[nameValue].ToString());
                    }
                    catch (SqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error !!!\tPobranie słownika nie powiodło się.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return dictionary;
        }

        static private void GetSqlConnection()
        {
            string myConnectionString = "user id= " + DataBase.userName + ";password=" + DataBase.password + ";" +
                                        "server=" + DataBase.serverName + ";database= applicationtimecounter;Trusted_Connection=yes;";

            Connection = new SqlConnection(myConnectionString);
            AdditionalConnection = new SqlConnection(myConnectionString);
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
        /// Metoda sprawdza czy nazwa użytkownika i hasło poznawają połączyć się z sql.
        /// </summary>
        /// <param name="serverName"> Nazwa servera.</param>
        /// <param name="userName"> Nazwa użytkownika.</param>
        /// <param name="password"> Hasło użytkownika.</param>
        /// <returns></returns>
        static public bool TryConnectToSql(string serverName, string userName, string password)
        {
            DataBase.serverName = serverName;
            DataBase.userName = userName;
            DataBase.password = password;

            string myConnectionString = "user id= " + DataBase.userName + ";password=" + DataBase.password + ";" +
                                        "server=" + DataBase.serverName + ";Trusted_Connection=yes;";

            SqlConnection testConnection = new SqlConnection(myConnectionString);
            bool isConnection = TryConnectToDataBase(testConnection);

            if (isConnection)
            {
                testConnection.Close();
                isOpenConnection = false;
                GetSqlConnectionAndTryCreateDataBaseAndTableIfNotExist();
            }

            return isConnection;
        }

        static private bool TryConnectToDataBase(SqlConnection connection)
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
        /// Pobieranie połącznienia do bazy Sql oraz tworzenie tabel jeśli nie istnieją.
        /// </summary>
        static private void GetSqlConnectionAndTryCreateDataBaseAndTableIfNotExist()
        {
            string myConnectionString = "user id= " + DataBase.userName + ";password=" + DataBase.password + ";" +
                                        "server=" + DataBase.serverName + ";Trusted_Connection=yes;";
            SqlConnection testConnection = new SqlConnection(myConnectionString);

            string stringCommand = "IF NOT EXISTS (SELECT * FROM SYS.DATABASES WHERE NAME = 'applicationtimecounter') CREATE DATABASE applicationtimecounter";
            SqlCommand command = new SqlCommand(stringCommand, testConnection);
            bool addnameActivity = true;
            bool addDefaultActivityApplication = true;
            bool addDateFirstStartApplication = true;
            testConnection.Open();
            ExecuteNonQuery(command);
            testConnection.Close();

            GetSqlConnection();
            ConnectToDataBase();

            addnameActivity = CheckIfExistTable("nameactivity", command);

            stringCommand = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'nameactivity') 
                            CREATE TABLE nameactivity (
                                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                NameActivity VARCHAR(256) COLLATE Polish_CI_AS NULL)";
            command = new SqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            stringCommand = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'membership') 
                            CREATE TABLE membership (
                                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                Title VARCHAR(256) COLLATE Polish_CI_AS NULL,
                                Date DATETIME DEFAULT GETDATE(),
                                Active TINYINT NOT NULL DEFAULT 1,
                                Configuration TINYINT NOT NULL DEFAULT 0,
                                Filter VARCHAR(256) COLLATE Polish_CI_AS NULL,
                                ActiveConfiguration TINYINT NOT NULL DEFAULT 0,
                                AsOneApplication TINYINT NOT NULL DEFAULT 0)";
            command = new SqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            addDefaultActivityApplication = CheckIfExistTable("activeapplications", command);

            stringCommand = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'activeapplications') 
                            CREATE TABLE activeapplications (
                                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                Title VARCHAR(256) COLLATE Polish_CI_AS NULL,
                                IdNameActivity INT,
                                IdMembership INT FOREIGN KEY REFERENCES membership(Id),
                                AutoGrouping TINYINT DEFAULT NULL)";
            command = new SqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            addDateFirstStartApplication = CheckIfExistTable("alldate", command);

            stringCommand = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'alldate') 
                            CREATE TABLE alldate (
                                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                Date DATE NULL,
                                IdTitle INT NOT NULL,
                                ActivityTime INT)";
            command = new SqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            stringCommand = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'dailyuseofapplication') 
                            CREATE TABLE dailyuseofapplication (
                                Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                IdTitle INT NOT NULL FOREIGN KEY REFERENCES activeapplications(Id),
                                ActivityTime INT)";
            command = new SqlCommand(stringCommand, Connection);
            ExecuteNonQuery(command);

            if (addnameActivity)
            {
                stringCommand = "INSERT INTO nameactivity (NameActivity) VALUES ('Brak') , ('Programowanie') ";
                command = new SqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);
            }

            if (addDefaultActivityApplication)
            {
                stringCommand = "INSERT INTO activeapplications (Title , idNameActivity) VALUES ('Wył. komputer', -2),('Brak Aktyw.', -1)";
                command = new SqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);

                stringCommand = "ALTER TABLE activeapplications WITH NOCHECK ADD FOREIGN KEY (IdNameActivity) REFERENCES nameActivity(Id)";
                command = new SqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);
            }

            if (addDateFirstStartApplication)
            {
                stringCommand = "INSERT INTO alldate (Date , IdTitle , ActivityTime) VALUES (DATEADD(day, -1, GETDATE()), 0, 0)";
                command = new SqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);

                stringCommand = "ALTER TABLE alldate WITH NOCHECK ADD FOREIGN KEY (IdTitle) REFERENCES activeapplications(Id)";
                command = new SqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);

                stringCommand = "INSERT INTO dailyuseofapplication (IdTitle, ActivityTime) VALUES (1, 0), (2, 0)";
                command = new SqlCommand(stringCommand, Connection);
                ExecuteNonQuery(command);
            }

            CloseConnection();
        }


        private static bool CheckIfExistTable(string nameTable, SqlCommand command)
        {
            bool returnValue = true;
            string stringCommand = @"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = " + SqlValidator.Validate(nameTable);
            command = new SqlCommand(stringCommand, Connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) returnValue = false;
            reader.Close();
            return returnValue;
        }


    }
}
