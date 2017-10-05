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
        private MyLabel[] percentageOfActivity;
        private MyLabel[] growth;
        private MyLabel[] average;

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
            nameDay = new string[] { "Niedz", "Pon", "Wt", "Śr", "Czw", "Pt", "Sob" };
            percentageOfActivity = new MyLabel[2];
            growth = new MyLabel[2];
            average = new MyLabel[2];
            CreateControlUser();
            CreateChart();
            CreateListOfAddedApps();
            CreateTableWithInformation();
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
            ButtonCreator.SetToolTip(buttonAdd, "Dodaj nową aktywność");

            new MyLabel(mainCanvas, "-", 30, 50, 30, 560, 6, Color.FromArgb(255, 70, 70, 70),
               Color.FromArgb(255, 70, 70, 70), 0);
           

            Label buttonDelete = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 560, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonDelete.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonDelete.MouseMove += buttonDelete_MouseMove;
            buttonDelete.MouseLeave += buttonDelete_MouseLeave; 
            ButtonCreator.SetToolTip(buttonDelete, "Usuń aktywność");

            new MyLabel(mainCanvas, "x", 30, 50, 24, 590, 10, Color.FromArgb(255, 70, 70, 70),
                Color.FromArgb(255, 70, 70, 70), 0);
            

            Label buttonExit = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 590, 20,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(0, 155, 155, 155), 0);
            buttonExit.Background = new SolidColorBrush(Color.FromArgb(180, 215, 215, 215));
            buttonExit.MouseMove += buttonExit_MouseMove;
            buttonExit.MouseLeave += buttonExit_MouseLeave;
            buttonExit.MouseLeftButtonDown += buttonExit_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonExit, "Zamknij okno aktywności");
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
                scale[i] = new MyLabel(contentCanvas, (((maxValue / 3) * 3) - ((maxValue / 3) * i)).ToString() + " h", 30, 26, 12, 24, 56 + (i * 40), Color.FromArgb(180, 150, 150, 150), Color.FromArgb(0, 20, 20, 20));
                new MyRectangle(contentCanvas, 30, 1, Color.FromArgb(100, 150, 150, 150), 30, 80 + (i * 40));
            }

            int numberDayOfWeek = (int)DateTime.Now.DayOfWeek;
            for (int i = 0; i < 7; i++)
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

        private void CreateListOfAddedApps()
        {
            MyRectangle mr = new MyRectangle(contentCanvas, 185, 294, Color.FromArgb(0, 10, 10, 10), 420, 15, 2);
            MyRectangle mr2 = new MyRectangle(contentCanvas, 390, 294, Color.FromArgb(0, 10, 10, 10), 15, 15, 2);
            Canvas applicationInActivity = new Canvas() { Width = 140, Height = 146, Background = new SolidColorBrush(Color.FromArgb(255, 236, 236, 236)) };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(contentCanvas, 140, 146, 440, 60, applicationInActivity);

            new MyLabel(contentCanvas, "Dodane aplikacje", 140, 30, 14, 440, 20,
                Color.FromArgb(205, 125, 125, 125), Color.FromArgb(200, 255, 255, 255), 0);

            for (int i = 0; i < 20; i++)
            {
                new MyLabel(applicationInActivity, "test", 140, 30, 12, 0, 29 * i,
                        Color.FromArgb(255, 155, 155, 155), Color.FromArgb(200, 255, 255, 255), 0);
                new MyRectangle(applicationInActivity, 140, 1, Color.FromArgb(30, 150, 150, 150), 0, 30 + (29 * i));

                MyCircle circle = new MyCircle(applicationInActivity, 15, 1, (Color.FromArgb(155, 255, 93, 93)), 115, 8 + (29 * i), 1, true);
                Label buttonDeleteApplication = ButtonCreator.CreateButton(applicationInActivity, "x", 25, 25, 8, 110, 3 + (29 * i),
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 0, 0), 0, fontWeight: FontWeights.ExtraBold);
                buttonDeleteApplication.Background = new SolidColorBrush(Color.FromArgb(155, 236, 236, 236));
                buttonDeleteApplication.MouseMove += buttonDeleteApplication_MouseMove;
                buttonDeleteApplication.MouseLeave += buttonDeleteApplication_MouseLeave;
                buttonDeleteApplication.Name = "ID_" + i;
                ButtonCreator.SetToolTip(buttonDeleteApplication, "Usuń aplikacje z aktywności");
                applicationInActivity.Height += 29;
            }

            Label buttonDeleteAllApplication = ButtonCreator.CreateButton(contentCanvas, "Usuń wszystkie", 120, 28, 12, 450, 220,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(200, 255, 0, 0), 1);
            ButtonCreator.SetToolTip(buttonDeleteAllApplication, "Usuń wszystkie aplikacje z aktywności");
            buttonDeleteAllApplication.Background = new SolidColorBrush(Color.FromArgb(120, 255, 93, 93));
            buttonDeleteAllApplication.MouseMove += buttonDeleteAllApplication_MouseMove;
            buttonDeleteAllApplication.MouseLeave += buttonDeleteAllApplication_MouseLeave;

            Label buttonEditActivity = ButtonCreator.CreateButton(contentCanvas, "Edytuj aktywność", 120, 28, 12, 450, 260,
                   Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 255, 0), 1);
            buttonEditActivity.Background = new SolidColorBrush(Color.FromArgb(60, 0, 200, 0));
            buttonEditActivity.MouseMove += buttonEditActivity_MouseMove;
            buttonEditActivity.MouseLeave += buttonEditActivity_MouseLeave;

            applicationInActivity.Height = ((applicationInActivity.Height - 146) < 146) ? 146 : applicationInActivity.Height - 145;

        }

        private void CreateTableWithInformation()
        {
            new MyLabel(contentCanvas, "Dane tygodniowe", 120, 27, 11, 149, 220,
                       Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), 1);
            new MyLabel(contentCanvas, "Dane miesięczne", 120, 27, 11, 268, 220,
                       Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), 1);
            new MyLabel(contentCanvas, "Średnie użycie", 120, 23, 9, 30, 246,
                    Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), 0);
            new MyLabel(contentCanvas, "Wzrost do porzedniego", 120, 23, 9, 30, 266,
                    Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), 0);
            new MyLabel(contentCanvas, "Średnia wszystkich", 120, 23, 9, 30, 286,
                    Color.FromArgb(255, 115, 115, 115), Color.FromArgb(255, 117, 203, 255), 0);

            for (int i = 0; i < 3; i++ )
            {
                new MyRectangle(contentCanvas, 1, 86, Color.FromArgb(255, 117, 203, 255), 149 + (119 * i), 220);
                
                if(i < 2)
                {
                    percentageOfActivity[i] = new MyLabel(contentCanvas, "0 %", 50, 27, 11, 185 + (120 * i), 242,
                       Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 117, 203, 255));

                    growth[i] = new MyLabel(contentCanvas, "0 %", 50, 27, 11, 185 + (120 * i), 262,
                       Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 117, 203, 255));

                    average[i] = new MyLabel(contentCanvas, "0 %", 50, 27, 11, 185 + (120 * i), 282,
                       Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 117, 203, 255));

                }
            }
                
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

        private void buttonDeleteApplication_MouseMove(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 236, 236, 236));
        }

        private void buttonDeleteApplication_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(155, 236, 236, 236));
        }

        private void buttonDeleteAllApplication_MouseMove(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(180, 255, 93, 93));
        }

        private void buttonDeleteAllApplication_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 255, 93, 93));
        }

        private void buttonEditActivity_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(60, 0, 200, 0));
        }

        private void buttonEditActivity_MouseMove(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(160, 0, 250, 0));
        }
    }
}
