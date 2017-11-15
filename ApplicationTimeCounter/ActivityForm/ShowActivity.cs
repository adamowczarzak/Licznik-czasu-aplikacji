using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using ApplicationTimeCounter.Controls;
using ApplicationTimeCounter.Other;
using ApplicationTimeCounter.AdditionalWindows;

namespace ApplicationTimeCounter
{
    class ShowActivity
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas contentCanvas;
        private Canvas applicationInActivity;
        private MyLabel nameActivity;
        private MyRectangle[] charts;
        private MyLabel[] scaleLabel;
        private string[] nameDay;
        private MyLabel[] average;
        private MyLabel[] growth;
        private MyLabel[] time;
        private string[] namesActivity;
        private List<Label> footerActivity;
        private MyLabel[] footerActivityCounter;
        private int index;
        private int deleteApplicationID;
        private int viewActivityID;
        private bool isOnlyEditMode;
        private List<ActiveApplication> activeApplication;

        private TextBox nameEditActivity;
        private Label buttonSaveEditActivity;
        private Label buttonClose;

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowShowActivityDelegate;

        public ShowActivity(ref Canvas canvas)
        {
            this.canvas = canvas;

            mainCanvas = CanvasCreator.CreateCanvas(canvas, 620, 410, Color.FromArgb(255, 226, 240, 255), 0, 0);
            this.canvas.KeyDown += mainCanvas_KeyDown;
            contentCanvas = CanvasCreator.CreateCanvas(mainCanvas, 620, 320, Color.FromArgb(255, 236, 236, 236), 0, 50);

            new MyRectangle(mainCanvas, 620, 1, Color.FromArgb(30, 110, 110, 110), 0, 50);
            new MyRectangle(mainCanvas, 620, 1, Color.FromArgb(50, 110, 110, 110), 0, 370);
            new MyRectangle(contentCanvas, 185, 294, Color.FromArgb(0, 10, 10, 10), 420, 15, 2);
            new MyRectangle(contentCanvas, 390, 294, Color.FromArgb(0, 10, 10, 10), 15, 15, 2);

            nameActivity = new MyLabel(mainCanvas, "Nazwa aktywnosci", 300, 36, 16, 15, 15, Color.FromArgb(255, 160, 160, 160),
                Color.FromArgb(0, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);
            nameActivity.SetFont("Verdana");

            namesActivity = NameActivity_db.GetNameActivityDictionary().Keys.ToArray();
            CreateControlUser();
            CreateChart();
            CreateListOfAddedApps();
            CreateTableWithInformation();
            CreateActivityFooter();
            index = 0;
            this.canvas.Focusable = true;
            this.canvas.Focus();
            Update();
        }

        private void Update()
        {
            ActiveApplication parameters = new ActiveApplication();
            parameters.NameActivity = namesActivity[index];
            activeApplication = ActiveApplication_db.GetActiveApplication(parameters);
            viewActivityID = NameActivity_db.GetIDForNameActivity(namesActivity[index]);
            if (activeApplication.Count > 100)
                activeApplication.RemoveRange(0, activeApplication.Count - 100);

            nameActivity.SetContent(namesActivity[index]);
            UpdateListOfAddedApps();
            UpdateChart();
            UpdateTableWithInformation();
            UpdateActivityFooter(index);
        }

        private void mainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (nameActivity.Visibility == Visibility.Visible)
            {
                if (e.Key == Key.Right)
                {
                    if (index < namesActivity.Length - 1)
                    {
                        CreateAndStartBackgroundWorker();
                        index++;
                        Update();
                    }
                }
                if (e.Key == Key.Left)
                {
                    if (index > 0)
                    {
                        CreateAndStartBackgroundWorker();
                        index--;
                        Update();
                    }
                }
            }
        }

