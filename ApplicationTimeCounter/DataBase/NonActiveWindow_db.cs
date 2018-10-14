using ApplicationTimeCounter.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    public static class NonActiveWindow_db
    {
        public static Dictionary<string, string> GetIDNonActiveWindow()
        {
            string contentCommand = "SELECT IdNoActiveWindow, alldate.Date AS Date FROM noactivewindow ";
            contentCommand += "INNER JOIN alldate ON alldate.Id = noactivewindow.IdNoActiveWindow";
            return DataBase.GetDictionaryFromExecuteReader(contentCommand, "IdNoActiveWindow", "Date");
        }

        public static string GetTimeNonActiveWindow(string idNonActiveWindow)
        {
            string contentCommand = "SELECT alldate.ActivityTime AS ActivityTime FROM noactivewindow ";
            contentCommand += "INNER JOIN alldate ON alldate.Id = noactivewindow.IdNoActiveWindow ";
            contentCommand += "WHERE noactivewindow.IdNoActiveWindow = " + idNonActiveWindow;
            return DataBase.GetListStringFromExecuteReader(contentCommand, "ActivityTime")[0];
        }

        public static void DeleteNonActiveWindow(string idNonActiveWindow)
        {
            string contentCommand = "DELETE FROM noactivewindow WHERE IdNoActiveWindow = " + idNonActiveWindow;
            DataBase.ExecuteNonQuery(contentCommand);
        }

        public static void ChangeNameNonActiveWindow(string idNonActiveWindow, string newName)
        {
            if (!ActiveApplication_db.CheckIfExistTitle(SqlValidator.Validate(newName)))
            {
                string contentCommand = "INSERT INTO activeapplications (Title, IdNameActivity) VALUES ( " + SqlValidator.Validate(newName) + " , 1 )";
                DataBase.ExecuteNonQuery(contentCommand);

                string newIDApplication = ActiveApplication_db.GetIdActivityByName(SqlValidator.Validate(newName));
                new AllData_db().UpdateIDApplication(newIDApplication, idNonActiveWindow);

                DeleteNonActiveWindow(idNonActiveWindow);
            }
            else
            {
                string newIDApplication = ActiveApplication_db.GetIdActivityByName(SqlValidator.Validate(newName));
                new AllData_db().UpdateIDApplication(newIDApplication, idNonActiveWindow);

                DeleteNonActiveWindow(idNonActiveWindow);
            }
        }

    }
}
