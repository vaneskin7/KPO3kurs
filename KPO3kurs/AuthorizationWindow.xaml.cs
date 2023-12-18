using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KPO3kurs
{
    public partial class AuthorizationWindow : Window
    {
        bool isCorrect;
        MainWindow mainWindow = new MainWindow();

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private async void authorizationButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;
            string adress = adressTextBox.Text;
            try
            {
                int port;
                if (loginTextBox.Text.Length <= 1 && passwordTextBox.Text.Length <= 1 || adressTextBox.Text.Length == 0 ||
                    int.TryParse(portTextBox.Text, out port) == false)
                {
                    MessageBox.Show("Введены некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                InternetConnection.ipAdress = adressTextBox.Text;
                InternetConnection.PORT = port;

                authorizationButton.IsEnabled = false;
                try
                {
                    await Task.Run(() =>
                    {
                        isCorrect = InternetConnection.ConnectToServer(login, password, adress, port);
                    });
                    if (isCorrect)
                    {
                        mainWindow.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    authorizationButton.IsEnabled = true;
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Не удалось подключиться к серверу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageWindow languageWindow = new LanguageWindow();
            languageWindow.Show();
        }
    }
}
