using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    class AllData_db
    {

        private MySqlCommand command;
        private DataBase dataBase;


        public AllData_db()
        {
            command = new MySqlCommand();
            dataBase = new DataBase();


        }

        /// <summary>
        /// Dodaje rekord do tabeli 'alldate'
        /// </summary>
        /// <param name="title">Nazwa aktywności.</param>
        /// <param name="activityTime">Czas aktywności.</param>
        /// <param name="daysDifferenceFromToday">Dni różnicy od dzisiejszego dnia.</param>
        public void Add(string title, int activityTime, int daysDifferenceFromToday = 1)
        {
            dataBase.ConnectToDataBase();
            command.Connection = dataBase.Connection;
            command.CommandText = "INSERT INTO alldate (Date, Title, ActivityTime) VALUES (CURDATE() - INTERVAL "+daysDifferenceFromToday+" DAY" +
            " , '" + title + "' , '" + activityTime + "')";
            command.ExecuteNonQuery();
            dataBase.CloseConnection();
        }

        public bool CheckIfIsNextDay()
        {
            string contentCommand = "SELECT idAllDate from alldate WHERE Date = CURDATE() - INTERVAL 1 DAY OR Date = CURDATE()";
            string dateDifference = GetStringFromExecuteReader(contentCommand, "idAllDate");
            if (string.IsNullOrEmpty(dateDifference)) return false;
            else return true;
        }

        public void CheckIfIsActualDataInBaseAndUpdate()
        {
            int coutDay;
            bool successfulConversion = Int32.TryParse(GetDayWorkingApplication(), out coutDay);
            if (successfulConversion)
            {
                for (int i = coutDay; i < 0; i++)
                {
                    if (CheckIfDateExistInBase((-i).ToString()) == false)
                    {
                        Add("Wył. komputer", 60 * 24, -i);
                        ApplicationLog.LogService.AddRaportInformation("Information\tTabela 'alldata' została uzupełniona o brakujące rekordy.");
                    }
                        
                }      
            }
            else
            {
                ApplicationLog.LogService.AddRaportConvert("Warning!\tNie można uzupełnić bazy danych.",
                    ApplicationLog.LogService.GetNameCurrentMethod() + "()", "int",
                    System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\AllData_db.cs");
            }
        }

        /// <summary>
        /// Pobiera ilość dni pracy aplkacji.
        /// </summary>
        /// <returns>Zwraca ilość dni jako string.</returns>
        private string GetDayWorkingApplication()
        {
            string contentCommand = "SELECT DATEDIFF('" + GetDataRunApplication() + "', CURDATE()) as dateDifference";
            string dateDifference = GetStringFromExecuteReader(contentCommand, "dateDifference");
            return dateDifference;
        }

        /// <summary>
        /// Pobiera date uruchomienia aplikacji.
        /// </summary>
        /// <returns>Zwraca date jako string.</returns>
        private string GetDataRunApplication()
        {
            string contentCommand = "SELECT Date FROM alldate ORDER BY Date ASC LIMIT 1";
            string dataRunApplication = GetStringFromExecuteReader(contentCommand, "Date");
            return dataRunApplication;
        }


        /// <summary> 
        /// Metoda zwraca jeden wynik zapytania w postaci stringa. 
        /// </summary> 
        /// <param name="contentCommand">Cała zawartość zapytania.</param>
        /// <param name="nameReturnColumn">Nazwa kolumny z której ma być zwracana wartość.</param>
        private string GetStringFromExecuteReader(string contentCommand, string nameReturnColumn)
        {
            dataBase.ConnectToDataBase();
            command.Connection = dataBase.Connection;
            command.CommandText = contentCommand;
            MySqlDataReader reader = command.ExecuteReader();
            string returnString = string.Empty;
            if (reader.Read())
            {
                try
                {
                    returnString = reader[nameReturnColumn].ToString();
                }
                catch (MySqlException message)
                {
                    ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message.ToString());
                } 
            }

            dataBase.CloseConnection();
            reader.Dispose();
            return returnString;
        }

        /// <summary>
        /// Sprawdza czy date istnieje w bazie.
        /// </summary>
        /// <param name="numberDayBack">Ilość dni na minusie wstecz od dzieśejszej daty.</param>
        /// <returns></returns>
        private bool CheckIfDateExistInBase(string numberDayBack = "0")
        {
            string contentCommand = "SELECT Date FROM alldate WHERE Date = CURDATE() - INTERVAL " + numberDayBack + " DAY LIMIT 1";
            string dateDifference = GetStringFromExecuteReader(contentCommand, "Date");
            if (String.IsNullOrEmpty(dateDifference)) return false;
            else return true;
        }
    }
}
