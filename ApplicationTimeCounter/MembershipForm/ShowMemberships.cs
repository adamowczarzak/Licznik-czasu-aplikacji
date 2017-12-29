using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using System.Windows;
using System.Windows.Input;
using System;
using System.Windows.Media.Imaging;
using ApplicationTimeCounter.ApplicationObjectsType;
using System.Collections.Generic;
using ApplicationTimeCounter.AdditionalWindows;

namespace ApplicationTimeCounter
{
    class ShowMemberships
    {
        private Canvas canvas;
        private Canvas mainCanvas;
        private Canvas contentCanvas;
        private TextBox nameGroup;
        private Label buttonSaveGroup;
        private Label buttonClose;
        private Label buttonDeleteGroup;
        private Label buttonShowApplications;
        private Label buttonActivationGroup;
        private Image[] activationGroup;
        private int editedGroupId;

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowShowMembershipsDelegate;

        public ShowMemberships(ref Canvas canvas)
        {
            this.canvas = canvas;

            mainCanvas = CanvasCreator.CreateCanvas(canvas, 600, 390, Color.FromArgb(255, 30, 39, 93), 10, 10);
            new MyRectangle(mainCanvas, 600, 35, Color.FromArgb(255, 0, 125, 255), 0, 0);
            new MyLabel(mainCanvas, "LP", 30, 30, 13, 2, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Nazwa", 200, 30, 13, 32, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Akcje", 100, 30, 13, 232, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Konfiguracja", 110, 30, 13, 332, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Dodana", 90, 30, 13, 442, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            new MyLabel(mainCanvas, "Aktywna", 70, 30, 13, 532, 2, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));


            contentCanvas = new Canvas() { Width = 600, Height = 310};
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(mainCanvas, 600, 310, 0, 45, contentCanvas);

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
            List<Membership> allGroups = Membership_db.GetAllGroups();
            Application.Current.Dispatcher.Invoke(() => {

            activationGroup = new Image[allGroups.Count];

            for (int i = 0; i < allGroups.Count; i++)
            {
                new MyRectangle(contentCanvas, 600, 40, Color.FromArgb((byte)(50 + (i % 2 * 30)), 0, 125, 255), 0, i * 39, 2).SetStroke(Color.FromArgb(255, 30, 39, 93));
                new MyLabel(contentCanvas, (i+1).ToString(), 30, 30, 12, 5, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));
                new MyLabel(contentCanvas, allGroups[i].Title, 200, 30, 12, 35, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));
                new MyLabel(contentCanvas, allGroups[i].Date, 90, 30, 12, 445, 5 + (i * 39), Color.FromArgb(200, 220, 220, 220), Color.FromArgb(230, 230, 0, 0));

                ImageCreator.CreateImage(contentCanvas, 20, 20, 245, 10 + (i * 39), "Pictures/rubbishBin.png");                
                buttonDeleteGroup = ButtonCreator.CreateButton(contentCanvas, "", 30, 30, 10, 240, 5 + (i * 39), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0));
                buttonDeleteGroup.Name = "ID_" + allGroups[i].ID.ToString();
                buttonDeleteGroup.MouseEnter += buttonGroup_MouseEnter;
                buttonDeleteGroup.MouseLeave += buttonGroup_MouseLeave;
                buttonDeleteGroup.MouseLeftButtonDown += buttonDeleteGroup_MouseLeftButtonDown;

                ImageCreator.CreateImage(contentCanvas, 20, 20, 295, 10 + (i * 39), "Pictures/eye.png");
                buttonShowApplications = ButtonCreator.CreateButton(contentCanvas, "", 30, 30, 10, 290, 5 + (i * 39), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0));
                buttonShowApplications.MouseEnter += buttonGroup_MouseEnter;
                buttonShowApplications.MouseLeave += buttonGroup_MouseLeave;
                buttonShowApplications.MouseLeftButtonDown += buttonShowApplications_MouseLeftButtonDown;
                
                string loadPictures = string.Empty;
                if (allGroups[i].IfConfiguration) loadPictures = "Pictures/checkSymbol.png";
                else loadPictures = "Pictures/cancel.png";

                ImageCreator.CreateImage(contentCanvas, 20, 20, 380, 10 + (i * 39), loadPictures);

                if (allGroups[i].IfActive)loadPictures = "Pictures/checkSymbol.png";
                else loadPictures = "Pictures/cancel.png";

                activationGroup[i] = ImageCreator.CreateImage(contentCanvas, 20, 20, 565, 10 + (i * 39), loadPictures);
                activationGroup[i].Name = "ID_" + allGroups[i].ID.ToString();
                buttonActivationGroup = ButtonCreator.CreateButton(contentCanvas, "", 30, 30, 10, 560, 5 + (i * 39), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0));
                buttonActivationGroup.Name = "ID_" + allGroups[i].ID.ToString();
                buttonActivationGroup.MouseEnter += buttonGroup_MouseEnter;
                buttonActivationGroup.MouseLeave += buttonGroup_MouseLeave;
                buttonActivationGroup.MouseLeftButtonDown += buttonActivationGroup_MouseLeftButtonDown;
                
