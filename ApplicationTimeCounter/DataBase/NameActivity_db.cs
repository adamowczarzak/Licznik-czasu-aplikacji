using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string contentCommand = "SELECT COUNT(*) as numberNameActivity FROM nameactivity";
            return DataBase.GetListStringFromExecuteReader(contentCommand, "numberNameActivity")[0];
        }

    }
}
