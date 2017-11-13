using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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
            nameTitle = "'" + activeWindow.GetNameActiveWindow().Replace("'", "") + "'";
            DataBase.ConnectToDataBase();
            command.Connection = DataBase.Connection;

            if (GetTimeForTitle(nameTitle) > 0)
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
            string contentCommand = "SELECT ActivityTime from dailyuseofapplication INNER JOIN " +
                "activeapplications on dailyuseofapplication.IdTitle = activeapplications.Id " +
                "WHERE activeapplications.Title = " + nameTitle;
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) time = Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            DataBase.CloseConnection();
            return time;
        }

        public int GetTimeForNumberActivity(List<int> numbers, bool ifExcept = false)
        {
            int time = 0;
            string contentCommand = "SELECT ActivityTime FROM dailyuseofapplication INNER JOIN " +
                "activeapplications ON dailyuseofapplication.IdTitle = activeapplications.Id " +
                "WHERE 1 = 1";
            for(int i = 0; i < numbers.Count; i++)
            {
                contentCommand += " AND activeapplications.IdNameActivity " + (ifExcept? "!":"") + "= " + numbers[i];
            }
            
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) time += Convert.ToInt32(reader["ActivityTime"]);
            reader.Dispose();
            DataBase.CloseConnection();
            return time;
        }

        public int GetTimeForAllTitle()
        {
            int time = 0;
            string contentCommand = "SELECT ActivityTime FROM dailyuseofapplication " +
                "WHERE dailyuseofapplication.IdTitle > 2";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read()) time += Convert.ToInt32(reader["ActivityTime"]);
            DataBase.CloseConnection();
            return time;
        }

        public void TransferDataToAllDataAndClearTable()
        {
            string contentCommand = "SELECT IdTitle, ActivityTime from dailyuseofapplication";
            MySqlDataReader reader = GetExecuteReader(contentCommand);
            while (reader.Read())
            {
                var idTitle = reader["IdTitle"];
                var ActivityTime = reader["ActivityTime"];
                allData_db.Add(Convert.ToInt32(idTitle), Convert.ToInt32(ActivityTime), additionalConnection: true);
            }
            reader.Dispose();
            RestartContentTable();
            DataBase.CloseConnection();

        }

        public string[,] GetBiggestResults()
        {
            string[,] biggestResults = new string[4, 2];
            int i = 0;
            string contentCommand =
                "SELECT activeapplications.Title as Title, ActivityTime from dailyuseofapplication INNER JOIN " +
                "activeapplications on dailyuseofapplication.IdTitle = activeapplications.Id " +
                "WHERE IdTitle > 2 ORDER BY ActivityTime DESC LIMIT 4";
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
            string contentCommand = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + 1 + " WHERE IdTitle = 2";
            DataBase.ExecuteNonQuery(contentCommand);
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
            MySqlDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception message)
            {
                ApplicationLog.LogService.AddRaportCatchException("Error !!!\tNie udało się wykonać zapytania", message);
            }
            return reader;
        }

        private void AddNameTitleToTableDailyUse()
        {
            string contentCommand = string.Empty;
            if (!ActiveApplication_db.CheckIfExistTitle(nameTitle))
            {
                contentCommand = "INSERT INTO activeapplications (Title, IdNameActivity) VALUES ( "
                                + nameTitle + " , " + 1 + " )";
                DataBase.ExecuteNonQuery(contentCommand);
            }

            contentCommand = "INSERT INTO dailyuseofapplication (IdTitle, ActivityTime) " +
                "SELECT activeapplications.ID, 1 " +
                "FROM activeapplications WHERE activeapplications.Title = " + nameTitle;
            DataBase.ExecuteNonQuery(contentCommand);
        }

        private void UpDateTimeThisTitle()
        {
            string contentCommand = "UPDATE dailyuseofapplication INNER JOIN " +
                "activeapplications ON dailyuseofapplication.IdTitle = activeapplications.Id SET " +
                "ActivityTime = ActivityTime + 1 WHERE activeapplications.Title = " + nameTitle;
            DataBase.ExecuteNonQuery(contentCommand);
        }


        private void RestartContentTable()
        {
            string contentCommand = "TRUNCATE TABLE dailyuseofapplication";
            DataBase.ExecuteNonQuery(contentCommand);

            contentCommand = "INSERT INTO dailyuseofapplication (IdTitle, ActivityTime) VALUES (1, 0), (2, 0)";
            DataBase.ExecuteNonQuery(contentCommand);
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
            return (24 * 60) - GetAllTimeActivity();
        }

        private void AddTimeToDisabledComputer(int time)
        {
            string contentCommand = "UPDATE dailyuseofapplication SET ActivityTime = ActivityTime + "
                + time + " WHERE IdTitle = '1'";
            DataBase.ExecuteNonQuery(contentCommand);
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
