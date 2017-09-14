using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ApplicationTimeCounter
{
    class AssignedActivity
    {
        Canvas mainCanvas;
        Canvas nonAssignedApplications;
        Label buttonCloseCanvas;
        Canvas canvas;

        public AssignedActivity(ref Canvas canvas)
        {
            this.canvas = canvas;
            mainCanvas = CanvasCreator.CreateCanvas(canvas, 620, 410, Color.FromArgb(255, 226, 240, 255), 0, 0);
            nonAssignedApplications = new Canvas() { Width = 600, Height = 300, };

            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(mainCanvas, 560, 300, 30, 60, nonAssignedApplications);
            MyLabel labelNonAssignedApplication = new MyLabel(mainCanvas, "Aplikacje bez przypisanej aktywności",
                370, 38, 18, 30, 10, Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255));
            labelNonAssignedApplication.SetFont("Verdana");
            MyRectangle r = new MyRectangle(mainCanvas, 560, 4, Color.FromArgb(255, 220, 220, 220), 30, 45);

            buttonCloseCanvas = ButtonCreator.CreateButton(mainCanvas, "Zamknij", 80, 30, 13, 510, 370,
                Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255), 1);
            buttonCloseCanvas.MouseLeftButtonDown += buttonCloseCanvas_MouseLeftButtonDown;


            UpdateContent();

        }


        void buttonCloseCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainCanvas.Children.RemoveRange(0, mainCanvas.Children.Count);
            this.canvas.Children.Remove(mainCanvas);

        }

        void UpdateContent()
        {
            LoadNonAssignedApplication();
        }

        void LoadNonAssignedApplication()
        {
            AllData_db allData_db = new AllData_db();
            DailyUseOfApplication_db dailyApplication = new DailyUseOfApplication_db();
            ActiveApplication parameters = new ActiveApplication();
            parameters.IdNameActivity = 0;
            List<ActiveApplication> titlesAllNotAssignedApplication = allData_db.GetActiveApplication(parameters);
            titlesAllNotAssignedApplication.AddRange(dailyApplication.GetActiveApplication(parameters));
            titlesAllNotAssignedApplication.Reverse();

            for (int i = 0; i < titlesAllNotAssignedApplication.Count; i++)
            {
                Canvas nonAssignedAppCanvas = CanvasCreator.CreateCanvas(nonAssignedApplications, 560, 60, Color.FromArgb(0, 0, 0, 0), 0, 59 * i);

                string titleApplication = string.Empty;
                titleApplication = (titlesAllNotAssignedApplication[i].Title.Length > 40) ?
                    titlesAllNotAssignedApplication[i].Title.Remove(40, titlesAllNotAssignedApplication[i].Title.Length - 40) : titlesAllNotAssignedApplication[i].Title;

                MyCircle circle = new MyCircle(nonAssignedAppCanvas, 46, 2, Color.FromArgb(255, 150, 150, 150), 8, 8, 1, true);
                MyLabel nonAssignedApplication = new MyLabel(nonAssignedAppCanvas, "\t" + titleApplication,
                    560, 60, 14, 0, 0, Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100), 1,
                HorizontalAlignment.Left, fontWeight: FontWeights.Bold);

                MyLabel lab = new MyLabel(nonAssignedAppCanvas, "B", 50, 50, 20, 6, 11, Color.FromArgb(255, 240, 240, 240),
                    Color.FromArgb(0, 100, 100, 100), 0, HorizontalAlignment.Center, fontWeight: FontWeights.ExtraBold);

                MyLabel membership = new MyLabel(nonAssignedAppCanvas, "Brak przynależności", 300, 30, 12, 60, 30,
                    Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);

                MyLabel dayAgo = new MyLabel(nonAssignedAppCanvas, GetNumberDayAgo(titlesAllNotAssignedApplication[i].Date), 100, 30, 13, 466, 0, Color.FromArgb(255, 120, 120, 120),
                    Color.FromArgb(30, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);

                MyLabel numberApplication = new MyLabel(nonAssignedAppCanvas, "(" + (titlesAllNotAssignedApplication.Count - i) + ")", 100, 30, 9, 420, 0, Color.FromArgb(255, 120, 120, 120),
                    Color.FromArgb(30, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);

                MyCircle Mycircle = new MyCircle(nonAssignedAppCanvas, 25, 1, (Color.FromArgb(255, 0, 123, 255)), 525, 28, setFill: true);

                Label buttonAddActivity = ButtonCreator.CreateButton(nonAssignedAppCanvas, "+", 25, 34, 20, 525, 28,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 0, 0), 0, fontWeight: FontWeights.ExtraBold);
                buttonAddActivity.Margin = new Thickness(0, -8, 0, 0);
                buttonAddActivity.MouseLeftButtonDown += buttonAddActivity_MouseLeftButtonDown;
                buttonAddActivity.Name = "ID_" + titlesAllNotAssignedApplication[i].ID;


                nonAssignedApplications.Height += 59;
            }
            nonAssignedApplications.Height = ((nonAssignedApplications.Height - 300) < 300) ? 300 : nonAssignedApplications.Height - 299;
        }

        private void buttonAddActivity_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label btn = (Label)sender;
            AddActivity addActivity = new AddActivity();
            var location = btn.PointToScreen(new Point(0, 0));
            addActivity.Left = location.X + 16;
            addActivity.Top = location.Y + 20;
            addActivity.ShowDialog();
        }

        private string GetNumberDayAgo(string dateAgo)
        {
            DateTime dateApplication = DateTime.Parse(dateAgo);
            TimeSpan wynik = DateTime.Now - dateApplication;
            string returnValue = string.Empty;
            if (wynik.Days.ToString() == "0") returnValue = "Dziś";
            else if (wynik.Days.ToString() == "1") returnValue = "1 dzień temu";
            else returnValue = wynik.Days.ToString() + " dni temu";
            return returnValue;
        }
    }
}
