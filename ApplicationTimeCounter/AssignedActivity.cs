using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ApplicationTimeCounter
{
    class AssignedActivity
    {
        Canvas mainCanvas;
        public AssignedActivity(Canvas canvas)
        {
            mainCanvas = new Canvas()
            {
                Width = 620,
                Height = 410,
                Background = new SolidColorBrush(Color.FromArgb(255, 184, 216, 244)),

            };
            Canvas.SetLeft(mainCanvas, 0);
            Canvas.SetTop(mainCanvas, 0);
            canvas.Children.Add(mainCanvas);

            Label l = new Label()
            {
                Width = 40,
                Height = 40,
                Background = new SolidColorBrush(Color.FromArgb(255, 45, 255, 233)),
            };
            Label l2 = new Label()
            {
                Width = 40,
                Height = 40,
                Background = new SolidColorBrush(Color.FromArgb(255, 45, 255, 33)),
            };

            Canvas.SetLeft(l, 30);
            Canvas.SetTop(l, 100);
            mainCanvas.Children.Add(l);

            Canvas.SetLeft(l2, 80);
            Canvas.SetTop(l2, 100);
            mainCanvas.Children.Add(l2);
        }
    }
}
