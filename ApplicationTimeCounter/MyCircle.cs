﻿using System;
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

        public MyCircle(Canvas canvas ,int r, int strokeThickness, Color color, double setLeft, double setTop,
            double opacity = 1, bool setFill = false)
        {
            Mycircle = new Ellipse()
            {
                Width = r,
                Height = r,
                StrokeThickness = strokeThickness,
                Stroke = new SolidColorBrush(color),
                Opacity = opacity,              
            };
            if (setFill == true) Mycircle.Fill = new SolidColorBrush(color);
            Canvas.SetLeft(Mycircle, setLeft);
            Canvas.SetTop(Mycircle, setTop);
            canvas.Children.Add(Mycircle);
        }

        public void Opacity(double value)
        {
            Mycircle.Opacity = value;
        }

        public void Color(Color color)
        {
            Mycircle.Stroke = new SolidColorBrush(color);
        }

    }
}
