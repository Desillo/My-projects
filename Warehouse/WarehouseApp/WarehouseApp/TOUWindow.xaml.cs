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
    /// Логика взаимодействия для TOUWindow.xaml
    /// </summary>
    public partial class TOUWindow : Window                                                                                     
    {
        public TOUWindow(string table)
        {
            InitializeComponent();
            if (table == "type")
            {
                Title = "Типы";
            }
            else if (table == "unit")
            {    
                Title = "Единицы измерения";
            }

            pageinfo = new PageInfo()
            {
                PageNumber = 1,
                PageSize = 3,
                TotalItems = ServiceConnection.Channel.GetRowsCount(table)
            };
            
            typesorunits = ServiceConnection.Channel.GetData(table, (pageinfo.PageNumber-1)*pageinfo.PageSize, pageinfo.PageSize);
            if (typesorunits != null)
            {
                for (int i = 0; i < typesorunits.Data.Rows.Count; i++)
                    dataGrid1.Items.Add(typesorunits.Data.DefaultView[i]);
            }

            Table = table;
            ServiceCallBack.BaseUpdate += ServiceCallBack_BaseUpdate;
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
            if(N>0)
                btns[0].IsEnabled = false;
            search = "";
            
        }
        string search;
        PageInfo pageinfo;
        private void BtnPagin(object sender, RoutedEventArgs e)
        {
            int page = (int)(sender as Button).Tag;
            pageinfo.PageNumber = page;
            pageinfo.BtnPagination(ref btns);
            typesorunits = ServiceConnection.Channel.GetData(Table, (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                pageinfo.PageSize, search);
            dataGrid1.Items.Clear();
            if (typesorunits != null)
            {
                for (int i = 0; i < typesorunits.Data.Rows.Count; i++)
                    dataGrid1.Items.Add(typesorunits.Data.DefaultView[i]);
                dataGrid1.SelectedIndex = -1;
            }
        }

        WareHouseData typesorunits;
        Button[] btns;
        private void ServiceCallBack_BaseUpdate(string table)
        {
            if (table == "type" || table == "unit")
            {
                int totalpages = pageinfo.TotalPages;
                typesorunits = ServiceConnection.Channel.GetData(table, (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                    pageinfo.PageSize);
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
                dataGrid1.Items.Clear();
                if(typesorunits != null)
                {
                    for (int i = 0; i < typesorunits.Data.Rows.Count; i++)
                        dataGrid1.Items.Add(typesorunits.Data.DefaultView[i]);
                }
                
            }
        }

        string Table;

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            EditTypeOrUnit edit = new EditTypeOrUnit(Table);
            if (edit.ShowDialog() == true)
            {
                int totalpages = pageinfo.TotalPages;
                pageinfo.TotalItems++;
                pageinfo.PageNumber = pageinfo.TotalPages;
                typesorunits = ServiceConnection.Channel.GetData(Table, (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                    pageinfo.PageSize);
                if (totalpages < PageInfo.TotalBtns && pageinfo.TotalPages>totalpages)
                {
                    BtnsInitialize(ref btns);
                }
                pageinfo.BtnPagination(ref btns);
                dataGrid1.Items.Clear();
                for (int i = 0; i < typesorunits.Data.Rows.Count; i++)
                    dataGrid1.Items.Add(typesorunits.Data.DefaultView[i]);
                dataGrid1.SelectedIndex = -1;
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex == -1) return;
            DataRowView drv = (DataRowView)dataGrid1.SelectedItem;
            object[] obj = drv.Row.ItemArray;
            EditTypeOrUnit edit = new EditTypeOrUnit(Table, obj);
            if (edit.ShowDialog() == true)
            {
                typesorunits = ServiceConnection.Channel.GetData(Table, (pageinfo.PageNumber - 1) * pageinfo.PageSize,
                    pageinfo.PageSize, search);
                dataGrid1.Items.Clear();
                for (int i = 0; i < typesorunits.Data.Rows.Count; i++)
                    dataGrid1.Items.Add(typesorunits.Data.DefaultView[i]);
                dataGrid1.SelectedIndex = -1;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex == -1) return;
            IList list = dataGrid1.SelectedItems;

            DataRowView[] drv = new DataRowView[list.Count];
            list.CopyTo(drv, 0);
            
            
            if (MessageBox.Show("Вы действительно хотите удалить эту запись?",
                "Предупреждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                for (int i = 0; i < drv.Length; i++)
                {
                    int del = (int)drv[i].Row[0];
                    ServiceConnection.Channel.Delete(Table, (uint)del);
                }
                int totalpages = pageinfo.TotalPages;
                pageinfo.TotalItems = ServiceConnection.Channel.GetRowsCount(Table);
                if (pageinfo.PageNumber > pageinfo.TotalPages && pageinfo.TotalPages>0)
                {
                    pageinfo.PageNumber = pageinfo.TotalPages;
                }
                if (totalpages > pageinfo.TotalPages && pageinfo.TotalPages<PageInfo.TotalBtns)
                {
                    BtnsInitialize(ref btns);
                }
                pageinfo.BtnPagination(ref btns);
                typesorunits = ServiceConnection.Channel.GetData(Table, (pageinfo.PageNumber - 1) * pageinfo.PageSize, 
                    pageinfo.PageSize, search);
                dataGrid1.Items.Clear();
                if (typesorunits != null)
                {
                    for (int i = 0; i < typesorunits.Data.Rows.Count; i++)
                        dataGrid1.Items.Add(typesorunits.Data.DefaultView[i]);
                }
                dataGrid1.SelectedIndex = -1;
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.SelectedIndex == -1)
                VisBtn(false);
            else
                VisBtn(true);
        }

        void VisBtn(bool b)
        {
            if (b)
            {
                BtnUpdate.Visibility = Visibility.Visible;
                BtnDelete.Visibility = Visibility.Visible;
                BtnSelect.Visibility = Visibility.Visible;
            }
            else
            {
                BtnUpdate.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
                BtnSelect.Visibility = Visibility.Collapsed;
            }
            BtnUpdate.IsEnabled = b;
            BtnDelete.IsEnabled = b;
            BtnSelect.IsEnabled = b;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            pageinfo.PageNumber = 1;
            int totalpages = pageinfo.TotalPages;
            WareHouseData whd = ServiceConnection.Channel.GetData(Table, (pageinfo.PageNumber - 1) * pageinfo.PageSize, 
                pageinfo.PageSize, txt1.Text);
            pageinfo.TotalItems = ServiceConnection.Channel.GetRowsCount(Table, txt1.Text);
            
            if (totalpages!=pageinfo.TotalPages)
            {
                if(totalpages < PageInfo.TotalBtns || pageinfo.TotalPages < PageInfo.TotalBtns)
                    BtnsInitialize(ref btns);
                pageinfo.BtnPagination(ref btns);
            }
            
            dataGrid1.Items.Clear();
            if (whd != null)
            {
                for (int i = 0; i < whd.Data.Rows.Count; i++)
                    dataGrid1.Items.Add(whd.Data.DefaultView[i]);
            }
            else
            {
                MessageBox.Show("Ничего не найдено");
            }
            search = txt1.Text;
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex == -1) return;
            DataRowView drv = (DataRowView)dataGrid1.SelectedItem;
            object[] obj = drv.Row.ItemArray;
            Tag = obj[1].ToString();
            Close();
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
