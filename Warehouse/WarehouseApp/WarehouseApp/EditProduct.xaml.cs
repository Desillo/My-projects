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
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        public EditProduct()
        {
            InitializeComponent();

            WareHouseData type = ServiceConnection.Channel.GetData("type", 0,10);
            WareHouseData unit = ServiceConnection.Channel.GetData("unit", 0, 10);
            WareHouseData provider = ServiceConnection.Channel.GetData("provider", 0, 10);

            if (type != null)
            {
                foreach (DataRowView drv in type.Data.DefaultView)
                    comboType.Items.Add(drv);
            }
            if (unit != null)
            {
                foreach (DataRowView drv in unit.Data.DefaultView)
                    comboUnit.Items.Add(drv);
            }
            if (provider != null)
            {
                foreach (DataRowView drv in provider.Data.DefaultView)
                    comboProvider.Items.Add(drv);
            }


            comboProvider.SelectedIndex = 0;
            comboType.SelectedIndex = 0;
            comboUnit.SelectedIndex = 0;


            Title = "Добавление нового товара";
            
        }
        string Table;

        public EditProduct(string table, object[] proper)
        {
            InitializeComponent();
            properties = proper;
            WareHouseData type = ServiceConnection.Channel.GetData("type", 0, 10);
            WareHouseData unit = ServiceConnection.Channel.GetData("unit", 0, 10);
            WareHouseData provider = ServiceConnection.Channel.GetData("provider", 0, 10);

            if (type != null)
            {
                foreach (DataRowView drv in type.Data.DefaultView)
                    comboType.Items.Add(drv);
            }
            if (unit != null)
            {
                foreach (DataRowView drv in unit.Data.DefaultView)
                    comboUnit.Items.Add(drv);
            }
            if (provider != null)
            {
                foreach (DataRowView drv in provider.Data.DefaultView)
                    comboProvider.Items.Add(drv);
            }


            txtName.Text = properties[1].ToString();
            comboType.Text = properties[2].ToString();
            comboUnit.Text = properties[3].ToString();
            datereg.SelectedDate = DateTime.Parse(properties[4].ToString());
            txtPrice.Text = properties[5].ToString();
            txtAmount.Text = properties[6].ToString();
            comboProvider.Text = properties[7].ToString();
            Title = "Редактирование товара";
            Table = table;
        }

        private void BtnType_Click(object sender, RoutedEventArgs e)
        {
            TOUWindow tow = new TOUWindow("type");
            tow.ShowDialog();
            comboType.Items.Clear();
            WareHouseData type = ServiceConnection.Channel.GetData("type", 0, 10);
            if (type != null)
            {
                foreach (DataRowView drv in type.Data.DefaultView)
                    comboType.Items.Add(drv);
            }
            comboType.SelectedIndex = 0;
            if (tow.Tag != null)
                comboType.Text = tow.Tag.ToString();

        }

        private void BtnUnit_Click(object sender, RoutedEventArgs e)
        {
            TOUWindow tow = new TOUWindow("unit");
            tow.ShowDialog();
            comboUnit.Items.Clear();
            WareHouseData unit = ServiceConnection.Channel.GetData("unit", 0, 10);
            if (unit != null)
            {
                foreach (DataRowView drv in unit.Data.DefaultView)
                    comboUnit.Items.Add(drv);
            }
            comboUnit.SelectedIndex = 0;
            if (tow.Tag!=null)
                comboUnit.Text = tow.Tag.ToString();
        }

        private void ButtonProvider_Click(object sender, RoutedEventArgs e)
        {
            ProviderWindow1 pw1 = new ProviderWindow1();
            pw1.ShowDialog();
            comboProvider.Items.Clear();
            WareHouseData prov = ServiceConnection.Channel.GetData("provider", 0, 10);
            if (prov != null)
            {
                foreach (DataRowView drv in prov.Data.DefaultView)
                    comboProvider.Items.Add(drv);
            }
            comboProvider.SelectedIndex = 0;
            if(pw1.Tag!= null)
                comboProvider.Text = pw1.Tag.ToString();
        }
        object[] properties;
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            
            DateTime date;
            DateTime.TryParse(datereg.Text, out date);
            int amount, price;
            int.TryParse(txtPrice.Text, out price);
            int.TryParse(txtAmount.Text, out amount);
            if (properties == null)
            {
                ServiceConnection.Channel.Add("product", txtName.Text, comboType.Text, comboUnit.Text,
                    date, price, amount, comboProvider.Text);
            }
            else
            {
                ServiceConnection.Channel.Update(Table, properties[0], txtName.Text, comboType.Text, comboUnit.Text,
                    date, price, amount, comboProvider.Text);
            }
            DialogResult = true;
            Close();
        }
    }
}
