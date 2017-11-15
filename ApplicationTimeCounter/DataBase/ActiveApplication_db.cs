﻿using System;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using ApplicationTimeCounter.Other;

namespace ApplicationTimeCounter
{
    public static class ActiveApplication_db
    {

        private static MySqlCommand command = new MySqlCommand();

        public static string GetAllNotAssignedApplication()
        {
            string contentCommand = "SELECT COUNT(*) as noAssigmentApplication from activeapplications " +
                "WHERE idNameActivity = 1";
            string AllNotAssignedApplication = DataBase.GetListStringFromExecuteReader(contentCommand, "noAssigmentApplication")[0];
            return AllNotAssignedApplication;
        }

        public static bool AddActivityToApplication(string idApplication, string idActivity)
        {
            string contentCommand = "UPDATE activeapplications SET IdNameActivity = " + idActivity + " WHERE Id  = " + idApplication;
            return DataBase.ExecuteNonQuery(contentCommand);
        }

        public static bool DeleteAllApplicationsWithActivity(int idActivity)
        {
            string contentCommand = "UPDATE activeapplications SET IdNameActivity = "
                + ((int)ActiveApplication.IdNameActivityEnum.Lack).ToString()
                + " WHERE IdNameActivity  = " + idActivity;

            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportError("Nie udało się usunąć wszystkich aplikacji z aktywności",
                   ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                   System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\DialogWindow.xaml.cs");
                return false;
            }
            else return true;
        }

        public static bool DeleteOneApplicationWithActivity(int idActivity, int idApplication)
        {
            string contentCommand = "UPDATE activeapplications SET IdNameActivity = "
                + ((int)ActiveApplication.IdNameActivityEnum.Lack).ToString()
                + " WHERE IdNameActivity  = " + idActivity + " AND ID = " + idApplication;

            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportWarning("Nie udało się usunąć aplikacji z aktywności");
                return false;
            }
            else return true;
        }

        internal static List<ActiveApplication> GetNonAssignedApplication()
        {
            ActiveApplication parameters = new ActiveApplication();
            parameters.IdNameActivity = ActiveApplication.IdNameActivityEnum.Lack;
            List<ActiveApplication> activeApplication = GetActiveApplication(parameters);


            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                string contentCommand = string.Empty;
                for (int i = 0; i < activeApplication.Count; i++)
                {
                    contentCommand = "SELECT COUNT(*) as isToday FROM dailyuseofapplication WHERE IdTitle = " + activeApplication[i].ID;
                    if (DataBase.GetListStringFromExecuteReader(contentCommand, "isToday")[0] != "0")
                        activeApplication[i].Date = DateTime.Now.ToString();
                    else
                    {
                        contentCommand = "SELECT alldate.date AS Date FROM alldate WHERE IdTitle = " + activeApplication[i].ID;
                        activeApplication[i].Date = DataBase.GetListStringFromExecuteReader(contentCommand, "Date")[0];
                    }
                }
            }
            return activeApplication.OrderBy(x => x.Date).Reverse().ToList();
        }

        public static bool CheckIfExistTitle(string title)
        {
            string contentCommand = "SELECT COUNT(*) as TitleExist from activeapplications WHERE Title = " + title;
            if (DataBase.GetListStringFromExecuteReader(contentCommand, "TitleExist")[0] != "0") return true;
            else return false;
        }

        internal static List<ActiveApplication> GetActiveApplication(ActiveApplication parameters)
        {
            List<ActiveApplication> activeApplication = new List<ActiveApplication>();
            string query = "SELECT activeapplications.Id AS Id, activeapplications.Title AS Title, nameactivity.NameActivity AS NameActivity, " +
                "nameactivity.Id AS IdNameActivity FROM activeapplications LEFT OUTER JOIN " +
                "nameactivity ON activeapplications.IdNameActivity = nameactivity.Id WHERE 1 = 1";
            if (parameters.ID > 0) query += " AND Id = " + parameters.ID;
            if (!string.IsNullOrEmpty(parameters.Title)) query += " AND Title = " + SqlValidator.Validate(parameters.Title);
            if (!string.IsNullOrEmpty(parameters.NameActivity)) query += " AND NameActivity = " + SqlValidator.Validate(parameters.NameActivity);
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
                        application.IdNameActivity = (ActiveApplication.IdNameActivityEnum)(Int32.Parse((reader["IdNameActivity"]).ToString()));
                        activeApplication.Add(application);
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error!!!\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return activeApplication;
        }
    }
}
