using ApplicationTimeCounter.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Diagnostics;
using ApplicationTimeCounter.ApplicationObjectsType;
using ApplicationTimeCounter.Other;

namespace ApplicationTimeCounter
{
    class StatisticsForm
    {
        public Canvas MainCanvasStatistics { get; set; }
        private Canvas contentPage;
        private Canvas filterCanvas;
        private Canvas intervalTimeCanvas;
        private Canvas chartCanvas;
        private Canvas chartContentCanvas;
        private DatePicker datePickerFrom;
        private DatePicker datePickerTo;
        private Label buttonShowFilter;
        private MyLabel countTitleApplication;
        private MyLabel countActivityApplication;
        private MyLabel ifActivity;
        private MyLabel ifTitleApplication;
        private Label buttonChooseActivity;
        private Label buttonChooseTitleApplication;
        private Label buttonChooseTimeInterval;
        private Label buttonGoFilter;
        private TextBox searchingNames;
        private bool isFilter;
        private int indexResultFilter;
        private int indexIntervalTime;
        private Stopwatch stopwatch;
        private string[] tableNameTitleInterval;
        private Color[] colorTable;
        private MyLabel[] scalePercent;
        private string[] tempDateCount;

        private Dictionary<string, string> titleApplication;
        private Dictionary<string, string> namesActivity;
        private Dictionary<string, string> foundList;

        private ViewContent viewContent;
        private AllData_db allData_db;

        public StatisticsForm(Canvas contentPage, ref ViewContent viewContent)
        {
            this.contentPage = contentPage;
            this.viewContent = viewContent;
            viewContent.ContentDelegateLoad += viewContent_Delegate;
            allData_db = new AllData_db();

            CreateStatisticsForm();
        }

        public void ShowStatisticsForm()
        {
            viewContent.ChangeContent(MainCanvasStatistics);
            Canvas.SetZIndex(MainCanvasStatistics, 1);
        }

        private void viewContent_Delegate(Visibility visibility)
        {
            filterCanvas.Visibility = Visibility.Hidden;
            buttonShowFilter.Content = "        Pokaż filtry";
        }

        private void CreateStatisticsForm()
        {
            MainCanvasStatistics = CanvasCreator.CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 10, 10);
            MainCanvasStatistics.MouseLeftButtonDown += MainCanvasStatistics_MouseLeftButtonDown;
            MainCanvasStatistics.Opacity = 0;
            tableNameTitleInterval = new string[5];
            tableNameTitleInterval[0] = "Wczoraj";
            tableNameTitleInterval[1] = "Ostatnie 3 dni";
            tableNameTitleInterval[2] = "Ostatni tydzień";
            tableNameTitleInterval[3] = "Ostatni miesiąc";
            tableNameTitleInterval[4] = "Ostatnie 3 miesiące";
            colorTable = new Color[4];
            colorTable[0] = Color.FromArgb(255, 106, 162, 187);
            colorTable[1] = Color.FromArgb(255, 9, 42, 75);
            colorTable[2] = Color.FromArgb(255, 2, 53, 101);
            colorTable[3] = Color.FromArgb(255, 21, 66, 144);
            indexIntervalTime = -1;
            indexResultFilter = -1;

            CreateBackground();
            CreateControlPanel();
        }

        private void CreateControlPanel()
        {
            new MyRectangle(MainCanvasStatistics, 600, 35, Color.FromArgb(255, 0, 125, 255), 0, 0);
            ImageCreator.CreateImage(MainCanvasStatistics, 40, 50, 4, -8, "Pictures/application.png");
            countTitleApplication = new MyLabel(MainCanvasStatistics, tempDateCount[0], 60, 30, 14, 45, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0),
                horizontalAlignment: HorizontalAlignment.Left);

