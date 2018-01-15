using ApplicationTimeCounter.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ApplicationTimeCounter
{
    class SettingsForm
    {
        public Canvas MainCanvasSettings { get; set; }
        private Canvas contentPage;

        private ViewContent viewContent;

        public SettingsForm(Canvas contentPage, ref ViewContent viewContent)
        {
            this.contentPage = contentPage;
            this.viewContent = viewContent;
            viewContent.ContentDelegateLoad += viewContent_Delegate;

            CreateSettingsForm();
        }

        public void ShowSettingsForm()
        {
            viewContent.ChangeContent(MainCanvasSettings);
            UpdateView();
        }

        public void UpdateView()
        {

        }

        private void viewContent_Delegate(Visibility visibility)
        {

        }

        private void CreateSettingsForm()
        {
            MainCanvasSettings = CanvasCreator.CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 0, 0);
            MainCanvasSettings.Opacity = 0;
            Canvas.SetZIndex(MainCanvasSettings, 0);
        }
    }
}
