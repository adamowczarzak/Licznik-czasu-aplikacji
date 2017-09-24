using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ApplicationTimeCounter
{
    class ActivityUser
    {
        Point prewCoordinates;
        AllData_db allData_db;

        public ActivityUser()
        {
            prewCoordinates = new Point(0, 0);
            allData_db = new AllData_db();
        }

        public bool UserIsActive()
        {
            bool userIsActive;
            System.Drawing.Point point = Control.MousePosition;
            if (prewCoordinates.X != point.X || prewCoordinates.Y != point.Y)userIsActive = true;          
            else userIsActive = false;
            prewCoordinates.X = point.X;
            prewCoordinates.Y = point.Y;        
            return userIsActive;
        }

        public bool CheckIfIsNextDay()
        {
            if (allData_db.CheckIfIsNextDay() == true) return true;
            else return false;
        }

        public void CheckIfIsActualDataInBaseAndUpdate()
        {
            allData_db.CheckIfIsActualDataInBaseAndUpdate();
        }


    }
}
