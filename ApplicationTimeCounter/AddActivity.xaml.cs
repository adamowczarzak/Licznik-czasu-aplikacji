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

namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for AddActivity.xaml
    /// </summary>
    public partial class AddActivity : Window
    {
        private bool IsClosed;
        private string idApplication;

        public AddActivity(string nameApplication)
        {
            InitializeComponent();
            IsClosed = false;
            idApplication = nameApplication.Replace("ID_", "");

            Canvas activity = new Canvas() { Width = 150, Height = 200, };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(addActivityCanvas, 150, 200, 0, 0, activity);

            Dictionary<string, string> nameDailyActivity = NameActivity_db.GetNameActivityList();

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

        }

        private void buttonActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string idActivity = (sender as Label).Name.Replace("ID_", "");
            ActiveApplication_db.AddActivityToApplication(idApplication, "0");
            IsClosed = true;
            this.Close();
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
                this.Close();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (IsClosed == false) this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsClosed = false;
        }
    }
}
