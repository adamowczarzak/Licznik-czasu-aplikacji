using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ApplicationTimeCounter
{
    static class ImageCreator
    {
        public static Image CreateImage(Canvas _canvas, int width, int height, int x, int y, string nameImage)
        {
            Image image = new Image();
            image.Width = width;
            image.Height = height;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(nameImage, UriKind.Relative);
            Canvas.SetTop(image, x);
            Canvas.SetLeft(image, y);
            _canvas.Children.Add(image);
            src.EndInit();
            image.Source = src;

            return image;
        }
    }
}
