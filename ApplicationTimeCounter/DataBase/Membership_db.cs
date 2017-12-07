using ApplicationTimeCounter.Other;
using System;

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
    }
}
