
using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

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
        private MyLabel ifActivity;
        private TextBox filterContent;
        private Label addFiltrAccept;
        private MyLabel addFiltrPlus;
        private Label addFiltrButton;
        private Label applyFilterButton;
        private MyLabel resultsApplyFilter;
        private Label deleteApplicationWithFilterButton;
        private MyLabel resultsDeleteApplication;
        private int selectGroupId;
        

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

        private void CreateAddFilterButton()
        {
            addFiltrPlus = new MyLabel(addConfigurationCanvas, "+", 30, 50, 24, 90, 260, Color.FromArgb(255, 170, 170, 170),
                Color.FromArgb(255, 70, 70, 70), 0);

            addFiltrButton = ButtonCreator.CreateButton(addConfigurationCanvas, "", 30, 30, 14, 90, 269,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(255, 155, 155, 155), 1);
            addFiltrButton.Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
            addFiltrButton.MouseEnter += buttonExit_MouseEnter;
            addFiltrButton.MouseLeave += buttonExit_MouseLeave;
            addFiltrButton.MouseLeftButtonDown += buttonaddFiltr_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(addFiltrButton, "Dodaj filtr");
        }

        private void CreateAddFilterAccept()
        {
            addFiltrAccept = ButtonCreator.CreateButton(addConfigurationCanvas, "Dodaj", 60, 26, 12, 180, 310,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            addFiltrAccept.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            addFiltrAccept.MouseEnter += buttonContent_MouseEnter;
            addFiltrAccept.MouseLeave += buttonContent_MouseLeave;
            addFiltrAccept.MouseLeftButtonDown += addFiltrButton_MouseLeftButtonDown;
            addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CreateApplyFilterButton()
        {
            applyFilterButton = ButtonCreator.CreateButton(addConfigurationCanvas, "Zastosuj filtr", 150, 26, 12, 20, 350,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            applyFilterButton.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            applyFilterButton.MouseEnter += buttonContent_MouseEnter;
            applyFilterButton.MouseLeave += buttonContent_MouseLeave;
            applyFilterButton.MouseLeftButtonDown += applyFilterButton_MouseLeftButtonDown;
            applyFilterButton.Visibility = System.Windows.Visibility.Hidden;

            resultsApplyFilter = new MyLabel(addConfigurationCanvas, "", 150, 30, 10, 180, 350,
                        Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 70, 70, 70), 0, System.Windows.HorizontalAlignment.Left);
            resultsApplyFilter.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CreateDeleteApplicationWithFilterButton()
        {
            deleteApplicationWithFilterButton = ButtonCreator.CreateButton(addConfigurationCanvas, "Usuń aplikacje z filtru", 200, 26, 12, 350, 310,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            deleteApplicationWithFilterButton.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            deleteApplicationWithFilterButton.MouseEnter += buttonContent_MouseEnter;
            deleteApplicationWithFilterButton.MouseLeave += buttonContent_MouseLeave;
            deleteApplicationWithFilterButton.MouseLeftButtonDown += deleteApplicationWithFilterButton_MouseLeftButtonDown;

            resultsDeleteApplication = new MyLabel(addConfigurationCanvas, "", 150, 30, 10, 350, 350,
                        Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 70, 70, 70), 0, System.Windows.HorizontalAlignment.Left);
            resultsDeleteApplication.Visibility = System.Windows.Visibility.Hidden;
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

            MyRectangle r = new MyRectangle(addConfigurationCanvas, 30, 30, Color.FromArgb(0, 244, 244, 255), 460, 20, 1);r.Opacity(0.5);
            ifActivity = new MyLabel(addConfigurationCanvas, "x", 30, 50, 24, 460, 10, Color.FromArgb(255, 0, 155, 0),
                Color.FromArgb(255, 70, 70, 70), 0);
            ifActivity.Visibility = System.Windows.Visibility.Hidden;
            Label buttonChangeActivity = ButtonCreator.CreateButton(addConfigurationCanvas, "    Aktywna", 120, 30, 14, 460, 20,
                Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 155, 155, 155));
            buttonChangeActivity.MouseEnter += buttonChangeActivity_MouseEnter;
            buttonChangeActivity.MouseLeave += buttonChangeActivity_MouseLeave;
            buttonChangeActivity.MouseLeftButtonDown += buttonChangeActivity_MouseLeftButtonDown;


            new MyLabel(addConfigurationCanvas, "Filtr", 50, 30, 14, 20, 269, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70));
            CreateAddFilterButton();
            CreateAddFilterAccept();
            CreateApplyFilterButton();
            CreateDeleteApplicationWithFilterButton();
        }

        private void buttonOpenChooseGroup(object sender, MouseButtonEventArgs e)
        {
            chooseGroupCanvas = new Canvas() { Width = 200, Height = 190 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(addConfigurationCanvas, 200, 190, 20, 60, chooseGroupCanvas);
            Dictionary<string, string> namesGroup = Membership_db.GetNameGroupsDictionaryWithoutConfiguration();
            chooseGroup.MouseLeftButtonDown -= buttonOpenChooseGroup;
            chooseGroup.MouseLeftButtonDown += buttonCloseChooseGroup;
            chooseGroup.Content = "Ukryj grupy";           
            int nextIndex = 0;

            addConfigurationCanvas.Children.Remove(filterContent);
            addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
            applyFilterButton.Visibility = System.Windows.Visibility.Hidden;
            if (resultsApplyFilter != null)
                resultsApplyFilter.Visibility = System.Windows.Visibility.Hidden;
            if (resultsDeleteApplication != null)
                resultsDeleteApplication.Visibility = System.Windows.Visibility.Hidden;
            foreach (KeyValuePair<string, string> name in namesGroup)
            {
                Label group = ButtonCreator.CreateButton(chooseGroupCanvas, name.Value , 200, 29, 12, 0, 0 + (nextIndex * 32),
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
                group.Background = new SolidColorBrush(Color.FromArgb((byte)(50 + (nextIndex % 2 * 30)), 0, 125, 255));
                group.Name = "ID_"+ name.Key;
                group.MouseEnter += buttonContent_MouseEnter;
                group.MouseLeave += buttonContent_MouseLeave;
                group.MouseLeftButtonDown += buttonChooseGroup_MouseLeftButtonDown;

                chooseGroupCanvas.Height += 32;
                nextIndex++;
            }
            chooseGroupCanvas.Height = ((chooseGroupCanvas.Height - 190) < 190) ? 190 : chooseGroupCanvas.Height - 189;
        }

        private void buttonChangeActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ifActivity.Visibility == System.Windows.Visibility.Hidden)
            {
                ifActivity.Visibility = System.Windows.Visibility.Visible;
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
            }
            else
            {
                ifActivity.Visibility = System.Windows.Visibility.Hidden;
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
            }
        }

        private void buttonaddFiltr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!addConfigurationCanvas.Children.Contains(filterContent) && selectGroupId > 0)
            {
                filterContent = new TextBox
                {
                    Width = 150,
                    Height = 26,
                    FontSize = 14,
                    MaxLength = 30,
                    Background = new SolidColorBrush(Color.FromArgb(255, 0, 123, 255)),
                };
                Canvas.SetLeft(filterContent, 20);
                Canvas.SetTop(filterContent, 310);
                addConfigurationCanvas.Children.Add(filterContent);
                filterContent.PreviewMouseLeftButtonDown += filterContent_MouseLeftButtonDown;
                addFiltrAccept.Visibility = System.Windows.Visibility.Visible; 
            }
        }

        private void filterContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            addFiltrAccept.Visibility = System.Windows.Visibility.Visible;
            applyFilterButton.Visibility = System.Windows.Visibility.Hidden;
            if (resultsApplyFilter != null)
                resultsApplyFilter.Visibility = System.Windows.Visibility.Hidden;
            if (resultsDeleteApplication != null)
                resultsDeleteApplication.Visibility = System.Windows.Visibility.Hidden;
        }

        private void addFiltrButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(filterContent.Text) && (filterContent.Text).Length > 3)
            {
                if (Membership_db.AddFilterToConfiguration(selectGroupId, filterContent.Text))
                {
                    addFiltrAccept.Content = "Zmień";
                    addFiltrAccept.MouseLeftButtonDown += changeFiltrButton_MouseLeftButtonDown;
                    addFiltrAccept.MouseLeftButtonDown -= addFiltrButton_MouseLeftButtonDown;
                    addFiltrPlus.Visibility = System.Windows.Visibility.Hidden;
                    addFiltrButton.Visibility = System.Windows.Visibility.Hidden;
                    addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
                    applyFilterButton.Visibility = System.Windows.Visibility.Visible;

                }
            }
            else
            {
                Membership_db.DeleteFilterToConfiguration(selectGroupId);
                addConfigurationCanvas.Children.Remove(filterContent);
                addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void applyFilterButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Regex regex = new Regex(filterContent.Text, RegexOptions.IgnoreCase);
            Dictionary<string, string> namesApplication = ActiveApplication_db.GetNameApplicationDictionaryWithoutGroup();
            List<int> idApplicationFiltered = new List<int>();
            int count = 0;
            foreach (KeyValuePair<string, string> name in namesApplication)
            {
                if (regex.Matches(name.Value).Count > 0)
                {
                    idApplicationFiltered.Add(Convert.ToInt32(name.Key));
                    count++;
                }
            }
            if (idApplicationFiltered.Count > 0)
            {
                if (ActiveApplication_db.AddGroupToApplications(idApplicationFiltered, selectGroupId.ToString()))
                    resultsApplyFilter.SetContent("Znaleziono " + count + " element" + ((count == 1) ? "" : "ów."));
                else
                    resultsApplyFilter.SetContent("Wystąpił błąd z filtrem.");
            }
            else
                resultsApplyFilter.SetContent("Znaleziono " + count + " element" + ((count == 1) ? "" : "ów."));
            resultsApplyFilter.Visibility = System.Windows.Visibility.Visible;
        }

        private void changeFiltrButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(filterContent.Text) && (filterContent.Text).Length > 3)
            {
                if(Membership_db.AddFilterToConfiguration(selectGroupId, filterContent.Text))
                {
                    addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
                    applyFilterButton.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
            {
                if (Membership_db.DeleteFilterToConfiguration(selectGroupId))
                {
                    addFiltrAccept.Content = "Dodaj";
                    addFiltrAccept.MouseLeftButtonDown += addFiltrButton_MouseLeftButtonDown;
                    addFiltrAccept.MouseLeftButtonDown -= changeFiltrButton_MouseLeftButtonDown;
                    addFiltrPlus.Visibility = System.Windows.Visibility.Visible;
                    addFiltrButton.Visibility = System.Windows.Visibility.Visible;
                    addConfigurationCanvas.Children.Remove(filterContent);
                    addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
                    applyFilterButton.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private void buttonChooseGroup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectGroupId = Convert.ToInt32((sender as Label).Name.Replace("ID_", ""));
            CloseChooseGroup();
            chooseGroup.Content = (sender as Label).Content;
            addFiltrPlus.Visibility = System.Windows.Visibility.Visible;
            addFiltrButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void deleteApplicationWithFilterButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string count = ActiveApplication_db.GetAllAutoGroupingApplication(selectGroupId);
            if(ActiveApplication_db.DeleteAllApplicationsWithGroup(selectGroupId, true, true))
                resultsDeleteApplication.SetContent("Usunięto " + count + " element" + ((string.Equals(count, "1")) ? "" : "ów."));
            else
                resultsDeleteApplication.SetContent("Wystąpił błąd podczas usuwania.");
            resultsDeleteApplication.Visibility = System.Windows.Visibility.Visible;
        }

        private void buttonCloseChooseGroup(object sender, MouseButtonEventArgs e)
        {
            CloseChooseGroup();
            chooseGroup.Content = "Wybierz grupę";
            selectGroupId = 0;
        }

        private void CloseChooseGroup()
        {
            chooseGroupCanvas.Children.Clear();
            chooseGroup.MouseLeftButtonDown -= buttonCloseChooseGroup;
            chooseGroup.MouseLeftButtonDown += buttonOpenChooseGroup;
        }

        private void buttonExitConfigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectGroupId = 0;
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

        private void buttonChangeActivity_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ifActivity.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
        }

        private void buttonChangeActivity_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ifActivity.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 100, 100));
        }

    }
}
