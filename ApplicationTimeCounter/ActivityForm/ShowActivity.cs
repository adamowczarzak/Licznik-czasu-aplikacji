using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ApplicationTimeCounter
{
    class ShowActivity
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas contentCanvas;
        private MyLabel nameActivity;
        private MyRectangle[] charts;
        private MyLabel[] scale;
        private string[] nameDay;

        public ShowActivity(ref Canvas canvas)
        {
            this.canvas = canvas;
            mainCanvas = CanvasCreator.CreateCanvas(canvas, 620, 410, Color.FromArgb(255, 226, 240, 255), 0, 0);
            this.canvas.KeyDown += mainCanvas_KeyDown;
            contentCanvas = CanvasCreator.CreateCanvas(mainCanvas, 620, 320, Color.FromArgb(255, 236, 236, 236), 0, 50);

            new MyRectangle(mainCanvas, 620, 1, Color.FromArgb(30, 110, 110, 110), 0, 50);
            new MyRectangle(mainCanvas, 620, 1, Color.FromArgb(50, 110, 110, 110), 0, 370);

            nameActivity = new MyLabel(mainCanvas, "Nazwa aktywnosci", 300, 36, 16, 15, 15, Color.FromArgb(255, 160, 160, 160),
                Color.FromArgb(0, 100, 100, 100), horizontalAlignment: HorizontalAlignment.Left);
            nameActivity.SetFont("Verdana");

            charts = new MyRectangle[7];
            scale = new MyLabel[4];
            nameDay = new string[]{"Niedz","Pon","Wt","Śr","Czw","Pt","Sob"};
            CreateControlUser();
            CreateChart();
            this.canvas.Focusable = true;
            this.canvas.Focus();
            
        }

        private void mainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {                
                // zmiana danych ++
            }
            if (e.Key == Key.Left)
            {
                // zmiana danych --
            }
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                BackgroundWorker backgroundWorkerLoadingWindow = new BackgroundWorker();
                backgroundWorkerLoadingWindow.DoWork += backgroundWorkerLoadingWindow_DoWork;
                backgroundWorkerLoadingWindow.RunWorkerAsync();
            }
        }

        private void backgroundWorkerLoadingWindow_DoWork(object sender, DoWorkEventArgs e)
        {
            Canvas canvasLoad = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                canvasLoad = CanvasCreator.CreateCanvas(mainCanvas, (int)mainCanvas.Width, (int)mainCanvas.Height, Color.FromArgb(255, 255, 255, 255), 0, 0);
                canvasLoad.Opacity = 0;
            });

            for (int i = 0; i < 30; i++)
            {
                Application.Current.Dispatcher.Invoke(() => { canvasLoad.Opacity += 0.04; });
                Thread.Sleep(20);
            }
            for (int i = 0; i < 30; i++)
            {
                Application.Current.Dispatcher.Invoke(() => { canvasLoad.Opacity -= 0.04; });
                Thread.Sleep(20);
            }
            Application.Current.Dispatcher.Invoke(() => { mainCanvas.Children.Remove(canvasLoad); this.canvas.Focus(); });
        }

        private void CreateControlUser()
        {
            new MyLabel(mainCanvas, "+", 30, 50, 30, 530, 6, Color.FromArgb(255, 70, 70, 70),
                Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonAdd = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 530, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonAdd.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonAdd.MouseMove += buttonAdd_MouseMove;
            buttonAdd.MouseLeave += buttonAdd_MouseLeave;

            new MyLabel(mainCanvas, "-", 30, 50, 30, 560, 6, Color.FromArgb(255, 70, 70, 70),
               Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonDelete = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 560, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonDelete.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonDelete.MouseMove += buttonDelete_MouseMove;
            buttonDelete.MouseLeave += buttonDelete_MouseLeave;

            new MyLabel(mainCanvas, "x", 30, 50, 24, 590, 10, Color.FromArgb(255, 70, 70, 70),
                Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonExit = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 590, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonExit.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonExit.MouseMove += buttonExit_MouseMove;
            buttonExit.MouseLeave += buttonExit_MouseLeave;
            buttonExit.MouseLeftButtonDown += buttonExit_MouseLeftButtonDown;
        }

        private void CreateChart()
        {
            for (int i = 0; i < 58; i++)
            {
                new MyRectangle(contentCanvas, 3, 1, Color.FromArgb(150, 150, 150, 150), 30 + (i * 6), 50);
            }

            int maxValue = 10;
            for (int i = 0; i < 4; i++)
            {
                scale[i] = new MyLabel(contentCanvas, (((maxValue/3) * 3) - ((maxValue/3) * i)).ToString(), 30, 26, 12, 20, 56 + (i * 40), Color.FromArgb(180, 150, 150, 150), Color.FromArgb(0, 20, 20, 20));
                new MyRectangle(contentCanvas, 30, 1, Color.FromArgb(100, 150, 150, 150), 30, 80 + (i * 40));
            }

            int numberDayOfWeek = (int)DateTime.Now.DayOfWeek;
            for(int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    new MyRectangle(contentCanvas, 1, 3, Color.FromArgb(100, 150, 150, 150), 88 + (i * 40), 50 + (j * 6));
                }
                charts[i] = new MyRectangle(contentCanvas, 16, 120, Color.FromArgb(255, 190, 190, 190), 80 + (i * 40), 80);
                if (i == 6) charts[i].SetFillColor(Color.FromArgb(255, 117, 203, 255));
                new MyCircle(contentCanvas, 4, 0, Color.FromArgb(255, 180, 180, 180), 86 + (i * 40), 48, 1, true);
                new MyLabel(contentCanvas, nameDay[GetNumberDayOfWeek(i)], 50, 26, 12, 64 + (i * 40), 24, Color.FromArgb(180, 150, 150, 150), Color.FromArgb(0, 20, 20, 20));

                Random rnd = new Random();               
                int value = rnd.Next(0, 120);
                Thread.Sleep(10);
                charts[i].Resize(value, 16);
                charts[i].Position(y: 200 - value);
            }
            new MyRectangle(contentCanvas, 350, 1, Color.FromArgb(150, 150, 150, 150), 30, 200);

        }

        private int GetNumberDayOfWeek(int nextDay)
        {
            int numberDayOfWeek = (int)DateTime.Now.DayOfWeek + 1 + nextDay;
            return ((numberDayOfWeek) < 7) ? (numberDayOfWeek) : numberDayOfWeek % 7;
        }

        private void buttonExit_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mainCanvas.Children.RemoveRange(0, mainCanvas.Children.Count);
            this.canvas.Children.Remove(mainCanvas);
        }

        private void buttonExit_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
        }

        private void buttonExit_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 177, 255));
        }

        private void buttonDelete_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
        }

        private void buttonDelete_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 255, 132, 132));
        }

        private void buttonAdd_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
        }

        private void buttonAdd_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 255, 140));
        }
    }
}
