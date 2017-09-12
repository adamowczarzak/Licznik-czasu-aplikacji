using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApplicationTimeCounter
{
    class CategoryForm
    {
        public Canvas MainCanvasCategory { get; set; }
        private Canvas contentPage;
        private MyLabel notAssignedApplications;
        private Label buttonAssignActivity;

        private ViewContent viewContent;

        public CategoryForm(Canvas contentPage, ref ViewContent viewContent)
        {
            this.contentPage = contentPage;
            this.viewContent = viewContent;

            CreateCategoryForm();
        }

        private void CreateCategoryForm()
        {
            MainCanvasCategory = CanvasCreator.CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 0, 0);
            MainCanvasCategory.Opacity = 0;
            Canvas.SetZIndex(MainCanvasCategory, 0);

            Canvas canvasMembership = CanvasCreator.CreateCanvas(MainCanvasCategory, 250, 350, Color.FromArgb(200, 130, 200, 255), 40, 30);
            Canvas canvasActivity = CanvasCreator.CreateCanvas(MainCanvasCategory, 250, 350, Color.FromArgb(200, 130, 200, 255), 330, 30);
            ImageCreator.CreateImage(canvasMembership, 100, 100, 10, 10, "Pictures/MembershipImages.png");
            notAssignedApplications = new MyLabel(canvasActivity, "0", 50, 38, 18, 10, 200, Color.FromArgb(255, 125, 255, 0), Color.FromArgb(255, 255, 255, 255), 1);
            buttonAssignActivity = ButtonCreator.CreateButton(canvasActivity, "Przypisz Aktywności", 170, 38, 16, 65, 200,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 255, 255), 1);

            buttonAssignActivity.MouseMove += buttonAssignActivity_MouseMove;
            buttonAssignActivity.MouseLeave += buttonAssignActivity_MouseLeave;
            buttonAssignActivity.MouseLeftButtonDown += buttonAssignActivity_MouseLeftButtonDown;
        }
            
          


        public void ShowCategoryForm()
        {
            viewContent.ChangeContent(MainCanvasCategory);
            UpdateView();
        }

        public void UpdateView()
        {
            GetNotAssignedApplicationsAndSetControl();
        }

        /// <summary>
        /// Pobiera ilość nie przypisanych aplikacji i ustawia kontrolkę 'notAssignedApplications'.
        /// </summary>
        private void GetNotAssignedApplicationsAndSetControl()
        {
            AllData_db allData_db = new AllData_db();
            string allNotAssignedApplicationString = allData_db.GetAllNotAssignedApplication();
            int allNotAssignedApplicationInt;
            if (int.TryParse(allNotAssignedApplicationString, out allNotAssignedApplicationInt))
            {
                DailyUseOfApplication_db DailyUseOfApplication_db = new DailyUseOfApplication_db();
                allNotAssignedApplicationString = DailyUseOfApplication_db.GetAllNotAssignedApplication();
                int temp = allNotAssignedApplicationInt;
                if (int.TryParse(allNotAssignedApplicationString, out allNotAssignedApplicationInt))
                {
                    allNotAssignedApplicationInt += temp;
                    notAssignedApplications.SetContent(allNotAssignedApplicationInt.ToString());
                    SetColorNotAssignedApplications(allNotAssignedApplicationInt);
                }
                else notAssignedApplications.SetContent("-1");
            }
            else notAssignedApplications.SetContent("-1");
        }


        private void SetColorNotAssignedApplications(int value)
        {
            int red = 125 + value * 2;
            int green = 255 - value * 2;
            red = (red > 255) ? 255 : red;
            green = (green < 0) ? 0 : green;
            notAssignedApplications.SetFontColor(Color.FromArgb(255, (byte)red, (byte)green, 0));
        }
        void buttonAssignActivity_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonAssignActivity.Background = new SolidColorBrush(Color.FromArgb(40, 0, 0, 150));
        }

        void buttonAssignActivity_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonAssignActivity.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        void buttonAssignActivity_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tempRef = MainCanvasCategory;
            AssignedActivity assignedActivity = new AssignedActivity(ref tempRef);
        }

    }
}
