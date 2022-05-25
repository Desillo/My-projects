using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        public EditUser()
        {
            InitializeComponent();
            Title = "Добавление пользователя";
            WareHouseData posit = ServiceConnection.Channel.GetData("position", 0, 10);
            if (posit != null)
            {
                foreach (DataRowView drv in posit.Data.DefaultView)
                    comboPosit.Items.Add(drv);
            }
            comboPosit.SelectedIndex = 0;
        }
        public EditUser(object[] prop)
        {
            InitializeComponent();
            properties = prop;
            txtLog.Text = (string)properties[2];
            txtPass.Text = (string)properties[3];
            WareHouseData posit = ServiceConnection.Channel.GetData("position", 0, 10);
            if (posit != null)
            {
                comboPosit.Items.Add("");
                foreach (DataRowView drv in posit.Data.DefaultView)
                    comboPosit.Items.Add(drv);
            }
            comboPosit.Text = (string)properties[4];
            Title = "Редактирование пользователя";
        }
        object[] properties;
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (properties == null)
            {
                ServiceConnection.Channel.Add("clients", txtLog.Text, txtPass.Text, comboPosit.Text);
            }
            else
            {
                ServiceConnection.Channel.Update("clients", properties[0], txtLog.Text, txtPass.Text, comboPosit.Text);
            }
            DialogResult = true;
            Close();
        }

        private void BtnPosit_Click(object sender, RoutedEventArgs e)
        {
            TOUWindow tow = new TOUWindow("position");
            tow.ShowDialog();
            comboPosit.Items.Clear();
            WareHouseData posit = ServiceConnection.Channel.GetData("position", 0, 10);
            if (posit != null)
            {
                comboPosit.Items.Add("");
                foreach (DataRowView drv in posit.Data.DefaultView)
                    comboPosit.Items.Add(drv);
            }
            if (tow.Tag != null)
                comboPosit.Text = tow.Tag.ToString();
        }

    }
}
