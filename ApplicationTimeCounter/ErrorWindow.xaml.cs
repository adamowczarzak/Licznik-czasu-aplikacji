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
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            InitializeComponent();
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        public void DisplayErrorConnectToMySql()
        {
            errorMessages.Text = "Błąd!!!\n\nNie udało się otworzyć połączenia MySql. Spróbuj zresetować ponownie  i wprowadzić Nazwę użytkownika i Hasło.";
            resetButton.Visibility = System.Windows.Visibility.Visible;
        }

        public void DisplayErrorConnectToDataBase()
        {
            errorMessages.Text = "Błąd!!!\n\nNie można połączyć się z bazą danych. Aplikacja nie może być uruchomiona.";
            resetButton.Visibility = System.Windows.Visibility.Hidden;
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists("Config.file"))
            {
                System.IO.File.Delete("Config.file");
            }
        }
    }
}
