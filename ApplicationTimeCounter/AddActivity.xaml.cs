using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for AddActivity.xaml
    /// </summary>
    public partial class AddActivity : Window
    {
        private bool IsClosed = false;
        public AddActivity()
        {
            InitializeComponent();

            ScrollViewer sv = new ScrollViewer();
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            sv.Height = 200;
            sv.Width = 150;
            Canvas.SetLeft(sv, 0);
            Canvas.SetTop(sv, 0);
            addActivityCanvas.Children.Add(sv);

            Canvas activity = new Canvas()
            {
                Width = 150,
                Height = 200,
            };
            sv.Content = activity;

            Label buttonActivity = CreateButton(activity, "Nazwa atywności", 150, 30, 12, 0, 0,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 255, 255), 1);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            { 
                IsClosed = true;
                this.Close();           
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (IsClosed == false)this.Close();          
        }

        private Label CreateButton(Canvas canvas, string content, int widthLabel, int heightLabel, int labelFontSize,
            double x, double y, Color colorFont, Color colorBorder, int borderThickness = 0,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            FontWeight fontWeight = default(FontWeight))
        {
            if (object.Equals(fontWeight, default(FontWeight))) fontWeight = FontWeights.Normal;

            Label button = new Label()
            {
                Content = content,
                Foreground = new SolidColorBrush(colorFont),
                FontSize = labelFontSize,
                Width = widthLabel,
                FontWeight = fontWeight,
                Height = heightLabel,
                FontFamily = new FontFamily("Comic Sans MS"),
                BorderThickness = new Thickness(borderThickness),
                BorderBrush = new SolidColorBrush(colorBorder),
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                Cursor = System.Windows.Input.Cursors.Hand,
                HorizontalContentAlignment = horizontalAlignment,

            };
            Canvas.SetLeft(button, x);
            Canvas.SetTop(button, y);
            canvas.Children.Add(button);
            return button;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsClosed = false;
        }

    }
}
