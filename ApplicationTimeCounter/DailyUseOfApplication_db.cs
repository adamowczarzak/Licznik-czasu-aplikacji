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
        private DataBase dataBase;
        private AllData_db allData_db;

        public DailyUseOfApplication_db()
        {
            
            activeWindow = new ActiveWindow();
            command = new MySqlCommand();
            dataBase = new DataBase();
            allData_db = new AllData_db();           
        }

        public void Update()
        {         
            nameTitle = "'" + activeWindow.GetNameActiveWindow().Replace("'","") + "'";
            dataBase.ConnectToDataBase();
            command.Connection = dataBase.Connection;
            
            if (GetNumberElementFromTable() > 0)
                UpDateTimeThisTitle();
            else
                AddNameTitleToTableDailyUse();

            dataBase.CloseConnection();
        }

        public int GetTimeForTitle(string title)
        {
            int time = 0;
            nameTitle = "'" + title.Replace("'", "") + "'";
            string contentCommand = "SELECT ActivityTime from dailyuseofapplication WHERE Title = " + nameTitle;
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read())time = Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            dataBase.CloseConnection();
            return time;
        }

        public int GetTimeForNumberActivity(int number)
        {
            int time = 0;
            string contentCommand = "SELECT ActivityTime from dailyuseofapplication WHERE idNameDailyActivity = " + number;
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) time += Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            dataBase.CloseConnection();
            return time;
        }

        public int GetTimeForAllTitle()
        {
            int time = 0;
            string contentCommand = "SELECT ActivityTime from dailyuseofapplication WHERE idTitle > 2";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) time += Convert.ToInt32(reader["ActivityTime"]);
            dataBase.CloseConnection();
            return time;
        }
        
        public void TransferDataToAllDataAndClearTable()
        {
            string contentCommand = "SELECT Title, ActivityTime from dailyuseofapplication WHERE ActivityTime > 10 OR idTitle < 3";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read())
            {
                var Title = reader["Title"];
                var ActivityTime = reader["ActivityTime"];
                allData_db.Add(Title.ToString(), Convert.ToInt32(ActivityTime));
            }
            reader.Dispose();                       
            RestartContentTable();
            dataBase.CloseConnection();        
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
            dataBase.CloseConnection();

            return biggestResults;
        }

        public void UpdateTimeNonActiv()
        {
            dataBase.ConnectToDataBase();
            command.Connection = dataBase.Connection;
            command.CommandText = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + 1 + " WHERE Title = 'Brak Aktyw.'";
            command.ExecuteNonQuery();
            dataBase.CloseConnection();
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
            dataBase.ConnectToDataBase();
            command.Connection = dataBase.Connection;
            command.CommandText = contentCommand;
            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        private void AddNameTitleToTableDailyUse()
        {
            command.CommandText = "INSERT INTO dailyuseofapplication (Title, ActivityTime, idNameDailyActivity) VALUES ("
                + nameTitle + "," + 1 + "," + 2 + ")";
            command.ExecuteNonQuery();
        }

        private void UpDateTimeThisTitle()
        {
            command.CommandText = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + 1 + " WHERE Title =" + nameTitle;
            command.ExecuteNonQuery();
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
            command.ExecuteNonQuery();
            command.CommandText =
                "INSERT INTO dailyuseofapplication (Title, ActivityTime, idNameDailyActivity) VALUES ('Wył. komputer', + 0 , 0)";
            command.ExecuteNonQuery();
            command.CommandText =
                "INSERT INTO dailyuseofapplication (Title, ActivityTime, idNameDailyActivity) VALUES ('Brak Aktyw.', + 0 , 0)";
            command.ExecuteNonQuery();
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
            dataBase.ConnectToDataBase();
            command.Connection = dataBase.Connection;
            command.CommandText = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + time + " WHERE Title = 'Wyl. komputer'";
            command.ExecuteNonQuery();
            dataBase.CloseConnection();
        }

        private int GetAllTimeActivity()
        {
            int timeActivity = 0;

            string contentCommand = "SELECT ActivityTime from dailyuseofapplication";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) timeActivity += Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            dataBase.CloseConnection();

            return timeActivity;
        }
    }
}
