using ApplicationTimeCounter.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ApplicationTimeCounter
{
    class SettingsForm
    {
        public Canvas MainCanvasSettings { get; set; }
        private Canvas contentPage;
        private Canvas exceptionApplicationCanvas;
        private Canvas searchApplicationCanvas;
        private Canvas nonTitleApplicationCanvas;
        private Label buttonAddException;
        private Label buttonAcceptAddException;
        private Label buttonSaveNewName;
        private TextBox nameApplicationToException;
        private TextBox nameApplicationToChange;
        private Label highlightRect;
        private int idAddExceptionApplication;
        private int idAddNewTitleApplication;

        private const string defaultContentnameApplicationToException = "Wpisz nazwe aplikacji";
        private const string defaultContentnameApplicationToChange = "Wpisz nową nazwę dla aplikacji";

        private ViewContent viewContent;

        public SettingsForm(Canvas contentPage, ref ViewContent viewContent)
        {
            this.contentPage = contentPage;
            this.viewContent = viewContent;
            viewContent.ContentDelegateLoad += viewContent_Delegate;

            CreateSettingsForm();
            LoadExceptionApplication();
            LoadNonTitleApplication();
        }

        public void ShowSettingsForm()
        {
            viewContent.ChangeContent(MainCanvasSettings);
            Canvas.SetZIndex(MainCanvasSettings, 1);
            UpdateView();
        }

        public void UpdateView()
        {

        }

        private void viewContent_Delegate(Visibility visibility)
        {

        }

        private void CreateSettingsForm()
        {
            MainCanvasSettings = CanvasCreator.CreateCanvas(contentPage, 620, 410, Color.FromArgb(255, 30, 39, 93), 10, 10);
            MainCanvasSettings.Opacity = 0;

            new MyRectangle(MainCanvasSettings, 295, 35, Color.FromArgb(255, 0, 125, 255), 0, 0);
            new MyRectangle(MainCanvasSettings, 295, 35, Color.FromArgb(255, 0, 125, 255), 305, 0);
            new MyRectangle(MainCanvasSettings, 276, 25, Color.FromArgb(255, 0, 125, 255), 10, 140);
            new MyLabel(MainCanvasSettings, "Wyjątki aktywnych aplikacji", 295, 30, 13, 0, 4, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            new MyLabel(MainCanvasSettings, "Zmiana braku aktywnego okna na aplikacje", 295, 30, 13, 305, 4, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            new MyLabel(MainCanvasSettings, "Dodane wyjątki", 276, 26, 11, 10, 140, Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));

            exceptionApplicationCanvas = new Canvas() { Width = 300, Height = 220 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(MainCanvasSettings, 300, 220, 0, 170, exceptionApplicationCanvas);

            nonTitleApplicationCanvas = new Canvas() { Width = 300, Height = 286 };
            ScrollViewer sv_2 = ScrollViewerCreator.CreateScrollViewer(MainCanvasSettings, 300, 286, 300, 50, nonTitleApplicationCanvas);

            CreateButtonsAndTextBox();
        }

        private void CreateButtonsAndTextBox()
        {
            buttonAddException = ButtonCreator.CreateButton(MainCanvasSettings, "Dodaj wyjątek aktywności", 276, 30, 13, 10, 50,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            buttonAddException.Background = new SolidColorBrush(Color.FromArgb(255, 2, 53, 101));
            buttonAddException.MouseEnter += Behavior_MouseEnter;
            buttonAddException.MouseLeave += Behavior_MouseLeave;
            buttonAddException.MouseLeftButtonDown += buttonAddException_MouseLeftButtonDown;

            nameApplicationToException = new TextBox
            {
                Text = defaultContentnameApplicationToException,
                Width = 216,
                Height = 26,
                FontSize = 15,
                MaxLength = 30,
                Background = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 85, 85)),
                FontFamily = new FontFamily("Comic Sans MS"),
            };
            Canvas.SetLeft(nameApplicationToException, 10);
            Canvas.SetTop(nameApplicationToException, 100);
            MainCanvasSettings.Children.Add(nameApplicationToException);
            nameApplicationToException.PreviewMouseLeftButtonDown += nameApplicationToException_MouseLeftButtonDown;
            //mameApplicationToException.LostFocus += searchingNames_LostFocus;
            //mameApplicationToException.PreviewKeyDown += searchingNames_KeyDown;
            //mameApplicationToException.TextChanged += searchingNames_TextChanged;
            nameApplicationToException.Visibility = Visibility.Hidden;

            buttonAcceptAddException = ButtonCreator.CreateButton(MainCanvasSettings, "Szukaj", 54, 30, 13, 232, 98,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            buttonAcceptAddException.Background = new SolidColorBrush(Color.FromArgb(255, 2, 53, 101));
            buttonAcceptAddException.MouseEnter += Behavior_MouseEnter;
            buttonAcceptAddException.MouseLeave += Behavior_MouseLeave;
            buttonAcceptAddException.MouseLeftButtonDown += buttonAcceptAddException_MouseLeftButtonDown;
            buttonAcceptAddException.Visibility = Visibility.Hidden;

            nameApplicationToChange = new TextBox
            {
                Text = defaultContentnameApplicationToChange,
                Width = 230,
                Height = 26,
                FontSize = 15,
                MaxLength = 30,
                Background = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 85, 85)),
                FontFamily = new FontFamily("Comic Sans MS"),
            };
            Canvas.SetLeft(nameApplicationToChange, 310);
            Canvas.SetTop(nameApplicationToChange, 350);
            MainCanvasSettings.Children.Add(nameApplicationToChange);
            nameApplicationToChange.PreviewMouseLeftButtonDown += nameApplicationToChange_MouseLeftButtonDown;
            nameApplicationToChange.Visibility = Visibility.Hidden;

            buttonSaveNewName = ButtonCreator.CreateButton(MainCanvasSettings, "Zapisz", 54, 30, 13, 545, 348,
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(0, 0, 0, 0));
            buttonSaveNewName.Background = new SolidColorBrush(Color.FromArgb(255, 2, 53, 101));
            buttonSaveNewName.MouseEnter += Behavior_MouseEnter;
            buttonSaveNewName.MouseLeave += Behavior_MouseLeave;
            buttonSaveNewName.MouseLeftButtonDown += buttonSaveNewName_MouseLeftButtonDown;
            buttonSaveNewName.Visibility = Visibility.Hidden;
        }

        private void LoadExceptionApplication()
        {
            exceptionApplicationCanvas.Height = 220;
            int nextIndex = 0;

            Dictionary<string, string> nameApplicationDictionary = ExceptionApplication_db.GetNameApplicationDictionary();

            foreach (KeyValuePair<string, string> name in nameApplicationDictionary)
            {
                Label application = ButtonCreator.CreateButton(exceptionApplicationCanvas, (nextIndex + 1) + ".    " + name.Value, 250, 29, 12, 20, 0 + (nextIndex * 32),
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155), horizontalAlignment:HorizontalAlignment.Left);
                application.Background = new SolidColorBrush(Color.FromArgb((byte)(50 + (nextIndex % 2 * 30)), 0, 125, 255));
                application.Name = "ID_" + name.Key;
                application.MouseEnter += buttonContent_MouseEnter;
                application.MouseLeave += buttonContent_MouseLeave;
                application.MouseLeftButtonDown += selectApplication_MouseLeftButtonDown;

                Label deleteButton = ButtonCreator.CreateButton(exceptionApplicationCanvas, "X", 22, 22, 8, 244, 3 + (nextIndex * 32),
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155), 1);
                deleteButton.Background = new SolidColorBrush(Color.FromArgb(100, 250, 0, 0));
                application.Name = "ID_" + name.Key;
                deleteButton.MouseEnter += deleteButton_MouseEnter;
                deleteButton.MouseLeave += deleteButton_MouseLeave;
                deleteButton.MouseLeftButtonDown += deleteExceptionButton_MouseLeftButtonDown;

                exceptionApplicationCanvas.Height += 32;
                nextIndex++;
            }
            exceptionApplicationCanvas.Height = ((exceptionApplicationCanvas.Height - 220) < 220) ? 220 : exceptionApplicationCanvas.Height - 219;
            idAddExceptionApplication = 0;
        }

        private void LoadNonTitleApplication()
        {

            nonTitleApplicationCanvas.Height = 286;
            int nextIndex = 0;

            //foreach (KeyValuePair<string, string> name in namesGroup)
            for (int i = 0; i < 10; i++)
            {
                Label application = ButtonCreator.CreateButton(nonTitleApplicationCanvas, (i + 1) + "." + "\t2018-02-19" + "\t Brak nazwy", 250, 29, 11, 20, 0 + (nextIndex * 32),
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155), horizontalAlignment: HorizontalAlignment.Left);
                application.Background = new SolidColorBrush(Color.FromArgb((byte)(50 + (nextIndex % 2 * 30)), 0, 125, 255));
                // application.Name = "ID_" + name.Key;
                application.MouseEnter += buttonContent_MouseEnter;
                application.MouseLeave += buttonContent_MouseLeave;
                application.MouseLeftButtonDown += selectNonTitleApplication_MouseLeftButtonDown;

                Label deleteButton = ButtonCreator.CreateButton(nonTitleApplicationCanvas, "X", 22, 22, 8, 244, 3 + (nextIndex * 32),
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155), 1);
                deleteButton.Background = new SolidColorBrush(Color.FromArgb(100, 250, 0, 0));
                // application.Name = "ID_" + name.Key;
                deleteButton.MouseEnter += deleteButton_MouseEnter;
                deleteButton.MouseLeave += deleteButton_MouseLeave;
                deleteButton.MouseLeftButtonDown += deleteButton_MouseLeftButtonDown;

                nonTitleApplicationCanvas.Height += 32;
                nextIndex++;
            }
            nonTitleApplicationCanvas.Height = ((nonTitleApplicationCanvas.Height - 286) < 286) ? 286 : nonTitleApplicationCanvas.Height - 285;
            idAddNewTitleApplication = 0;
        }

        private void buttonAcceptAddException_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (idAddExceptionApplication > 0)
            {
                if (!string.IsNullOrEmpty(nameApplicationToException.Text) && !nameApplicationToException.Text.Equals(defaultContentnameApplicationToException))
                {
                    if (!ExceptionApplication_db.CheckIfExistApplication(idAddExceptionApplication))
                        ExceptionApplication_db.AddExceptionApplication(idAddExceptionApplication);
                    buttonAcceptAddException.Content = "Szukaj";
                    buttonAcceptAddException.Visibility = Visibility.Hidden;
                    nameApplicationToException.Text = defaultContentnameApplicationToException;
                    nameApplicationToException.Visibility = Visibility.Hidden;
                    nameApplicationToException.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 85, 85));
                }
            }
            else
            {
                if (FindApplication() > 0)
                {
                    buttonAcceptAddException.Content = "Dodaj";
                }
            }
        }

        private int FindApplication()
        {
            Regex regex = new Regex(nameApplicationToException.Text.Trim(), RegexOptions.IgnoreCase);
            Dictionary<string, string> titleApplication = ActiveApplication_db.GetNameApplicationDictionaryWithoutGroup();
            Dictionary<string, string> titleMembership = Membership_db.GetNameGroupsDictionaryWithConfiguration();
            Dictionary<string, string> titleFindApplication = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> title in titleApplication)
            {
                if (regex.Matches(title.Value).Count > 0)
                {
                    titleFindApplication[title.Key] = title.Value;
                }
            }

            foreach (KeyValuePair<string, string> title in titleMembership)
            {
                if (regex.Matches(title.Value).Count > 0)
                {
                    titleFindApplication[title.Key + "M"] = title.Value;
                }
            }

            searchApplicationCanvas.Children.Clear();
            searchApplicationCanvas.Height = 252;
            int nextIndex = 0;
            foreach (KeyValuePair<string, string> name in titleFindApplication)
            {
                Label application = ButtonCreator.CreateButton(searchApplicationCanvas, name.Value, 250, 29, 12, 20, 0 + (nextIndex * 32),
                    Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 155, 155, 155), horizontalAlignment: HorizontalAlignment.Left);
                application.Background = new SolidColorBrush(Color.FromArgb(155, 0, 125, 255));
                application.Name = "ID_" + name.Key;
                application.MouseEnter += buttonContent_MouseEnter;
                application.MouseLeave += buttonContent_MouseLeave;
                application.MouseLeftButtonDown += findApplication_MouseLeftButtonDown;

                searchApplicationCanvas.Height += 32;
                nextIndex++;
            }
            searchApplicationCanvas.Height = ((searchApplicationCanvas.Height - 252) < 252) ? 252 : searchApplicationCanvas.Height - 251;
            idAddExceptionApplication = 0;

            return 10;
        }

        private void buttonSaveNewName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if(idAddNewTitleApplication > 0)
           // {
                if (!string.IsNullOrEmpty(nameApplicationToChange.Text) && !nameApplicationToChange.Text.Equals(defaultContentnameApplicationToChange))
                {
                    // zapisanie zmian

                    if (highlightRect != null)
                        nonTitleApplicationCanvas.Children.Remove(highlightRect);
                    nameApplicationToChange.Text = defaultContentnameApplicationToChange;
                    nameApplicationToChange.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 85, 85));
                    nameApplicationToChange.Visibility = Visibility.Hidden;
                    buttonSaveNewName.Visibility = Visibility.Hidden;
                    idAddNewTitleApplication = 0;   
                }
           // }
        }

        private void buttonAddException_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nameApplicationToException.Visibility = Visibility.Visible;
            buttonAcceptAddException.Visibility = Visibility.Visible;

            searchApplicationCanvas = new Canvas() { Width = 300, Height = 252 };
            ScrollViewer sv = ScrollViewerCreator.CreateScrollViewer(MainCanvasSettings, 300, 252, 0, 140, searchApplicationCanvas);
            searchApplicationCanvas.Background = new SolidColorBrush(Color.FromArgb(255, 30, 39, 93));
        }

        private void selectNonTitleApplication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (highlightRect != null)
                nonTitleApplicationCanvas.Children.Remove(highlightRect);

            Point relativePoint = (sender as Label).TransformToAncestor(nonTitleApplicationCanvas).Transform(new Point(0, 0));
            highlightRect = ButtonCreator.CreateButton(nonTitleApplicationCanvas, "", 250, 29, 12, relativePoint.X, relativePoint.Y,
                Color.FromArgb(80, 0, 250, 0), Color.FromArgb(0, 0, 0, 0));
            highlightRect.Background = new SolidColorBrush(Color.FromArgb(80, 0, 250, 0));
            highlightRect.MouseLeftButtonDown += highlightRect_MouseLeftButtonDown;
            //idAddNewTitleApplication = Convert.ToInt32((sender as Label).Name.Replace("ID_", string.Empty));           
            nameApplicationToChange.Visibility = Visibility.Visible;
            buttonSaveNewName.Visibility = Visibility.Visible;            
        }

        private void findApplication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Label).Name.Contains("M"))
                 idAddExceptionApplication = Convert.ToInt32((sender as Label).Name.Replace("ID_", string.Empty).Replace("M", string.Empty));
            else idAddExceptionApplication = Convert.ToInt32((sender as Label).Name.Replace("ID_", string.Empty));
            searchApplicationCanvas.Children.Clear();
            searchApplicationCanvas.Visibility = Visibility.Hidden;
            nameApplicationToException.Text = (sender as Label).Content.ToString();
        }

        private void deleteExceptionButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //
        }

        private void deleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // reload
        }

        private void selectApplication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //idAddExceptionApplication = Convert.ToInt32((sender as Label).Name.Replace("ID_", string.Empty));
        }


        private void nameApplicationToChange_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nameApplicationToChange.Foreground = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
            nameApplicationToChange.Text = "";
        }

        private void nameApplicationToException_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nameApplicationToException.Foreground = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
            nameApplicationToException.Text = "";
        }

        private void highlightRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nonTitleApplicationCanvas.Children.Remove((sender as Label));
            nameApplicationToChange.Visibility = Visibility.Hidden;
            buttonSaveNewName.Visibility = Visibility.Hidden;
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

        private void Behavior_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(255, 2, 53, 101));
        }

        private void Behavior_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(255, 1, 38, 84));
        }

        private void deleteButton_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(100, 250, 0, 0));
        }

        private void deleteButton_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(200, 250, 0, 0));
        }
    }
}
