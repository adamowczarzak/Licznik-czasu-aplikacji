using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ApplicationTimeCounter
{

    class AnimatedClock
    {
        private int angleRotation;

        private int lastElementClockFace;
        private MyRectangle[] clockFace;
        private DispatcherTimer timerAnimation;
        private MyRectangle handOfClock;
        private Canvas canvas;
        private MyLabel remainingTimePercent;
        private MyLabel remainingTime;
        private CircleBar circleBar;


        public AnimatedClock(Canvas canvas)
        {
            angleRotation = 0;
            lastElementClockFace = 90;

            timerAnimation = new DispatcherTimer();
            timerAnimation.Tick += new EventHandler(AnimatedWatch);
            timerAnimation.Interval = new TimeSpan(0, 0, 0, 0, 40);

            MyLabel title = new MyLabel(canvas, "Pozostały czas", 120, 30, 14, 0, 0, Color.FromArgb(255, 47, 79, 79) , Color.FromArgb(0, 0, 0, 0));
            remainingTimePercent = new MyLabel(canvas, GetRemainingTimePercent(), 80, 45, 24, 55, 45, Color.FromArgb(255, 255, 0, 0), Color.FromArgb(0, 0, 0, 0));
            remainingTime = new MyLabel(canvas, GetRemainingTime(), 90, 30, 14, 50, 100, Color.FromArgb(255, 255, 0, 0), Color.FromArgb(0, 0, 0, 0));

            CreateAnimatedClock(canvas);
        }

        public void Update()
        {
            remainingTime.SetContent(GetRemainingTime());
            remainingTimePercent.SetContent(GetRemainingTimePercent());
            UpdateLookAnimatedClock();
        }

        public void CreateAnimatedClock(Canvas canvas)
        {
            this.canvas = canvas;
            CreateClockFace();

            MyCircle myCircle = new MyCircle(canvas, 110, 2, Color.FromArgb(255, 128, 128, 128), (this.canvas.Width / 2) - 53,
                (this.canvas.Height / 2) - 43, 0.5);
            circleBar = new CircleBar(this.canvas, 0.08, Color.FromArgb(255, 255, 99, 71), 4, 0, 10, 55, false);
            CreateHandOfClock();
            timerAnimation.Start();
            UpdateLookAnimatedClock();
        }

        public void UpdateLookAnimatedClock()
        {
            double percent = ((24 * 60) - GetRemainingTimeInMinuts()) / (24 * 60);
            if (GetRemainingTimeInMinuts() != 1) DeleteElementsAnimatedClock(percent);
            else RestartAnimatedClock();
        }

        private double GetRemainingTimeInMinuts()
        {
            string m = DateTime.Now.ToString("mm");
            string h = DateTime.Now.ToString("HH");
            return (60 * Convert.ToInt32(h) + Convert.ToInt32(m)) + 1;
        }
        private void AnimatedWatch(object sender, EventArgs e)
        {
            handOfClock.Rotate(angleRotation, 1.5, 54);
            angleRotation++;
            if (angleRotation > lastElementClockFace * 4) angleRotation = 0;
        }

        private void CreateHandOfClock()
        {
            handOfClock = new MyRectangle(canvas, 2, 54, Color.FromArgb(255, 0, 0, 0), (canvas.Width / 2) + 1,
                (canvas.Height / 2) - 42, opacity: 0.5);
        }

        private void CreateClockFace()
        {
            clockFace = new MyRectangle[lastElementClockFace];
            for (int i = 0; i < lastElementClockFace; i++)
            {
                clockFace[i] = new MyRectangle(canvas, 4, 54, Color.FromArgb(255, 255, 0, 0), (canvas.Width / 2) + 1,
                    (canvas.Height / 2) - 42, opacity: 0.1);
                clockFace[i].Rotate(i * 4, 1.5, 54);
            }
        }

        private void DeleteElementsAnimatedClock(double partActiv)
        {
            int newLastElementClockFace = Convert.ToInt32(Math.Round(clockFace.Length * partActiv));
            if (newLastElementClockFace > lastElementClockFace) RestartAnimatedClock();

            circleBar.SetValue(partActiv);
            for (int i = newLastElementClockFace; i < lastElementClockFace; i++)
            {
                clockFace[i].Opacity(0);
            }
            lastElementClockFace = newLastElementClockFace;
        }

        public string GetRemainingTimePercent()
        {
            int percent = Convert.ToInt32(((24 * 60) - GetRemainingTimeInMinuts()) / (24 * 60) * 100);
            return percent + " %";
        }

        public string GetRemainingTime()
        {
            int time = (24 * 60) - (Convert.ToInt32(GetRemainingTimeInMinuts()));
            return (time / 60) + " h " + ((time) - ((time / 60) * 60)) + " min";
        }

        private void RestartAnimatedClock()
        {
            circleBar.SetValue(1);
            lastElementClockFace = 90;
            for (int i = 0; i < lastElementClockFace; i++)
                clockFace[i].Opacity(0.05);
        }
    }
}
