using System;
using System.Windows;

namespace ApplicationTimeCounter.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private DialogWindowsMessage dialogWindowsMessage;

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowAcceptButtonDelegate;
        public event CloseWindowDelegate CloseWindowCancelButtonDelegate;
        public event CloseWindowDelegate CloseWindowOKButtonDelegate;

        public DialogWindow(DialogWindowsState dialogWindowsState, DialogWindowsMessage dialogWindowsMessage)
        {
            InitializeComponent();
            SetComponents(dialogWindowsState);
            SetMessageContent(dialogWindowsMessage);
            this.dialogWindowsMessage = dialogWindowsMessage;
        }

        private void SetComponents(DialogWindowsState dialogWindowsState)
        {
            switch (dialogWindowsState)
            {
                case DialogWindowsState.YesCancel:
                    acceptButton.Visibility = System.Windows.Visibility.Visible;
                    cancelButton.Visibility = System.Windows.Visibility.Visible;
                    okButton.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case DialogWindowsState.Ok:
                    acceptButton.Visibility = System.Windows.Visibility.Hidden;
                    cancelButton.Visibility = System.Windows.Visibility.Hidden;
                    okButton.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        private void SetMessageContent(DialogWindowsMessage dialogWindowsMessage)
        {
            switch (dialogWindowsMessage)
            {
                case DialogWindowsMessage.DeleteAllApplicationsWithAcitivty:
                    questionMessages.Text = "Czy napewno chcesz usunąć wszystkie aplikacje z tej aktywności? " + Environment.NewLine + Environment.NewLine + "Pamiętaj że zmiany będą nieodwracalne!";
                    break;
                case DialogWindowsMessage.DeleteOneApplicationWithAcitivty:
                    questionMessages.Text = "Czy napewno chcesz usunąć tą applikacje z tej aktywności? " + Environment.NewLine + Environment.NewLine + "Pamiętaj że zmiany będą nieodwracalne!";
                    break;
                case DialogWindowsMessage.DeleteAcitivty:
                    questionMessages.Text = "Czy napewno chcesz usunąć tą aktywność? " + Environment.NewLine + Environment.NewLine + "Pamiętaj że zmiany będą nieodwracalne!";
                    break;
                case DialogWindowsMessage.EditNameDefaultActivity:
                    questionMessages.Text = "Nie można zmienić nazwy standardowej aktywności - Programowanie.";
                    break;
                case DialogWindowsMessage.DeleteDefaultActivity:
                    questionMessages.Text = "Nie można usunąć standardowej " + Environment.NewLine + "aktywnowści - Programowanie.";
                    break;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowCancelButtonDelegate();
            this.Close();
        }           
           
        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowAcceptButtonDelegate();
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowOKButtonDelegate();
            this.Close();
        }
    }
}
