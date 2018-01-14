using ApplicationTimeCounter.ApplicationObjectsType;
using ApplicationTimeCounter.Other;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ApplicationTimeCounter
{
    public static class Membership_db
    {
        public static bool Add(string name)
        {
            string contentCommand = "INSERT INTO membership (Title) VALUES (" + SqlValidator.Validate(name) + ")";
            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportWarning("Nie udało się dodać nowej grupy");
                return false;
            }
            else return true;
        }

        public static bool CheckIfExistName(string name)
        {
            string contentCommand = "SELECT COUNT(*) AS ifExistName FROM membership WHERE Title = " + SqlValidator.Validate(name);
            if (Convert.ToInt32(DataBase.GetListStringFromExecuteReader(contentCommand, "ifExistName")[0]) > 0)
                return true;
            else
                return false;
        }

        public static bool ActivationGroup(string idGroup, bool activationGroup = true)
        {
            int ifActivation = Convert.ToInt32(activationGroup);
            string contentCommand = "UPDATE membership SET Active = " + ifActivation + " WHERE Id  = " + idGroup;
            return DataBase.ExecuteNonQuery(contentCommand);
        }

        public static string GetNumberAllGroups()
        {
            string contentCommand = "SELECT COUNT(*) AS numberGroups FROM membership";
            return DataBase.GetListStringFromExecuteReader(contentCommand, "numberGroups")[0];
        }

        public static string GetNumberAllConfiguredGroups()
        {
            string contentCommand = "SELECT COUNT(*) AS numberGroups FROM membership WHERE Configuration = 1";
            return DataBase.GetListStringFromExecuteReader(contentCommand, "numberGroups")[0];
        }

        public static List<Membership> GetAllGroups(CommandParameters parameters)
        {
            List<Membership> allGroups = new List<Membership>();

            string query = "SELECT membership.Id AS " + ColumnNames.ID +
                ", membership.Title AS " + ColumnNames.Title +
                ", membership.Date AS " + ColumnNames.Date +
                ", membership.Active AS " + ColumnNames.IfActive +
                ", membership.Configuration AS " + ColumnNames.IfConfiguration +
                ", membership.Filter AS " + ColumnNames.Filter +
                ", membership.ActiveConfiguration AS " + ColumnNames.IfActiveConfiguration +
                ", membership.AsOneApplication AS " + ColumnNames.IfAsOneApplication +
                " FROM membership WHERE 1 = 1 ";
                query += CommandParameters.CheckParameters(parameters);

                 if (DataBase.ConnectToDataBase())
            {
                SqlCommand command = new SqlCommand();
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        allGroups.Add(Membership.GetMembershipFromReader(reader));
                    }
                    catch (SqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error!!!\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
                 return allGroups;
        }

        public static Dictionary<string, string> GetNameGroupsDictionary()
        {
            string contentCommand = "SELECT Id, Title FROM membership";
            Dictionary<string, string> nameGroups = DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
            return nameGroups;
        }

        public static bool DeleteGroup(int idGroup)
        {
            string contentCommand = "SELECT Title from membership WHERE Id = " + idGroup;
            string nameGroup = DataBase.GetListStringFromExecuteReader(contentCommand, "Title")[0];

            contentCommand = "DELETE FROM membership WHERE Id = " + idGroup;

            if (DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportInformation("Została usunięta aktywność '" + nameGroup + "'");
                return true;
            }
            else
            {
                ApplicationLog.LogService.AddRaportWarning("Nie udało się usunąć aktywności '" + nameGroup + "'");
                return false;
            }

        }

        public static Dictionary<string, string> GetNameGroupsDictionaryWithConfiguration()
        {
            string contentCommand = "SELECT Id, Title FROM membership WHERE Configuration = 1 AND Active = 1";
            Dictionary<string, string> nameGroups = DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
            return nameGroups;
        }

        public static Dictionary<string, string> GetNameGroupsDictionaryWithoutConfiguration()
        {
            string contentCommand = "SELECT Id, Title FROM membership WHERE Configuration = 0 AND Active = 1";
            Dictionary<string, string> nameGroups = DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
            return nameGroups;
        }

        public static bool AddFilterToConfiguration(int idGroup, string filter)
        {
            string contentCommand = "UPDATE membership SET Filter = " + SqlValidator.Validate(filter)
                + " WHERE Id = " + idGroup;

            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportError("Nie udało się dodać filtru ",
                   ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                   System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\Membership_db.cs");
                return false;
            }
            else return true;
        }

        public static bool DeleteFilterToConfiguration(int idGroup)
        {
            string contentCommand = "UPDATE membership SET Filter = NULL WHERE Id = " + idGroup;

            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportError("Nie udało się usunąć filtru ",
                   ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                   System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\Membership_db.cs");
                return false;
            }
            else return true;
        }

        public static string GetNumberApplicationInGroup(int idGroup)
        {
            string contentCommand = "SELECT COUNT(*) AS Num FROM activeapplications  WHERE IdMembership = " + idGroup;
            return DataBase.GetListStringFromExecuteReader(contentCommand, "Num")[0];
        }

        public static string GetNumberApplicationInGroupWithFilter(int idGroup)
        {
            string contentCommand = "SELECT COUNT(*) AS Num FROM activeapplications  WHERE IdMembership = " + idGroup + " AND AutoGrouping = 1";
            return DataBase.GetListStringFromExecuteReader(contentCommand, "Num")[0];
        }

        public static bool SetActivityConfiguration(int idGroup, bool ifActivity)
        {
            string contentCommand = "UPDATE membership SET ActiveConfiguration = " + Convert.ToInt32(ifActivity) + " WHERE Id = " + idGroup;
            return DataBase.ExecuteNonQuery(contentCommand);       
        }

        public static bool SetAsOneApplication(int idGroup, bool ifAsOneApplication)
        {
            string contentCommand = "UPDATE membership SET AsOneApplication = " + Convert.ToInt32(ifAsOneApplication) + " WHERE Id = " + idGroup;
            return DataBase.ExecuteNonQuery(contentCommand);
        }

        public static bool SaveConfiguration(int idGroup)
        {
            string contentCommand = "UPDATE membership SET Configuration = 1 WHERE Id = " + idGroup;
            return DataBase.ExecuteNonQuery(contentCommand);
        }

        public static bool DeleteConfiguration(int idGroup)
        {
            string contentCommand = "UPDATE membership SET Configuration = 0, Filter = NULL, ActiveConfiguration = 0, AsOneApplication = 0 WHERE Id = " + idGroup;
            return DataBase.ExecuteNonQuery(contentCommand);
        }
    }
}
