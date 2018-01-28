using System;
using System.Linq;
using System.Collections.Generic;
using ApplicationTimeCounter.Other;
using ApplicationTimeCounter.ApplicationObjectsType;
using System.Data.SqlClient;

namespace ApplicationTimeCounter
{
    public static class ActiveApplication_db
    {

        private static SqlCommand command = new SqlCommand();

        public static string GetAllNotAssignedApplication()
        {
            string contentCommand = "SELECT COUNT(*) as noAssigmentApplication from activeapplications " +
                "WHERE idNameActivity = 1";
            return DataBase.GetListStringFromExecuteReader(contentCommand, "noAssigmentApplication")[0];
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

        public static bool AddGroupToApplications(List<int> idApplications, string idGroup)
        {
            string contentCommand = "UPDATE activeapplications SET IdMembership = " + idGroup + ", AutoGrouping = 1 WHERE Id " + SqlValidator.Validate_IN(idApplications);
            return DataBase.ExecuteNonQuery(contentCommand);
        }

        public static bool DeleteAllApplicationsWithActivity(int idActivity)
        {
            string contentCommand = "UPDATE activeapplications SET IdNameActivity = "
                + ((int)IdNameActivityEnum.Lack).ToString()
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
                + ((int)IdNameActivityEnum.Lack).ToString()
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
            string contentCommand = "UPDATE activeapplications SET IdMembership = NULL , AutoGrouping = NULL WHERE ID = " + idApplication;

            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportWarning("Nie udało się usunąć aplikacji z aktywności");
                return false;
            }
            else return true;
        }

        public static bool DeleteAllApplicationsWithGroup(int idGroup, bool ifAutoGrouping = false, bool ifAddAutoGrouping = false)
        {
            string autoGrouping = string.Empty;
            if (ifAddAutoGrouping)
                autoGrouping = " AND AutoGrouping = " + Convert.ToInt32(ifAutoGrouping);

            string contentCommand = "UPDATE activeapplications SET IdMembership = NULL , AutoGrouping = NULL"
                + " WHERE IdMembership = " + idGroup + autoGrouping;

            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportError("Nie udało się usunąć wszystkich aplikacji z grupy",
                   ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                   System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ActiveApplication_db.cs");
                return false;
            }
            else return true;
        }

        internal static List<ActiveApplication> GetNonAssignedApplication()
        {
            CommandParameters parameters = new CommandParameters();
            parameters.IdNameActivity = IdNameActivityEnum.Lack;
            List<ActiveApplication> activeApplication = GetActiveApplication(parameters);
            return GetDateForActiveApplication(activeApplication);
        }

        public static bool CheckIfExistTitle(string title)
        {
            string contentCommand = "SELECT COUNT(*) as TitleExist from activeapplications WHERE Title = " + title;
            if (DataBase.GetListStringFromExecuteReader(contentCommand, "TitleExist")[0] != "0") return true;
            else return false;
        }

        internal static List<ActiveApplication> GetActiveApplication(CommandParameters parameters)
        {
            List<ActiveApplication> activeApplications = new List<ActiveApplication>();
            string query = "SELECT activeapplications.Id AS " + ColumnNames.ID +
                ", activeapplications.Title AS " + ColumnNames.Title +
                ", nameactivity.NameActivity AS " + ColumnNames.NameActivity +
                ", nameactivity.Id AS " + ColumnNames.IdNameActivity +
                ", activeapplications.IdMembership AS " + ColumnNames.IdMembership +
                ", activeapplications.AutoGrouping AS " + ColumnNames.IfAutoGrouping +
                " FROM activeapplications " +
                " LEFT JOIN nameactivity ON activeapplications.IdNameActivity = nameactivity.Id " +
                " WHERE activeapplications.Id > 2 ";
            query += CommandParameters.CheckParameters(parameters);
            

            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        activeApplications.Add(ActiveApplication.GetActiveApplicationFromReader(reader));
                    }
                    catch (SqlException message)
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
            return DataBase.GetListStringFromExecuteReader(contentCommand, "noJoinedApplication")[0];
        }

