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
using WarehouseDLL;

namespace WarehouseApp
{
    /// <summary>
    /// Логика взаимодействия для EditProvider.xaml
    /// </summary>
    public partial class EditProvider : Window
    {
        public EditProvider()
        {
            InitializeComponent();
            Title = "Добавление нового поставщика";
        }

        public EditProvider(object[] prop)
        {
            InitializeComponent();
            properties = prop;
            txtName.Text = properties[1].ToString();
            txtAdress.Text = properties[2].ToString();
            txtContact.Text = properties[3].ToString();
            Title = "Редактирование поставщика";
        }
        object[] properties;
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (properties == null)
            {
                ServiceConnection.Channel.Add("provider", txtName.Text, txtAdress.Text, txtContact.Text);
            }
            else
            {
                ServiceConnection.Channel.Update("provider", properties[0], txtName.Text, txtAdress.Text, txtContact.Text);
            }
            DialogResult = true;
            Close();
        }
    }
}
