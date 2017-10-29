using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ApplicationTimeCounter
{
    class ViewContent
    {
        private int speedOfMovement;
        private DispatcherTimer timerAnimation;
        private Canvas lastContentPage;
        private Canvas newContentPage;

        public ViewContent()
        {
            speedOfMovement = 0;
            timerAnimation = new DispatcherTimer();
            timerAnimation.Tick += new EventHandler(MoveAnimation);
            timerAnimation.Interval = new TimeSpan(0, 0, 0, 0, 10);
            lastContentPage = null;
        }

        public void ChangeContent(Canvas newContentPage)
        {
            if (timerAnimation.IsEnabled == false)
            {
                lastContentPage = this.newContentPage;
                this.newContentPage = newContentPage;
                if (this.newContentPage != lastContentPage)
                {
                    TranslateTransform moveStartPosition = new TranslateTransform(this.newContentPage.Width, 0);
                    this.newContentPage.RenderTransform = moveStartPosition;
                    timerAnimation.Start();
                }
            }
        }

        private void MoveAnimation(object sender, EventArgs e)
        {
            MoveCanvas();
            SetOpacity();
            if (speedOfMovement < -1020) EndAnimation();

        }

        private void MoveCanvas()
        {
            TranslateTransform positionLastContentPage = new TranslateTransform(speedOfMovement, 0);
            TranslateTransform positionNewContentPage = new TranslateTransform(newContentPage.Width + 400 + speedOfMovement, 0);
            if (lastContentPage != null) lastContentPage.RenderTransform = positionLastContentPage;
            newContentPage.RenderTransform = positionNewContentPage;
            speedOfMovement = speedOfMovement - 20;
        }

        private void SetOpacity()
        {
            if (lastContentPage != null) lastContentPage.Opacity -= 0.02;
            newContentPage.Opacity += 0.02;
        }

        private void EndAnimation()
        {
            timerAnimation.Stop();
            if (lastContentPage != null) lastContentPage.Opacity = 0;
            newContentPage.Opacity = 1;
            speedOfMovement = 0;

            if (lastContentPage != null)
            {
                TranslateTransform startPosition = new TranslateTransform(newContentPage.Width + 400, 0);
                lastContentPage.RenderTransform = startPosition;
            }
        }
    }
}
