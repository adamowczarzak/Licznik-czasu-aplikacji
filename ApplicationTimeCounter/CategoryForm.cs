using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

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
            MainCanvasCategory = new Canvas();
            MainCanvasCategory.Width = 620;
            MainCanvasCategory.Height = 410;
            MainCanvasCategory.Background = new SolidColorBrush(Color.FromArgb(255, 30, 39, 93));
            contentPage.Children.Add(MainCanvasCategory);
            MainCanvasCategory.Opacity = 0;
            Canvas.SetZIndex(MainCanvasCategory, 0);

            Canvas MainCanvas2 = new Canvas();
            MainCanvas2.Width = 200;
            MainCanvas2.Height = 200;
            MainCanvas2.Background = new SolidColorBrush(Color.FromArgb(255, 255, 9, 3));
            MainCanvasCategory.Children.Add(MainCanvas2);
        }

        public void ShowCategoryForm()
        {
            viewContent.ChangeContent(MainCanvasCategory);
        }
    }
}
