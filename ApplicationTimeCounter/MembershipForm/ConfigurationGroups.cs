
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace ApplicationTimeCounter
{
    class ConfigurationGroups
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas contentCanvas;
        private Canvas addConfigurationCanvas;
        private Canvas chooseGroupCanvas;
        private Label chooseGroup;


        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowShowMembershipsDelegate;

        public ConfigurationGroups(ref Canvas canvas)
        {
            this.canvas = canvas;

            mainCanvas = CanvasCreator.CreateCanvas(canvas, 600, 390, Color.FromArgb(255, 30, 39, 93), 10, 10);
            new MyRectangle(mainCanvas, 600, 35, Color.FromArgb(255, 0, 125, 255), 0, 0);
            new MyLabel(mainCanvas, "Konfiguracje grup", 600, 40, 15, 0, 0, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));

            LoadGroupName();
            CreateControlUser();
        }

        private void LoadGroupName()
        {
            contentCanvas = new Canvas() { Width = 300, Height = 316 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(mainCanvas, 300, 316, 0, 50, contentCanvas);
            Dictionary<string, string> namesGroup = Membership_db.GetNameGroupsDictionaryWithConfiguration();

            int nextIndex = 0;
            foreach (KeyValuePair<string, string> name in namesGroup)
            {
                Label group = ButtonCreator.CreateButton(contentCanvas, name.Value, 250, 29, 12, 20, 0 + (nextIndex * 32),
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
                group.Background = new SolidColorBrush(Color.FromArgb((byte)(50 + (nextIndex % 2 * 30)), 0, 125, 255));
                group.Name = "ID_" + name.Key;
                group.MouseEnter += buttonContent_MouseEnter;
                group.MouseLeave += buttonContent_MouseLeave;
                group.MouseLeftButtonDown += buttonAdd_MouseLeftButtonDown;
                ButtonCreator.SetToolTip(group, "Edytuj");

                contentCanvas.Height += 32;
                nextIndex++;
            }
            contentCanvas.Height = ((contentCanvas.Height - 316) < 316) ? 316 : contentCanvas.Height - 315;
        }
        private void CreateControlUser()
        {
            Label buttonAdd = ButtonCreator.CreateButton(mainCanvas, "Dodaj nową", 150, 30, 14, 320, 50,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            buttonAdd.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            buttonAdd.MouseEnter += buttonContent_MouseEnter;
            buttonAdd.MouseLeave += buttonContent_MouseLeave;
            buttonAdd.MouseLeftButtonDown += buttonAdd_MouseLeftButtonDown;

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

        private void buttonAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            addConfigurationCanvas = CanvasCreator.CreateCanvas(mainCanvas, 600, 390, Color.FromArgb(255, 30, 39, 93), 0, 0);


            SetConfigurationButton();
        }

        private void SetConfigurationButton()
        {
            chooseGroup = ButtonCreator.CreateButton(addConfigurationCanvas, "Wybierz grupę", 200, 30, 14, 20, 20,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            chooseGroup.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            chooseGroup.MouseEnter += buttonContent_MouseEnter;
            chooseGroup.MouseLeave += buttonContent_MouseLeave;
            chooseGroup.MouseLeftButtonDown += buttonOpenChooseGroup;


            new MyLabel(addConfigurationCanvas, "x", 30, 50, 24, 573, 356, Color.FromArgb(255, 170, 170, 170),
                Color.FromArgb(255, 70, 70, 70), 0);

            Label buttonExit = ButtonCreator.CreateButton(addConfigurationCanvas, "", 30, 30, 14, 573, 366,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(255, 155, 155, 155), 1);
            buttonExit.Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
            buttonExit.MouseEnter += buttonExit_MouseEnter;
            buttonExit.MouseLeave += buttonExit_MouseLeave;
            buttonExit.MouseLeftButtonDown += buttonExitConfigure_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(buttonExit, "Zamknij okno");
        }

        private void buttonOpenChooseGroup(object sender, MouseButtonEventArgs e)
        {
            chooseGroupCanvas = new Canvas() { Width = 200, Height = 190 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(addConfigurationCanvas, 200, 190, 20, 60, chooseGroupCanvas);
            Dictionary<string, string> namesGroup = Membership_db.GetNameGroupsDictionaryWithoutConfiguration();
            chooseGroup.MouseLeftButtonDown -= buttonOpenChooseGroup;
            chooseGroup.MouseLeftButtonDown += buttonCloseChooseGroup;

            int nextIndex = 0;
            //foreach (KeyValuePair<string, string> name in namesGroup)
            for (int i = 0; i < 100; i++ )
            {
                Label group = ButtonCreator.CreateButton(chooseGroupCanvas, "name.Value" + i, 200, 29, 12, 0, 0 + (nextIndex * 32),
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
                group.Background = new SolidColorBrush(Color.FromArgb((byte)(50 + (nextIndex % 2 * 30)), 0, 125, 255));
                group.Name = "ID_";//+ "name.Key";
                group.MouseEnter += buttonContent_MouseEnter;
                group.MouseLeave += buttonContent_MouseLeave;
                group.MouseLeftButtonDown += buttonChooseGroup_MouseLeftButtonDown;

                chooseGroupCanvas.Height += 32;
                nextIndex++;
            }
            chooseGroupCanvas.Height = ((chooseGroupCanvas.Height - 190) < 190) ? 190 : chooseGroupCanvas.Height - 189;
        }

        private void buttonChooseGroup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CloseChooseGroup();
        }

        private void buttonCloseChooseGroup(object sender, MouseButtonEventArgs e)
        {
            CloseChooseGroup();
        }

        private void CloseChooseGroup()
        {
            chooseGroupCanvas.Children.Clear();
            chooseGroup.MouseLeftButtonDown -= buttonCloseChooseGroup;
            chooseGroup.MouseLeftButtonDown += buttonOpenChooseGroup;
        }

        private void buttonExitConfigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            chooseGroupCanvas.Children.Clear();
            addConfigurationCanvas.Children.Clear();
            mainCanvas.Children.Remove(addConfigurationCanvas);
        }

        private void buttonExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            contentCanvas.Children.Clear();
            mainCanvas.Children.Clear();
            this.canvas.Children.Remove(mainCanvas);
            CloseWindowShowMembershipsDelegate();
        }

        private void buttonExit_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
        }

        private void buttonExit_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 177, 255));
        }

        private void buttonContent_MouseLeave(object sender, MouseEventArgs e)
        {
            Color t = ((SolidColorBrush)(sender as Label).Background).Color;
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb((byte)(t.A - 150), 0, 125, 255));
        }

        private void buttonContent_MouseEnter(object sender, MouseEventArgs e)
        {
            Color t = ((SolidColorBrush)(sender as Label).Background).Color;
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb((byte)(t.A + 150), 0, 125, 255));
        }

    }
}
