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
        public bool Add(string title, int activityTime, int idNameActivity, int daysDifferenceFromToday = 1,
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
                    command.CommandText = @"INSERT INTO alldate (Date, Title, ActivityTime, idNameActivity) VALUES (CURDATE() - INTERVAL " + daysDifferenceFromToday + " DAY" +
                    " , '" + title + "' , '" + activityTime + "' , '" + idNameActivity + "' )";
                    if (DataBase.ExecuteNonQuery(command)) addRecord = true;
                }
                if (closingConnectionDataBase) DataBase.CloseConnection();
            }
            else
            {
                DataBase.AdditionalConnectToDataBase();
                command.Connection = DataBase.AdditionalConnection;
                command.CommandText = @"INSERT INTO alldate (Date, Title, ActivityTime, idNameActivity) VALUES (CURDATE() - INTERVAL " + daysDifferenceFromToday + " DAY" +
                " , '" + title + "' , '" + activityTime + "' , '" + idNameActivity + "' )";
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
            string contentCommand = "SELECT id from alldate WHERE Date = CURDATE() - INTERVAL 1 DAY OR Date = CURDATE()";
            if (!GetListStringFromExecuteReader(contentCommand, "id").Any()) return true;
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
                        if (Add("Wył. komputer", 60 * 24, -1, -i, true, true))
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
        /// Pobiera ilość aplikacji które nie mają przypisane żadnej altywności.
        /// </summary>
        /// <returns></returns>
        internal List<ActiveApplication> GetActiveApplication(ActiveApplication parameters)
        {
            List<ActiveApplication> activeApplication = new List<ActiveApplication>();
            string query = "SELECT id, Date , Title, ActivityTime, idNameActivity from alldate WHERE 1 = 1";
            if (parameters.ID > 0) query += " AND id = " + parameters.ID;
            if (!string.IsNullOrEmpty(parameters.Date)) query += " AND Date = " + parameters.Date;
            if (!string.IsNullOrEmpty(parameters.Title)) query += " AND Title = " + parameters.Title;
            if (parameters.ActivityTime > 0) query += " AND ActivityTime = " + parameters.ActivityTime;
            if (parameters.IdNameActivity != -3) query += " AND idNameActivity = " + parameters.IdNameActivity;
            //query += " ORDER BY id desc";
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ActiveApplication application = new ActiveApplication();
                        application.ID = Int32.Parse((reader["id"]).ToString());
                        application.Title = (reader["Title"]).ToString();
                        application.ActivityTime = Int32.Parse((reader["ActivityTime"]).ToString());
                        application.Date = (reader["Date"]).ToString();
                        application.IdNameActivity = Int32.Parse((reader["idNameActivity"]).ToString());
                        activeApplication.Add(application);
                    }
                    catch (MySqlException message)
                    {
                        ApplicationLog.LogService.AddRaportCatchException("Error\tZapytanie nie zwróciło żadnej wartości.", message);
                    }
                }
                DataBase.CloseConnection();
                reader.Dispose();
            }
            return activeApplication;
        }

        public string GetAllNotAssignedApplication()
        {
            string contentCommand = "SELECT COUNT(*) as noAssigmentApplication from alldate WHERE idNameActivity = 0";
            string AllNotAssignedApplication = GetListStringFromExecuteReader(contentCommand, "noAssigmentApplication")[0];
            return AllNotAssignedApplication;
        }

        /// <summary>
        /// Pobiera ilość dni pracy aplkacji.
        /// </summary>
        /// <returns>Zwraca ilość dni jako string.</returns>
        public string GetDayWorkingApplication()
        {
            string contentCommand = "SELECT DATEDIFF('" + GetDataRunApplication() + "', CURDATE()) as dateDifference";
            string dateDifference = GetListStringFromExecuteReader(contentCommand, "dateDifference")[0];
            return dateDifference;
        }

        /// <summary>
        /// Pobiera date uruchomienia aplikacji.
        /// </summary>
        /// <returns>Zwraca date jako string.</returns>
        private string GetDataRunApplication()
        {
            string contentCommand = "SELECT Date FROM alldate ORDER BY Date ASC LIMIT 1";
            string dataRunApplication = GetListStringFromExecuteReader(contentCommand, "Date")[0];
            return dataRunApplication;
        }

        /// <summary> 
        /// Metoda zwraca jeden wynik zapytania w postaci listy stringów. 
        /// </summary> 
        /// <param name="contentCommand">Cała zawartość zapytania.</param>
        /// <param name="nameReturnColumn">Nazwa kolumny z której ma być zwracana wartość.</param>
        private List<string> GetListStringFromExecuteReader(string contentCommand, string nameReturnColumn)
        {
            List<string> returnList = new List<string>();
            if (DataBase.ConnectToDataBase())
            {
                command.Connection = DataBase.Connection;
                command.CommandText = contentCommand;
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

        /// <summary>
        /// Sprawdza czy date istnieje w bazie.
        /// </summary>
        /// <param name="numberDayBack">Ilość dni na minusie wstecz od dzieśejszej daty.</param>
        /// <returns></returns>
        private bool CheckIfDateExistInBase(string numberDayBack = "0")
        {
            string contentCommand = "SELECT Date FROM alldate WHERE Date = CURDATE() - INTERVAL " + numberDayBack + " DAY LIMIT 1";
            if (!GetListStringFromExecuteReader(contentCommand, "Date").Any()) return false;
            else return true;
        }
    }
}
