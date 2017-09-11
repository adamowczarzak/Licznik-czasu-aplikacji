using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ApplicationTimeCounter
{
    class AssignedActivity
    {
        Canvas mainCanvas;
        Canvas nonAssignedApplications;
        Label buttonCloseCanvas;
        Canvas canvas;

        public AssignedActivity(ref Canvas canvas)
        {
            this.canvas = canvas;
            mainCanvas = new Canvas()
            {
                Width = 620,
                Height = 410,
                Background = new SolidColorBrush(Color.FromArgb(255, 226, 240, 255)),

            };
            Canvas.SetLeft(mainCanvas, 0);
            Canvas.SetTop(mainCanvas, 0);
            canvas.Children.Add(mainCanvas);

            nonAssignedApplications = new Canvas()
            {
                Width = 600,
                Height = 300,
            };

            ScrollViewer sv = new ScrollViewer();
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            sv.Height = 300;
            sv.Width = 560;
            //  sv.Background = new SolidColorBrush(Color.FromArgb(255, 255, 34, 45));
            Canvas.SetLeft(sv, 30);
            Canvas.SetTop(sv, 60);
            mainCanvas.Children.Add(sv);
            sv.Content = nonAssignedApplications;

            MyLabel labelNonAssignedApplication = new MyLabel(mainCanvas, "Aplikacje bez przypisanej aktywności",
                370, 38, 18, 30, 10, Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255));
            labelNonAssignedApplication.SetFont("Verdana");
            MyRectangle r = new MyRectangle(mainCanvas, 560, 4, Color.FromArgb(255, 220, 220, 220), 30, 45);

            buttonCloseCanvas = CreateButton(mainCanvas, "Zamknij", 80, 30, 13, 510, 370,
                Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255), 1);
            buttonCloseCanvas.MouseLeftButtonDown += buttonCloseCanvas_MouseLeftButtonDown;


            UpdateContent();

        }


        void buttonCloseCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainCanvas.Children.RemoveRange(0, mainCanvas.Children.Count);
            this.canvas.Children.Remove(mainCanvas);

        }

        void UpdateContent()
        {
            LoadNonAssignedApplication();
        }

        void LoadNonAssignedApplication()
        {
            nonAssignedApplications.Height = 0;
            for (int i = 0; i < 10; i++)
            {
                Canvas nonAssignedAppCanvas = new Canvas()
                {
                    Width = 560,
                    Height = 60,
                };
                Canvas.SetLeft(nonAssignedAppCanvas, 0);
                Canvas.SetTop(nonAssignedAppCanvas, (59 * i));
                nonAssignedApplications.Children.Add(nonAssignedAppCanvas);


                MyCircle circle = new MyCircle(nonAssignedAppCanvas, 46, 2, Color.FromArgb(255, 150, 150, 150), 8, 8, 1, true);
                MyLabel nonAssignedApplication = new MyLabel(nonAssignedAppCanvas, "\tAplikacje bez przypisanej aktywności\t(" + (10 - i) + ")",
                560, 60, 14, 0, 0, Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100), 1,
                HorizontalAlignment.Left, fontWeight: FontWeights.Bold);

                MyLabel lab = new MyLabel(nonAssignedAppCanvas, "B",
                50, 50, 20, 6, 11, Color.FromArgb(255, 240, 240, 240), Color.FromArgb(0, 100, 100, 100), 0,
                HorizontalAlignment.Center, fontWeight: FontWeights.ExtraBold);

                MyLabel membership = new MyLabel(nonAssignedAppCanvas, "Brak przynależności",
                300, 30, 12, 60, 30, Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100),
                horizontalAlignment: HorizontalAlignment.Left);

                MyLabel dayAgo = new MyLabel(nonAssignedAppCanvas, "2 dni temu",
                100, 30, 13, 466, 0, Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100),
                horizontalAlignment: HorizontalAlignment.Left);


                Ellipse Mycircle = new Ellipse()
                {
                    Width = 25,
                    Height = 25,
                    Fill = new SolidColorBrush(Color.FromArgb(255, 0, 123, 255)),
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 30, 255)),
                };
                Canvas.SetLeft(Mycircle, 525);
                Canvas.SetTop(Mycircle, 28);
                nonAssignedAppCanvas.Children.Add(Mycircle);

                Label buttonAddActivity = CreateButton(nonAssignedAppCanvas, "+", 25, 34, 20, 525, 28,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 0, 0), 0, fontWeight: FontWeights.ExtraBold);
                buttonAddActivity.Margin = new Thickness(0, -8, 0, 0);
                buttonAddActivity.MouseLeftButtonDown += buttonAddActivity_MouseLeftButtonDown;
                buttonAddActivity.Name = "DV"; //to będzie id aplikacji.


                nonAssignedApplications.Height += 60;
            }
        }

        void buttonAddActivity_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label btn = (Label)sender;
            AddActivity addActivity = new AddActivity();
            var location = btn.PointToScreen(new Point(0, 0));
            addActivity.Left = location.X + 16;
            addActivity.Top = location.Y + 20;
            addActivity.ShowDialog();
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
    }
}
