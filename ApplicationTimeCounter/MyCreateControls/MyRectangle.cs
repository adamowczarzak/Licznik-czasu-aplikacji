using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ApplicationTimeCounter.Controls
{
    class MyRectangle
    {
        Rectangle myRectangle;
        MyLabel toolTip;
        bool enableResizeToolTip;

        public MyRectangle(Canvas canvas, int width, int height, Color color, double x, double y,
            int strokeThickness = 0, double opacity = 1)
        {
            myRectangle = new Rectangle()
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(color),
                StrokeThickness = strokeThickness,
                Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                Opacity = opacity
            };
            Canvas.SetLeft(myRectangle, x);
            Canvas.SetTop(myRectangle, y);
            canvas.Children.Add(myRectangle);

            enableResizeToolTip = true;
            toolTip = new MyLabel(canvas, "", width, height, 10, x, y, Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0));
        }

        public void Resize(int height, int width)
        {
            myRectangle.Height = height;
            myRectangle.Width = width;

            if (enableResizeToolTip)
                toolTip.Resize(height, width);
        }

        public void Position(double x = default(double), double y = default(double))
        {
            if (object.Equals(x, default(double))) x = Canvas.GetLeft(myRectangle);
            if (object.Equals(y, default(double))) y = Canvas.GetTop(myRectangle);
            Canvas.SetLeft(myRectangle, x);
            Canvas.SetTop(myRectangle, y);

            if (enableResizeToolTip)
                toolTip.Position(x, y);
        }

        public void Rotate(double angle, double centerX, double centerY)
        {
            RotateTransform rotate = new RotateTransform(angle, centerX, centerY);
            myRectangle.RenderTransform = rotate;
        }

        public void Opacity(double value)
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

        /// <summary>
        /// Rozszerzenie wyłapywania ToolTipa o dodatkowe wielkości szerokości i wysokości.
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="ifToZeroHeight">Czy wysokość rozrzerzając się zmierza do warości 0.</param>
        /// <param name="ifToZeroWidth">Czy szerokość rozrzerzając się zmierza do warości 0.</param>
        public void ToolTipResizeAbout(int height, int width, bool ifToZeroHeight = false, bool ifToZeroWidth = false)
        {
            int newHeight = ((int)myRectangle.Height + height > 0) ? (int)myRectangle.Height + height : 0;
            int newWidth = ((int)myRectangle.Width + width > 0) ? (int)myRectangle.Width + width : 0;

            height = ifToZeroHeight ? -height : height;
            width = ifToZeroWidth ? -width : width;

            toolTip.Resize(newHeight, newWidth);
            toolTip.Position(Canvas.GetLeft(myRectangle) + width, Canvas.GetTop(myRectangle) + height);
        }

        public void EnableResizeToolTip(bool ifResize)
        {
            enableResizeToolTip = ifResize;
        }

    }
}
