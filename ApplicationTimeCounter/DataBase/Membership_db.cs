using ApplicationTimeCounter.ApplicationObjectsType;
using ApplicationTimeCounter.Other;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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

        public static List<Membership> GetAllGroups()
        {
            List<Membership> allGroups = new List<Membership>();

            string query = "SELECT membership.Id AS " + ColumnNames.ID +
                ", membership.Title AS " + ColumnNames.Title +
                ", membership.Date AS " + ColumnNames.Date +
                ", membership.Active AS " + ColumnNames.IfActive +
                ", membership.Configuration AS " + ColumnNames.IfConfiguration +
                " FROM membership ";

                 if (DataBase.ConnectToDataBase())
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        allGroups.Add(Membership.GetMembershipFromReader(reader));
                    }
                    catch (MySqlException message)
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
            Dictionary<string, string> nameGroups = DataBase.GetDictionaryFromExecuteReader(contentCommand, "Title", "Id");
            return nameGroups;
        }

        public static bool DeleteAllApplicationsWithGroup(int idGroup)
        {
            string contentCommand = "UPDATE activeapplications SET IdMembership = NULL"
                + " WHERE IdMembership = " + idGroup;

            if (!DataBase.ExecuteNonQuery(contentCommand))
            {
                ApplicationLog.LogService.AddRaportError("Nie udało się usunąć wszystkich aplikacji z grupy",
                   ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                   System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\Membership_db.cs");
                return false;
            }
            else return true;
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
    }
}
