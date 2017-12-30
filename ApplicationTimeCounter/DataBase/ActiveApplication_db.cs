using System;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using ApplicationTimeCounter.Other;
using ApplicationTimeCounter.ApplicationObjectsType;

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

        public static bool AddGroupToApplication(string idApplication, string idGroup)
        {
            string contentCommand = "UPDATE activeapplications SET IdMembership = " + idGroup + ", AutoGrouping = 0 WHERE Id = " + idApplication;
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
                   System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ActiveApplication_db.cs");
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

        public static bool DeleteOneApplicationWithGroup(int idApplication)
        {
            string contentCommand = "UPDATE activeapplications SET IdMembership = NULL WHERE ID = " + idApplication;

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
            return GetDateForActiveApplication(activeApplication);
        }

        public static bool CheckIfExistTitle(string title)
        {
            string contentCommand = "SELECT COUNT(*) as TitleExist from activeapplications WHERE Title = " + title;
            if (DataBase.GetListStringFromExecuteReader(contentCommand, "TitleExist")[0] != "0") return true;
            else return false;
        }

        internal static List<ActiveApplication> GetActiveApplication(ActiveApplication parameters)
        {
            List<ActiveApplication> activeApplications = new List<ActiveApplication>();
            string query = "SELECT activeapplications.Id AS " + ColumnNames.ID + 
                ", activeapplications.Title AS " + ColumnNames.Title + 
                ", nameactivity.NameActivity AS " + ColumnNames.NameActivity + 
                ", nameactivity.Id AS " + ColumnNames.IdNameActivity + 
                ", activeapplications.IdMembership AS " + ColumnNames.IdMembership + 
                ", activeapplications.AutoGrouping AS " + ColumnNames.IfAutoGrouping + 
                " FROM activeapplications LEFT OUTER JOIN " +
                "nameactivity ON activeapplications.IdNameActivity = nameactivity.Id WHERE activeapplications.Id > 2 ";
            if (parameters.ID > 0) query += " AND Id = " + parameters.ID;
            if (!string.IsNullOrEmpty(parameters.Title)) query += " AND Title = " + SqlValidator.Validate(parameters.Title);
            if (!string.IsNullOrEmpty(parameters.NameActivity)) query += " AND NameActivity = " + SqlValidator.Validate(parameters.NameActivity);
            if (parameters.IdNameActivity > 0) query += " AND IdNameActivity = " + (int)parameters.IdNameActivity;
            if (parameters.IdMembership > 0) query += " AND IdMembership = " + parameters.IdMembership;
            if (parameters.IdMembership == -1) query += " AND IdMembership IS NULL ";

            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        activeApplications.Add(ActiveApplication.GetActiveApplicationFromReader(reader));
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error!!!\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return activeApplications;
        }

        public static string GetAllNotJoinedApplication()
        {
            string contentCommand = "SELECT COUNT(*) as noJoinedApplication from activeapplications " +
                "WHERE IdMembership IS NULL AND Id > 2";
            string AllNotAssignedApplication = DataBase.GetListStringFromExecuteReader(contentCommand, "noJoinedApplication")[0];
            return AllNotAssignedApplication;
        }

        internal static List<ActiveApplication> GetNonJoinedApplication()
        {
            ActiveApplication parameters = new ActiveApplication();
            parameters.IdMembership = -1;
            List<ActiveApplication> activeApplication = GetActiveApplication(parameters);
            return GetDateForActiveApplication(activeApplication);
        }

        private static List<ActiveApplication> GetDateForActiveApplication(List<ActiveApplication> activeApplication)
        {
            string contentCommand = string.Empty;
            Dictionary<string, string> dateIdTitileList = new Dictionary<string, string>();

            contentCommand = "SELECT IdTitle, Date FROM alldate WHERE IdTitle IN(";
            for (int i = 0; i < activeApplication.Count; i++)
                contentCommand += activeApplication[i].ID + ((i < activeApplication.Count-1) ? ", " : "");
            contentCommand += ")";
            dateIdTitileList = DataBase.GetDictionaryFromExecuteReader(contentCommand, "IdTitle", "Date");
            for (int i = 0; i < activeApplication.Count; i++)
            {
                activeApplication[i].Date = dateIdTitileList.FirstOrDefault(x => string.Equals(x.Key.ToString(), activeApplication[i].ID.ToString())).Value;
                if (String.IsNullOrEmpty(activeApplication[i].Date)) 
                    activeApplication[i].Date = DateTime.Now.ToString();
            }

            return activeApplication.OrderBy(x => x.Date).Reverse().ToList();
        }
    }
}
