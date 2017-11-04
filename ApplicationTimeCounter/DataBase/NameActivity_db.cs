using System;
using System.Collections.Generic;

namespace ApplicationTimeCounter
{
    static class NameActivity_db
    {
        public static Dictionary<string, string> GetNameActivityDictionary()
        {
            string contentCommand = "SELECT Id, NameActivity FROM nameactivity WHERE ID > 1";
            Dictionary<string, string> nameActivity = DataBase.GetDictionaryFromExecuteReader(contentCommand, "NameActivity", "Id");
            return nameActivity;
        }

        public static string GetAllNameActivity()
        {
            string contentCommand = "SELECT COUNT(*) AS numberNameActivity FROM nameactivity WHERE ID != 1";
            return DataBase.GetListStringFromExecuteReader(contentCommand, "numberNameActivity")[0];
        }

        public static int GetIDForNameActivity(string nameActivity)
        {
            string contentCommand = "SELECT Id FROM nameactivity WHERE NameActivity = '" + nameActivity + "'";
            return Convert.ToInt32(DataBase.GetListStringFromExecuteReader(contentCommand, "Id")[0]);
        }

        public static bool CheckIfExistName(string name)
        {
            string contentCommand = "SELECT COUNT(*) AS ifExistName FROM nameactivity WHERE NameActivity = '" + name + "'";
            if (Convert.ToInt32(DataBase.GetListStringFromExecuteReader(contentCommand, "ifExistName")[0]) > 0)
                return true;
            else
                return false;
        }

        public static void ChangeNameActivity(string oldNameActivity, string newNameActivity)
        {
            string contentCommand = "UPDATE nameactivity SET NameActivity = '" + newNameActivity + "'"
                + " WHERE NameActivity = '" + oldNameActivity + "'";

            if (DataBase.ExecuteNonQuery(contentCommand))
                ApplicationLog.LogService.AddRaportInformation("Nazwa aktywności '" + oldNameActivity + "' została zmieniona na '" + newNameActivity + "'");
            else
                ApplicationLog.LogService.AddRaportWarning("Nie udało się zamienić nazwy aktywności '" + oldNameActivity + "' na '" + newNameActivity + "'");
        }

        public static void AddNewActivity(string nameActivity)
        {
            string contentCommand = "INSERT INTO nameactivity (NameActivity) VALUES ('" + nameActivity + "')";

            if (DataBase.ExecuteNonQuery(contentCommand))
                ApplicationLog.LogService.AddRaportInformation("Została dodana nowa aktywność '" + nameActivity + "'");
            else
                ApplicationLog.LogService.AddRaportWarning("Nie udało się dodać nowej aktywności'" + nameActivity + "'");
        }

    }
}
