using System;
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;


namespace ApplicationTimeCounter
{
    class HistoryActivity
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas contentCanvas;
        private Canvas chartCanvas;
        private DatePicker datePicker;

        private AllData_db allData_db;

        public HistoryActivity(ref Canvas canvas)
        {
            this.canvas = canvas;
            allData_db = new AllData_db();
            mainCanvas = CanvasCreator.CreateCanvas(canvas, 620, 410, Color.FromArgb(255, 226, 240, 255), 0, 0);
            contentCanvas = CanvasCreator.CreateCanvas(mainCanvas, 620, 340, Color.FromArgb(255, 236, 236, 236), 0, 50);

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
            // szukanie
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
            //chartCanvas.Children.Clear();
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
