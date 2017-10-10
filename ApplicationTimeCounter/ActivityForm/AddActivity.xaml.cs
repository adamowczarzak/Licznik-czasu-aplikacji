using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for AddActivity.xaml
    /// </summary>
    public partial class AddActivity : Window
    {
        private bool IsClosed;
        private string idApplication;
        private Canvas parentCanvas;
        private DispatcherTimer timerAnimation;
        private int repeatIntervals;


        public AddActivity(Canvas parent, bool revert)
        {
            IsClosed = false;
            idApplication = parent.Name.Replace("ID_", "");
            parentCanvas = parent;

            timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = new TimeSpan(0, 0, 0, 0, 10);
            repeatIntervals = 0;

            if (ActiveApplication_db.AddActivityToApplication(idApplication, "1"))
            {
                ((Label)(parentCanvas.Children[7])).Content = "+";
                Canvas.SetTop((Label)(parentCanvas.Children[7]), 28);
                timerAnimation.Tick += new EventHandler(AnimationButtonRestart);
                timerAnimation.Start();
            }


        }
        public AddActivity(Canvas parent)
        {
            InitializeComponent();
            IsClosed = false;
            idApplication = parent.Name.Replace("ID_", "");
            parentCanvas = parent;

            timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = new TimeSpan(0, 0, 0, 0, 10);
            repeatIntervals = 0;

            Canvas activity = new Canvas() { Width = 150, Height = 200, };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(addActivityCanvas, 150, 200, 0, 0, activity);

            Dictionary<string, string> nameDailyActivity = NameActivity_db.GetNameActivityDictionary();

            int nextIndex = 0;
            foreach (KeyValuePair<string, string> dictioanry in nameDailyActivity)
            {
                Label buttonActivity = ButtonCreator.CreateButton(activity, dictioanry.Key, 150, 30, 12, 0, 29 * nextIndex,
                        Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 255, 255), 1);
                activity.Height += 29;

                buttonActivity.MouseMove += buttonActivity_MouseMove;
                buttonActivity.MouseLeave += buttonActivity_MouseLeave;
                buttonActivity.MouseLeftButtonDown += buttonActivity_MouseLeftButtonDown;
                buttonActivity.Name = "ID_" + dictioanry.Value;
                nextIndex++;
            }
            activity.Height = ((activity.Height - 200) < 200) ? 200 : activity.Height - 199;

            timerAnimation.Tick += new EventHandler(AnimationAddActivityShow);
            timerAnimation.Start();

        }

        private void buttonActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!timerAnimation.IsEnabled)
            {
                IsClosed = true;
                string idActivity = (sender as Label).Name.Replace("ID_", "");
                if (ActiveApplication_db.AddActivityToApplication(idApplication, idActivity))
                {
                    ((Label)(parentCanvas.Children[7])).Content = "-";
                    Canvas.SetTop((Label)(parentCanvas.Children[7]), 27);
                    timerAnimation.Tick += new EventHandler(AnimationButton);
                    timerAnimation.Start();
                }
                timerAnimation.Tick += new EventHandler(AnimationAddActivity);
            }
        }

        private void buttonActivity_MouseLeave(object sender, MouseEventArgs e)
        {
            Label button = (Label)sender;
            button.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void buttonActivity_MouseMove(object sender, MouseEventArgs e)
        {
            Label button = (Label)sender;
            button.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 200));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                IsClosed = true;
                timerAnimation.Tick -= new EventHandler(AnimationAddActivityShow);
                timerAnimation.Tick += new EventHandler(AnimationAddActivity);
                timerAnimation.Start();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (IsClosed == false)
            {
                timerAnimation.Tick -= new EventHandler(AnimationAddActivityShow);
                timerAnimation.Tick += new EventHandler(AnimationAddActivity);
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

        private void AnimationAddActivity(object sender, EventArgs e)
        {
            repeatIntervals++;
            AddActiviityWindow.Opacity -= 0.02;
            if(StopTimerAndReset(40))this.Close();
        }

        private void AnimationAddActivityShow(object sender, EventArgs e)
        {
            repeatIntervals++;
            AddActiviityWindow.Opacity += 0.016;
            if (StopTimerAndReset(30))
                timerAnimation.Tick -= new EventHandler(AnimationAddActivityShow);
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
                timerAnimation.Tick -= new EventHandler(AnimationAddActivity);
                repeatIntervals = 0;
                return true;
            }
            return false;
        }

    }
}
