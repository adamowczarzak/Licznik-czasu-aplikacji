using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    static class NameDailyActivity_db
    {
        private static MySqlCommand command;


        public static List<string> GetNameDailyActivityList()
        {         
            string contentCommand = "SELECT NameDailyActivity from namedailyactivity";
            List<string> nameDailyActivity = GetListStringFromExecuteReader(contentCommand, "NameDailyActivity");
            return nameDailyActivity;
        }


        /// <summary> 
        /// Metoda zwraca jeden wynik zapytania w postaci listy stringów. 
        /// </summary> 
        /// <param name="contentCommand">Cała zawartość zapytania.</param>
        /// <param name="nameReturnColumn">Nazwa kolumny z której ma być zwracana wartość.</param>
        private static List<string> GetListStringFromExecuteReader(string contentCommand, string nameReturnColumn)
        {
            List<string> returnList = new List<string>();
            if (DataBase.ConnectToDataBase())
            {
                command = new MySqlCommand(contentCommand, DataBase.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        returnList.Add(reader[nameReturnColumn].ToString());
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return returnList;
        }
    }
}
