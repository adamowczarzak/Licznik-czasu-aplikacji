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
           // MyLabel label = new MyLabel(canvasActivity, "Przypisz aktywności", 170, 40, 16, 60, 220, "White", fontWeight: System.Windows.FontWeights.Regular);
            //label.SetFont("Verdana");
           // MyLabel applicationsNotAssignedToActivity = new MyLabel(canvasActivity, " 0 ", 40, 40, 18, 10, 220, "Tomato", fontWeight: System.Windows.FontWeights.Bold);
           // applicationsNotAssignedToActivity.SetBackgroundColor("Silver");
            MyButton m = new MyButton(canvasActivity, 150, 40, 30, 30, Color.FromArgb(0, 130, 200, 255), Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(200, 130, 200, 255), "cos ss s ", 2);
        }

        public void ShowCategoryForm()
        {
            viewContent.ChangeContent(MainCanvasCategory);
        }

        public void UpdateView()
        {
            //
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
