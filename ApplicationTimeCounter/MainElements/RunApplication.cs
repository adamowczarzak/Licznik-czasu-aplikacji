using System;

namespace ApplicationTimeCounter
{
    public class RunApplication
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
                        ErrorWindow errorWindow = new ErrorWindow();
                        errorWindow.DisplayErrorConnectToDataBase();
                        errorWindow.ShowDialog();                    
                    }
                }
                else
                {
                    ErrorWindow errorWindow = new ErrorWindow();
                    errorWindow.DisplayErrorConnectToMySql();
                    errorWindow.ShowDialog();
                }
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
            bool isLoadAndConnect = true;

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
                ApplicationLog.LogService.AddRaportError("Nie udało się otworzyć pliku konfiguracyjnego.",
                    ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                    System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\RunApplication.cs");
                isLoadAndConnect = false;
            }
            if(isLoadAndConnect)
                isLoadAndConnect = DataBase.TryConnectToMySql(nameUser, password);
            return isLoadAndConnect;
        }
    }

    


}
