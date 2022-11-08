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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TKExercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLServerConnection serverConnection;

        public MainWindow()
        {
            InitializeComponent();

            serverConnection = new SQLServerConnection();
        }

        //Disable loading data if it's not already disabled
        private void DisableLoading()
        {
            if (LoadDataButton.IsEnabled)
            {
                LoadDataButton.IsEnabled = false;
            }
        }

        private void ButtonCheckConnection_Click(object sender, RoutedEventArgs e)
        {
            serverConnection.SetConnectionDetails(TextHost.Text, TextUser.Text, TextPassword.Password);

            if (serverConnection.TryConnect())
            {
                MessageBox.Show("Połączenie z bazą danych powiodło się.", "Informacja");
                LoadDataButton.IsEnabled = true;
            }    
            else
            {
                MessageBox.Show("Wystąpił błąd podczas połączenia: " + serverConnection.ErrorMessage, "Błąd");
                DisableLoading();
            }
        }

        //Disable loading data if connection details were changed to force user to check connection before trying to load data
        private void TextConnection_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisableLoading();
        }

        //Same as before
        private void TextPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            DisableLoading();
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
