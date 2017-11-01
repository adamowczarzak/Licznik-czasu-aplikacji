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
        private int activityID;

        public delegate void CloseWindowDelegate();
        public event CloseWindowDelegate CloseWindowAcceptButtonDelegate;
        public event CloseWindowDelegate CloseWindowCancelButtonDelegate;

        public DialogWindow(DialogWindowsState dialogWindowsState, DialogWindowsMessage dialogWindowsMessage)
        {
            InitializeComponent();
            SetComponents(dialogWindowsState);
            SetMessageContent(dialogWindowsMessage);
            this.dialogWindowsMessage = dialogWindowsMessage;
        }

        public void SetActivityID(int activityID)
        {
            this.activityID = activityID;
        }

        private void SetComponents(DialogWindowsState dialogWindowsState)
        {
            switch (dialogWindowsState)
            {
                case DialogWindowsState.YesCancel:
                    acceptButton.Visibility = System.Windows.Visibility.Visible;
                    cancelButton.Visibility = System.Windows.Visibility.Visible;
                    break;
                case DialogWindowsState.Ok:
                    acceptButton.Visibility = System.Windows.Visibility.Visible;
                    cancelButton.Visibility = System.Windows.Visibility.Hidden;
                    break;
            }
        }

        private void SetMessageContent(DialogWindowsMessage dialogWindowsMessage)
        {
            switch (dialogWindowsMessage)
            {
                case DialogWindowsMessage.DeleteApplicationWithAcitivty:
                    questionMessages.Text = "Czy napewno chcesz usunąć wszystkie aplikacje z tej aktywności? " + Environment.NewLine + Environment.NewLine + "Pamiętaj że zmiany będą nieodwracalne!";
                    break;
                case DialogWindowsMessage.Test:
                    questionMessages.Text = "to jest testowa treść";
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
            switch (dialogWindowsMessage)
            {
                case DialogWindowsMessage.DeleteApplicationWithAcitivty:
                    DeleteApplicationWithActivity();                    
                    break;
                case DialogWindowsMessage.Test:
                    questionMessages.Text = "to jest testowa treść";
                    break;
            }
            this.Close();
            CloseWindowAcceptButtonDelegate();
        }

        private void DeleteApplicationWithActivity()
        {
            ActiveApplication_db.DeleteAllApplicationWithActivity(activityID);
        }

        private void Test()
        {

        }
    }
}
