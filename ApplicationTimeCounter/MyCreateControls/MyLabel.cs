using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace ApplicationTimeCounter.Controls
{
    class MyLabel : Label
    {
        private Label myLabel = new Label();

        public MyLabel(Canvas canvas, string content, int widthLabel, int heightLabel, int labelFontSize,
            double x, double y, Color colorFont, Color colorBorder, int borderThickness = 0,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            FontWeight fontWeight = default(FontWeight))
        {
            if (object.Equals(fontWeight, default(FontWeight))) fontWeight = FontWeights.Normal;

            myLabel = new Label()
            {
                Content = content,
                Foreground = new SolidColorBrush(colorFont),
                FontSize = labelFontSize,
                Width = widthLabel,
                FontWeight = fontWeight,
                Height = heightLabel,
                FontFamily = new FontFamily("Comic Sans MS"),
                HorizontalContentAlignment = horizontalAlignment,
                BorderThickness = new Thickness(borderThickness),
                BorderBrush = new SolidColorBrush(colorBorder),

            };
           
            Canvas.SetLeft(myLabel, x);
            Canvas.SetTop(myLabel, y);
            canvas.Children.Add(myLabel);

           // myLabel.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Blue");
        }

        public void SetContent(string contentLabel)
        {
            myLabel.Content = contentLabel;
        }

        public string GetContent()
        {
            return myLabel.Content.ToString();
        }

        public void Position(double x = default(double), double y = default(double))
        {
            if (object.Equals(x, default(double))) x = Canvas.GetLeft(myLabel);
            if (object.Equals(y, default(double))) y = Canvas.GetTop(myLabel);
            Canvas.SetLeft(myLabel, x);
            Canvas.SetTop(myLabel, y);
        }

        public new void Opacity(double value)
        {
            myLabel.Opacity = value;
        }

        public void SetBackgroundColor(Color color)
        {
            myLabel.Background = new SolidColorBrush(color);
        }

        public void SetFontColor(Color color)
        {
            myLabel.Foreground = new SolidColorBrush(color);
        }

        public void SetFont(string nameFont)
        {
            myLabel.FontFamily = new FontFamily(nameFont);
        }

        public new void ToolTip(string content)
        {
            myLabel.ToolTip = content;
            ToolTipService.SetShowDuration(myLabel, 5000);
            ToolTipService.SetInitialShowDelay(myLabel, 1000);
        }

        public void Resize(int height, int width)
        {
            myLabel.Height = height;
            myLabel.Width = width;
        }

        new public Visibility Visibility
        {
            get
            {
                return myLabel.Visibility;
            }
            set
            {
                myLabel.Visibility = value;
            }
        }

    }
}
