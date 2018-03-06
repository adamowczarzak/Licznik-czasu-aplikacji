using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    public static class ExceptionApplication_db
    {
        public static void AddExceptionApplication(int idApplication)
        {
            string contentCommand = "INSERT INTO exceptionapplication (IdExceptionApplication) VALUES (" + idApplication + ")";
            DataBase.ExecuteNonQuery(contentCommand);
        }

        public static bool CheckIfExistApplication(int idApplication)
        {
            string contentCommand = "SELECT COUNT(*) AS ifExistName FROM exceptionapplication WHERE IdExceptionApplication = " + idApplication;
            if (Convert.ToInt32(DataBase.GetListStringFromExecuteReader(contentCommand, "ifExistName")[0]) > 0)
                return true;
            else
                return false;
        }

        public static Dictionary<string, string> GetNameApplicationDictionary()
        {
            string contentCommand = "SELECT activeapplications.Id, activeapplications.Title FROM exceptionapplication ";
            contentCommand += "INNER JOIN activeapplications ON activeapplications.Id = exceptionapplication.IdExceptionApplication ";
            contentCommand += "WHERE activeapplications.IdMembership IS NULL";
            return DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
        }

        public static Dictionary<string, string> GetNameApplicationWitGroupDictionary()
        {
            string contentCommand = "SELECT activeapplications.IdMembership AS IdMembership, membership.Title AS Title FROM exceptionapplication ";
            contentCommand += "INNER JOIN activeapplications ON activeapplications.Id = exceptionapplication.IdExceptionApplication ";
            contentCommand += "INNER JOIN membership ON membership.Id = activeapplications.IdMembership ";
            contentCommand += "WHERE activeapplications.IdMembership IS NOT NULL ";
            contentCommand += "GROUP BY activeapplications.IdMembership, membership.Title";
            return DataBase.GetDictionaryFromExecuteReader(contentCommand, "IdMembership", "Title");
        }

        public static void DeleteExceptionApplication(string idApplication)
        {
            string contentCommand = "DELETE FROM exceptionapplication WHERE IdExceptionApplication = " + idApplication;
            DataBase.ExecuteNonQuery(contentCommand);
        }
    }
}
