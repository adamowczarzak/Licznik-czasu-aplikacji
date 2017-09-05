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
            Image image_event = new Image();
           // image_event.Width = 100;
          //  image_event.Height = 100;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("Pictures/MembershipImages.png", UriKind.Relative);
            Canvas.SetTop(image_event, 10);
            Canvas.SetLeft(image_event, 10);
            canvasMembership.Children.Add(image_event);
            src.EndInit();
            image_event.Source = src;
            

            
        }

        public void ShowCategoryForm()
        {
            viewContent.ChangeContent(MainCanvasCategory);
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
    }
}
