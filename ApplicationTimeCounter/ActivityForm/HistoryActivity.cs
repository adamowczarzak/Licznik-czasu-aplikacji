using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using System.Collections.Generic;
using ApplicationTimeCounter.ApplicationObjectsType;


namespace ApplicationTimeCounter
{
    class HistoryActivity
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas contentCanvas;
        private Canvas chartCanvas;
        private DatePicker datePicker;
        private ScrollViewer sv;
        private MyLabel[] scaleLabel;

        private AllData_db allData_db;

        public HistoryActivity(ref Canvas canvas)
        {
            this.canvas = canvas;
            allData_db = new AllData_db();
            mainCanvas = CanvasCreator.CreateCanvas(canvas, 620, 410, Color.FromArgb(255, 226, 240, 255), 0, 0);
            contentCanvas = CanvasCreator.CreateCanvas(mainCanvas, 620, 360, Color.FromArgb(255, 236, 236, 236), 0, 50);
            new MyRectangle(mainCanvas, 620, 1, Color.FromArgb(60, 110, 110, 110), 0, 50);

            MyLabel l = new MyLabel(mainCanvas, "Historia aktywności", 200, 38, 18, 30, 10, Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255));
            l.SetFont("Verdana");


            new MyLabel(mainCanvas, "x", 30, 50, 24, 590, 10, Color.FromArgb(255, 70, 70, 70),
                Color.FromArgb(255, 70, 70, 70), 0);
            Label buttonCloseActivityHistory = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 590, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonCloseActivityHistory.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonCloseActivityHistory.MouseEnter += buttonCloseActivityHistory_MouseEnter;
            buttonCloseActivityHistory.MouseLeave += buttonCloseActivityHistory_MouseLeave;
            buttonCloseActivityHistory.MouseLeftButtonDown += buttonCloseActivityHistory_MouseLeftButtonDown;

            CreateDatePickier();
            CreateChart();
            
        }

        private void CreateChart()
        {
            chartCanvas = new Canvas() { Width = 500, Height = 320, Background = new SolidColorBrush(Color.FromArgb(0, 236, 200, 200)) };
            sv = ScrollViewerCreator.CreateScrollViewer(contentCanvas, 500, 350, 70, 10, chartCanvas);
            scaleLabel = new MyLabel[5];

            new MyRectangle(contentCanvas, 500, 1, Color.FromArgb(80, 110, 110, 110), 70, 300);
            new MyRectangle(contentCanvas, 1, 280, Color.FromArgb(80, 110, 110, 110), 70, 20);
            
            for (int i = 0; i < 4; i++ )
            {
                scaleLabel[i] = new MyLabel(contentCanvas, "0", 40, 30, 16, 24, 210 - (60 * i), Color.FromArgb(180, 150, 150, 150), Color.FromArgb(0, 20, 20, 20));
                new MyRectangle(contentCanvas, 30, 1, Color.FromArgb(100, 150, 150, 150), 40, 240 - (60 * i));
            }
            new MyLabel(contentCanvas, "[%]", 40, 30, 16, 30, 0, Color.FromArgb(180, 150, 150, 150), Color.FromArgb(180, 150, 150, 150));       
        }

        private void UpdateChart(List<Activity> dailyActivity)
        {
            chartCanvas.Children.Clear();
            int maxScale = dailyActivity.Select(x => x.ActivityTime).Max();
            int sumScale = dailyActivity.Select(x => x.ActivityTime).Sum();
            for (int i = 0; i < 4; i++)
            {
                scaleLabel[i].SetContent(((((maxScale * 100) / sumScale) / 4) * (i + 1)).ToString());
            }

            for (int i = 0; i < dailyActivity.Count; i++)
            {
                new MyRectangle(chartCanvas, 25, ((dailyActivity[i].ActivityTime * 235) / maxScale), Color.FromArgb(200, 0, 125, 250), 30 + (70 * i),
                    272 - ((dailyActivity[i].ActivityTime * 235) / maxScale));
                string sa = dailyActivity[i].Name;
                new MyLabel(chartCanvas, sa , 70, 50, 10, 10 + (70 * i), 275, Color.FromArgb(255, 100, 100, 100), Color.FromArgb(200, 200, 0, 0), 1);
            }

            if (chartCanvas.Width > sv.Width)
                sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        private void CreateDatePickier()
        {
            datePicker = new DatePicker();
            datePicker.DisplayDateStart = DateTime.Now.AddDays(Convert.ToInt32(allData_db.GetDayWorkingApplication()) + 1);
            datePicker.DisplayDateEnd = DateTime.Now.AddDays(-1);

            mainCanvas.Children.Add(datePicker);
            Canvas.SetLeft(datePicker, 370);
            Canvas.SetTop(datePicker, 15);

            Label buttonShowActivityHistory = ButtonCreator.CreateButton(mainCanvas, "OK", 30, 25, 10, 486, 15,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(250, 0, 123, 255), 1);
            buttonShowActivityHistory.Background = new SolidColorBrush(Color.FromArgb(125, 0, 123, 255));
            buttonShowActivityHistory.MouseEnter += buttonShowActivityHistory_MouseEnter;
            buttonShowActivityHistory.MouseLeave += buttonShowActivityHistory_MouseLeave;
            buttonShowActivityHistory.MouseLeftButtonDown += buttonShowActivityHistory_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonShowActivityHistory, "Wyszukaj aktywności dla wybranego dnia");

        }

        private void buttonShowActivityHistory_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            List<Activity> dailyActivity = GetDailyActivity();
            UpdateChart(dailyActivity);
        }

        private List<Activity> GetDailyActivity()
        {
            CommandParameters commandParameters = new CommandParameters();
            commandParameters.StartDate = commandParameters.EndDate = datePicker.SelectedDate.ToString();
            List<Activity> dailyActivity = allData_db.GetDailyActivity(commandParameters).OrderBy(x => x.ActivityTime).ToList();

            Activity activity = new Activity();
            activity.ActivityTime = Convert.ToInt32(allData_db.GetTimeActivityForDateAndIdActivity(datePicker.SelectedDate.ToString(), 2));
            activity.Name = "Brak akty. użytkownika";
            dailyActivity.Add(activity);

            activity = new Activity();
            activity.ActivityTime = Convert.ToInt32(allData_db.GetTimeActivityForDateAndIdActivity(datePicker.SelectedDate.ToString(), 1));
            activity.Name = "Wyłączony komputer";
            dailyActivity.Add(activity);

            dailyActivity.Reverse();

            return dailyActivity;
        }

        private void buttonShowActivityHistory_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(125, 0, 123, 255));
        }

        private void buttonShowActivityHistory_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 177, 255));
        }

        void buttonCloseActivityHistory_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            chartCanvas.Children.Clear();
            contentCanvas.Children.Clear();
            mainCanvas.Children.Clear();
            this.canvas.Children.Remove(mainCanvas);
        }

        void buttonCloseActivityHistory_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
        }

        void buttonCloseActivityHistory_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 177, 255));
        }

    }
}
