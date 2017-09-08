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
            MainCanvasCategory = CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 0, 0);
            MainCanvasCategory.Opacity = 0;
            Canvas.SetZIndex(MainCanvasCategory, 0);

            Canvas canvasMembership = CreateCanvas(MainCanvasCategory, 250, 350, Color.FromArgb(200, 130, 200, 255), 40, 30);
            Canvas canvasActivity = CreateCanvas(MainCanvasCategory, 250, 350, Color.FromArgb(200, 130, 200, 255), 330, 30);
            CreateImage(canvasMembership, 100, 100, 10, 10, "Pictures/MembershipImages.png");
            notAssignedApplications = new MyLabel(canvasActivity, "0", 50, 38, 18, 10, 200, Color.FromArgb(255, 125, 255, 0), Color.FromArgb(255, 255, 255, 255), 1);
            buttonAssignActivity = CreateButton(canvasActivity, "Przypisz Aktywności", 170, 38, 16, 65, 200,
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
                    SetColorNotAssignedApplications(0);
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

        private Label CreateButton(Canvas canvas, string content, int widthLabel, int heightLabel, int labelFontSize,
            double x, double y, Color colorFont, Color colorBorder, int borderThickness = 0,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            FontWeight fontWeight = default(FontWeight))
        {
            if (object.Equals(fontWeight, default(FontWeight))) fontWeight = FontWeights.Normal;

            Label button = new Label()
            {
                Content = content,
                Foreground = new SolidColorBrush(colorFont),
                FontSize = labelFontSize,
                Width = widthLabel,
                FontWeight = fontWeight,
                Height = heightLabel,
                FontFamily = new FontFamily("Comic Sans MS"),
                BorderThickness = new Thickness(borderThickness),
                BorderBrush = new SolidColorBrush(colorBorder),
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                Cursor = System.Windows.Input.Cursors.Hand,
                
            };
            Canvas.SetLeft(button, x);
            Canvas.SetTop(button, y);
            canvas.Children.Add(button);
            return button;
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
            AssignedActivity assignedActivity = new AssignedActivity(MainCanvasCategory);
        }

    }
}
