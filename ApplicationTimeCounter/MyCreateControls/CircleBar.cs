using System;
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;

namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for CircleBar.xaml
    /// </summary>
    public partial class CircleBar
    {
        private MyCircle[] progressBar;
        private double value;

        public CircleBar(Canvas canvas, double increaseAngle, Color color, int widthBar,  int xCenter,
            int yCenter, int radiusBar, bool drawCloseCircle = true)
        {
     
            progressBar = new MyCircle[Convert.ToInt32((6.28/increaseAngle)+1)];
            double angle = 0;
            value = 1;
            if (drawCloseCircle == true)
            {
                MyCircle circle = new MyCircle(canvas, radiusBar * 2, 2, Color.FromArgb(255, 128, 128, 128),
                    (canvas.Width / 2) - 48, (canvas.Height / 2) - 33, 0.5);
            }

            for (int i = 0; i < progressBar.Length; i++)
            {
                double x = Math.Cos(angle - 1.55) * radiusBar + canvas.Width / 2 + xCenter;
                double y = Math.Sin(angle - 1.55) * radiusBar + canvas.Height / 2 + yCenter;
                progressBar[i] = new MyCircle(canvas, widthBar, 2, color, x, y, 1);
                angle = angle + increaseAngle;
            }         
        }

        public void SetValue(double value)
        {
            int newValue = Convert.ToInt32(Math.Round(value * progressBar.Length));
            int oldValue = Convert.ToInt32(Math.Round(this.value * progressBar.Length));
           
            if (newValue > oldValue)
            {
                for (int i = oldValue; i < newValue; i++) 
                    if (i < progressBar.Length) progressBar[i].Opacity(1);                
            }
            else if (newValue < oldValue)
            {
                for (int i = newValue; i < oldValue; i++)
                    if (i < progressBar.Length) progressBar[i].Opacity(0);
            }                           
            this.value = value;
        }

        public void SetColorsOnParts(double startPosition, double value, Color color)
        {
            for (int i = (int)(startPosition * progressBar.Length); i < 
                (int)((value + startPosition) * progressBar.Length)+1; i++)
                if (i < progressBar.Length) progressBar[i].Color(color);
        }

        public int GetLenghtprogressBar()
        {
           return progressBar.Length;
        }

       
    }
}