        private void CreateAndStartBackgroundWorker()
        {
            BackgroundWorker backgroundWorkerLoadingWindow = new BackgroundWorker();
            backgroundWorkerLoadingWindow.DoWork += backgroundWorkerLoadingWindow_DoWork;
            backgroundWorkerLoadingWindow.RunWorkerAsync();
        }

        private void backgroundWorkerLoadingWindow_DoWork(object sender, DoWorkEventArgs e)
        {
            Canvas canvasLoad = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                canvasLoad = CanvasCreator.CreateCanvas(mainCanvas, (int)mainCanvas.Width, (int)mainCanvas.Height, Color.FromArgb(255, 255, 255, 255), 0, 0);
                canvasLoad.Opacity = 0;
            });

            for (int i = 0; i < 30; i++)
            {
                Application.Current.Dispatcher.Invoke(() => { canvasLoad.Opacity += 0.04; });
                Thread.Sleep(20);
            }
            for (int i = 0; i < 30; i++)
            {
                Application.Current.Dispatcher.Invoke(() => { canvasLoad.Opacity -= 0.04; });
                Thread.Sleep(20);
            }
            Application.Current.Dispatcher.Invoke(() => { mainCanvas.Children.Remove(canvasLoad); this.canvas.Focus(); });
        }

        private void CreateControlUser()
        {
            new MyLabel(mainCanvas, "+", 30, 50, 30, 530, 6, Color.FromArgb(255, 70, 70, 70),
                Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonAdd = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 530, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonAdd.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonAdd.MouseEnter += buttonAdd_MouseEnter;
            buttonAdd.MouseLeave += buttonAdd_MouseLeave;
            buttonAdd.MouseLeftButtonDown += buttonAdd_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonAdd, "Dodaj nową aktywność");

            new MyLabel(mainCanvas, "-", 30, 50, 30, 560, 6, Color.FromArgb(255, 70, 70, 70),
               Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonDeleteActivity = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 560, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonDeleteActivity.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonDeleteActivity.MouseEnter += buttonDelete_MouseEnter;
            buttonDeleteActivity.MouseLeave += buttonDelete_MouseLeave;
            buttonDeleteActivity.MouseLeftButtonDown += buttonDeleteActivity_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonDeleteActivity, "Usuń aktywność");

            new MyLabel(mainCanvas, "x", 30, 50, 24, 590, 10, Color.FromArgb(255, 70, 70, 70),
                Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonExit = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 590, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonExit.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonExit.MouseEnter += buttonExit_MouseEnter;
            buttonExit.MouseLeave += buttonExit_MouseLeave;
            buttonExit.MouseLeftButtonDown += buttonExit_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonExit, "Zamknij okno aktywności");
        }

        private void CreateChart()
        {
            charts = new MyRectangle[7];
            scaleLabel = new MyLabel[4];
            nameDay = new string[] { "Niedz", "Pon", "Wt", "Śr", "Czw", "Pt", "Sob" };

            for (int i = 0; i < 59; i++)
            {
                new MyRectangle(contentCanvas, 3, 1, Color.FromArgb(150, 150, 150, 150), 30 + (i * 6), 50);
            }
            for (int i = 0; i < 4; i++)
            {
                scaleLabel[i] = new MyLabel(contentCanvas, "", 50, 26, 12, 24, 56 + (i * 40), Color.FromArgb(180, 150, 150, 150), Color.FromArgb(0, 20, 20, 20));
                new MyRectangle(contentCanvas, 30, 1, Color.FromArgb(100, 150, 150, 150), 30, 80 + (i * 40));
            }
            int numberDayOfWeek = (int)DateTime.Now.DayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    new MyRectangle(contentCanvas, 1, 3, Color.FromArgb(100, 150, 150, 150), 88 + (i * 40), 50 + (j * 6));
                }
                charts[i] = new MyRectangle(contentCanvas, 16, 150, Color.FromArgb(255, 190, 190, 190), 80 + (i * 40), 50);
                charts[i].EnableResizeToolTip(false);
                if (i == 6) charts[i].SetFillColor(Color.FromArgb(255, 117, 203, 255));
                new MyCircle(contentCanvas, 4, 0, Color.FromArgb(255, 180, 180, 180), 86 + (i * 40), 48, 1, true);
                new MyLabel(contentCanvas, nameDay[GetNumberDayOfWeek(i)], 50, 26, 12, 64 + (i * 40), 24, Color.FromArgb(180, 150, 150, 150), Color.FromArgb(0, 20, 20, 20));
            }
            new MyRectangle(contentCanvas, 358, 1, Color.FromArgb(150, 150, 150, 150), 30, 200);
        }

        private void UpdateChart()
        {
            AllData_db allData_db = new AllData_db();
            DailyUseOfApplication_db dailyUseOfApplication_db = new DailyUseOfApplication_db();
            List<int> activityID = new List<int>();
            int[] timeActivity = new int[7];
            DateTime dateTime = DateTime.Now;
            activityID.Add(viewActivityID);

            for (int i = 0; i < 7; i++)
            {
                if (i < 6)
                    timeActivity[i] = allData_db.GetTimeForNumberActivity(activityID, dateTime.AddDays(-(7 - (i + 1))).ToShortDateString());
                else
                    timeActivity[i] = dailyUseOfApplication_db.GetTimeForNumberActivity(activityID);
            }

            double maxValue = ActionOnNumbers.DivisionD((timeActivity.Max() > 2) ? timeActivity.Max() : 3, 60);
            for (int i = 0; i < 4; i++)
            {
                scaleLabel[i].SetContent((((maxValue / 3.0) * 3) - ((maxValue / 3.0) * i)).ToString("0.0") + " h");
            }

            if (timeActivity.Max() > 0)
            {
                double scale = maxValue / Convert.ToDouble(scaleLabel[0].GetContent().Replace(" h", ""));
                for (int i = 0; i < 7; i++)
                {
                    charts[i].Resize((int)(timeActivity[i] * (120 * scale) / timeActivity.Max()), 16);
                    charts[i].Position(y: 200 - timeActivity[i] * (120 * scale / timeActivity.Max()));
                    charts[i].ToolTip(ActionOnTime.GetTime(timeActivity[i]));
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    charts[i].Resize(0, 16);
                    charts[i].ToolTip(ActionOnTime.GetTime(timeActivity[i]));
                }
            }
            SetVisibleScale();
        }

        private void CreateListOfAddedApps()
        {
            applicationInActivity = new Canvas() { Width = 140, Height = 146, Background = new SolidColorBrush(Color.FromArgb(255, 236, 236, 236)) };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(contentCanvas, 140, 146, 440, 60, applicationInActivity);
            sv.Focusable = false;

            new MyLabel(contentCanvas, "Dodane aplikacje", 140, 30, 14, 440, 20,
                Color.FromArgb(205, 125, 125, 125), Color.FromArgb(200, 255, 255, 255), 0);

            Label buttonDeleteAllApplication = ButtonCreator.CreateButton(contentCanvas, "Usuń wszystkie", 120, 28, 12, 450, 220,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 0, 0), 1);
            ButtonCreator.SetToolTip(buttonDeleteAllApplication, "Usuń wszystkie aplikacje z aktywności");
            buttonDeleteAllApplication.Background = new SolidColorBrush(Color.FromArgb(120, 255, 93, 93));
            buttonDeleteAllApplication.MouseEnter += buttonDeleteAllApplication_MouseEnter;
            buttonDeleteAllApplication.MouseLeave += buttonDeleteAllApplication_MouseLeave;
            buttonDeleteAllApplication.MouseLeftButtonDown += buttonDeleteAllApplication_MouseLeftButtonDown;

            Label buttonEditActivity = ButtonCreator.CreateButton(contentCanvas, "Edytuj aktywność", 120, 28, 12, 450, 260,
                   Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 255, 0), 1);
            buttonEditActivity.Background = new SolidColorBrush(Color.FromArgb(60, 0, 200, 0));
            buttonEditActivity.MouseEnter += buttonEditActivity_MouseEnter;
            buttonEditActivity.MouseLeave += buttonEditActivity_MouseLeave;
            buttonEditActivity.MouseLeftButtonDown += buttonEditActivity_MouseLeftButtonDown;
        }

        private void UpdateListOfAddedApps()
        {
            applicationInActivity.Children.Clear();
            string titleApplication = string.Empty;
            applicationInActivity.Height = 0;
            for (int i = 0; i < activeApplication.Count; i++)
            {
                titleApplication = (activeApplication[i].Title.Length > 16) ?
                    activeApplication[i].Title.Remove(16, activeApplication[i].Title.Length - 16) : activeApplication[i].Title;
                MyLabel l = new MyLabel(applicationInActivity, titleApplication, 140, 30, 12, 0, 29 * i,
                      Color.FromArgb(255, 155, 155, 155), Color.FromArgb(200, 255, 255, 255), 0, HorizontalAlignment.Left);
                l.ToolTip(activeApplication[i].Title);
                new MyRectangle(applicationInActivity, 140, 1, Color.FromArgb(30, 150, 150, 150), 0, 30 + (29 * i));

                MyCircle circle = new MyCircle(applicationInActivity, 15, 1, (Color.FromArgb(155, 255, 93, 93)), 115, 8 + (29 * i), 1, true);
                Label buttonDeleteApplication = ButtonCreator.CreateButton(applicationInActivity, "x", 25, 25, 8, 110, 3 + (29 * i),
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 0, 0), 0, fontWeight: FontWeights.ExtraBold);
                buttonDeleteApplication.Background = new SolidColorBrush(Color.FromArgb(155, 236, 236, 236));
                buttonDeleteApplication.MouseEnter += buttonDeleteApplication_MouseEnter;
                buttonDeleteApplication.MouseLeave += buttonDeleteApplication_MouseLeave;
                buttonDeleteApplication.MouseLeftButtonDown += buttonDeleteApplication_MouseLeftButtonDown;
                buttonDeleteApplication.Name = "ID_" + activeApplication[i].ID;
                ButtonCreator.SetToolTip(buttonDeleteApplication, "Usuń aplikacje z aktywności");
                applicationInActivity.Height += 29;
            }
            applicationInActivity.Height = ((applicationInActivity.Height - 146) < 146) ? 146 : applicationInActivity.Height - 145;
        }

        private void CreateTableWithInformation()
        {
            time = new MyLabel[2];
            growth = new MyLabel[2];
            average = new MyLabel[2];

            new MyLabel(contentCanvas, "Dane tygodniowe", 120, 27, 11, 149, 210,
                       Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), 1);
            new MyLabel(contentCanvas, "Dane miesięczne", 120, 27, 11, 268, 210,
                       Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), 1);
            new MyLabel(contentCanvas, "Średnie użycie", 120, 23, 10, 25, 236,
                    Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), horizontalAlignment: HorizontalAlignment.Left);
            new MyLabel(contentCanvas, "Wzrost do porzedniego", 120, 23, 10, 25, 256,
                    Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), horizontalAlignment: HorizontalAlignment.Left);
            new MyLabel(contentCanvas, "Czas użycia", 120, 23, 10, 25, 276,
                    Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), horizontalAlignment: HorizontalAlignment.Left);

            for (int i = 0; i < 3; i++)
            {
                new MyRectangle(contentCanvas, 1, 91, Color.FromArgb(255, 117, 203, 255), 149 + (119 * i), 210);

                if (i < 2)
                {
                    average[i] = new MyLabel(contentCanvas, "0 %", 120, 27, 11, 149 + (119 * i), 236,
                       Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 117, 3, 255));

                    growth[i] = new MyLabel(contentCanvas, "0 %", 120, 27, 11, 149 + (119 * i), 256,
                       Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 117, 3, 255));

                    time[i] = new MyLabel(contentCanvas, "0 %", 120, 27, 11, 149 + (119 * i), 276,
                       Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 117, 3, 255));
                }
            }
            new MyRectangle(contentCanvas, 238, 1, Color.FromArgb(255, 117, 203, 255), 149, 300);
        }

        private void UpdateTableWithInformation()
        {
            AllData_db allData_db = new AllData_db();
            List<int> activityID = new List<int>();

            activityID.Add(viewActivityID);
            double[, ,] valueQuery = new double[2, 2, 2];
            valueQuery[0, 0, 0] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-7).ToShortDateString(), DateTime.Now.ToShortDateString());
            valueQuery[1, 0, 0] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-30).ToShortDateString(), DateTime.Now.ToShortDateString());
            valueQuery[0, 1, 0] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-14).ToShortDateString(), DateTime.Now.AddDays(-7).ToShortDateString());
            valueQuery[1, 1, 0] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-60).ToShortDateString(), DateTime.Now.AddDays(-30).ToShortDateString());
            activityID.Clear();

            activityID.Add(-1); activityID.Add(-2);
            valueQuery[0, 0, 1] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-7).ToShortDateString(), DateTime.Now.ToShortDateString(), true);
            valueQuery[1, 0, 1] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-30).ToShortDateString(), DateTime.Now.ToShortDateString(), true);
            valueQuery[0, 1, 1] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-14).ToShortDateString(), DateTime.Now.AddDays(-7).ToShortDateString(), true);
            valueQuery[1, 1, 1] = allData_db.GetTimeForNumberActivity(activityID, DateTime.Now.AddDays(-60).ToShortDateString(), DateTime.Now.AddDays(-30).ToShortDateString(), true);


            average[0].SetContent((ActionOnNumbers.DivisionD(valueQuery[0, 0, 0], valueQuery[0, 0, 1]) * 100).ToString("0.00") + " %");
            average[1].SetContent((ActionOnNumbers.DivisionD(valueQuery[1, 0, 0], valueQuery[1, 0, 1]) * 100).ToString("0.00") + " %");

            growth[0].SetContent(((ActionOnNumbers.DivisionD(valueQuery[0, 1, 0], valueQuery[0, 1, 1])
                - ActionOnNumbers.DivisionD(valueQuery[0, 0, 0], valueQuery[0, 0, 1])) * 100 * -1).ToString("0.00") + " %");
            growth[1].SetContent(((ActionOnNumbers.DivisionD(valueQuery[1, 1, 0], valueQuery[1, 1, 1])
                - ActionOnNumbers.DivisionD(valueQuery[1, 0, 0], valueQuery[1, 0, 1])) * 100 * -1).ToString("0.00") + " %");

            time[0].SetContent(ActionOnTime.GetTimeAndDays((int)valueQuery[0, 0, 0]));
            time[1].SetContent(ActionOnTime.GetTimeAndDays((int)valueQuery[1, 0, 0]));
        }

        private void CreateActivityFooter()
        {
            footerActivity = new List<Label>();
            footerActivityCounter = new MyLabel[2];
            for (int i = 0; i < namesActivity.Length; i++)
            {
                footerActivity.Add(ButtonCreator.CreateButton(contentCanvas, namesActivity[i], 120, 28, 12, 0 + (i * 120), 320,
                   Color.FromArgb(255, 55, 55, 55), Color.FromArgb(255, 255, 255, 255), 1));
                footerActivity[i].Background = new SolidColorBrush(Color.FromArgb(255, 206, 206, 206));
            }
            for (int i = 0; i < 2; i++)
            {
                footerActivityCounter[i] = new MyLabel(contentCanvas, "", 24, 23, 10, 1 + i * 594, 298, Color.FromArgb(255, 100, 100, 100),
                    Color.FromArgb(255, 200, 200, 200), 1, fontWeight: FontWeights.Bold);
            }

        }
        private void UpdateActivityFooter(int index, int prewIndex = 0)
        {
            bool moveLeft = (Canvas.GetLeft(footerActivity[index]) + footerActivity[index].Width > contentCanvas.Width) ? true : false;
            bool moveRight = (Canvas.GetLeft(footerActivity[index]) < 0) ? true : false;
            int numberShift = (Math.Abs(index - this.index) != 0) ? Math.Abs(index - this.index) : 1;

            if (prewIndex > 0)
                numberShift = prewIndex - index;
            for (int i = 0; i < footerActivity.Count; i++)
            {
                footerActivity[i].Background = new SolidColorBrush(Color.FromArgb(255, 206, 206, 206));
                if (moveLeft)
                    Canvas.SetLeft(footerActivity[i], Canvas.GetLeft(footerActivity[i]) - (footerActivity[index].Width * numberShift));
                if (moveRight)
                    Canvas.SetLeft(footerActivity[i], Canvas.GetLeft(footerActivity[i]) + (footerActivity[index].Width * numberShift));
            }
            footerActivity[index].Background = new SolidColorBrush(Color.FromArgb(255, 236, 236, 236));
            UpdateFooterActivityCounter();
        }

        private void UpdateFooterActivityCounter()
        {
            footerActivityCounter[0].SetContent((-Canvas.GetLeft(footerActivity[0]) / 120).ToString());
            footerActivityCounter[1].SetContent(((Canvas.GetLeft(footerActivity[footerActivity.Count - 1]) / 120) - 4).ToString());

            for (int i = 0; i < 2; i++)
            {
                if (Int32.Parse(footerActivityCounter[i].GetContent()) > 0)
                    footerActivityCounter[i].Visibility = Visibility.Visible;
                else
                    footerActivityCounter[i].Visibility = Visibility.Hidden;
            }
        }

        private int GetNumberDayOfWeek(int nextDay)
        {
            int numberDayOfWeek = (int)DateTime.Now.DayOfWeek + 1 + nextDay;
            return ((numberDayOfWeek) < 7) ? (numberDayOfWeek) : numberDayOfWeek % 7;
        }

        private void SetVisibleScale()
        {
            if (scaleLabel[3].GetContent().Contains("0 h") || scaleLabel[3].GetContent().Contains("1 h") || scaleLabel[3].GetContent().Contains("2 h"))
            {
                scaleLabel[1].Opacity(0);
                scaleLabel[2].Opacity(0);
            }
            else
            {
                scaleLabel[1].Opacity(1);
                scaleLabel[2].Opacity(1);
            }
        }

        private void buttonAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isOnlyEditMode = false;
            SetEditModeActivity();
        }

        private void buttonEditActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isOnlyEditMode = true;
            SetEditModeActivity();
        }

        private void SetEditModeActivity()
        {
            if (nameActivity.Visibility == Visibility.Visible)
            {
                nameActivity.Visibility = Visibility.Hidden;
                nameEditActivity = new TextBox
                {
                    Width = 270,
                    Height = 26,
                    FontSize = 18,
                    MaxLength = 30,
                    Background = new SolidColorBrush(Color.FromArgb(255, 0, 123, 255)),
                };
                Canvas.SetLeft(nameEditActivity, 15);
                Canvas.SetTop(nameEditActivity, 15);
                mainCanvas.Children.Add(nameEditActivity);
                nameEditActivity.PreviewMouseLeftButtonDown += nameEditActivity_DisableError;
                nameEditActivity.PreviewMouseRightButtonDown += nameEditActivity_DisableError;

                buttonSaveEditActivity = ButtonCreator.CreateButton(mainCanvas, "Dodaj", 80, 28, 12, 300, 14,
                   Color.FromArgb(255, 55, 55, 55), Color.FromArgb(255, 255, 255, 255), 1);
                buttonSaveEditActivity.Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
                buttonSaveEditActivity.MouseEnter += buttonSaveEditActivity_MouseEnter;
                buttonSaveEditActivity.MouseLeave += buttonSaveEditActivity_MouseLeave;
                buttonSaveEditActivity.MouseLeftButtonDown += buttonSaveEditActivity_MouseLeftButtonDown;

                buttonClose = ButtonCreator.CreateButton(mainCanvas, "X", 25, 25, 11, 400, 16,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 255, 255), 1);
                buttonClose.Background = new SolidColorBrush(Color.FromArgb(185, 240, 0, 0));
                buttonClose.FontWeight = FontWeights.ExtraBold;
                ButtonCreator.SetToolTip(buttonClose, "Anuluj");
                buttonClose.MouseEnter += buttonClose_MouseEnter;
                buttonClose.MouseLeave += buttonClose_MouseLeave;
                buttonClose.MouseLeftButtonDown += buttonClose_MouseLeftButtonDown;


                if (isOnlyEditMode)
                {
                    buttonSaveEditActivity.Content = "Zmień";
                    nameEditActivity.Text = namesActivity[index];
                }
                else
                {
                    footerActivity.Add(ButtonCreator.CreateButton(contentCanvas, "Nowa aktywność", 120, 28, 12, Canvas.GetLeft(footerActivity[footerActivity.Count - 1]) + 120, 320,
                    Color.FromArgb(255, 55, 55, 55), Color.FromArgb(255, 255, 255, 255), 1));
                    UpdateActivityFooter(footerActivity.Count - 1);
                }
            }
        }

        private void buttonSaveEditActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isOnlyEditMode)
            {
                if (!NameActivity_db.CheckIfExistName(nameEditActivity.Text))
                {
                    NameActivity_db.ChangeNameActivity(namesActivity[index], nameEditActivity.Text);
                    namesActivity[index] = nameEditActivity.Text;
                    nameActivity.SetContent(nameEditActivity.Text);
                    CloseEditModeActivity(false);
                }
                else
                {
                    nameEditActivity.Background = new SolidColorBrush(Color.FromArgb(220, 255, 55, 55));
                }
            }
            else
            {
                if (!NameActivity_db.CheckIfExistName(nameEditActivity.Text))
                {
                    NameActivity_db.AddNewActivity(nameEditActivity.Text);
                    namesActivity = NameActivity_db.GetNameActivityDictionary().Keys.ToArray();
                    nameActivity.SetContent(nameEditActivity.Text);
                    footerActivity[footerActivity.Count - 1].Content = nameEditActivity.Text;
                    index = footerActivity.Count - 1;
                    CloseEditModeActivity(true);
                }
                else
                {
                    nameEditActivity.Background = new SolidColorBrush(Color.FromArgb(220, 255, 55, 55));
                }
            }
        }
        private void buttonClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CloseEditModeActivity(false);
        }

        private void CloseEditModeActivity(bool ifAddActivity)
        {
            mainCanvas.Children.Remove(nameEditActivity);
            mainCanvas.Children.Remove(buttonSaveEditActivity);
            mainCanvas.Children.Remove(buttonClose);
            nameActivity.Visibility = Visibility.Visible;
            this.canvas.Focus();
            if (!isOnlyEditMode && !ifAddActivity)
            {
                contentCanvas.Children.Remove(footerActivity[footerActivity.Count - 1]);
                footerActivity.RemoveAt(footerActivity.Count - 1);
                UpdateActivityFooter(index, footerActivity.Count);
            }
        }

        private void buttonDeleteAllApplication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow(DialogWindowsState.YesCancel, DialogWindowsMessage.DeleteAllApplicationsWithAcitivty);
            dialogWindow.CloseWindowCancelButtonDelegate += dialogWindow_CloseWindowCancelButtonDelegate;
            dialogWindow.CloseWindowAcceptButtonDelegate += DeleteAllApplicationFromActivity;
            dialogWindow.ShowDialog();
        }

        private void buttonDeleteApplication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            deleteApplicationID = Int32.Parse((sender as Label).Name.ToString().Replace("ID_", ""));
            DialogWindow dialogWindow = new DialogWindow(DialogWindowsState.YesCancel, DialogWindowsMessage.DeleteOneApplicationWithAcitivty);
            dialogWindow.CloseWindowCancelButtonDelegate += dialogWindow_CloseWindowCancelButtonDelegate;
            dialogWindow.CloseWindowAcceptButtonDelegate += DeleteOneApplicationFromActivity;
            dialogWindow.ShowDialog();
        }

        private void buttonDeleteActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow(DialogWindowsState.YesCancel, DialogWindowsMessage.DeleteAcitivty);
            dialogWindow.CloseWindowCancelButtonDelegate += dialogWindow_CloseWindowCancelButtonDelegate;
            dialogWindow.CloseWindowAcceptButtonDelegate += DeleteActivity;
            dialogWindow.ShowDialog();
        }

        private void DeleteOneApplicationFromActivity()
        {
            if (ActiveApplication_db.DeleteOneApplicationWithActivity(viewActivityID, deleteApplicationID))
                Update();           
        }

        private void DeleteAllApplicationFromActivity()
        {
            if (ActiveApplication_db.DeleteAllApplicationsWithActivity(viewActivityID))
            {
               Update();
            }
            
        }

        private void DeleteActivity()
        {
            if (ActiveApplication_db.DeleteAllApplicationsWithActivity(viewActivityID))
            {
                if (NameActivity_db.DeleteActivity(namesActivity[index]))
                {                  
                    namesActivity = NameActivity_db.GetNameActivityDictionary().Keys.ToArray();
                    contentCanvas.Children.Remove(footerActivity[index]);
                    footerActivity.RemoveAt(index);
                    for (int i = index; i < footerActivity.Count; i++ )
                    {
                        Canvas.SetLeft(footerActivity[i], Canvas.GetLeft(footerActivity[i]) - 120);
                    }
                    index = index - 1;
                    Update();
                }
            }
        }

        private void dialogWindow_CloseWindowCancelButtonDelegate() { }

        private void buttonExit_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainCanvas.Children.RemoveRange(0, mainCanvas.Children.Count);
            this.canvas.Children.Remove(mainCanvas);
            this.canvas.KeyDown -= mainCanvas_KeyDown;
            CloseWindowShowActivityDelegate();
        }

        private void buttonSaveEditActivity_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
        }

        private void buttonSaveEditActivity_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(155, 200, 200, 200));
        }

        private void buttonExit_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
        }

        private void buttonExit_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 177, 255));
        }

        private void buttonDelete_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
        }

        private void buttonDelete_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 255, 132, 132));
        }

        private void buttonAdd_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
        }

        private void buttonAdd_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 255, 140));
        }

        private void buttonDeleteApplication_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 236, 236, 236));
        }

        private void buttonDeleteApplication_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(155, 236, 236, 236));
        }

        private void buttonDeleteAllApplication_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 255, 93, 93));
        }

        private void buttonDeleteAllApplication_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 255, 93, 93));
        }

        private void buttonEditActivity_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(60, 0, 200, 0));
        }

        private void buttonEditActivity_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(160, 0, 250, 0));
        }

        private void buttonClose_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(185, 240, 0, 0));
        }

        private void buttonClose_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(255, 240, 0, 0));
        }

        private void nameEditActivity_DisableError(object sender, MouseButtonEventArgs e)
        {
            nameEditActivity.Background = new SolidColorBrush(Color.FromArgb(255, 0, 123, 255));
        }
    }
}
