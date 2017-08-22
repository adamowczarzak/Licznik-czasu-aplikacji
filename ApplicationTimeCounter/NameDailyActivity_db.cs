using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    class NameDailyActivity_db
    {
        private MySqlCommand command;
        private DataBase dataBase;

        public NameDailyActivity_db()
        {
            command = new MySqlCommand();
            dataBase = new DataBase();
        }

      
    }
}
