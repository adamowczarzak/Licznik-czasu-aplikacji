using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationTimeCounter
{
    public class DailyUseOfApplication_db
    {
        private string nameTitle;
        private ActiveWindow activeWindow;
        private MySqlCommand command;
        private AllData_db allData_db;


        public DailyUseOfApplication_db()
        {
            activeWindow = new ActiveWindow();
            command = new MySqlCommand();
            allData_db = new AllData_db();           
        }

        public void Update()
        {         
            nameTitle = "'" + activeWindow.GetNameActiveWindow().Replace("'","") + "'";
            DataBase.ConnectToDataBase();
            command.Connection = DataBase.Connection;
            
            if (GetNumberElementFromTable() > 0)
                UpDateTimeThisTitle();
            else
                AddNameTitleToTableDailyUse();

            DataBase.CloseConnection();
        }

        /// <summary>
        /// Pobiera czas aktywności dla tytułu aplikacji.
        /// </summary>
        /// <param name="title">Tytuł aplikacji/onaczenie czynności.</param>
        /// <returns></returns>
        public int GetTimeForTitle(string title)
        {
            int time = 0;
            nameTitle = "'" + title.Replace("'", "") + "'";
            string contentCommand = "SELECT ActivityTime from dailyuseofapplication WHERE Title = " + nameTitle;
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read())time = Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            DataBase.CloseConnection();
            return time;
        }

        public int GetTimeForNumberActivity(int number)
        {
            int time = 0;
            string contentCommand = "SELECT ActivityTime from dailyuseofapplication WHERE idNameDailyActivity = " + number;
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) time += Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            DataBase.CloseConnection();
            return time;
        }

        public int GetTimeForAllTitle()
        {
            int time = 0;
            string contentCommand = "SELECT ActivityTime from dailyuseofapplication WHERE idTitle > 2";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) time += Convert.ToInt32(reader["ActivityTime"]);
            DataBase.CloseConnection();
            return time;
        }
        
        public void TransferDataToAllDataAndClearTable()
        {
            if (allData_db.GetDayWorkingApplication() != "0")
            {
                string contentCommand = "SELECT Title, ActivityTime from dailyuseofapplication WHERE ActivityTime > 10 OR idTitle < 3";
                MySqlDataReader reader = GetExecuteReader(contentCommand);
                while (reader.Read())
                {
                    var Title = reader["Title"];
                    var ActivityTime = reader["ActivityTime"];
                    allData_db.Add(Title.ToString(), Convert.ToInt32(ActivityTime), additionalConnection: true);
                }
                reader.Dispose();
                RestartContentTable();
                DataBase.CloseConnection();
            }
        }

        public string[,] GetBiggestResults()
        {
            string[,] biggestResults = new string[4, 2];
            int i = 0;
            string contentCommand =
                "SELECT Title, ActivityTime from dailyuseofapplication WHERE idTitle > 2 ORDER BY ActivityTime DESC LIMIT 4";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read())
            {
                biggestResults[i, 0] = reader["Title"].ToString();
                biggestResults[i, 1] = reader["ActivityTime"].ToString();
                i++;
            }
            reader.Dispose();
            DataBase.CloseConnection();

            return biggestResults;
        }

        public void UpdateTimeNonActiv()
        {
            DataBase.ConnectToDataBase();
            command.Connection = DataBase.Connection;
            command.CommandText = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + 1 + " WHERE Title = 'Brak Aktyw.'";
            command.ExecuteNonQuery();
            DataBase.CloseConnection();
        }

        public void AddTimeToNowDisableComputer()
        {
            int time = GetTimeDisabledComputerToNow();
            AddTimeToDisabledComputer(time);
        }

        public void AddTimeToDayDisableComputer()
        {
            int time = GetTimeDisabledComputerToDay();
            AddTimeToDisabledComputer(time);
        }

        private MySqlDataReader GetExecuteReader(string contentCommand)
        {
            DataBase.ConnectToDataBase();
            command.Connection = DataBase.Connection;
            command.CommandText = contentCommand;
            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        private void AddNameTitleToTableDailyUse()
        {
            command.CommandText = "INSERT INTO dailyuseofapplication (Title, ActivityTime, idNameDailyActivity) VALUES ("
                + nameTitle + "," + 1 + "," + 2 + ")";
            DataBase.ExecuteNonQuery(command);
        }

        private void UpDateTimeThisTitle()
        {
            command.CommandText = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + 1 + " WHERE Title =" + nameTitle;
            DataBase.ExecuteNonQuery(command);
        }

        private int GetNumberElementFromTable()
        {
            int numberElement = 0;
            command.CommandText = "SELECT idTitle from dailyuseofapplication WHERE Title = " + nameTitle;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) numberElement++;
            reader.Dispose();

            return numberElement;
        }

        private void RestartContentTable()
        {
            command.CommandText = "TRUNCATE TABLE dailyuseofapplication";
            DataBase.ExecuteNonQuery(command);
            command.CommandText =
                "INSERT INTO dailyuseofapplication (Title, ActivityTime, idNameDailyActivity) VALUES ('Wył. komputer', + 0 , 0)";
            DataBase.ExecuteNonQuery(command);
            command.CommandText =
                "INSERT INTO dailyuseofapplication (Title, ActivityTime, idNameDailyActivity) VALUES ('Brak Aktyw.', + 0 , 0)";
            DataBase.ExecuteNonQuery(command);
        }

        private int GetTimeDisabledComputerToNow()
        {
            int m = Convert.ToInt32(DateTime.Now.ToString("mm"));
            int h = Convert.ToInt32(DateTime.Now.ToString("HH"));
            int currentTime = h * 60 + m;
            return currentTime - GetAllTimeActivity();
        }

        private int GetTimeDisabledComputerToDay()
        {
            return (24*60) - GetAllTimeActivity();
        }

        private void AddTimeToDisabledComputer(int time)
        {
            DataBase.ConnectToDataBase();
            command.Connection = DataBase.Connection;
            command.CommandText = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + time + " WHERE Title = 'Wyl. komputer'";
            DataBase.ExecuteNonQuery(command);
            DataBase.CloseConnection();
        }

        private int GetAllTimeActivity()
        {
            int timeActivity = 0;

            string contentCommand = "SELECT ActivityTime from dailyuseofapplication";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) timeActivity += Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            DataBase.CloseConnection();

            return timeActivity;
        }
    }
}
