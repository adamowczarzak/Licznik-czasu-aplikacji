using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApplicationTimeCounter
{
    class CategoryForm
    {
        public Canvas MainCanvasCategory { get; set; }
        private Canvas contentPage;
        private MyButton notAssignedApplications;

        private ViewContent viewContent;

        public CategoryForm(Canvas contentPage, ref ViewContent viewContent)
        {
            this.contentPage = contentPage;
            this.viewContent = viewContent;

            CreateCategoryForm();
        }

        private void CreateCategoryForm()
        {
            MainCanvasCategory = CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 0 ,0);
            MainCanvasCategory.Opacity = 0;
            Canvas.SetZIndex(MainCanvasCategory, 0);

            Canvas canvasMembership = CreateCanvas(MainCanvasCategory, 250, 350, Color.FromArgb(200, 130, 200, 255), 40, 30);
            Canvas canvasActivity = CreateCanvas(MainCanvasCategory, 250, 350, Color.FromArgb(200, 130, 200, 255), 330, 30);
            CreateImage(canvasMembership, 100, 100, 10, 10, "Pictures/MembershipImages.png");
            MyButton buttonAssignActivity = new MyButton(canvasActivity, 170, 38, 65, 200, Color.FromArgb(0, 130, 200, 255), Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(255, 255, 255, 255), "Przypisz Aktywności", 16);
            notAssignedApplications = new MyButton(canvasActivity, 50, 38, 10, 200, Color.FromArgb(0, 130, 200, 255), Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(255, 125, 255, 135), "0", 18);
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
                    notAssignedApplications.SetText(allNotAssignedApplicationInt.ToString());
                }
                else notAssignedApplications.SetText("-1");
            }
            else notAssignedApplications.SetText("-1");           
        }
    
    

        private Canvas CreateCanvas(Canvas _canvas, int width, int height, Color color, int x, int y)
        {
            Canvas canvas = new Canvas();
            canvas.Width = width;
            canvas.Height = height;
            canvas.Background = new SolidColorBrush(color);
            Canvas.SetLeft(canvas, x);
            Canvas.SetTop(canvas, y);
            _canvas.Children.Add(canvas);
            return canvas;
        }

        private Image CreateImage(Canvas _canvas, int width, int height, int x, int y, string nameImage)
        {
            Image image = new Image();
            image.Width = width;
            image.Height = height;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(nameImage, UriKind.Relative);
            Canvas.SetTop(image, x);
            Canvas.SetLeft(image, y);
            _canvas.Children.Add(image);
            src.EndInit();
            image.Source = src;

            return image;
        }
    }
}
