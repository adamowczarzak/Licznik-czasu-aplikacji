using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplicationTimeCounter
{
    /// <summary>
    /// Interaction logic for GetDataBaseWindow.xaml
    /// </summary>
    public partial class GetDataBaseWindow : Window
    {
        public GetDataBaseWindow()
        {
            InitializeComponent();
        }


        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if(CheckIfGiveDataIsCorrect())
            {
                if (DataBase.TryConnectToMySql(nameUser.Text, password.Password))
                {
                    if (DataBase.ConnectToDataBase()) this.Close();
                    else { }// komunikaty że się nie udało!!

                    // nie wiem, wyświetlenie komunikatu że się udało czy coś

                }
                else
                {
                    // komunikaty o nie powodzeniach łączenia
                }
            }
            //
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();

        }


        private bool CheckIfGiveDataIsCorrect()
        {
            bool dateCorrect = true;

            if (string.IsNullOrEmpty(nameUser.Text))
            {
                nameUser.Background = Brushes.Red;
                dateCorrect = false;
            }

            if (string.IsNullOrEmpty(password.Password))
            {
                password.Background = Brushes.Red;
                dateCorrect = false;
            }
            BuildAndDisplayMessage();
            return dateCorrect;
        }


        private void BuildAndDisplayMessage()
        {
            if (nameUser.Background == Brushes.Red && password.Background == Brushes.Red)
            {
                loginMessages.Text = "Podaj Użytkownika oraz Hasło do połączenia.";
                loginMessages.Foreground = Brushes.Coral;
            }

            else if (nameUser.Background == Brushes.Red && password.Background != Brushes.Red)
            {
                loginMessages.Text = "Podaj Użytkownika do połączenia.";
                loginMessages.Foreground = Brushes.Coral;
            }

            else if (nameUser.Background != Brushes.Red && password.Background == Brushes.Red)
            {
                loginMessages.Text = "Podaj Hasło do połączenia.";
                loginMessages.Foreground = Brushes.Coral;
            }
            
            else if(nameUser.Background != Brushes.Red && password.Background != Brushes.Red)
            {
                loginMessages.Text = "";
            }
                
        }













        private void nameUser_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox(nameTextBox:nameUser);
        }

        private void nameUser_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox(nameTextBox: nameUser);
        }

        private void password_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox(namePasswordBox: password);
        }

        private void password_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMessgesAndTextBox(namePasswordBox:password);
        }

        private void SetMessgesAndTextBox(TextBox nameTextBox = null, PasswordBox namePasswordBox = null)
        {
            BrushConverter bc = new BrushConverter();
            if (nameTextBox != null)       
                nameTextBox.Background = (Brush)bc.ConvertFrom("#007BFF");

            if(namePasswordBox != null)
                namePasswordBox.Background = (Brush)bc.ConvertFrom("#007BFF");

                BuildAndDisplayMessage();
            
        }
    }
}