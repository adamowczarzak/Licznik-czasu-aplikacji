using System.Windows.Controls;
using System.Windows.Media;
using ApplicationTimeCounter.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using ApplicationTimeCounter.Other;
using ApplicationTimeCounter.ApplicationObjectsType;

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
        private MyLabel treatAsOne;
        private TextBox contentsOfFilter;
        private Label addFiltrAccept;
        private MyLabel addFiltrPlus;
        private Label addFiltrButton;
        private Label applyFilterButton;
        private MyLabel resultsApplyFilter;
        private Label deleteApplicationWithFilterButton;
        private MyLabel resultsDeleteApplication;
        private Label deleteConfigurationButton;
        private Label saveConfigurationButton;
        private MyLabel applicationInGroup;
        private MyLabel addingByFilter;
        private MyLabel effectivenessFilter;
        private Label buttonChangeActivity;
        private Label buttontreatAsOne;

        private int selectGroupId;


        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowShowMembershipsDelegate;

        public ConfigurationGroups(ref Canvas canvas)
        {
            this.canvas = canvas;

            mainCanvas = CanvasCreator.CreateCanvas(canvas, 600, 390, Color.FromArgb(255, 30, 39, 93), 10, 10);
            new MyRectangle(mainCanvas, 600, 35, Color.FromArgb(255, 0, 125, 255), 0, 0);
            new MyLabel(mainCanvas, "Konfiguracje grup", 600, 40, 15, 0, 0, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));

            contentCanvas = new Canvas() { Width = 300, Height = 316 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(mainCanvas, 300, 316, 0, 50, contentCanvas);
            LoadGroupName();
            CreateControlUser();
        }

        private void LoadGroupName()
        {          
            Dictionary<string, string> namesGroup = Membership_db.GetNameGroupsDictionaryWithConfiguration();
            int nextIndex = 0;
            contentCanvas.Height = 316;
            foreach (KeyValuePair<string, string> name in namesGroup)
            {
                Label group = ButtonCreator.CreateButton(contentCanvas, name.Value, 250, 29, 12, 20, 0 + (nextIndex * 32),
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
                group.Background = new SolidColorBrush(Color.FromArgb((byte)(50 + (nextIndex % 2 * 30)), 0, 125, 255));
                group.Name = "ID_" + name.Key;
                group.MouseEnter += buttonContent_MouseEnter;
                group.MouseLeave += buttonContent_MouseLeave;
                group.MouseLeftButtonDown += buttonEditGroup_MouseLeftButtonDown;
                ButtonCreator.SetToolTip(group, "Edytuj");

                contentCanvas.Height += 32;
                nextIndex++;
            }
            contentCanvas.Height = ((contentCanvas.Height - 316) < 316) ? 316 : contentCanvas.Height - 315;

            if (!namesGroup.Any())
            {
                new MyLabel(contentCanvas, "Brak konfiguracji", 120, 40, 12, 90, 10, Color.FromArgb(200, 255, 255, 255), Color.FromArgb(255, 230, 0, 0));
            }
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
            addFiltrPlus = new MyLabel(addConfigurationCanvas, "+", 30, 50, 24, 90, 200, Color.FromArgb(255, 170, 170, 170),
                Color.FromArgb(255, 70, 70, 70), 0);

            addFiltrButton = ButtonCreator.CreateButton(addConfigurationCanvas, "", 30, 30, 14, 90, 209,
                Color.FromArgb(0, 155, 155, 155), Color.FromArgb(255, 155, 155, 155), 1);
            addFiltrButton.Background = new SolidColorBrush(Color.FromArgb(0, 215, 215, 215));
            addFiltrButton.MouseEnter += buttonExit_MouseEnter;
            addFiltrButton.MouseLeave += buttonExit_MouseLeave;
            addFiltrButton.MouseLeftButtonDown += buttonaddFiltr_MouseLeftButtonDown;
            ButtonCreator.SetToolTip(addFiltrButton, "Dodaj filtr");
        }

        private void CreateAddFilterAccept()
        {
            addFiltrAccept = ButtonCreator.CreateButton(addConfigurationCanvas, "Dodaj", 60, 26, 12, 190, 250,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            addFiltrAccept.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            addFiltrAccept.MouseEnter += buttonContent_MouseEnter;
            addFiltrAccept.MouseLeave += buttonContent_MouseLeave;
            addFiltrAccept.MouseLeftButtonDown += addFiltrButton_MouseLeftButtonDown;
            addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;

            contentsOfFilter = new TextBox
            {
                Width = 150,
                Height = 26,
                FontSize = 14,
                MaxLength = 30,
                Background = new SolidColorBrush(Color.FromArgb(255, 0, 123, 255)),
            };
            Canvas.SetLeft(contentsOfFilter, 30);
            Canvas.SetTop(contentsOfFilter, 250);
            addConfigurationCanvas.Children.Add(contentsOfFilter);
            contentsOfFilter.PreviewMouseLeftButtonDown += filterContent_MouseLeftButtonDown;
            contentsOfFilter.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CreateApplyFilterButton()
        {
            applyFilterButton = ButtonCreator.CreateButton(addConfigurationCanvas, "Zastosuj filtr", 150, 26, 12, 30, 290,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            applyFilterButton.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            applyFilterButton.MouseEnter += buttonContent_MouseEnter;
            applyFilterButton.MouseLeave += buttonContent_MouseLeave;
            applyFilterButton.MouseLeftButtonDown += applyFilterButton_MouseLeftButtonDown;
            applyFilterButton.Visibility = System.Windows.Visibility.Hidden;

            resultsApplyFilter = new MyLabel(addConfigurationCanvas, "", 150, 30, 10, 190, 290,
                        Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 70, 70, 70), 0, System.Windows.HorizontalAlignment.Left);
            resultsApplyFilter.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CreateDeleteApplicationWithFilterButton()
        {
            deleteApplicationWithFilterButton = ButtonCreator.CreateButton(addConfigurationCanvas, "Usuń aplikacje z filtru", 180, 26, 12, 350, 250,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            deleteApplicationWithFilterButton.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            deleteApplicationWithFilterButton.MouseEnter += buttonContent_MouseEnter;
            deleteApplicationWithFilterButton.MouseLeave += buttonContent_MouseLeave;
            deleteApplicationWithFilterButton.MouseLeftButtonDown += deleteApplicationWithFilterButton_MouseLeftButtonDown;

            resultsDeleteApplication = new MyLabel(addConfigurationCanvas, "", 150, 30, 10, 350, 290,
                        Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 70, 70, 70), 0, System.Windows.HorizontalAlignment.Left);
            resultsDeleteApplication.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CreateSaveDeleteConfigureButton()
        {
            saveConfigurationButton = ButtonCreator.CreateButton(addConfigurationCanvas, "Zapisz konfiguracje", 190, 30, 14, 20, 360,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            saveConfigurationButton.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            saveConfigurationButton.MouseEnter += buttonContent_MouseEnter;
            saveConfigurationButton.MouseLeave += buttonContent_MouseLeave;
            saveConfigurationButton.MouseLeftButtonDown += saveConfigurationButton_MouseLeftButtonDown;

            deleteConfigurationButton = ButtonCreator.CreateButton(addConfigurationCanvas, "Usuń konfiguracje", 190, 30, 14, 360, 360,
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
            deleteConfigurationButton.Background = new SolidColorBrush(Color.FromArgb(50, 0, 125, 255));
            deleteConfigurationButton.MouseEnter += buttonContent_MouseEnter;
            deleteConfigurationButton.MouseLeave += buttonContent_MouseLeave;
            deleteConfigurationButton.MouseLeftButtonDown += deleteConfigurationButton_MouseLeftButtonDown;
        }

        private void CreateCheckBox()
        {
            MyRectangle r = new MyRectangle(addConfigurationCanvas, 30, 30, Color.FromArgb(0, 244, 244, 255), 400, 70, 1); r.Opacity(0.5);
            ifActivity = new MyLabel(addConfigurationCanvas, "x", 30, 50, 24, 400, 60, Color.FromArgb(255, 0, 155, 0),
                Color.FromArgb(255, 70, 70, 70), 0);
            ifActivity.Visibility = System.Windows.Visibility.Hidden;
            buttonChangeActivity = ButtonCreator.CreateButton(addConfigurationCanvas, "    Aktywna", 120, 30, 14, 400, 70,
                Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 155, 155, 155));
            buttonChangeActivity.MouseEnter += buttonChangeActivity_MouseEnter;
            buttonChangeActivity.MouseLeave += buttonChangeActivity_MouseLeave;
            buttonChangeActivity.MouseLeftButtonDown += buttonChangeActivity_MouseLeftButtonDown;

            MyRectangle r2 = new MyRectangle(addConfigurationCanvas, 30, 30, Color.FromArgb(0, 244, 244, 255), 400, 130, 1); r2.Opacity(0.5);
            treatAsOne = new MyLabel(addConfigurationCanvas, "x", 30, 50, 24, 400, 120, Color.FromArgb(255, 0, 155, 0),
                Color.FromArgb(255, 70, 70, 70), 0);
            treatAsOne.Visibility = System.Windows.Visibility.Hidden;
            buttontreatAsOne = ButtonCreator.CreateButton(addConfigurationCanvas, "    Jako applikacja", 160, 30, 14, 400, 130,
                Color.FromArgb(255, 155, 155, 155), Color.FromArgb(255, 155, 155, 155));
            buttontreatAsOne.MouseEnter += buttonChangeTreatAsOne_MouseEnter;
            buttontreatAsOne.MouseLeave += buttonChangeTreatAsOne_MouseLeave;
            buttontreatAsOne.MouseLeftButtonDown += buttonChangeTreatAsOne_MouseLeftButtonDown;
        }

        private void CreateLabelWithInformation()
        {
            new MyLabel(addConfigurationCanvas, "Aplikacje w grupie: ", 120, 30, 11, 240, 50, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70), 0);
            applicationInGroup = new MyLabel(addConfigurationCanvas, "0", 120, 30, 11, 240, 70, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70), 0);

            new MyLabel(addConfigurationCanvas, "Dodane przez filtr: ", 120, 30, 11, 240, 100, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70), 0);
            addingByFilter = new MyLabel(addConfigurationCanvas, "0", 120, 30, 11, 240, 120, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70), 0);

            new MyLabel(addConfigurationCanvas, "Skuteczność filtru: ", 120, 30, 11, 240, 150, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70), 0);
            effectivenessFilter = new MyLabel(addConfigurationCanvas, "0.00 %", 120, 30, 11, 240, 170, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70), 0);
        }

        private void buttonAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            addConfigurationCanvas = CanvasCreator.CreateCanvas(mainCanvas, 600, 390, Color.FromArgb(255, 30, 39, 93), 0, 0);
            SetConfigurationButton();
        }

        private void buttonEditGroup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectGroupId = Convert.ToInt32((sender as Label).Name.Replace("ID_", ""));
            addConfigurationCanvas = CanvasCreator.CreateCanvas(mainCanvas, 600, 390, Color.FromArgb(255, 30, 39, 93), 0, 0);
            SetConfigurationButton();
            chooseGroup.MouseLeftButtonDown -= buttonOpenChooseGroup;
            chooseGroup.Content = (sender as Label).Content;
            LoadGroup();
        }

        private void SetConfigurationButton()
        {
            chooseGroupCanvas = new Canvas() { Width = 200, Height = 126 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(addConfigurationCanvas, 200, 126, 20, 60, chooseGroupCanvas);
            sv.Background = new SolidColorBrush(Color.FromArgb(255, 35, 45, 100));

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

            MyRectangle background = new MyRectangle(addConfigurationCanvas, 530, 140, Color.FromArgb(0, 244, 244, 255), 20, 200);
            background.SetFillColor(Color.FromArgb(255, 35, 45, 100));
            new MyLabel(addConfigurationCanvas, "Filtr", 50, 30, 14, 20, 209, Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 70, 70, 70));
            CreateCheckBox();
            CreateLabelWithInformation();
            CreateAddFilterButton();
            CreateAddFilterAccept();
            CreateApplyFilterButton();
            CreateDeleteApplicationWithFilterButton();
            CreateSaveDeleteConfigureButton();
        }

        private void buttonOpenChooseGroup(object sender, MouseButtonEventArgs e)
        {
            Dictionary<string, string> namesGroup = Membership_db.GetNameGroupsDictionaryWithoutConfiguration();
            chooseGroup.MouseLeftButtonDown -= buttonOpenChooseGroup;
            chooseGroup.MouseLeftButtonDown += buttonCloseChooseGroup;
            chooseGroup.Content = "Ukryj grupy";
            int nextIndex = 0;
            chooseGroupCanvas.Height = 126;

            contentsOfFilter.Visibility = System.Windows.Visibility.Hidden;
            contentsOfFilter.Text = string.Empty;
            addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
            applyFilterButton.Visibility = System.Windows.Visibility.Hidden;
            resultsApplyFilter.Visibility = System.Windows.Visibility.Hidden;
            resultsDeleteApplication.Visibility = System.Windows.Visibility.Hidden;
            ifActivity.Visibility = System.Windows.Visibility.Hidden;
            buttonChangeActivity.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
            treatAsOne.Visibility = System.Windows.Visibility.Hidden;
            buttontreatAsOne.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
            applicationInGroup.SetContent("0");
            addingByFilter.SetContent("0");
            effectivenessFilter.SetContent("0.00 %");

            foreach (KeyValuePair<string, string> name in namesGroup)
            {
                Label group = ButtonCreator.CreateButton(chooseGroupCanvas, name.Value, 200, 29, 12, 0, 0 + (nextIndex * 32),
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155));
                group.Background = new SolidColorBrush(Color.FromArgb((byte)(50 + (nextIndex % 2 * 30)), 0, 125, 255));
                group.Name = "ID_" + name.Key;
                group.MouseEnter += buttonContent_MouseEnter;
                group.MouseLeave += buttonContent_MouseLeave;
                group.MouseLeftButtonDown += buttonChooseGroup_MouseLeftButtonDown;

                chooseGroupCanvas.Height += 32;
                nextIndex++;
            }
            chooseGroupCanvas.Height = ((chooseGroupCanvas.Height - 126) < 126) ? 126 : chooseGroupCanvas.Height - 125;
        }

        private void buttonChangeActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectGroupId > 0)
            {
                if (ifActivity.Visibility == System.Windows.Visibility.Hidden)
                {
                    if (Membership_db.SetActivityConfiguration(selectGroupId, true))
                    {
                        ifActivity.Visibility = System.Windows.Visibility.Visible;
                        (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
                    }
                }
                else
                {
                    if (Membership_db.SetActivityConfiguration(selectGroupId, false))
                    {
                        ifActivity.Visibility = System.Windows.Visibility.Hidden;
                        (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
                    }
                }
            }
        }

        private void buttonChangeTreatAsOne_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectGroupId > 0)
            {
                if (treatAsOne.Visibility == System.Windows.Visibility.Hidden)
                {
                    if (Membership_db.SetAsOneApplication(selectGroupId, true))
                    {
                        treatAsOne.Visibility = System.Windows.Visibility.Visible;
                        (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
                    }          
                }
                else
                {
                    if (Membership_db.SetAsOneApplication(selectGroupId, false))
                    {
                        treatAsOne.Visibility = System.Windows.Visibility.Hidden;
                        (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
                    }
                }
            }
        }

        private void buttonaddFiltr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectGroupId > 0)
            {
                contentsOfFilter.Visibility = System.Windows.Visibility.Visible;
                addFiltrAccept.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void filterContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            addFiltrAccept.Visibility = System.Windows.Visibility.Visible;
            applyFilterButton.Visibility = System.Windows.Visibility.Hidden;
            resultsApplyFilter.Visibility = System.Windows.Visibility.Hidden;
            resultsDeleteApplication.Visibility = System.Windows.Visibility.Hidden;
        }

        private void addFiltrButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(contentsOfFilter.Text) && (contentsOfFilter.Text).Length > 3)
            {
                if (Membership_db.AddFilterToConfiguration(selectGroupId, contentsOfFilter.Text))
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
                contentsOfFilter.Visibility = System.Windows.Visibility.Hidden;
                contentsOfFilter.Text = string.Empty;
                addFiltrAccept.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void applyFilterButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Regex regex = new Regex(contentsOfFilter.Text, RegexOptions.IgnoreCase);
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
            if (!string.IsNullOrEmpty(contentsOfFilter.Text) && (contentsOfFilter.Text).Length > 3)
            {
                if (Membership_db.AddFilterToConfiguration(selectGroupId, contentsOfFilter.Text))
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
                    contentsOfFilter.Visibility = System.Windows.Visibility.Hidden;
                    contentsOfFilter.Text = string.Empty;
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
            LoadGroup();
        }

        private void LoadGroup()
        {
            addFiltrPlus.Visibility = System.Windows.Visibility.Visible;
            addFiltrButton.Visibility = System.Windows.Visibility.Visible;
            int allInGroup = Convert.ToInt32(Membership_db.GetNumberApplicationInGroup(selectGroupId));
            int inGroupWithFilter = Convert.ToInt32(Membership_db.GetNumberApplicationInGroupWithFilter(selectGroupId));

            applicationInGroup.SetContent(allInGroup.ToString());
            addingByFilter.SetContent(inGroupWithFilter.ToString());
            effectivenessFilter.SetContent((ActionOnNumbers.DivisionD(inGroupWithFilter, allInGroup) * 100).ToString("0.00") + " %");

            CommandParameters parameters = new CommandParameters();
            parameters.ID.Add(selectGroupId);
            List<Membership> groups = Membership_db.GetAllGroups(parameters);

            if (!string.IsNullOrEmpty(groups[0].Filter))
            {
                contentsOfFilter.Text = groups[0].Filter;
                contentsOfFilter.Visibility = System.Windows.Visibility.Visible;
            }
            if (groups[0].IfActiveConfiguration)
            {
                ifActivity.Visibility = System.Windows.Visibility.Visible;
                buttonChangeActivity.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
            }
            if (groups[0].IfAsOneApplication)
            {
                treatAsOne.Visibility = System.Windows.Visibility.Visible;
                buttontreatAsOne.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
            }
        }

        private void deleteApplicationWithFilterButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectGroupId != 0)
            {
                string count = ActiveApplication_db.GetAllAutoGroupingApplication(selectGroupId);
                if (ActiveApplication_db.DeleteAllApplicationsWithGroup(selectGroupId, true, true))
                    resultsDeleteApplication.SetContent("Usunięto " + count + " element" + ((string.Equals(count, "1")) ? "" : "ów."));
                else
                    resultsDeleteApplication.SetContent("Wystąpił błąd podczas usuwania.");
                resultsDeleteApplication.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void saveConfigurationButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectGroupId != 0)
            {
                if (Membership_db.SaveConfiguration(selectGroupId))
                {
                    (sender as Label).Background = new SolidColorBrush(Color.FromArgb(220, 0, 255, 0));
                }
                else
                {
                    (sender as Label).Background = new SolidColorBrush(Color.FromArgb(220, 255, 0, 0));
                }
            }
        }

        private void deleteConfigurationButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectGroupId != 0)
            {
                if (Membership_db.DeleteConfiguration(selectGroupId))
                {
                    ExitConfigure();
                }
                else
                {
                    (sender as Label).Background = new SolidColorBrush(Color.FromArgb(220, 255, 0, 0));
                }
            }
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

        private void ExitConfigure()
        {
            selectGroupId = 0;
            addConfigurationCanvas.Children.Clear();
            mainCanvas.Children.Remove(addConfigurationCanvas);
            contentCanvas.Children.Clear();
            LoadGroupName();
        }

        private void buttonExitConfigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExitConfigure();
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

        private void buttonChangeTreatAsOne_MouseLeave(object sender, MouseEventArgs e)
        {
            if (treatAsOne.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
        }

        private void buttonChangeTreatAsOne_MouseEnter(object sender, MouseEventArgs e)
        {
            if (treatAsOne.Visibility == System.Windows.Visibility.Hidden)
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 155, 0));
            else
                (sender as Label).Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 100, 100));
        }

    }
}
