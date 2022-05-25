using System;
using System.Collections;
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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            pageinfo = new PageInfo()
            {
                PageNumber = 1,
                PageSize = 10,
                TotalItems = ServiceConnection.Channel.GetRowsCount("clients")
            };
            users = ServiceConnection.Channel.GetData("clients", (pageinfo.PageNumber-1)*pageinfo.PageSize, pageinfo.PageSize);
            if (users != null)
                dataGrid1.ItemsSource = users.Data.DefaultView;
            else
                dataGrid1.ItemsSource = null;
            ServiceCallBack.BaseUpdate += ServiceCallBack_BaseUpdate; ;
            VisBtn(false);
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
                btns[i].Click += BtnPagin;
                spanel1.Children.Add(btns[i]);
            }
            if (N > 0)
                btns[0].IsEnabled = false;

            Title = "Пользователи";
        }

        private void BtnPagin(object sender, RoutedEventArgs e)
        {
            int page = (int)(sender as Button).Tag;
            pageinfo.PageNumber = page;
            pageinfo.BtnPagination(ref btns);
            users = ServiceConnection.Channel.GetData("clients", (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                pageinfo.PageSize, log, pass);
            if (users != null)
                dataGrid1.ItemsSource = users.Data.DefaultView;
            else
                dataGrid1.ItemsSource = null;
            dataGrid1.SelectedIndex = -1;
        }

        PageInfo pageinfo;
        Button[] btns;
        private void ServiceCallBack_BaseUpdate(string table)
        {
            if (table == "clients")
            {
                int totalpages = pageinfo.TotalPages;
                users = ServiceConnection.Channel.GetData("clients", (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                pageinfo.PageSize, log, pass);
                if (pageinfo.PageNumber > pageinfo.TotalPages && pageinfo.TotalPages > 0)
                {
                    pageinfo.PageNumber = pageinfo.TotalPages;
                }
                if ((totalpages < PageInfo.TotalBtns && pageinfo.TotalPages > totalpages)
                    || (totalpages > pageinfo.TotalPages && pageinfo.TotalPages < PageInfo.TotalBtns))
                {
                    BtnsInitialize(ref btns);
                }
                pageinfo.BtnPagination(ref btns);
                if (users != null)
                    dataGrid1.ItemsSource = users.Data.DefaultView;
                else
                    dataGrid1.ItemsSource = null;
            }
        }

        WareHouseData users;
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            EditUser edit = new EditUser();
            if (edit.ShowDialog() == true)
            {
                int totalpages = pageinfo.TotalPages;
                pageinfo.TotalItems++;
                pageinfo.PageNumber = pageinfo.TotalPages;
                users = ServiceConnection.Channel.GetData("clients", 0, 10);
                if (totalpages < PageInfo.TotalBtns && pageinfo.TotalPages > totalpages)
                {
                    BtnsInitialize(ref btns);
                }
                pageinfo.BtnPagination(ref btns);
                dataGrid1.ItemsSource = users.Data.DefaultView;
                dataGrid1.SelectedIndex = -1;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex == -1)
                return;
            DataRowView drv = (DataRowView)dataGrid1.SelectedItem;
            object[] obj = drv.Row.ItemArray;
            EditUser edit = new EditUser(obj);
            if (edit.ShowDialog() == true)
            {
                users = ServiceConnection.Channel.GetData("clients", (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                pageinfo.PageSize, log, pass);
                dataGrid1.ItemsSource = users.Data.DefaultView;
                dataGrid1.SelectedIndex = -1;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex == -1) return;
            IList list = dataGrid1.SelectedItems;

            DataRowView[] drv = new DataRowView[list.Count];
            list.CopyTo(drv, 0);
            if (MessageBox.Show("Вы действительно хотите удалить эту запись?",
                "Предупреждение!", MessageBoxButton.OKCancel, MessageBoxImage.Warning)
                == MessageBoxResult.OK)
            {
                for (int i = 0; i < drv.Length; i++)
                {
                    int del = (int)drv[i].Row[0];
                    ServiceConnection.Channel.Delete("clients", (uint)del);
                }
                int totalpages = pageinfo.TotalPages;
                pageinfo.TotalItems = ServiceConnection.Channel.GetRowsCount("clients");
                if (pageinfo.PageNumber > pageinfo.TotalPages && pageinfo.TotalPages > 0)
                {
                    pageinfo.PageNumber = pageinfo.TotalPages;
                }
                if (totalpages > pageinfo.TotalPages && pageinfo.TotalPages < PageInfo.TotalBtns)
                {
                    BtnsInitialize(ref btns);
                }
                pageinfo.BtnPagination(ref btns);
                users = ServiceConnection.Channel.GetData("clients", (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                pageinfo.PageSize, log, pass);
                if (users != null)
                    dataGrid1.ItemsSource = users.Data.DefaultView;
                else
                    dataGrid1.ItemsSource = null;
                dataGrid1.SelectedIndex = -1;
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (dataGrid1.SelectedIndex == -1 )
                VisBtn(false);
            else
            {
                DataRowView drv = (DataRowView)dataGrid1.SelectedItem;
                object[] obj = drv.Row.ItemArray;
                if ((string)obj[3] == "admin")
                    VisBtn(false);
                else
                    VisBtn(true);
            }
        }
        string log, pass;
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            pageinfo.PageNumber = 1;
            int totalpages = pageinfo.TotalPages;
            WareHouseData whd = ServiceConnection.Channel.GetData("clients", 0, 10, txtLogin.Text, txtPassword.Text);
            pageinfo.TotalItems = ServiceConnection.Channel.GetRowsCount("clients", txtLogin.Text, txtPassword.Text);
            if (totalpages != pageinfo.TotalPages)
            {
                if (totalpages < PageInfo.TotalBtns || pageinfo.TotalPages < PageInfo.TotalBtns)
                    BtnsInitialize(ref btns);
                pageinfo.BtnPagination(ref btns);
            }
            if (whd != null)
                dataGrid1.ItemsSource = whd.Data.DefaultView;
            else
            {
                dataGrid1.ItemsSource = null;
                MessageBox.Show("Ничего не найдено");
            }
            log = txtLogin.Text;
            pass = txtPassword.Text;
        }
        void VisBtn(bool b)
        {
            if (b)
            {
                BtnUpdate.Visibility = Visibility.Visible;
                BtnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                BtnUpdate.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
            }
            BtnUpdate.IsEnabled = b;
            BtnDelete.IsEnabled = b;
        }

        void BtnsInitialize(ref Button[] btns)
        {
            spanel1.Children.RemoveRange(0, btns.Length);
            int N = pageinfo.TotalPages;
            if (N > PageInfo.TotalBtns)
                N = PageInfo.TotalBtns;
            btns = new Button[N];
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i] = new Button();
                btns[i].Width = 20;
                btns[i].Height = 20;
                btns[i].Click += BtnPagin;
                btns[i].Tag = i + 1;
                btns[i].Content = i + 1;
                spanel1.Children.Add(btns[i]);
            }
        }
    }
}
