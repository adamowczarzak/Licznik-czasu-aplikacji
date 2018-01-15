using System.Data.Sql;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for GetDataBaseWindow.xaml
    /// </summary>
    public partial class GetDataBaseWindow : Window
    {
        public bool CanRunApplication { get; set; }
        public GetDataBaseWindow()
        {
            InitializeComponent();
            CanRunApplication = false;
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if(CheckIfGiveDataIsCorrect())
            {
                if (DataBase.TryConnectToSql(nameServer.Text, nameUser.Text, password.Password))
                {
                    if (DataBase.ConnectToDataBase())
                    {
                        DataBase.CloseConnection();
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("Config.file", true))
                        {
                            file.WriteLine(nameServer.Text);
                            file.WriteLine(nameUser.Text);
                            file.WriteLine(password.Password);
                        }
                        CanRunApplication = true;
                        this.Close();
                    }
                    else
                    {
                        CanRunApplication = false;
                        BuildAndDisplayMessageErrorConnectToDataBase();
                    }
                }
                else
                {
                    CanRunApplication = false;
                    BuildAndDisplayMessageErrorConnectToSql();
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            CanRunApplication = false;
            System.Windows.Application.Current.Shutdown();

        }


        private bool CheckIfGiveDataIsCorrect()
        {
            bool dateCorrect = true;
            if (string.IsNullOrEmpty(nameServer.Text))
            {
                nameServer.Background = Brushes.Red;
                dateCorrect = false;
            }         
            BuildAndDisplayMessage();
            return dateCorrect;
        }


        private void BuildAndDisplayMessage()
        {
            loginMessages.Foreground = Brushes.Coral;

            if (nameUser.Background != Brushes.Red && nameServer.Background == Brushes.Red)
                loginMessages.Text = "Podaj Server do połączenia.";

            else if (nameUser.Background != Brushes.Red && nameServer.Background != Brushes.Red)
                loginMessages.Text = "";
                
        }

        private void BuildAndDisplayMessageErrorConnectToSql()
        {
            loginMessages.Text = "Połączenie z Sql nie powiodło się, Nazwa Serwera, Użytkownik lub Hasło jest nie prawidłowe.";
            loginMessages.Foreground = Brushes.Coral;
            nameServer.Background = Brushes.Red;
            nameUser.Background = Brushes.Red;
            password.Background = Brushes.Red;
        }

        private void BuildAndDisplayMessageErrorConnectToDataBase()
        {
            loginMessages.Text = "Połączenie z Bazą Danych nie powiodło się.";
            loginMessages.Foreground = Brushes.Coral;
        }

        private void nameUser_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox();
        }

        private void nameUser_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox();
        }

        private void nameServer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox(nameServerBox: nameServer);
        }

        private void nameServer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox(nameServerBox: nameServer);
        }

        private void password_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox();
        }


        private void password_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox();
        }

        private void SetMessgesAndTextBox(TextBox nameServerBox = null)
        {
            BrushConverter bc = new BrushConverter();
            if (nameServerBox != null)
                nameServerBox.Background = (Brush)bc.ConvertFrom("#007BFF");

            nameUser.Background = (Brush)bc.ConvertFrom("#007BFF");
            password.Background = (Brush)bc.ConvertFrom("#007BFF");
            BuildAndDisplayMessage();
        }
    }
}