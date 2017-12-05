using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using System.Windows;

namespace ApplicationTimeCounter
{
    class ShowMemberships
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas contentCanvas;


        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowShowMembershipsDelegate;

        public ShowMemberships(ref Canvas canvas)
        {
            this.canvas = canvas;

            mainCanvas = CanvasCreator.CreateCanvas(canvas, 600, 390, Color.FromArgb(255, 30, 39, 93), 10, 10);
            new MyRectangle(mainCanvas, 600, 35, Color.FromArgb(255, 0, 125, 255), 0, 0);
            new MyLabel(mainCanvas, "LP", 30, 30, 13, 2, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Nazwa", 200, 30, 13, 32, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Akcje", 70, 30, 13, 232, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Aplikacje", 70, 30, 13, 302, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Konfi.", 70, 30, 13, 372, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Dodana", 90, 30, 13, 442, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Aktywna", 70, 30, 13, 532, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            //new MyLabel(mainCanvas, "LP\t\tNazwa\t\tAkcje\t   Aplikacje\tKonfiguracja\tDodana\t     Aktywna  ", 600, 40, 12, 0, 5, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));

            contentCanvas = new Canvas() { Width = 600, Height = 310};
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(mainCanvas, 600, 310, 0, 45, contentCanvas);
            new MyRectangle(mainCanvas, 603, 1, Color.FromArgb(250, 110, 110, 110), 0, 359);

            CreateControlUser();
            LoadGroups();
        }

        private void CreateControlUser()
        {
            new MyLabel(mainCanvas, "+", 30, 50, 30, 0, 353, Color.FromArgb(255, 170, 170, 170),
                Color.FromArgb(255, 170, 170, 170), 0);

            Label buttonAdd = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 0, 366,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(255, 155, 155, 155), 1);
            buttonAdd.Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
            buttonAdd.MouseEnter += buttonAdd_MouseEnter;
            buttonAdd.MouseLeave += buttonAdd_MouseLeave;
            buttonAdd.MouseLeftButtonDown += buttonAdd_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonAdd, "Dodaj nową grupę");

            new MyLabel(mainCanvas, "x", 30, 50, 24, 573, 356, Color.FromArgb(255, 170, 170, 170),
                Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonExit = ButtonCreator.CreateButton(mainCanvas, "", 30, 30, 14, 573, 366,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(255, 155, 155, 155), 1);
            buttonExit.Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
            buttonExit.MouseEnter += buttonExit_MouseEnter;
            buttonExit.MouseLeave += buttonExit_MouseLeave;
            buttonExit.MouseLeftButtonDown += buttonExit_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonExit, "Zamknij okno");
        }

        private void LoadGroups()
        {
            Application.Current.Dispatcher.Invoke(() => {
            for(int i = 0; i < 10; i++)
            {
                new MyRectangle(contentCanvas, 600, 40, Color.FromArgb((byte)(50 + (i % 2 * 30)), 0, 125, 255), 0, i * 39, 2).SetStroke(Color.FromArgb(255, 30, 39, 93));
                new MyLabel(contentCanvas, (i+1).ToString(), 30, 30, 12, 5, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));
                new MyLabel(contentCanvas, "Jakaś tam nazwa "+ (i + 1), 200, 30, 12, 35, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));
                new MyLabel(contentCanvas, "1" + (i + 1), 70, 30, 12, 305, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));
                new MyLabel(contentCanvas, "Tak", 70, 30, 12, 375, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));
                new MyLabel(contentCanvas, "2017-11-22", 90, 30, 12, 445, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));
                new MyLabel(contentCanvas, "Tak", 70, 30, 12, 535, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));            
                contentCanvas.Height += 39;
            }
            
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                contentCanvas.Height = ((contentCanvas.Height - 310) < 310) ? 310 : contentCanvas.Height - 309;
            });
        }



        private void buttonAdd_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void buttonExit_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            contentCanvas.Children.Clear();
            mainCanvas.Children.Clear();
            this.canvas.Children.Remove(mainCanvas);
            CloseWindowShowMembershipsDelegate();
        }

        private void buttonAdd_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
        }

        private void buttonAdd_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 255, 140));
        }

        private void buttonExit_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
        }

        private void buttonExit_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 177, 255));
        }
    }
}
