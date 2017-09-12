using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ApplicationTimeCounter
{
    static class ScrollViewerCreator
    {
        public static ScrollViewer CreateScrollViewer(Canvas canvas, int width, int height, int x, int y, Canvas contentCanvas)
        {
            ScrollViewer sv = new ScrollViewer();
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            sv.Height = height;
            sv.Width = width;
            Canvas.SetLeft(sv, x);
            Canvas.SetTop(sv, y);
            canvas.Children.Add(sv);
            sv.Content = contentCanvas;
            return sv;
        }
    }
}
