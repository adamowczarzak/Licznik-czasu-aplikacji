using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ApplicationTimeCounter
{
    class BarUsingApplication
    {
        private CircleBar circleBar;
        private MyLabel valueBarUsingApplication;
        private DailyUseOfApplication_db dailyUseOfApplication_db;

        public BarUsingApplication(Canvas canvas)
        {
            circleBar = new CircleBar(canvas, 0.09, "DarkGreen", 4, 0, 15, 50);
            valueBarUsingApplication = new MyLabel(canvas, "-", 74, 40, 22,
                            canvas.Width / 2 - 33, canvas.Height / 2 - 5, "DarkGreen");

            MyLabel title = new MyLabel(canvas, "Aktywna aplikacja", 140, 30, 14, 0, 0);
            dailyUseOfApplication_db = new DailyUseOfApplication_db();
        }

        public void Update(string title)
        {
            double timeApp = dailyUseOfApplication_db.GetTimeForTitle(title);
            double timeAllApp = dailyUseOfApplication_db.GetTimeForAllTitle();
            if (timeAllApp > 0)
            {
                circleBar.SetValue((timeApp / timeAllApp));
                valueBarUsingApplication.SetContent((int)((timeApp / timeAllApp) * 100) + " %");
            }
            
        }

       
    }
}
