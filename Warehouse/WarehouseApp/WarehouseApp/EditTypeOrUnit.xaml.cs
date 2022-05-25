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
    /// Логика взаимодействия для EditTypeOrUnit.xaml
    /// </summary>
    public partial class EditTypeOrUnit : Window
    {
        public EditTypeOrUnit(string table)
        {
            InitializeComponent();
            Table = table;
            string s="";
            switch (table)
            {
                case "type":
                    s = "го типа"; break;
                case "unit":
                    s = "й единицы измерения"; break;
                case "client":
                    s = "й должности"; break;
            }
            Title = "Добавление ново" + s;
        }

        public EditTypeOrUnit(string table, params object[] proper)
        {
            InitializeComponent();
            properties = proper;
            txt1.Text = properties[1].ToString();
            Table = table;
            string s = "";
            switch (table)
            {
                case "type":
                    s = "типа"; break;
                case "unit":
                    s = "единицы измерения"; break;
                case "client":
                    s = "должности"; break;
            }
            Title = "Редактирование " + s;
        }
        object[] properties;
        string Table;
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (properties == null)
            {
                ServiceConnection.Channel.Add(Table, txt1.Text);
            }
            else
            {
                ServiceConnection.Channel.Update(Table, properties[0], txt1.Text);
            }
            DialogResult = true;
            Close();
        }
    }
}
