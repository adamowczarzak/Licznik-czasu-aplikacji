using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ApplicationTimeCounter.Other;
using ApplicationTimeCounter.ApplicationObjectsType;

namespace ApplicationTimeCounter
{
    public class AllData_db
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
                            ApplicationLog.LogService.AddRaportInformation("Tabela 'alldata' została uzupełniona o brakujące rekordy.");
                        else
                        {
                            ApplicationLog.LogService.AddRaportError("Nie Udało się dodać nowego rekordu do tabeli 'alldate'",
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
        /// Pobiera ilość dni pracy aplikacji. Zwraca ilość dni z minusem jako string.
        /// </summary>
        /// <returns>Zwraca ilość dni jako string.</returns>
        public string GetDayWorkingApplication()
        {
            string contentCommand = "SELECT DATEDIFF('" + GetDataRunApplication() + "', CURDATE()) as dateDifference";
            string dateDifference = DataBase.GetListStringFromExecuteReader(contentCommand, "dateDifference")[0];
            return dateDifference;
        }

        public int GetTimeForNumberActivity(List<int> numbers, string startDate = "", string endDate = "", bool ifExcept = false)
        {
            string contentCommand = "SELECT SUM(ActivityTime) as sumTimeActivity FROM alldate INNER JOIN " +
                "activeapplications ON alldate.IdTitle = activeapplications.Id " +
                "WHERE 1 = 1";
            for (int i = 0; i < numbers.Count; i++)
            {
                contentCommand += " AND activeapplications.IdNameActivity " + (ifExcept ? "!" : "") + "= " + numbers[i];
            }
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                contentCommand += " AND " + SqlValidator.Validate(startDate) + " < alldate.Date AND alldate.Date < " + SqlValidator.Validate(endDate);
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                contentCommand += " AND alldate.Date = " + SqlValidator.Validate(startDate);
            }
            int returnValue = 0;
            Int32.TryParse(DataBase.GetListStringFromExecuteReader(contentCommand, "sumTimeActivity")[0], out returnValue);
            return returnValue;
        }

        /// <summary>
        /// Pobierz dzienne aktywności jako obiekt 'Activity'
        /// </summary>
        /// <returns>Zwraca listę dziennych aktywności</returns>
        public List<Activity> GetDailyActivity(CommandParameters parameters)
        {
            List<Activity> dailyActivity = new List<Activity>();
            string query = "SELECT nameactivity.Id AS " + ColumnNames.ID + 
                ", nameactivity.NameActivity AS " + ColumnNames.Name +
                ", SUM(alldate.ActivityTime) AS " + ColumnNames.ActivityTime +
                ", alldate.Date AS " + ColumnNames.Date +
                " FROM nameactivity " +
                "INNER JOIN activeapplications ON nameactivity.Id = activeapplications.IdNameActivity " +
                "INNER JOIN alldate ON alldate.IdTitle = activeapplications.Id " +
                "WHERE 1 = 1 ";
            query += CommandParameters.CheckParameters(parameters);
            query += " GROUP BY nameactivity.Id ";
            
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        dailyActivity.Add(Activity.GetActivityFromReader(reader));
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error!!!\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return dailyActivity;
        }

        public string GetTimeActivityForDateAndIdActivity(string date, int idTitle)
        {
            string contentCommand = "SELECT ActivityTime from alldate WHERE Date = " + SqlValidator.Validate(date)
                + " AND IdTitle = " + idTitle;

            string returnValue = DataBase.GetListStringFromExecuteReader(contentCommand, "ActivityTime")[0];
            return (!string.IsNullOrEmpty(returnValue)) ? returnValue : "0";
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
