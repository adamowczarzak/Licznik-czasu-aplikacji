using ApplicationTimeCounter.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for WindowAddTo.xaml
    /// </summary>
    public partial class WindowAddTo : Window
    {
        private bool IsClosed;
        private string idApplication;
        private Canvas parentCanvas;
        private DispatcherTimer timerAnimation;
        private int repeatIntervals;
        private AddTo windowAddTo;



        public WindowAddTo(Canvas parent, bool revert, AddTo addTo)
        {
            IsClosed = false;
            bool ifAdd = false;
            idApplication = parent.Name.Replace("ID_", "");
            parentCanvas = parent;
            windowAddTo = addTo;

            timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = new TimeSpan(0, 0, 0, 0, 10);
            repeatIntervals = 0;

            if (addTo == AddTo.Activity)
                if (ActiveApplication_db.AddActivityToApplication(idApplication, "1")) ifAdd = true;
            if (addTo == AddTo.Group)
                if (ActiveApplication_db.AddGroupToApplication(idApplication, "NULL")) ifAdd = true;
            
            if (ifAdd)
            {
                ((Label)(parentCanvas.Children[7])).Content = "+";
                Canvas.SetTop((Label)(parentCanvas.Children[7]), 28);
                timerAnimation.Tick += new EventHandler(AnimationButtonRestart);
                timerAnimation.Start();
            }


        }
        public WindowAddTo(Canvas parent, AddTo addTo)
        {
            InitializeComponent();
            IsClosed = false;
            idApplication = parent.Name.Replace("ID_", "");
            parentCanvas = parent;
            windowAddTo = addTo;

            timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = new TimeSpan(0, 0, 0, 0, 10);
            repeatIntervals = 0;

            Canvas activity = new Canvas() { Width = 150, Height = 200, };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(addToCanvas, 150, 200, 0, 0, activity);

            Dictionary<string, string> names = new Dictionary<string,string>();
            if (addTo == AddTo.Activity) names = NameActivity_db.GetNameActivityDictionary();
            else if (addTo == AddTo.Group) names = Membership_db.GetNameGroupsDictionary();
            
            int nextIndex = 0;
            foreach (KeyValuePair<string, string> name in names)
            {
                Label button = ButtonCreator.CreateButton(activity, name.Key, 150, 30, 12, 0, 29 * nextIndex,
                        Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 255, 255), 1);
                activity.Height += 29;

                button.MouseEnter += button_MouseEnter;
                button.MouseLeave += button_MouseLeave;
                button.MouseLeftButtonDown += button_MouseLeftButtonDown;
                button.Name = "ID_" + name.Value;
                nextIndex++;
            }
            activity.Height = ((activity.Height - 200) < 200) ? 200 : activity.Height - 199;

            timerAnimation.Tick += new EventHandler(AnimationAddShow);
            timerAnimation.Start();

        }

        private void button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!timerAnimation.IsEnabled)
            {
                IsClosed = true;
                string idActivity = (sender as Label).Name.Replace("ID_", "");
                if (ActionOnBase(idActivity))
                {
                    ((Label)(parentCanvas.Children[7])).Content = "-";
                    Canvas.SetTop((Label)(parentCanvas.Children[7]), 27);
                    timerAnimation.Tick += new EventHandler(AnimationButton);
                    timerAnimation.Start();
                }
                timerAnimation.Tick += new EventHandler(AnimationAdd);
            }
        }
        private bool ActionOnBase(string idActivity)
        {
            if (windowAddTo == AddTo.Activity)
                return ActiveApplication_db.AddActivityToApplication(idApplication, idActivity);
            else if (windowAddTo == AddTo.Group)
                return ActiveApplication_db.AddGroupToApplication(idApplication, idActivity);
            else return false;
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            Label button = (Label)sender;
            button.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            Label button = (Label)sender;
            button.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 200));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                IsClosed = true;
                timerAnimation.Tick -= new EventHandler(AnimationAddShow);
                timerAnimation.Tick += new EventHandler(AnimationAdd);
                timerAnimation.Start();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (IsClosed == false)
            {
                timerAnimation.Tick -= new EventHandler(AnimationAddShow);
                timerAnimation.Tick += new EventHandler(AnimationAdd);
                timerAnimation.Start();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsClosed = false;
        }

        private void AnimationButton(object sender, EventArgs e)
        {
            repeatIntervals++;
            parentCanvas.Background = new SolidColorBrush(Color.FromArgb((byte)(repeatIntervals * 5), 154, 253, 154));
            ((Ellipse)(parentCanvas.Children[6])).Fill = new SolidColorBrush(Color.FromArgb((byte)(repeatIntervals * 5), 244, 114, 114));
            StopTimerAndReset(50);
        }

        private void AnimationAdd(object sender, EventArgs e)
        {
            repeatIntervals++;
            AddToWindow.Opacity -= 0.02;
            if(StopTimerAndReset(40))this.Close();
        }

        private void AnimationAddShow(object sender, EventArgs e)
        {
            repeatIntervals++;
            AddToWindow.Opacity += 0.016;
            if (StopTimerAndReset(30))
                timerAnimation.Tick -= new EventHandler(AnimationAddShow);
        }

        private void AnimationButtonRestart(object sender, EventArgs e)
        {
            repeatIntervals++;
            parentCanvas.Background = new SolidColorBrush(Color.FromArgb((byte)(255 - (repeatIntervals * 5)), 154, 253, 154));
            ((Ellipse)(parentCanvas.Children[6])).Fill = new SolidColorBrush(Color.FromArgb((byte)(repeatIntervals * 5), 0, 123, 255));
            StopTimerAndReset(50);
        }

        private bool StopTimerAndReset(int repeatIntervalsReset)
        {
            if (repeatIntervals == repeatIntervalsReset)
            {
                timerAnimation.Stop();
                timerAnimation.Tick -= new EventHandler(AnimationAdd);
                repeatIntervals = 0;
                return true;
            }
            return false;
        }
    }
}
