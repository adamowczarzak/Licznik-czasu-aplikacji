using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using ApplicationTimeCounter.Other;

namespace ApplicationTimeCounter
{
    class TimeUsingApplication
    {
        private MyLabel timeOfAplication;
        private MyLabel totalTime;
        private DailyUseOfApplication_db dailyUseOfApplication_db;

        public TimeUsingApplication(Canvas canvas)
        {
            MyLabel title = new MyLabel(canvas, "Czas użycia", 100, 30, 14, 0, 0, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            MyLabel label1 = new MyLabel(canvas, "Całkowity", 80, 30, 12, 0, 100, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            MyLabel label2 = new MyLabel(canvas, "Aplikacji", 80, 30, 12, 0, 50, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            timeOfAplication = new MyLabel(canvas, "0 h 0 min", 110, 35, 18, 70, 65, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            totalTime = new MyLabel(canvas, "0 h 0 min", 110, 35, 18, 70, 110, Color.FromArgb(255, 47, 79, 79), Color.FromArgb(0, 0, 0, 0));
            MyRectangle line = new MyRectangle(canvas, 170, 1, Color.FromArgb(255, 47, 79, 79), 10, 100);

            dailyUseOfApplication_db = new DailyUseOfApplication_db();
        }

        public void Update(string title)
        {
            int timeApp = dailyUseOfApplication_db.GetTimeForTitle(title);
            int timeAllApp = dailyUseOfApplication_db.GetTimeForAllTitle();

            if (timeApp > 0) timeOfAplication.SetContent(ActionOnTime.GetTime(timeApp));
            if (timeAllApp > 0) totalTime.SetContent(ActionOnTime.GetTime(timeAllApp));
        }
    }
}