        internal static List<ActiveApplication> GetNonJoinedApplication()
        {
            CommandParameters parameters = new CommandParameters();
            parameters.IdMembership = -1;
            List<ActiveApplication> activeApplication = GetActiveApplication(parameters);
            return GetDateForActiveApplication(activeApplication);
        }

        private static List<ActiveApplication> GetDateForActiveApplication(List<ActiveApplication> activeApplication)
        {
            if (activeApplication.Any())
            {
                string contentCommand = string.Empty;
                Dictionary<string, string> dateIdTitileList = new Dictionary<string, string>();

                contentCommand = "SELECT IdTitle, Date FROM alldate WHERE IdTitle IN(";
                for (int i = 0; i < activeApplication.Count; i++)
                    contentCommand += activeApplication[i].ID + ((i < activeApplication.Count - 1) ? ", " : "");
                contentCommand += ")";
                dateIdTitileList = DataBase.GetDictionaryFromExecuteReader(contentCommand, "IdTitle", "Date");
                for (int i = 0; i < activeApplication.Count; i++)
                {
                    activeApplication[i].Date = dateIdTitileList.FirstOrDefault(x => string.Equals(x.Key.ToString(), activeApplication[i].ID.ToString())).Value;
                    if (String.IsNullOrEmpty(activeApplication[i].Date))
                        activeApplication[i].Date = DateTime.Now.ToString();
                }
            }

            return activeApplication.OrderBy(x => x.Date).Reverse().ToList();
        }

        public static Dictionary<string, string> GetNameApplicationDictionary()
        {
            string contentCommand = "SELECT Id, Title FROM activeapplications";
            return DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
        }

        public static Dictionary<string, string> GetNameApplicationDictionaryWithoutGroup()
        {
            string contentCommand = "SELECT Id, Title FROM activeapplications WHERE IdMembership IS NULL";
            return DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
        }

        public static Dictionary<string, string> GetNameApplicationDictionaryWithGroup(int idGroup)
        {
            string contentCommand = "SELECT Id, Title FROM activeapplications WHERE IdMembership " + idGroup + " AND AutoGrouping = 1";
            return DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
        }

        public static string GetAllAutoGroupingApplication(int idGroup)
        {
            string contentCommand = "SELECT COUNT(*) as AutoGroupingApplication from activeapplications WHERE IdMembership = " 
                + idGroup + " AND AutoGrouping = 1";
            return DataBase.GetListStringFromExecuteReader(contentCommand, "AutoGroupingApplication")[0];
        }

        /// <summary>
        /// Metoda tylko dla szukania id aktywności, brak w metodzie walidacji SQL.
        /// </summary>
        /// <param name="name">Tytuł szukaniej aktywności.</param>
        /// <returns></returns>
        public static string GetIdActivityByName(string name)
        {
            string contentCommand = "SELECT Id from activeapplications WHERE Title = " + name;
            return DataBase.GetListStringFromExecuteReader(contentCommand, "Id")[0];
        }

        public static string GetNameGroupByIdTitle(int idTitle)
        {
            string contentCommand = "SELECT membership.Title as nameGroup from activeapplications " +
                " INNER JOIN membership ON membership.Id = activeapplications.IdMembership " +
                " WHERE activeapplications.Id = " + idTitle;
            if (DataBase.GetListStringFromExecuteReader(contentCommand, "nameGroup").Any())
                return DataBase.GetListStringFromExecuteReader(contentCommand, "nameGroup")[0];
            else return "Brak";
        }

        public static string GetNameActivityByIdTitle(int idTitle)
        {
            string contentCommand = "SELECT nameactivity.NameActivity as nameActivity from activeapplications " +
                " INNER JOIN nameactivity ON nameactivity.Id = activeapplications.IdNameActivity " +
                " WHERE activeapplications.Id = " + idTitle;
            return DataBase.GetListStringFromExecuteReader(contentCommand, "nameActivity")[0];
        }
    }
}
