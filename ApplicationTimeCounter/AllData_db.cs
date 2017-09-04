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


        public AllData_db()
        {
            command = new MySqlCommand();
        }

        /// <summary>
        /// Dodaje rekord do tabeli 'alldate', obługuje również dodatkowe połączenia.
        /// </summary>
        /// <param name="title">Nazwa aktywności.</param>
        /// <param name="activityTime">Czas aktywności.</param>
        /// <param name="daysDifferenceFromToday">Dni różnicy od dzisiejszego dnia.</param>
        /// <param name="openningDataBase">Czy połącznie do bazy danych w metodzie ma być otwierane.</param>
        /// <param name="closingConnectionDataBase">Czy połącznie do bazy danych w metodzie ma być zamykane.</param>
        /// <param name="additionalConnection">Czy używać dodatkowego połącznia z bazy danych.</param>
        public bool Add(string title, int activityTime, int daysDifferenceFromToday = 1,
            bool openninConnectiongDataBase = false, bool closingConnectionDataBase = false, 
            bool additionalConnection = false)
        {
            bool addRecord = false;
            if (additionalConnection == false)
            {
                if (openninConnectiongDataBase) DataBase.ConnectToDataBase();

                if (DataBase.CheckIsOpenConnection())
                {
                    command.Connection = DataBase.Connection;
                    command.CommandText = "INSERT INTO alldate (Date, Title, ActivityTime) VALUES (CURDATE() - INTERVAL " + daysDifferenceFromToday + " DAY" +
                    " , '" + title + "' , '" + activityTime + "')";
                    if (DataBase.ExecuteNonQuery(command)) addRecord = true;         
                }
                if (closingConnectionDataBase) DataBase.CloseConnection();
            }
            else
            {
                DataBase.AdditionalConnectToDataBase();              
                command.Connection = DataBase.AdditionalConnection;
                command.CommandText = "INSERT INTO alldate (Date, Title, ActivityTime) VALUES (CURDATE() - INTERVAL " + daysDifferenceFromToday + " DAY" +
                " , '" + title + "' , '" + activityTime + "')";
                if (DataBase.ExecuteNonQuery(command)) addRecord = true;
                DataBase.AdditionalConnection.Close();
            }
            return addRecord;
        }


        /// <summary>
        /// Sprawdzanie czy obecna data jest kolejnym dniem, którego jeszcze nie ma w tabeli.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfIsNextDay()
        {
            string contentCommand = "SELECT idAllDate from alldate WHERE Date = CURDATE() - INTERVAL 1 DAY OR Date = CURDATE()";
            string dateDifference = GetStringFromExecuteReader(contentCommand, "idAllDate");
            if (string.IsNullOrEmpty(dateDifference)) return true;
            else return false;
        }

        /// <summary>
        /// Sprawdzanie czy dane w tabeli w bazie są kompletne, jeśli nie uzupenia je.
        /// </summary>
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
                        if (Add("Wył. komputer", 60 * 24, -i, true, true))
                            ApplicationLog.LogService.AddRaportInformation("Information\tTabela 'alldata' została uzupełniona o brakujące rekordy.");
                        else
                        {
                            ApplicationLog.LogService.AddRaportError("Error !!!\tNie Udało się dodać nowego rekordu do tabeli 'alldate'",
                                ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                                System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\AllData_db.cs");
                        }
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
        public string GetDayWorkingApplication()
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
            string returnString = string.Empty;
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = contentCommand;
                MySqlDataReader reader = command.ExecuteReader();             
                if (reader.Read())
                {
                    try
                    {
                        returnString = reader[nameReturnColumn].ToString();
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }

                DataBase.CloseConnection();
                reader.Dispose();
            }
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
