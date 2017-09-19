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
        public static List<string> GetNameDailyActivityList()
        {         
            string contentCommand = "SELECT NameActivity FROM nameactivity WHERE ID > 1";
            List<string> nameDailyActivity = DataBase.GetListStringFromExecuteReader(contentCommand, "NameActivity");
            return nameDailyActivity;
        }
    }
}
