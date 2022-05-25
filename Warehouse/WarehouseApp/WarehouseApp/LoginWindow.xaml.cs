using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ServiceModel;

namespace WarehouseApp
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceConnection.Initialize();
                string position;
                if (ServiceConnection.Channel.LoginClient(txtLogin.Text, passBox.Password, out position))
                {
                    MainWindow mw = new MainWindow(txtLogin.Text, position);
                    mw.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch(CommunicationException ex)
            {
                MessageBox.Show(ex.Message);
                ServiceConnection.Channel = null;
            }
        }
    }
}
