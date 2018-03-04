using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    class ExceptionApplication_db
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
            contentCommand += "INNER JOIN activeapplications ON activeapplications.Id = exceptionapplication.IdExceptionApplication";
            return DataBase.GetDictionaryFromExecuteReader(contentCommand, "Id", "Title");
        }
    }
}
