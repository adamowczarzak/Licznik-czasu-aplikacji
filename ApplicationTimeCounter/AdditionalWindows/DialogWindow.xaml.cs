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
        private int applicationID;

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

        public void SetActivityIDAndApplicationID(int activityID, int applicationID)
        {
            this.activityID = activityID;
            this.applicationID = applicationID;
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
                case DialogWindowsMessage.DeleteAllApplicationsWithAcitivty:
                    questionMessages.Text = "Czy napewno chcesz usunąć wszystkie aplikacje z tej aktywności? " + Environment.NewLine + Environment.NewLine + "Pamiętaj że zmiany będą nieodwracalne!";
                    break;
                case DialogWindowsMessage.DeleteOneApplicationWithAcitivty:
                    questionMessages.Text = "Czy napewno chcesz usunąć tą applikacje z tej aktywności? " + Environment.NewLine + Environment.NewLine + "Pamiętaj że zmiany będą nieodwracalne!";
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
                case DialogWindowsMessage.DeleteAllApplicationsWithAcitivty:
                    DeleteAllApplicationsWithActivity();                    
                    break;
                case DialogWindowsMessage.DeleteOneApplicationWithAcitivty:
                    DeleteOneApplicationWithActivity();
                    break;
            }
            this.Close();
            CloseWindowAcceptButtonDelegate();
        }

        private void DeleteAllApplicationsWithActivity()
        {
            if (!ActiveApplication_db.DeleteAllApplicationsWithActivity(activityID))
            {
                ApplicationLog.LogService.AddRaportError("Nie udało się usunąć wszystkich aplikacji z aktywności",
                    ApplicationLog.LogService.GetNameCurrentMethod() + "()",
                    System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\DialogWindow.xaml.cs"); 
            }
        }

        private void DeleteOneApplicationWithActivity()
        {
            if (!ActiveApplication_db.DeleteOneApplicationWithActivity(activityID, applicationID))
            {
                ApplicationLog.LogService.AddRaportWarning("Nie udało się usunąć aplikacji z aktywności");
            }
        }
    }
}
