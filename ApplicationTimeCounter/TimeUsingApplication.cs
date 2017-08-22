using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationTimeCounter
{
    class TimeUsingApplication
    {
        private MyLabel timeOfAplication;
        private MyLabel totalTime;
        private DailyUseOfApplication_db dailyUseOfApplication_db;

        public TimeUsingApplication(Canvas canvas)
        {
            MyLabel title = new MyLabel(canvas, "Czas użycia", 100, 30, 14,  0, 0);
            MyLabel label1 = new MyLabel(canvas, "Całkowity", 80, 30, 12, 0, 100);
            MyLabel label2 = new MyLabel(canvas, "Aplikacji",80, 30, 12,  0, 50);
            timeOfAplication = new MyLabel(canvas, "0 h 0 min", 110, 35, 18, 70, 65);
            totalTime = new MyLabel(canvas, "0 h 0 min", 110, 35, 18, 70, 110);
            MyRectangle line = new MyRectangle(canvas, 170, 1, "DarkSlateGray", 10, 100);

            dailyUseOfApplication_db = new DailyUseOfApplication_db();
        }

        public void Update(string title)
        {
            int timeApp = dailyUseOfApplication_db.GetTimeForTitle(title);
            int timeAllApp = dailyUseOfApplication_db.GetTimeForAllTitle();

            if (timeApp > 0) timeOfAplication.SetContent(GetTimeInString(timeApp));
            if(timeAllApp > 0)totalTime.SetContent(GetTimeInString(timeAllApp));
        }

        private string GetTimeInString(int time)
        {
            int minutes = 0, hours = 0;
            hours = time / 60;
            minutes = time - (hours*60);

            return hours + " h " + minutes + " min ";
        }
    }
}
