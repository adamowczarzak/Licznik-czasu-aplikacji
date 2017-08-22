using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ApplicationTimeCounter
{
    class MyCircle
    {
        Ellipse Mycircle;

        public MyCircle(Canvas canvas ,int r, int strokeThickness, string nameColor, double setLeft, double setTop,
            double opacity = 1, bool setFill = false)
        {
            Mycircle = new Ellipse()
            {
                Width = r,
                Height = r,
                StrokeThickness = strokeThickness,
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(nameColor),
                Opacity = opacity,              
            };
            if (setFill == true) Mycircle.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(nameColor);
            Canvas.SetLeft(Mycircle, setLeft);
            Canvas.SetTop(Mycircle, setTop);
            canvas.Children.Add(Mycircle);
        }

        public void Opacity(double value)
        {
            Mycircle.Opacity = value;
        }

        public void Color(string nameColor)
        {
            Mycircle.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(nameColor);
        }

    }
}
