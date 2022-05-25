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
    /// Логика взаимодействия для SellingWindow.xaml
    /// </summary>
    public partial class SellingWindow : Window
    {
        public SellingWindow()
        {
            InitializeComponent();
            Title = "Продажи";
            pageinfo = new PageInfo()
            {
                PageNumber = 1,
                 PageSize = 10,
                  TotalItems = ServiceConnection.Channel.GetRowsCount("selling")
            };

            selling = ServiceConnection.Channel.GetData("selling", (pageinfo.PageNumber-1)*pageinfo.PageSize, pageinfo.PageSize);
            if (selling != null)
                dataGrid1.ItemsSource = selling.Data.DefaultView;
            else
                dataGrid1.ItemsSource = null;
            int N = pageinfo.TotalPages;
            if (N > PageInfo.TotalBtns)
                N = PageInfo.TotalBtns;
            btns = new Button[N];
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i] = new Button();
                btns[i].Width = 20;
                btns[i].Height = 20;
                btns[i].Tag = i + pageinfo.PageNumber;
                btns[i].Content = i + pageinfo.PageNumber;
                btns[i].Click += BtnPagin; ;
                spanel1.Children.Add(btns[i]);
            }
            if (N > 0)
                btns[0].IsEnabled = false;
        }

        private void BtnPagin(object sender, RoutedEventArgs e)
        {
            int page = (int)(sender as Button).Tag;
            pageinfo.PageNumber = page;
            pageinfo.BtnPagination(ref btns);
            selling = ServiceConnection.Channel.GetData("selling", (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                pageinfo.PageSize);
            if (selling != null)
                dataGrid1.ItemsSource = selling.Data.DefaultView;
            else
                dataGrid1.ItemsSource = null;
            dataGrid1.SelectedIndex = -1;
        }

        WareHouseData selling;
        PageInfo pageinfo;
        Button[] btns;
    }
}
