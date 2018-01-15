using ApplicationTimeCounter.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ApplicationTimeCounter
{
    class StatisticsForm
    {
        public Canvas MainCanvasStatistics { get; set; }
        private Canvas contentPage;

        private ViewContent viewContent;

        public StatisticsForm(Canvas contentPage, ref ViewContent viewContent)
        {
            this.contentPage = contentPage;
            this.viewContent = viewContent;
            viewContent.ContentDelegateLoad += viewContent_Delegate;

            CreateStatisticsForm();
        }

        public void ShowStatisticsForm()
        {
            viewContent.ChangeContent(MainCanvasStatistics);
            UpdateView();
        }

        public void UpdateView()
        {

        }

        private void viewContent_Delegate(Visibility visibility)
        {

        }

        private void CreateStatisticsForm()
        {
            MainCanvasStatistics = CanvasCreator.CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 0, 0);
            MainCanvasStatistics.Opacity = 0;
            Canvas.SetZIndex(MainCanvasStatistics, 0);

        }

    }
}