                contentCanvas.Height += 39;
            }
            
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                contentCanvas.Height = ((contentCanvas.Height - 310) < 310) ? 310 : contentCanvas.Height - 309;
            });
        }

        private void buttonActivationGroup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int id = Convert.ToInt32(((sender as Label).Name).Replace("ID_", ""));
            int indexImage = 1;
            bool ifActivation = false;

            for (int i = 0; i < activationGroup.Length; i++)
                if (activationGroup[i].Name.Replace("ID_", "") == id.ToString()) indexImage = i;        

            ifActivation = (activationGroup[indexImage].Source.ToString().Contains("checkSymbol.png")) ? false : true;
            
            if (Membership_db.ActivationGroup(id.ToString(), ifActivation))
            {
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                if (ifActivation) src.UriSource = new Uri("Pictures/checkSymbol.png", UriKind.Relative);
                else src.UriSource = new Uri("Pictures/cancel.png", UriKind.Relative);
                src.EndInit();
                activationGroup[indexImage].Source = src;
            }
        }

        private void buttonShowApplications_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        
        }

        private void buttonDeleteGroup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editedGroupId = Convert.ToInt32(((sender as Label).Name).Replace("ID_", ""));
            DialogWindow dialogWindow = new DialogWindow(DialogWindowsState.YesCancel, DialogWindowsMessage.DeleteGroup);
            dialogWindow.CloseWindowCancelButtonDelegate += dialogWindow_CloseWindowCancelButtonDelegate;
            dialogWindow.CloseWindowAcceptButtonDelegate += DeleteGroup;
            dialogWindow.ShowDialog();
        }

        private void buttonAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!mainCanvas.Children.Contains(nameGroup))
            {
                nameGroup = new TextBox
                {
                    Width = 270,
                    Height = 26,
                    FontSize = 18,
                    MaxLength = 30,
                    Background = new SolidColorBrush(Color.FromArgb(255, 0, 123, 255)),
                };
                Canvas.SetLeft(nameGroup, 40);
                Canvas.SetTop(nameGroup, 368);
                mainCanvas.Children.Add(nameGroup);
                nameGroup.PreviewMouseLeftButtonDown += nameGroup_DisableError;
                nameGroup.PreviewMouseRightButtonDown += nameGroup_DisableError;

                buttonSaveGroup = ButtonCreator.CreateButton(mainCanvas, "Dodaj", 80, 28, 12, 320, 368,
                   Color.FromArgb(255, 55, 55, 55), Color.FromArgb(255, 255, 255, 255), 1);
                buttonSaveGroup.Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
                buttonSaveGroup.MouseEnter += buttonSaveGroup_MouseEnter;
                buttonSaveGroup.MouseLeave += buttonSaveGroup_MouseLeave;
                buttonSaveGroup.MouseLeftButtonDown += buttonSaveGroup_MouseLeftButtonDown;

                buttonClose = ButtonCreator.CreateButton(mainCanvas, "X", 25, 25, 11, 415, 370,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 255, 255), 1);
                buttonClose.Background = new SolidColorBrush(Color.FromArgb(185, 240, 0, 0));
                buttonClose.FontWeight = FontWeights.ExtraBold;
                ButtonCreator.SetToolTip(buttonClose, "Anuluj");
                buttonClose.MouseEnter += buttonClose_MouseEnter;
                buttonClose.MouseLeave += buttonClose_MouseLeave;
                buttonClose.MouseLeftButtonDown += buttonClose_MouseLeftButtonDown;
            }
        }

        private void buttonSaveGroup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Membership_db.CheckIfExistName(nameGroup.Text))
            {
                Membership_db.Add(nameGroup.Text);
                CloseAddingGroup();
            }
            else
            {
                nameGroup.Background = new SolidColorBrush(Color.FromArgb(220, 255, 55, 55));
            }          
        }

        private void buttonExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            contentCanvas.Children.Clear();
            mainCanvas.Children.Clear();
            this.canvas.Children.Remove(mainCanvas);
            CloseWindowShowMembershipsDelegate();
        }

        private void buttonClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CloseAddingGroup();
        }

        private void CloseAddingGroup()
        {
            mainCanvas.Children.Remove(nameGroup);
            mainCanvas.Children.Remove(buttonSaveGroup);
            mainCanvas.Children.Remove(buttonClose);

            contentCanvas.Children.Clear();
            LoadGroups();
        }

        private void dialogWindow_CloseWindowCancelButtonDelegate() { }

        private void DeleteGroup()
        {
            if (Membership_db.DeleteAllApplicationsWithGroup(editedGroupId))
            {
                if (Membership_db.DeleteGroup(editedGroupId))
                {
                    contentCanvas.Children.Clear();
                    LoadGroups();
                }
            }
        }

        private void buttonAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
        }

        private void buttonAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 255, 140));
        }

        private void buttonExit_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
        }

        private void buttonExit_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(120, 132, 177, 255));
        }

        private void nameGroup_DisableError(object sender, MouseButtonEventArgs e)
        {
            nameGroup.Background = new SolidColorBrush(Color.FromArgb(255, 0, 123, 255));
        }

        private void buttonSaveGroup_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
        }

        private void buttonSaveGroup_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(155, 200, 200, 200));
        }

        private void buttonClose_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(185, 240, 0, 0));
        }

        private void buttonClose_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(255, 240, 0, 0));
        }

        private void buttonGroup_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
        }

        private void buttonGroup_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(80, 215, 215, 215));
        }
    }
}
