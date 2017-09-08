using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int frequencyCheckingUsingApplication = 60;
        

        private IconInTaskbar notifyIcon;
        private InterfaceApplication interfaceApplication;
        private Counter counter;
        private ViewContent viewContent;
        private HomeForm homeForm;
        private CategoryForm categoryForm;
        private Activity activity;
        private RunApplication runApplication;

        

        private DispatcherTimer timer;


        public MainWindow()
        {
            runApplication = new RunApplication();

            if (runApplication.CheckIfRunApplication())
            {
                InitializeComponent();
                SetDispatcherTimer();


                interfaceApplication = new InterfaceApplication();
                counter = new Counter();
                viewContent = new ViewContent();
                homeForm = new HomeForm(contentPage, ref viewContent);
                categoryForm = new CategoryForm(contentPage, ref viewContent);
                notifyIcon = new IconInTaskbar(ref mainWindow, ref homeForm);
                activity = new Activity();


                if (activity.CheckIfIsNextDay() == true)
                {
                    counter.Reset();
                    homeForm.UpdateView();
                }
                else
                {
                    counter.UpdateTimeDisableCompurte();
                }
                activity.CheckIfIsActualDataInBaseAndUpdate();
            }

        }

       
        private void SetDispatcherTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_trick);
            timer.Interval = new TimeSpan(0, 0, frequencyCheckingUsingApplication);
            timer.Start();
        }

        private void Timer_trick(object sender, EventArgs e)
        {
            if(Convert.ToInt32(DateTime.Now.ToString("mm")) == 0 && 
                Convert.ToInt32(DateTime.Now.ToString("HH")) == 0)
            {
                counter.Reset();
                homeForm.UpdateView();
            }

            if (activity.UserIsActive() == true)
            {
                counter.Update();
                homeForm.UpdateTitleApllication();
                if (mainWindow.Visibility == Visibility.Visible) homeForm.UpdateView();
            }
            else
            {
                counter.UpdateTimeNonActive();
            }
            
        }

        
        //----------------------------- Zdarzenia ------------------------------------//

        private void hideWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            notifyIcon.SetMainWindow();
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

      


        //********************* Wygląd ******************************************//

        private void hideWindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            interfaceApplication.IfCanHighLightHideWindow(hideWindow);
        }

        private void hideWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            interfaceApplication.DisableHighLightHideWindow(hideWindow);
        }

        private void upperBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            homeForm.ShowHomeForm();         
        }

        private void categoryButton_Click(object sender, RoutedEventArgs e)
        {        
            categoryForm.ShowCategoryForm();  
        }

        


       


       



        

        
    }
}
