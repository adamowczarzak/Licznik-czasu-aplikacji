using ApplicationTimeCounter.Controls;
using ApplicationTimeCounter.Other;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace ApplicationTimeCounter
{
    class JoinApplications
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas applicationsWithoutGroup;
        private Label buttonCloseCanvas;
        private Canvas applicationWithoutGroup;
        private BackgroundWorker backgroundWorkerLoadingWindow;
        private LoadingWindow loadingWindow;

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowAssignedActivityDelegate;

        public JoinApplications(ref Canvas canvas)
        {
            this.canvas = canvas;
            mainCanvas = CanvasCreator.CreateCanvas(canvas, 620, 410, Color.FromArgb(255, 226, 240, 255), 0, 0);
            applicationsWithoutGroup = new Canvas() { Width = 600, Height = 300, };

            loadingWindow = new LoadingWindow(ref this.canvas);
            backgroundWorkerLoadingWindow = new BackgroundWorker();
            backgroundWorkerLoadingWindow.DoWork += loadingWindow.Load;
            backgroundWorkerLoadingWindow.RunWorkerAsync();

            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(mainCanvas, 560, 300, 30, 60, applicationsWithoutGroup);
            MyLabel labelNonAssignedApplication = new MyLabel(mainCanvas, "Aplikacje bez przypisanej grupy",
                370, 38, 18, 30, 10, Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255));
            labelNonAssignedApplication.SetFont("Verdana");

            buttonCloseCanvas = ButtonCreator.CreateButton(mainCanvas, "Zamknij", 80, 30, 13, 510, 370,
                Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255), 1);
            buttonCloseCanvas.MouseLeftButtonDown += buttonCloseCanvas_MouseLeftButtonDown;

            var backgroundWorkerUpdateContent = new BackgroundWorker();
            backgroundWorkerUpdateContent.DoWork += UpdateContent;
            backgroundWorkerUpdateContent.RunWorkerAsync();
        }

        private void UpdateContent(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LoadNonAssignedApplication();
        }

        private void buttonCloseCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainCanvas.Children.RemoveRange(0, mainCanvas.Children.Count);
            this.canvas.Children.Remove(mainCanvas);
            CloseWindowAssignedActivityDelegate();
        }

        private void LoadNonAssignedApplication()
        {
            List<ActiveApplication> titlesAllNotAssignedApplication = ActiveApplication_db.GetNonJoinedApplication();

            for (int i = 0; i < titlesAllNotAssignedApplication.Count; i++)
            {
                Thread.Sleep(10);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    applicationWithoutGroup = new Canvas();
                    applicationWithoutGroup = CanvasCreator.CreateCanvas(applicationsWithoutGroup, 560, 60,
                        Color.FromArgb(0, 110, 0, 0), 0, 59 * i);
                });
                string titleApplication = string.Empty;
                titleApplication = (titlesAllNotAssignedApplication[i].Title.Length > 40) ?
                    titlesAllNotAssignedApplication[i].Title.Remove(40, titlesAllNotAssignedApplication[i].Title.Length - 40) : titlesAllNotAssignedApplication[i].Title;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    new MyCircle(applicationWithoutGroup, 46, 2, Color.FromArgb(255, 150, 150, 150), 8, 8, 1, true);

                    MyLabel l = new MyLabel(applicationWithoutGroup, "\t" + titleApplication,
                        560, 60, 14, 0, 0, Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100), 1,
                        HorizontalAlignment.Left, fontWeight: FontWeights.Bold);
                    l.ToolTip(titlesAllNotAssignedApplication[i].Title);

                    new MyLabel(applicationWithoutGroup, ActionOnString.GetFirstLetterFromString(titleApplication), 50, 50, 20, 6, 11, Color.FromArgb(255, 240, 240, 240),
                        Color.FromArgb(0, 100, 100, 100), 0, HorizontalAlignment.Center, fontWeight: FontWeights.ExtraBold);

                    new MyLabel(applicationWithoutGroup, titlesAllNotAssignedApplication[i].NameActivity, 300, 30, 12, 60, 30,
                        Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);

                    new MyLabel(applicationWithoutGroup, ActionOnTime.GetNumberDayAgo(titlesAllNotAssignedApplication[i].Date), 100, 30, 13, 466, 0, Color.FromArgb(255, 120, 120, 120),
                    Color.FromArgb(30, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);

                    new MyLabel(applicationWithoutGroup, "(" + (titlesAllNotAssignedApplication.Count - i) + ")", 100, 30, 9, 420, 0, Color.FromArgb(255, 120, 120, 120),
                    Color.FromArgb(30, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);

                    new MyCircle(applicationWithoutGroup, 25, 1, (Color.FromArgb(255, 0, 123, 255)), 525, 28, setFill: true);

                    Label buttonAddActivity = ButtonCreator.CreateButton(applicationWithoutGroup, "+", 25, 34, 20, 525, 28,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 0, 0), 0, fontWeight: FontWeights.ExtraBold);
                    buttonAddActivity.Margin = new Thickness(0, -8, 0, 0);
                    applicationWithoutGroup.Name = "ID_" + titlesAllNotAssignedApplication[i].ID;
                    buttonAddActivity.MouseLeftButtonDown += buttonAddActivity_MouseLeftButtonDown;
                    applicationsWithoutGroup.Height += 59;
                });
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                applicationsWithoutGroup.Height = ((applicationsWithoutGroup.Height - 300) < 300) ? 300 : applicationsWithoutGroup.Height - 299;
            });
            loadingWindow.notClose = false;
            Thread.Sleep(200);
            backgroundWorkerLoadingWindow.DoWork -= loadingWindow.Load;
            backgroundWorkerLoadingWindow.DoWork += loadingWindow.Close;
            backgroundWorkerLoadingWindow.RunWorkerAsync();
        }

        private void buttonAddActivity_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label btn = (Label)sender;
            if (string.Equals(btn.Content, "+"))
            {
                WindowAddTo windowAddTo = new WindowAddTo((Canvas)btn.Parent, AddTo.Group);
                var location = btn.PointToScreen(new Point(0, 0));
                windowAddTo.Left = location.X + 16;
                windowAddTo.Top = location.Y + 20;
                windowAddTo.ShowDialog();
            }
            else
            {
                WindowAddTo windowAddTo = new WindowAddTo((Canvas)btn.Parent, true, AddTo.Group);
            }
        }
    }
}
