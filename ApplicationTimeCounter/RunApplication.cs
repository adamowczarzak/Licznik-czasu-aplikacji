using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    class RunApplication
    {
        public RunApplication()
        {
            if (!System.IO.File.Exists("Config.file"))
            {
                GetDataBaseWindow getDataBaseWindow = new GetDataBaseWindow();
                getDataBaseWindow.ShowDialog();
            }
            else
            {
                // sprawdzenie czy łączy się za bazą
                // tak -> włącz program
                // nie -> komunikat i okono logowania
            }
                
        }
    }

    


}