            ImageCreator.CreateImage(MainCanvasStatistics, 26, 26, 110, 4, "Pictures/ActivityImages.png");
            countActivityApplication = new MyLabel(MainCanvasStatistics, tempDateCount[1], 60, 30, 14, 140, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0),
                horizontalAlignment: HorizontalAlignment.Left);


            CreateButtonShowFilter();
            CreateFilter();
        }

        private void CreateBackground()
        {
            MyRectangle r = new MyRectangle(MainCanvasStatistics, 600, 320, Color.FromArgb(0, 0, 0, 0), 0, 40, 1);
            r.SetStroke(Color.FromArgb(255, 20, 29, 83));
            chartCanvas = CanvasCreator.CreateCanvas(MainCanvasStatistics, 590, 350, Color.FromArgb(0, 0, 0, 0), 5, 40);
            new MyRectangle(chartCanvas, 600, 1, Color.FromArgb(255, 20, 29, 83), 0, 240);
            new MyRectangle(chartCanvas, 600, 1, Color.FromArgb(255, 20, 29, 83), 0, 160);
            new MyRectangle(chartCanvas, 600, 1, Color.FromArgb(255, 20, 29, 83), 0, 80);
            chartContentCanvas = new Canvas() { Width = 590, Height = 362 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(chartCanvas, 590, 364, 0, 0, chartContentCanvas);
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scalePercent = new MyLabel[3];
            scalePercent[0] = new MyLabel(chartCanvas, "", 40, 30, 11, -20, 225, Color.FromArgb(160, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            scalePercent[1] = new MyLabel(chartCanvas, "", 40, 30, 11, -20, 145, Color.FromArgb(160, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            scalePercent[2] = new MyLabel(chartCanvas, "", 40, 30, 11, -20, 65, Color.FromArgb(160, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));

            CommandParameters parameters = new CommandParameters();
            parameters.StartDate = DateTime.Now.AddDays(-5).ToShortDateString();
            parameters.EndDate = DateTime.Now.ToShortDateString();
            List<Activity> activity = allData_db.GetDailyActivity(parameters);
            CreateChartActivity(activity, parameters);
        }

        private void CreateButtonShowFilter()
        {
            ImageCreator.CreateImage(MainCanvasStatistics, 26, 26, 490, 4, "Pictures/showFilter.png");
            buttonShowFilter = ButtonCreator.CreateButton(MainCanvasStatistics, "        Pokaż filtry", 114, 30, 13, 484, 2,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            buttonShowFilter.Background = new SolidColorBrush(Color.FromArgb(35, 2, 53, 101));
            buttonShowFilter.MouseEnter += Behavior_MouseEnter;
            buttonShowFilter.MouseLeave += Behavior_MouseLeave;
            buttonShowFilter.MouseLeftButtonDown += buttonShowFilter_MouseLeftButtonDown;
        }

        private void CreateFilter()
        {
            filterCanvas = CanvasCreator.CreateCanvas(MainCanvasStatistics, 600, 80, Color.FromArgb(255, 0, 125, 255), 0, 36);
            filterCanvas.Visibility = Visibility.Hidden;

            CreateDatePickiers();
            CreateCheckBoxes();
            CreateTextBox();
            CreateComboBoxAndButtonGoFilter();
        }

        private void CreateDatePickiers()
        {
            new MyLabel(filterCanvas, "Data od", 60, 30, 13, 6, 10, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            new MyLabel(filterCanvas, "Data do", 60, 30, 13, 6, 48, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            datePickerFrom = CreateDatePicker(datePickerFrom);
            datePickerTo = CreateDatePicker(datePickerTo);
            Canvas.SetTop(datePickerTo, 48);
        }

        private DatePicker CreateDatePicker(DatePicker datePicker)
        {
            datePicker = new DatePicker();
            datePicker.DisplayDateStart = DateTime.Now.AddDays(Convert.ToInt32(allData_db.GetDayWorkingApplication()) + 1);
            datePicker.DisplayDateEnd = DateTime.Now.AddDays(-1);
            datePicker.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            filterCanvas.Children.Add(datePicker);
            Canvas.SetLeft(datePicker, 70);
            Canvas.SetTop(datePicker, 10);
            datePicker.SelectedDateChanged += datePicker_SelectedDateChanged;

            if (datePicker.DisplayDateEnd != datePicker.DisplayDateStart) datePicker.IsEnabled = true;
            else datePicker.IsEnabled = false;

            return datePicker;
        }

        private void CreateCheckBoxes()
        {
            MyRectangle r = new MyRectangle(filterCanvas, 20, 20, Color.FromArgb(0, 244, 244, 255), 200, 15, 1); r.Opacity(0.5);
            ifActivity = new MyLabel(filterCanvas, "x", 25, 40, 20, 197, 3, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70));
            ifActivity.Visibility = System.Windows.Visibility.Visible;
            buttonChooseActivity = ButtonCreator.CreateButton(filterCanvas, "    Aktywności", 120, 34, 16, 196, 8,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(205, 205, 205, 205));
            buttonChooseActivity.MouseEnter += buttonChooseActivity_MouseEnter;
            buttonChooseActivity.MouseLeave += buttonChooseActivity_MouseLeave;
            buttonChooseActivity.MouseLeftButtonDown += buttonChooseActivity_MouseLeftButtonDown;

            MyRectangle r2 = new MyRectangle(filterCanvas, 20, 20, Color.FromArgb(0, 244, 244, 255), 200, 50, 1); r2.Opacity(0.5);
            ifTitleApplication = new MyLabel(filterCanvas, "x", 25, 40, 20, 197, 38, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70));
            ifTitleApplication.Visibility = System.Windows.Visibility.Hidden;
            buttonChooseTitleApplication = ButtonCreator.CreateButton(filterCanvas, "    Tytuły aplikacji", 150, 34, 16, 196, 43,
                Color.FromArgb(255, 205, 205, 205), Color.FromArgb(205, 205, 205, 205));
            buttonChooseTitleApplication.MouseEnter += buttonChooseTitleApplication_MouseEnter;
            buttonChooseTitleApplication.MouseLeave += buttonChooseTitleApplication_MouseLeave;
            buttonChooseTitleApplication.MouseLeftButtonDown += buttonChooseTitleApplication_MouseLeftButtonDown;
        }

        private void CreateComboBoxAndButtonGoFilter()
        {
            buttonChooseTimeInterval = ButtonCreator.CreateButton(filterCanvas, "Wybierz czas", 130, 30, 13, 360, 44,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            buttonChooseTimeInterval.Background = new SolidColorBrush(Color.FromArgb(35, 2, 53, 101));
            buttonChooseTimeInterval.MouseEnter += Behavior_MouseEnter;
            buttonChooseTimeInterval.MouseLeave += Behavior_MouseLeave;
            buttonChooseTimeInterval.MouseLeftButtonDown += ChooseTimeInterval;

            intervalTimeCanvas = CanvasCreator.CreateCanvas(filterCanvas, 130, 100, Color.FromArgb(0, 0, 123, 255), 360, 80);
            intervalTimeCanvas.Visibility = Visibility.Hidden;

            for (int i = 0; i < 5; i++)
            {
                Label intervalTime = ButtonCreator.CreateButton(intervalTimeCanvas, tableNameTitleInterval[i], 130, 26, 10, 0, 0 + (i * 27),
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
                intervalTime.Background = new SolidColorBrush(Color.FromArgb(100, 0, 123, 255));
                intervalTime.Name = "ID_" + i;
                intervalTime.MouseEnter += Behavior_MouseEnter;
                intervalTime.MouseLeave += intervalTime_MouseLeave;
                intervalTime.MouseLeftButtonDown += intervalTime_MouseLeftButtonDown;
            }

            buttonGoFilter = ButtonCreator.CreateButton(filterCanvas, "Filtruj", 80, 30, 14, 514, 44,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            buttonGoFilter.Background = new SolidColorBrush(Color.FromArgb(35, 2, 53, 101));
            buttonGoFilter.MouseEnter += Behavior_MouseEnter;
            buttonGoFilter.MouseLeave += Behavior_MouseLeave;
            buttonGoFilter.MouseLeftButtonDown += GoFilter;
        }

        private void CreateTextBox()
        {
            searchingNames = new TextBox
            {
                Text = "Wpisz nazwę aktywności",
                Width = 234,
                Height = 26,
                FontSize = 15,
                MaxLength = 30,
                Background = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 85, 85)),
                FontFamily = new FontFamily("Comic Sans MS"),
            };
            Canvas.SetLeft(searchingNames, 360);
            Canvas.SetTop(searchingNames, 10);
            filterCanvas.Children.Add(searchingNames);
            searchingNames.PreviewMouseLeftButtonDown += searchingNames_MouseLeftButtonDown;
            searchingNames.LostFocus += searchingNames_LostFocus;
            searchingNames.PreviewKeyDown += searchingNames_KeyDown;
            searchingNames.TextChanged += searchingNames_TextChanged;
        }

        private void GetNameActivityAndTitleApplication()
        {
            foundList = new Dictionary<string, string>();
            stopwatch = new Stopwatch();
            titleApplication = ActiveApplication_db.GetNameApplicationDictionary();
            namesActivity = NameActivity_db.GetNameActivityDictionary();
        }

        private void searchingNames_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                if (isFilter)
                {
                    if (searchingNames.Text.Length > 3)
                    {
                        if (stopwatch.IsRunning)
                        {
                            foundList.Clear();
                            string regexString = searchingNames.Text.Trim();
                            Regex regex = new Regex(regexString, RegexOptions.IgnoreCase);
                            Dictionary<string, string> tempDictionary;
                            if (ifActivity.Visibility == Visibility.Visible)
                                tempDictionary = namesActivity;
                            else tempDictionary = titleApplication;

                            foreach (KeyValuePair<string, string> name in tempDictionary)
                            {
                                if (regex.Matches(name.Value).Count > 0)
                                {
                                    if (!foundList.Any()) searchingNames.Text = name.Value;
                                    if (!foundList.ContainsKey(name.Key))
                                        foundList.Add(name.Key, name.Value); ;
                                }
                            }
                        }
                        indexResultFilter = 0;
                    }
                }
            }
            stopwatch.Restart();
        }

        private void searchingNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (indexResultFilter < foundList.Count - 1) indexResultFilter++;
                isFilter = false;
                searchingNames.Text = foundList.ElementAt(indexResultFilter).Value;
            }
            else if (e.Key == Key.Up)
            {
                if (indexResultFilter > 0) indexResultFilter--;
                isFilter = false;
                searchingNames.Text = foundList.ElementAt(indexResultFilter).Value;
            }
            else
            {
                isFilter = true;
            }
        }

        private void GoFilter(object sender, MouseButtonEventArgs e)
        {
            stopwatch.Reset();
            chartContentCanvas.Children.Clear();
            filterCanvas.Visibility = Visibility.Hidden;
            buttonShowFilter.Content = "        Pokaż filtry";
            CommandParameters parameters = new CommandParameters();

            if (indexIntervalTime > -1)
            {
                string[] date = ActionOnTime.GetTimeInterwal(indexIntervalTime);
                parameters.StartDate = date[0];
                parameters.EndDate = date[1];
            }
            else if (!string.IsNullOrEmpty(datePickerFrom.SelectedDate.ToString())
                || !string.IsNullOrEmpty(datePickerTo.SelectedDate.ToString()))
            {
                parameters.StartDate = datePickerFrom.SelectedDate.ToString();
                parameters.EndDate = datePickerTo.SelectedDate.ToString();
            }
            else
            {
                parameters.StartDate = DateTime.Now.AddDays(-5).ToShortDateString();
                parameters.EndDate = DateTime.Now.ToShortDateString();
            }

            if (ifActivity.Visibility == Visibility.Visible)
            {
                List<Activity> activity;
                activity = allData_db.GetDailyActivity(parameters);
                CreateChartActivity(activity, parameters);
            }
            else if (ifTitleApplication.Visibility == Visibility.Visible)
            {
                List<ActiveApplication> activeApplication;
                activeApplication = allData_db.GetActiveApplicationGrouping(parameters);
                parameters.IdMembership = -1;
                activeApplication.AddRange(allData_db.GetActiveApplication(parameters));
                CreateChartActiveApplication(activeApplication);
            }
        }

        private void CreateChartActiveApplication(List<ActiveApplication> activeApplication)
        {
            if (activeApplication.Any())
            {
                activeApplication = activeApplication.OrderBy(x => x.Date).ToList();
                int maxValue = activeApplication.Max(x => x.ActivityTime);
                List<Tuple<int, string>> activeApplicationTupleList = GetTupleList(activeApplication);
                List<int> sumDateList = GetSumTimeList(activeApplicationTupleList);
                double scaleMultiplier = ActionOnNumbers.DivisionD(300, maxValue);
                int width = 10;
                int space = 0;
                int lenghtLabelDate = 0;
                int positionXLabelDate = 16;
                int numberElement = 0;
                chartContentCanvas.Width = 0;
                for (int i = 0; i < activeApplication.Count; i++)
                {
                    if (i > 0)
                        if (!activeApplication[i].Date.Equals(activeApplication[i - 1].Date))
                        {
                            space += 10;
                            lenghtLabelDate += 11;
                            DrawDateLabel(activeApplication[i - 1].Date.Remove(10), lenghtLabelDate, positionXLabelDate);
                            positionXLabelDate += lenghtLabelDate - 1;
                            lenghtLabelDate = 0;
                        }
                    if (!string.IsNullOrEmpty(searchingNames.Text) && !searchingNames.Text.Equals("Wpisz nazwę aktywności") && indexResultFilter > -1)
                    {
                        if (activeApplication[i].ID == Convert.ToInt32(foundList.ElementAt(indexResultFilter).Key))
                        {
                            MyRectangle r = new MyRectangle(chartContentCanvas, width, (int)(activeApplication[i].ActivityTime * scaleMultiplier),
                                colorTable[i % 4], 40 + space + ((width + 40) * numberElement) + 2, 320 - activeApplication[i].ActivityTime * scaleMultiplier, 1);
                            SetStrokeAndToolTip(r, activeApplication, i);
                            chartContentCanvas.Width += width * 5;
                            lenghtLabelDate += width * 5;
                            numberElement++;
                        }
                    }
                    else
                    {
                        MyRectangle r = new MyRectangle(chartContentCanvas, width, (int)(activeApplication[i].ActivityTime * scaleMultiplier),
                            colorTable[i % 4], 20 + space + ((width + 2) * i) + 2, 320 - activeApplication[i].ActivityTime * scaleMultiplier, 1);
                        SetStrokeAndToolTip(r, activeApplication, i);
                        chartContentCanvas.Width += (width + 2);
                        lenghtLabelDate += width + 2;
                    }
                }
                lenghtLabelDate += 21;
                DrawDateLabel(activeApplication[activeApplication.Count - 1].Date.Remove(10), lenghtLabelDate, positionXLabelDate);
                chartContentCanvas.Width += space + 50;
                SetScalePercent(maxValue, sumDateList.Max());

                countTitleApplication.SetContent(activeApplication.Select(x => x.Title).Distinct().Count().ToString());
                countActivityApplication.SetContent(activeApplication.Select(x => x.IdNameActivity).Distinct().Count().ToString());
            }
        }

        private void CreateChartActivity(List<Activity> activity, CommandParameters parameters)
        {
            if (activity.Any())
            {
                activity = activity.OrderBy(x => x.Date).ToList();
                int maxValue = activity.Max(x => x.ActivityTime);
                List<Tuple<int, string>> activityTupleList = GetTupleList(activity);
                List<int> sumDateList = GetSumTimeList(activityTupleList);
                double scaleMultiplier = ActionOnNumbers.DivisionD(300, maxValue);
                int width = 18;
                int space = 0;
                int lenghtLabelDate = 0;
                int positionXLabelDate = 12;
                int numberElement = 0;
                chartContentCanvas.Width = 0;
                for (int i = 0; i < activity.Count; i++)
                {
                    if (i > 0)
                        if (!activity[i].Date.Equals(activity[i - 1].Date))
                        {
                            space += 20;
                            lenghtLabelDate += 21;
                            DrawDateLabel(activity[i - 1].Date.Remove(10), lenghtLabelDate, positionXLabelDate);
                            positionXLabelDate += lenghtLabelDate - 1;
                            lenghtLabelDate = 0;
                        }
                    if (((searchingNames != null) ? !string.IsNullOrEmpty(searchingNames.Text) : true)
                        && ((searchingNames != null) ? !searchingNames.Text.Equals("Wpisz nazwę aktywności") : true) && indexResultFilter > -1)
                    {
                        if (activity[i].ID == Convert.ToInt32(foundList.ElementAt(indexResultFilter).Key))
                        {
                            MyRectangle r = new MyRectangle(chartContentCanvas, width, (int)(activity[i].ActivityTime * scaleMultiplier),
                                colorTable[i % 4], 40 + space + ((width + 36) * numberElement) + 2, 320 - activity[i].ActivityTime * scaleMultiplier, 1);
                            SetStrokeAndToolTip(r, activity, i);
                            chartContentCanvas.Width += width * 3;
                            lenghtLabelDate += width * 3;
                            numberElement++;
                        }
                    }
                    else
                    {
                        MyRectangle r = new MyRectangle(chartContentCanvas, width, (int)(activity[i].ActivityTime * scaleMultiplier),
                            colorTable[i % 4], 20 + space + ((width + 4) * i) + 4, 320 - activity[i].ActivityTime * scaleMultiplier, 1);
                        SetStrokeAndToolTip(r, activity, i);
                        chartContentCanvas.Width += (width + 4);
                        lenghtLabelDate += width + 4;
                    }
                }
                lenghtLabelDate += 21;
                DrawDateLabel(activity[activity.Count - 1].Date.Remove(10), lenghtLabelDate, positionXLabelDate);
                chartContentCanvas.Width += space + 50;
                SetScalePercent(maxValue, sumDateList.Max());

                if (countTitleApplication != null)
                {
                    countTitleApplication.SetContent(allData_db.CountApplicationInInterwalTime(parameters.StartDate, parameters.EndDate));
                    countActivityApplication.SetContent(activity.Select(x => x.Name).Distinct().Count().ToString());
                }
                else
                {
                    tempDateCount = new string[2];
                    tempDateCount[0] = allData_db.CountApplicationInInterwalTime(parameters.StartDate, parameters.EndDate);
                    tempDateCount[1] = activity.Select(x => x.Name).Distinct().Count().ToString();
                }
            }
        }

        private void SetStrokeAndToolTip(MyRectangle rectangle, List<Activity> activity, int index)
        {
            rectangle.SetStroke(Color.FromArgb(100, 255, 255, 255));
            rectangle.ToolTip(activity[index].Name + " - [" + ActionOnTime.GetTimeAndDays(activity[index].ActivityTime) + " / "
                                + ActionOnTime.GetTimeAndDays(activity.Where(x => x.Name == activity[index].Name).Sum(x => x.ActivityTime)) + "] [" +
                                ActionOnNumbers.DivisionI(activity[index].ActivityTime * 100, activity.Where(x => x.Date == activity[index].Date).Sum(x => x.ActivityTime))
                                + "% / " + ActionOnNumbers.DivisionI(activity.Where(x => x.Name == activity[index].Name).Sum(x => x.ActivityTime) * 100, activity.Sum(x => x.ActivityTime)) + "%]");
        }

        private void SetStrokeAndToolTip(MyRectangle rectangle, List<ActiveApplication> activeApplication, int index)
        {
            Dictionary<string, string> nameActivity = NameActivity_db.GetAllNameActivityDictionary();
            rectangle.SetStroke(Color.FromArgb(100, 255, 255, 255));
            rectangle.ToolTip(activeApplication[index].Title + " \nAktywność [ " + nameActivity[activeApplication[index].IdNameActivity.ToString()] + " ][" + ActionOnTime.GetTimeAndDays(activeApplication[index].ActivityTime) + " / "
                                + ActionOnTime.GetTimeAndDays(activeApplication.Where(x => x.Title == activeApplication[index].Title).Sum(x => x.ActivityTime)) + "] [" +
                                ActionOnNumbers.DivisionI(activeApplication[index].ActivityTime * 100, activeApplication.Where(x => x.Date == activeApplication[index].Date).Sum(x => x.ActivityTime))
                                + "% / " + ActionOnNumbers.DivisionI(activeApplication.Where(x => x.Title == activeApplication[index].Title).Sum(x => x.ActivityTime) * 100, activeApplication.Sum(x => x.ActivityTime)) + "%]");
        }

        private void SetScalePercent(int maxValue, int sumValue)
        {
            int minValue = ActionOnNumbers.DivisionI((maxValue * 100), sumValue) / 4;
            for (int i = 0; i < 3; i++)
            {
                scalePercent[i].SetContent((minValue * (i + 1)) + "%");
            }
        }

        private List<int> GetSumTimeList(List<Tuple<int, string>> dateAndTime)
        {
            List<int> sumActivityTimeList = new List<int>();
            string equalDate = (dateAndTime.Any()) ? dateAndTime[0].Item2 : string.Empty;
            int sumActivityTime = 0;
            foreach (var application in dateAndTime)
            {
                if (string.Equals(application.Item2, equalDate))
                    sumActivityTime += application.Item1;
                else
                {
                    if (sumActivityTime != 0)
                        sumActivityTimeList.Add(sumActivityTime);
                    equalDate = application.Item2;
                    sumActivityTime = application.Item1;
                }
            }
            if (sumActivityTime != 0)
                sumActivityTimeList.Add(sumActivityTime);
            return sumActivityTimeList;
        }

        private List<Tuple<int, string>> GetTupleList(List<ActiveApplication> activeApplication)
        {
            List<Tuple<int, string>> tupleList = new List<Tuple<int, string>>();
            foreach (var application in activeApplication)
                tupleList.Add(Tuple.Create(application.ActivityTime, application.Date));
            return tupleList;
        }

        private List<Tuple<int, string>> GetTupleList(List<Activity> activity)
        {
            List<Tuple<int, string>> tupleList = new List<Tuple<int, string>>();
            foreach (var application in activity)
                tupleList.Add(Tuple.Create(application.ActivityTime, application.Date));
            return tupleList;
        }

        private void DrawDateLabel(string content, int lenghtLabelDate, int positionXLabelDate)
        {
            MyLabel l2 = new MyLabel(chartContentCanvas, content, lenghtLabelDate, 22, 9, positionXLabelDate, 326, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 255, 255), 1);
            l2.SetBackgroundColor(Color.FromArgb(60, 255, 255, 255));
        }


        private void ChooseTimeInterval(object sender, MouseButtonEventArgs e)
        {
            if (intervalTimeCanvas.Visibility == Visibility.Hidden)
            {
                intervalTimeCanvas.Visibility = Visibility.Visible;
            }
            else
            {
                intervalTimeCanvas.Visibility = Visibility.Hidden;
            }
        }

        private void searchingNames_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            searchingNames.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
            searchingNames.Foreground = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
            searchingNames.Text = "";
        }

        private void searchingNames_LostFocus(object sender, RoutedEventArgs e)
        {
            searchingNames.Background = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
            searchingNames.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 85, 85));
            searchingNames.Text = "Wpisz nazwę aktywności";
        }

        private void buttonShowFilter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Label).Content.ToString().Contains("Ukryj Filtr"))
            {
                (sender as Label).Content = "        Pokaż filtry";
                ShowHideFilter(Visibility.Hidden);
            }
            else
            {
                (sender as Label).Content = "       Ukryj Filtr";
                ShowHideFilter(Visibility.Visible);
                GetNameActivityAndTitleApplication();
            }
        }

        private void buttonChooseTitleApplication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ifTitleApplication.Visibility == System.Windows.Visibility.Hidden)
            {
                ifTitleApplication.Visibility = System.Windows.Visibility.Visible;
                ifActivity.Visibility = System.Windows.Visibility.Hidden;
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                buttonChooseActivity.Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
            }
            else
            {
                ifTitleApplication.Visibility = System.Windows.Visibility.Hidden;
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
            }
        }

        private void buttonChooseActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ifActivity.Visibility == System.Windows.Visibility.Hidden)
            {
                ifActivity.Visibility = System.Windows.Visibility.Visible;
                ifTitleApplication.Visibility = System.Windows.Visibility.Hidden;
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                buttonChooseTitleApplication.Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));

            }
            else
            {
                ifActivity.Visibility = System.Windows.Visibility.Hidden;
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
            }
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonChooseTimeInterval.Content = "Wybierz czas";
            indexIntervalTime = -1;
        }

        private void intervalTime_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            datePickerTo.SelectedDate = null;
            datePickerFrom.SelectedDate = null;
            intervalTimeCanvas.Visibility = Visibility.Hidden;
            buttonChooseTimeInterval.Content = (sender as Label).Content;
            indexIntervalTime = Convert.ToInt32((sender as Label).Name.Replace("ID_", ""));
        }

        private void ShowHideFilter(Visibility visibility)
        {
            filterCanvas.Visibility = visibility;
        }

        private void buttonChooseTitleApplication_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ifTitleApplication.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void buttonChooseTitleApplication_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ifTitleApplication.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
        }

        private void buttonChooseActivity_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ifActivity.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void buttonChooseActivity_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ifActivity.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
        }

        private void Behavior_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(35, 2, 53, 101));
        }

        private void Behavior_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(80, 1, 28, 54));
        }

        private void intervalTime_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(100, 0, 123, 255));
        }


        private void MainCanvasStatistics_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (indexResultFilter == -1)
            {
                MainCanvasStatistics.Focusable = true;
                MainCanvasStatistics.Focus();
                if (stopwatch != null) stopwatch.Reset();
            }

        }
    }
}
