using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    class RunApplication
    {
        private bool runApllication = false;

        public RunApplication()
        {
            if (!System.IO.File.Exists("Config.file"))
            {
                GetDataBaseWindow getDataBaseWindow = new GetDataBaseWindow();
                getDataBaseWindow.ShowDialog();
                runApllication = getDataBaseWindow.CanRunApplication;
            }
            else
            {
                if (LoadConfigFileAndTryConnectMySql())
                {
                    if (DataBase.ConnectToDataBase())
                    {
                        DataBase.CloseConnection();
                        runApllication = true;
                    }
                    else
                    {
                        //próba stworzenie bazy danych
                    }
                }
                else
                {
                    // chyba trzeba będzie wywalić okno logowania.
                }
                // sprawdzenie czy łączy się za bazą
                // tak -> włącz program
                // nie -> komunikat i okono logowania
            }   
        }

        public bool CheckIfRunApplication()
        {
            return runApllication;
        }

        private bool LoadConfigFileAndTryConnectMySql()
        {
            string nameUser = string.Empty;
            string password = string.Empty;
            bool isLoadAndConnect = false;

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("Config.file"))
                {
                    nameUser = file.ReadLine();
                    password = file.ReadLine();
                }
            }
            catch (Exception)
            {
                ApplicationLog.LogService.AddRaportError("Error !!!\tNie udało się otworzyć pliku konfiguracyjnego.",
                    ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                    System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\RunApplication.cs");
            }

            isLoadAndConnect = DataBase.TryConnectToMySql(nameUser, password);
            return isLoadAndConnect;
        }
    }

    


}
