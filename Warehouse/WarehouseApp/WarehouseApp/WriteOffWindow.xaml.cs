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

namespace WarehouseApp
{
    /// <summary>
    /// Логика взаимодействия для WriteOffWindow.xaml
    /// </summary>
    public partial class WriteOffWindow : Window
    {
        public WriteOffWindow(DataRowView[] drv, string table1, string table2)
        {
            InitializeComponent();
            dataGrid1.ItemsSource = drv;
            amounts = new int[drv.Length];
            regs = new DateTime[drv.Length];
            for (int i = 0; i < amounts.Length; i++)
            {
                amounts[i] = (int)drv[i].Row["amount"];
                regs[i] = (DateTime)drv[i].Row["registration"];
            }
            
            Title = "Отправление товаров";
            tableadd = table1;
            tabledelete = table2;
            if (table1 == "selling")
            {
                btnsend.Content = "Списать";
                dataGrid1.Columns[5].Header = "Дата продажи";
                Title = "Списание товаров";
            }
                
        }
        int[] amounts;
        DateTime[] regs;
        string tableadd, tabledelete;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView[] dv = (DataRowView[])dataGrid1.ItemsSource;
            for (int i = 0; i < dv.Length; i++)
            {
                if ((int)dv[i].Row["amount"] > amounts[i])
                {
                    dv[i].Row["amount"] = amounts[i];
                }
                if ((DateTime)dv[i].Row["registration"] < regs[i])
                {
                    dv[i].Row["registration"] = regs[i];
                }
                List<object> obj = dv[i].Row.ItemArray.ToList();
                obj.RemoveAt(0);
                if (tableadd == "selling")
                {
                    obj.Add((int)dv[i].Row["amount"] * (int)dv[i].Row["price"]);
                }
                ServiceConnection.Channel.Add(tableadd, obj.ToArray());
                int a = (int)dv[i].Row["amount"];
                dv[i].Row["amount"] = amounts[i] - a;
                dv[i].Row["registration"] = regs[i];
                ServiceConnection.Channel.Update(tabledelete, dv[i].Row.ItemArray);
            }
            DialogResult = true;
            Close();
        }
    }
}
