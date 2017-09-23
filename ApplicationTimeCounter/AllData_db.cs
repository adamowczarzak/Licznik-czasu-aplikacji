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
        /// <param name="idNameActivity">Id aktywności do którego jest przypisana aplikacja.</param>
        /// <param name="daysDifferenceFromToday">Dni różnicy od dzisiejszego dnia.</param>
        /// <param name="openningDataBase">Czy połącznie do bazy danych w metodzie ma być otwierane.</param>
        /// <param name="closingConnectionDataBase">Czy połącznie do bazy danych w metodzie ma być zamykane.</param>
        /// <param name="additionalConnection">Czy używać dodatkowego połącznia z bazy danych.</param>
        public bool Add(int idTitle, int activityTime, int daysDifferenceFromToday = 1,
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
                    command.CommandText = "INSERT INTO alldate (Date, IdTitle, ActivityTime) VALUES (CURDATE() - INTERVAL " + daysDifferenceFromToday + " DAY " +
                    " , " + idTitle + " , " + activityTime + ")";
                    if (DataBase.ExecuteNonQuery(command)) addRecord = true;
                }
                if (closingConnectionDataBase) DataBase.CloseConnection();
            }
            else
            {
                DataBase.AdditionalConnectToDataBase();
                command.Connection = DataBase.AdditionalConnection;
                command.CommandText = "INSERT INTO alldate (Date, IdTitle, ActivityTime) VALUES (CURDATE() - INTERVAL " + daysDifferenceFromToday + " DAY " +
                    " , " + idTitle + " , " + activityTime + ")";
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
            string contentCommand = "SELECT Id from alldate WHERE Date = CURDATE() - INTERVAL 1 DAY OR Date = CURDATE()";
            if (!DataBase.GetListStringFromExecuteReader(contentCommand, "Id").Any()) return true;
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
                        if (Add(1, 60 * 24, -i, true, true))
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
            string dateDifference = DataBase.GetListStringFromExecuteReader(contentCommand, "dateDifference")[0];
            return dateDifference;
        }

        /// <summary>
        /// Pobiera date uruchomienia aplikacji.
        /// </summary>
        /// <returns>Zwraca date jako string.</returns>
        private string GetDataRunApplication()
        {
            string dataRunApplication = string.Empty;
            string contentCommand = "SELECT Date FROM alldate ORDER BY Date ASC LIMIT 1";
            if (DataBase.GetListStringFromExecuteReader(contentCommand, "Date").Any())
                dataRunApplication = DataBase.GetListStringFromExecuteReader(contentCommand, "Date")[0];
            else dataRunApplication = "0";
            return dataRunApplication;
        }

        /// <summary>
        /// Sprawdza czy date istnieje w bazie.
        /// </summary>
        /// <param name="numberDayBack">Ilość dni na minusie wstecz od dzieśejszej daty.</param>
        /// <returns></returns>
        private bool CheckIfDateExistInBase(string numberDayBack = "0")
        {
            string contentCommand = "SELECT Date FROM alldate WHERE Date = CURDATE() - INTERVAL " + numberDayBack + " DAY LIMIT 1";
            if (!DataBase.GetListStringFromExecuteReader(contentCommand, "Date").Any()) return false;
            else return true;
        }
    }
}
