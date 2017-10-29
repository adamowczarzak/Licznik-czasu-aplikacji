using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ApplicationTimeCounter
{
    public class ActiveWindow
    {

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);


        public string GetNameActiveWindow()
        {
            const int nChars = 256;
            string nameActiveWindow = "Brak aktywnego okna";
            IntPtr handle = GetForegroundWindow();
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                nameActiveWindow = Buff.ToString();               
            }
            return nameActiveWindow.Replace("'","");
        }
    }
}
