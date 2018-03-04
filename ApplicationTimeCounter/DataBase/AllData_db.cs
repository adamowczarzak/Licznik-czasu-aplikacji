using System;
using System.Linq;
using System.Collections.Generic;
using ApplicationTimeCounter.Other;
using ApplicationTimeCounter.ApplicationObjectsType;
using System.Data.SqlClient;

namespace ApplicationTimeCounter
{
    public class AllData_db
    {

        private SqlCommand command;


        public AllData_db()
        {
            command = new SqlCommand();
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
                    command.CommandText = "INSERT INTO alldate (Date, IdTitle, ActivityTime) VALUES (DATEADD(day, " + (daysDifferenceFromToday*(-1)) + ", GETDATE()) , " 
                        + idTitle + " , " + activityTime + ")";
                    if (DataBase.ExecuteNonQuery(command)) addRecord = true;
                }
                if (closingConnectionDataBase) DataBase.CloseConnection();
            }
            else
            {
                DataBase.AdditionalConnectToDataBase();
                command.Connection = DataBase.AdditionalConnection;
                command.CommandText = "INSERT INTO alldate (Date, IdTitle, ActivityTime) VALUES (DATEADD(day, " + (daysDifferenceFromToday * (-1)) + ", GETDATE()) , "
                    + idTitle + " , " + activityTime + ")";
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
            string contentCommand = "SELECT Id from alldate WHERE Date = CONVERT(VARCHAR(10), DATEADD(day, -1, GETDATE()), 23) OR Date = GETDATE()";
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
                    if (CheckIfDateExistInBase((i).ToString()) == false)
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
            string contentCommand = "SELECT DATEDIFF(DAY, GETDATE(), " + SqlValidator.Validate(GetDataRunApplication()) + ") as dateDifference";
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
                "WHERE " + ColumnNames.ActivityTime + " > 0 ";
            query += CommandParameters.CheckParameters(parameters);
            query += " GROUP BY nameactivity.Id, nameactivity.NameActivity, alldate.Date ";
            
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        dailyActivity.Add(Activity.GetActivityFromReader(reader));
                    }
                    catch (SqlException message)
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

            string returnValue = string.Empty;
            if (DataBase.GetListStringFromExecuteReader(contentCommand, "ActivityTime").Any())
                return DataBase.GetListStringFromExecuteReader(contentCommand, "ActivityTime")[0];
            else return "0";
        }

        /// <summary>
        /// Pobiera date uruchomienia aplikacji.
        /// </summary>
        /// <returns>Zwraca date jako string.</returns>
        private string GetDataRunApplication()
        {
            string dataRunApplication = string.Empty;
            string contentCommand = "SELECT TOP 1 Date FROM alldate ORDER BY Date ASC";
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
            string contentCommand = "SELECT TOP 1 Date FROM alldate WHERE Date = CONVERT(VARCHAR(10), DATEADD(day, " + numberDayBack + ", GETDATE()), 23)";
            if (!DataBase.GetListStringFromExecuteReader(contentCommand, "Date").Any()) return false;
            else return true;
        }

        /// <summary>
        /// Pobiera każdą pozycje tytułu aktywności.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal List<ActiveApplication> GetActiveApplication(CommandParameters parameters, bool ifGetOnlyOtherActivity = false)
        {
            List<ActiveApplication> activeApplication = new List<ActiveApplication>();
            string query = "SELECT activeapplications.Id AS " + ColumnNames.ID +
                ", activeapplications.Title AS " + ColumnNames.Title +
                ", alldate.ActivityTime AS " + ColumnNames.ActivityTime +
                ", activeapplications.IdNameActivity AS " + ColumnNames.IdNameActivity +
                ", alldate.Date AS " + ColumnNames.Date +
                " FROM alldate " +
                " INNER JOIN activeapplications ON activeapplications.Id = alldate.IdTitle ";
            if (!ifGetOnlyOtherActivity) query += "WHERE activeapplications.Id > 2 ";
            else query += "WHERE activeapplications.Id in (1, 2) ";
            query += CommandParameters.CheckParameters(parameters).Replace("ID", "activeapplications.ID");
            if (ifGetOnlyOtherActivity) query += " ORDER BY Date , ActivityTime DESC";

            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        activeApplication.Add(ActiveApplication.GetActiveApplicationFromReader(reader));
                    }
                    catch (SqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error!!!\tNie udało się pobrać ActiveApplication.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return activeApplication;
        }


        /// <summary>
        /// Pobiera pozycji połączone w grupy.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal List<ActiveApplication> GetActiveApplicationGrouping(CommandParameters parameters)
        {
            List<ActiveApplication> activeApplication = new List<ActiveApplication>();
            string query = "SELECT membership.Title AS " + ColumnNames.Title +
                ", sum(alldate.ActivityTime) AS " + ColumnNames.ActivityTime+
                ", alldate.Date AS " + ColumnNames.Date +
                ", activeapplications.Id AS " + ColumnNames.ID +
                ", activeapplications.IdNameActivity AS " + ColumnNames.IdNameActivity +
                " FROM alldate " +
                " INNER JOIN activeapplications ON activeapplications.Id = alldate.IdTitle " +
                " INNER JOIN membership ON membership.Id = activeapplications.IdMembership " +
                "WHERE activeapplications.Id > 2 AND AsOneApplication = 1 ";
            query += CommandParameters.CheckParameters(parameters).Replace("Date", "alldate.Date").Replace("ID", "activeapplications.ID");
            query += " GROUP BY membership.Title, alldate.Date, activeapplications.Id, activeapplications.IdNameActivity";

            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        activeApplication.Add(ActiveApplication.GetActiveApplicationFromReader(reader));
                    }
                    catch (SqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error!!!\tNie udało się pobrać zgrupowanych ActiveApplication.)", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return activeApplication;
        }

        public string CountApplicationInInterwalTime(string dateFrom, string dateTo)
        {
            string contentCommand = "SELECT COUNT(DISTINCT IdTitle) as countIdTitle FROM alldate "
                + " INNER JOIN activeapplications ON activeapplications.Id = alldate.IdTitle "
                + " LEFT JOIN membership ON membership.Id = activeapplications.IdMembership  "
                + " WHERE IdTitle > 2 AND (activeapplications.IdMembership IS NULL OR membership.AsOneApplication = 0) "
                + " AND " + SqlValidator.Validate_BETWEEN(ColumnNames.Date, dateFrom, dateTo).Replace("Date", "alldate.Date");

            int returnCount = Convert.ToInt32(DataBase.GetListStringFromExecuteReader(contentCommand, "countIdTitle")[0]);

            contentCommand = "SELECT COUNT(DISTINCT activeapplications.IdMembership) as countIdTitle FROM alldate "
               + " INNER JOIN activeapplications ON activeapplications.Id = alldate.IdTitle "
               + " INNER JOIN membership ON membership.Id = activeapplications.IdMembership "
               + " WHERE IdTitle > 2 AND membership.AsOneApplication = 1 "
               + " AND " + SqlValidator.Validate_BETWEEN(ColumnNames.Date, dateFrom, dateTo).Replace("Date", "alldate.Date");

            returnCount += Convert.ToInt32(DataBase.GetListStringFromExecuteReader(contentCommand, "countIdTitle")[0]);
            return returnCount.ToString();
        }
    }
}
