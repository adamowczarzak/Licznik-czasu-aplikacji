using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    public static class ActiveApplication_db
    {

        private static MySqlCommand command = new MySqlCommand();

        public static string GetAllNotAssignedApplication()
        {
            string contentCommand = "SELECT COUNT(*) as noAssigmentApplication from activeapplications " +
                "WHERE idNameActivity = 1";
            string AllNotAssignedApplication = GetListStringFromExecuteReader(contentCommand, "noAssigmentApplication")[0];
            return AllNotAssignedApplication;
        }

        internal static List<ActiveApplication> GetNonAssignedApplication()
        {
            List<ActiveApplication> activeApplication = new List<ActiveApplication>();
            string query = "SELECT activeapplications.Id AS Id, activeapplications.Title AS Title FROM  dailyuseofapplication " +
                "LEFT OUTER JOIN activeapplications ON dailyuseofapplication.IdTitle = activeapplications.Id WHERE activeapplications.IdNameActivity = 1";

            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ActiveApplication application = new ActiveApplication();
                        application.ID = Int32.Parse((reader["Id"]).ToString());
                        application.Title = (reader["Title"]).ToString();
                        application.Date = DateTime.Now.ToString();
                        activeApplication.Add(application);
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }

                query = "SELECT activeapplications.Id AS Id, activeapplications.Title AS Title, alldate.date AS Date FROM alldate " +
                "LEFT OUTER JOIN activeapplications ON  alldate.IdTitle = activeapplications.Id WHERE activeapplications.IdNameActivity = 1";
                reader.Dispose();
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ActiveApplication application = new ActiveApplication();
                        application.ID = Int32.Parse((reader["Id"]).ToString());
                        application.Title = (reader["Title"]).ToString();
                        application.Date = (reader["Date"]).ToString();
                        activeApplication.Add(application);
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return activeApplication;
        }

        public static bool CheckIfExistTitle(string title)
        {
            string contentCommand = "SELECT COUNT(*) as TitleExist from activeapplications WHERE Title = " + title;
            if (GetListStringFromExecuteReader(contentCommand, "TitleExist")[0] != "0") return true;
            else return false;
        }

        internal static List<ActiveApplication> GetActiveApplication(ActiveApplication parameters)
        {
            List<ActiveApplication> activeApplication = new List<ActiveApplication>();
            string query = "SELECT activeapplications.Id AS Id, activeapplications.Title AS Title, nameactivity.NameActivity AS NameActivity, "+
                "nameactivity.Id AS IdNameActivity FROM activeapplications LEFT OUTER JOIN " +
                "nameactivity ON activeapplications.IdNameActivity = nameactivity.Id WHERE 1 = 1";
            if (parameters.ID > 0) query += " AND Id = " + parameters.ID;
            if (!string.IsNullOrEmpty(parameters.Title)) query += " AND Title = " + parameters.Title;
            if (!string.IsNullOrEmpty(parameters.NameActivity)) query += " AND NameActivity = " + parameters.NameActivity;
            if (parameters.IdNameActivity > 0) query += " AND IdNameActivity = " + (int)parameters.IdNameActivity;

            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ActiveApplication application = new ActiveApplication();
                        application.ID = Int32.Parse((reader["Id"]).ToString());
                        application.Title = (reader["Title"]).ToString();
                        application.NameActivity = (reader["NameActivity"]).ToString();
                        application.IdNameActivity = (ActiveApplication.IdNameActivityEnum)Int32.Parse((reader["IdNameActivity"]).ToString());
                        activeApplication.Add(application);
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return activeApplication;
        }


        private static List<string> GetListStringFromExecuteReader(string contentCommand, string nameReturnColumn)
        {
            List<string> returnList = new List<string>();
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = contentCommand;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        returnList.Add(reader[nameReturnColumn].ToString());
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return returnList;
        }
    }
}
