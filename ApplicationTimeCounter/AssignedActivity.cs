using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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
                Background = new SolidColorBrush(Color.FromArgb(255, 226,240,255)),

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
            sv.Width = 450;
          //  sv.Background = new SolidColorBrush(Color.FromArgb(255, 255, 34, 45));
            Canvas.SetLeft(sv, 30);
            Canvas.SetTop(sv, 70);
            mainCanvas.Children.Add(sv);
            sv.Content = nonAssignedApplications;

            MyLabel labelNonAssignedApplication = new MyLabel(mainCanvas, "Aplikacje bez przypisanej aktywności",
                370, 38, 18, 30, 15, Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255));
            labelNonAssignedApplication.SetFont("Verdana");
            MyRectangle r = new MyRectangle(mainCanvas, 500, 4, Color.FromArgb(255, 220, 220, 220), 30, 50);

            buttonCloseCanvas = CreateButton(mainCanvas, "Zamknij", 100, 38, 18, 500, 350,
                Color.FromArgb(255, 0, 123, 255), Color.FromArgb(200, 0, 56, 255), 1);
            buttonCloseCanvas.MouseLeftButtonDown += buttonCloseCanvas_MouseLeftButtonDown;


            UpdateContent();

        }


        void buttonCloseCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainCanvas.Children.Clear();
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
                    Width = 450,
                    Height = 60,
                    //Background = new SolidColorBrush(Color.FromArgb(250, 30, 40, 20)),
                };
                Canvas.SetLeft(nonAssignedAppCanvas, 0);
                Canvas.SetTop(nonAssignedAppCanvas, (59 * i));
                nonAssignedApplications.Children.Add(nonAssignedAppCanvas);


                MyCircle circle = new MyCircle(nonAssignedAppCanvas, 46, 2, Color.FromArgb(255, 150, 150, 150), 8, 8, 1, true);
                MyLabel labelNonAssignedApplication2 = new MyLabel(nonAssignedAppCanvas, "\tAplikacje bez przypisanej aktywności",
                450, 60, 14, 0, 0, Color.FromArgb(255, 120, 120, 120), Color.FromArgb(30, 100, 100, 100), 1,
                HorizontalAlignment.Left, fontWeight: FontWeights.Bold);

                MyLabel lab = new MyLabel(nonAssignedAppCanvas, "B",
                50, 50, 20, 6, 11, Color.FromArgb(255, 240, 240, 240), Color.FromArgb(0, 100, 100, 100), 0,
                HorizontalAlignment.Center, fontWeight: FontWeights.ExtraBold);

                nonAssignedApplications.Height += 60;
            }
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
