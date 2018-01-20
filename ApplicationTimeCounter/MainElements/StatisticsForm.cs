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

namespace ApplicationTimeCounter
{
    class StatisticsForm
    {
        public Canvas MainCanvasStatistics { get; set; }
        private Canvas contentPage;
        private Canvas filterCanvas;
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
        private Stopwatch stopwatch;

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
            UpdateView();
        }

        public void UpdateView()
        {

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

            CreateBackground();
            CreateControlPanel();
        }

        private void CreateControlPanel()
        {
            new MyRectangle(MainCanvasStatistics, 600, 35, Color.FromArgb(255, 0, 125, 255), 0, 0);
            ImageCreator.CreateImage(MainCanvasStatistics, 40, 50, 4, -8, "Pictures/application.png");
            countTitleApplication = new MyLabel(MainCanvasStatistics, "946", 60, 30, 14, 45, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0),
                horizontalAlignment: HorizontalAlignment.Left);

            ImageCreator.CreateImage(MainCanvasStatistics, 26, 26, 110, 4, "Pictures/ActivityImages.png");
            countActivityApplication = new MyLabel(MainCanvasStatistics, "5", 60, 30, 14, 140, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0),
                horizontalAlignment: HorizontalAlignment.Left);

            CreateButtonShowFilter();
            CreateFilter();
        }

        private void CreateBackground()
        {
            MyRectangle r = new MyRectangle(MainCanvasStatistics, 600, 350, Color.FromArgb(0, 0, 0, 0), 0, 40, 1);
            r.SetStroke(Color.FromArgb(155, 0, 0, 200));
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
            buttonChooseTimeInterval = ButtonCreator.CreateButton(filterCanvas, "Wybierz czas", 130, 30, 14, 360, 44,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            buttonChooseTimeInterval.Background = new SolidColorBrush(Color.FromArgb(35, 2, 53, 101));
            buttonChooseTimeInterval.MouseEnter += Behavior_MouseEnter;
            buttonChooseTimeInterval.MouseLeave += Behavior_MouseLeave;
            buttonChooseTimeInterval.MouseLeftButtonDown += ChooseTimeInterval;

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
        }

        private void ChooseTimeInterval(object sender, MouseButtonEventArgs e)
        {

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

        private void MainCanvasStatistics_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainCanvasStatistics.Focusable = true;
            MainCanvasStatistics.Focus();
            stopwatch.Reset();
        }
    }
}
