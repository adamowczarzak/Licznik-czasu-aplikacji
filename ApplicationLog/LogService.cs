using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLog
{
    public class LogService
    {
        /// <summary>
        /// Metoda dodaje wpis błędu konwersji do pliku loga.
        /// </summary>
        /// <param name="statement">Komunikat błędu.</param>
        /// <param name="nameMethod">Nazwa metody w której występuje błąd.</param>
        /// <param name="nameType">Typ na któy nie udało się przekonwertować wartości</param>
        /// <param name="path">Ścieżka do pliku w którym występił błąd</param>
        public static void AddRaportConvert(string statement, string nameMethod, string nameType, string path)
        {
                CheckIfVariablesIsCorrect(ref statement, ref nameMethod, ref nameType, ref path);
                string raport = statement + Environment.NewLine + "Wartość metody '" + nameMethod + "' nie może byś konwertowana do wartości typu '" + nameType + "'." +
                    Environment.NewLine + path + Environment.NewLine + DateTime.Now + Environment.NewLine;
                SaveRaportInFileLog(raport);
        }

        /// <summary>
        /// Metoda dodaje wpis przechwycenia wyjątku do pliku loga.
        /// </summary>
        /// <param name="statement">Komunikat błędu.</param>
        /// <param name="catchException">Przechwycony komunikat.</param>
        public static void AddRaportCatchException(string statement, string catchException)
        {
            CheckIfVariablesIsCorrect(ref statement, ref catchException);
            string raport = statement + Environment.NewLine + catchException + Environment.NewLine + DateTime.Now + Environment.NewLine;
            SaveRaportInFileLog(raport);
        }

        /// <summary>
        /// Metoda dodaje wpis informacyjny do pliku log.
        /// </summary>
        /// <param name="statement">Treś komunikatu.</param>
        public static void AddRaportInformation(string statement)
        {
            if (string.IsNullOrEmpty(statement)) statement = "Brak zawartości";
            string raport = statement + Environment.NewLine + DateTime.Now + Environment.NewLine;
            SaveRaportInFileLog(raport);
        }

        public static void AddRaportTest(string statement)
        {
            SaveRaportInFileLog(statement);
        }

        /// <summary>
        /// Pobiera nazwę metody w której obecnie wykonuje się kod. 
        /// </summary>
        /// <returns>Zwraca nazwę obecnej metody jako string.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetNameCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        /// <summary>
        /// Zapis raportu do pliku loga.
        /// </summary>
        /// <param name="contentRaportToSave">Treść raportu do zapisu.</param>
        private static void SaveRaportInFileLog(string contentRaportToSave)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Log.txt", true))
            {
                file.WriteLine(contentRaportToSave);
            }
        }

        /// <summary>
        /// Sprawdza czy zmienne nie są puste, jeśli są uzupełnia jest informacją o tym że są puste.
        /// </summary>
        private static void CheckIfVariablesIsCorrect(ref string var1, ref string var2, ref string var3, ref string var4)
        {
            if (string.IsNullOrEmpty(var1)) var1 = "Brak zawartości";
            if (string.IsNullOrEmpty(var2)) var2 = "Brak zawartości";
            if (string.IsNullOrEmpty(var3)) var3 = "Brak zawartości";
            if (string.IsNullOrEmpty(var4)) var4 = "Brak zawartości";
        }

        /// <summary>
        /// Sprawdza czy zmienne nie są puste, jeśli są uzupełnia jest informacją o tym że są puste.
        /// </summary>
        private static void CheckIfVariablesIsCorrect(ref string var1, ref string var2)
        {
            if (string.IsNullOrEmpty(var1)) var1 = "Brak zawartości";
            if (string.IsNullOrEmpty(var2)) var2 = "Brak zawartości";
        }

        public static void Main()
        {

        }

    }
}
