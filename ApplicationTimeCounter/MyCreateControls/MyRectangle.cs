using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ApplicationTimeCounter.Controls
{
    class MyRectangle
    {
        Rectangle myRectangle;
        MyLabel toolTip;

        public MyRectangle(Canvas canvas, int width, int height, Color color, double x, double y,
            int strokeThickness = 0, double opacity = 1)
        {
            myRectangle = new Rectangle()
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(color),
                StrokeThickness = strokeThickness,
                Stroke = new SolidColorBrush(Color.FromArgb(255,255,255,255)),
                Opacity = opacity
            };
            Canvas.SetLeft(myRectangle, x);
            Canvas.SetTop(myRectangle, y);
            canvas.Children.Add(myRectangle);

            toolTip = new MyLabel(canvas, "", width, height, 10, x, y, Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0));
        }

        public void Resize(int height, int width)
        {
            myRectangle.Height = height;
            myRectangle.Width = width;
        }

        public void Position(double x = default(double), double y = default(double))
        {
            if (object.Equals(x, default(double))) x = Canvas.GetLeft(myRectangle);
            if (object.Equals(y, default(double))) y = Canvas.GetTop(myRectangle);
            Canvas.SetLeft(myRectangle, x);
            Canvas.SetTop(myRectangle, y);
        }

        public void Rotate(double angle, double centerX, double centerY)
        {
            RotateTransform rotate = new RotateTransform(angle, centerX, centerY);
            myRectangle.RenderTransform = rotate;
        }

        public void Opacity( double value )
        {
            myRectangle.Opacity = value;
        }

        public void SetFillColor(Color color)
        {
            myRectangle.Fill = new SolidColorBrush(color);
        }

        public void ToolTip(string content)
        {
            toolTip.ToolTip(content);
        }

    }
}
