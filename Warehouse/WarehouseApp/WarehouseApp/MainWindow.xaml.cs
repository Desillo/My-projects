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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseDLL;

namespace WarehouseApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string login, string position)
        {
            InitializeComponent();
            ServiceCallBack.BaseUpdate += ServiceCallBack_BaseUpdate;
            Title += " Пользователь: " + login + " Должность: " + position;

            pageproducts = new PageInfo()
            {
                PageNumber = 1,
                PageSize = 2,
                TotalItems = ServiceConnection.Channel.GetRowsCount("product")
            };
            pageproductshop = new PageInfo()
            {
                PageNumber = 1,
                PageSize = 2,
                TotalItems = ServiceConnection.Channel.GetRowsCount("productshop")
            };
            products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber-1)*pageproducts.PageSize,
                pageproducts.PageSize);
            if (products != null)
                dataGrid1.ItemsSource = products.Data.DefaultView;
            else
                dataGrid1.ItemsSource = null;

            productshop = ServiceConnection.Channel.GetData("productshop",
                (pageproductshop.PageNumber - 1) * pageproductshop.PageSize,
                pageproductshop.PageSize);

            if (productshop != null)
                dataGrid2.ItemsSource = productshop.Data.DefaultView;
            else
                dataGrid2.ItemsSource = null;

            comboProvider.DisplayMemberPath = "Name";
            comboType.DisplayMemberPath = "Name";
            comboUnit.DisplayMemberPath = "Name";

            WareHouseData types = ServiceConnection.Channel.GetData("type", 0, 10);
            WareHouseData units = ServiceConnection.Channel.GetData("unit", 0, 10);
            WareHouseData providers = ServiceConnection.Channel.GetData("provider", 0, 10);

            if (types != null)
            {
                foreach (DataRowView drv in types.Data.DefaultView)
                    comboType.Items.Add(drv);
            }
            if (units != null)
            {
                foreach (DataRowView drv in units.Data.DefaultView)
                    comboUnit.Items.Add(drv);
            }
            if (providers != null)
            {
                foreach (DataRowView drv in providers.Data.DefaultView)
                    comboProvider.Items.Add(drv);
            }
            comboProvider.SelectedIndex = 0;
            comboType.SelectedIndex = 0;
            comboUnit.SelectedIndex = 0;
            comboStock.Items.Add("Основной склад");
            comboStock.Items.Add("Торговый зал");
            switch (position)
            {
                case "кассир":
                    whouse.Visibility = Visibility.Collapsed;
                    whouse.IsEnabled = false;
                    comboStock.SelectedIndex = 1;
                    stocksearch.Visibility = Visibility.Collapsed;
                    stocksearch.IsEnabled = false;
                    break;
                case "кладовщик":
                    shoproom.Visibility = Visibility.Collapsed;
                    shoproom.IsEnabled = false;
                    stocksearch.Visibility = Visibility.Collapsed;
                    stocksearch.IsEnabled = false;
                    break;

            }
            comboStock.SelectedIndex = 0;

            btns1.Add(BtnUpdate);
            btns1.Add(BtnDelete);
            btns1.Add(BtnSend);
            btns2.Add(BtnUpdate2);
            btns2.Add(BtnDelete2);
            btns2.Add(BtnSend2);
            btns2.Add(BtnWriteOff);
            VisBtn(false, ref btns1);
            VisBtn(false, ref btns2);

            int N = pageproducts.TotalPages;
            if (N > PageInfo.TotalBtns)
                N = PageInfo.TotalBtns;
            btnsproduct = new Button[N];
            for (int i = 0; i < btnsproduct.Length; i++)
            {
                btnsproduct[i] = new Button();
                btnsproduct[i].Width = 20;
                btnsproduct[i].Height = 20;
                btnsproduct[i].Tag = i + pageproducts.PageNumber;
                btnsproduct[i].Content = i + pageproducts.PageNumber;
                btnsproduct[i].Click += BtnPagin; ;

                spanel1.Children.Add(btnsproduct[i]);
            }
            if (N > 0)
                btnsproduct[0].IsEnabled = false;

            N = pageproductshop.TotalPages;
            if (N > PageInfo.TotalBtns)
                N = PageInfo.TotalBtns;
            btnsproductshop = new Button[N];
            for (int i = 0; i < btnsproductshop.Length; i++)
            {
                btnsproductshop[i] = new Button();
                btnsproductshop[i].Width = 20;
                btnsproductshop[i].Height = 20;
                btnsproductshop[i].Tag = i + pageproductshop.PageNumber;
                btnsproductshop[i].Content = i + pageproductshop.PageNumber;
                btnsproductshop[i].Click += BtnPagin; ;

                spanel2.Children.Add(btnsproductshop[i]);
            }
            if (N > 0)
                btnsproductshop[0].IsEnabled = false;
            
        }

        private void BtnPagin(object sender, RoutedEventArgs e)
        {
            int page = (int)(sender as Button).Tag;
            if (tabControl1.SelectedIndex == 0)
            {
                pageproducts.PageNumber = page;
                pageproducts.BtnPagination(ref btnsproduct);
                products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber - 1) * pageproducts.PageSize,
                    pageproducts.PageSize, objects);
                if (products != null)
                    dataGrid1.ItemsSource = products.Data.DefaultView;
                else
                    dataGrid1.ItemsSource = null;
                dataGrid1.SelectedIndex = -1;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                pageproductshop.PageNumber = page;
                pageproductshop.BtnPagination(ref btnsproductshop);
                productshop = ServiceConnection.Channel.GetData("productshop", (pageproductshop.PageNumber - 1) * pageproductshop.PageSize,
                    pageproductshop.PageSize, objects);
                if (products != null)
                    dataGrid2.ItemsSource = productshop.Data.DefaultView;
                else
                    dataGrid2.ItemsSource = null;
                dataGrid1.SelectedIndex = -1;
            }
        }

        WareHouseData products;
        WareHouseData productshop;
        List<Button> btns1 = new List<Button>();
        List<Button> btns2 = new List<Button>();
        PageInfo pageproducts;
        PageInfo pageproductshop;
        Button[] btnsproduct;
        Button[] btnsproductshop;
        private void ServiceCallBack_BaseUpdate(string table)
        {
            if (table == "product")
            {
                int totalpages = pageproducts.TotalPages;
                products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber - 1) * pageproducts.PageSize,
                    pageproducts.PageSize, objects);
                pageproducts.TotalItems = ServiceConnection.Channel.GetRowsCount("product");
                if (pageproducts.PageNumber > pageproducts.TotalPages && pageproducts.TotalPages > 0)
                {
                    pageproducts.PageNumber = pageproducts.TotalPages;
                }
                if ((totalpages < PageInfo.TotalBtns && pageproducts.TotalPages > totalpages)
                    || (totalpages > pageproducts.TotalPages && pageproducts.TotalPages < PageInfo.TotalBtns))
                {
                    BtnsInitialize(ref btnsproduct, ref pageproducts, ref spanel1);
                }
                pageproducts.BtnPagination(ref btnsproduct);
                if (products != null)
                    dataGrid1.ItemsSource = products.Data.DefaultView;
                else
                    dataGrid1.ItemsSource = null;
                dataGrid1.SelectedIndex = -1;
            }
            else if (table == "productshop")
            {
                int totalpages = pageproducts.TotalPages;
                productshop = ServiceConnection.Channel.GetData("productshop", (pageproductshop.PageNumber - 1) * pageproductshop.PageSize,
                    pageproductshop.PageSize, objects);
                pageproductshop.TotalItems = ServiceConnection.Channel.GetRowsCount("productshop");
                if (pageproductshop.PageNumber > pageproductshop.TotalPages && pageproductshop.TotalPages > 0)
                {
                    pageproductshop.PageNumber = pageproductshop.TotalPages;
                }
                if ((totalpages < PageInfo.TotalBtns && pageproductshop.TotalPages > totalpages)
                    || (totalpages > pageproductshop.TotalPages && pageproductshop.TotalPages < PageInfo.TotalBtns))
                {
                    BtnsInitialize(ref btnsproductshop, ref pageproductshop, ref spanel2);
                }
                pageproductshop.BtnPagination(ref btnsproductshop);
                if (productshop != null)
                    dataGrid2.ItemsSource = productshop.Data.DefaultView;
                else
                    dataGrid2.ItemsSource = null;
                dataGrid2.SelectedIndex = -1;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            EditProduct edit = new EditProduct();
            if (edit.ShowDialog() == true)
            {
                int totalpages = pageproducts.TotalPages;
                pageproducts.TotalItems++;
                pageproducts.PageNumber = pageproducts.TotalPages;
                products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber - 1) * pageproducts.PageSize,
                    pageproducts.PageSize);
                if (totalpages < PageInfo.TotalBtns && pageproducts.TotalPages > totalpages)
                {
                    BtnsInitialize(ref btnsproduct, ref pageproducts, ref spanel1);
                }
                pageproducts.BtnPagination(ref btnsproduct);
                dataGrid1.ItemsSource = products.Data.DefaultView;
                dataGrid1.SelectedIndex = -1;
            }
            
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
        //    if (dataGrid1.SelectedIndex == -1) return;
          //  DataRowView drv = (DataRowView)dataGrid1.SelectedItem;
            object[] obj = drvs[0].Row.ItemArray;
            EditProduct edit = new EditProduct(table1, obj);
            if (edit.ShowDialog() == true)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber - 1) * pageproducts.PageSize,
                    pageproducts.PageSize, objects);
                    dataGrid1.ItemsSource = products.Data.DefaultView;
                    dataGrid1.SelectedIndex = -1;
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    productshop = ServiceConnection.Channel.GetData("productshop", (pageproductshop.PageNumber - 1) * pageproductshop.PageSize,
                    pageproductshop.PageSize, objects);
                    dataGrid2.ItemsSource = productshop.Data.DefaultView;
                    dataGrid2.SelectedIndex = -1;
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
       //     if (dataGrid1.SelectedIndex == -1) return;
            
            if (MessageBox.Show("Вы действительно хотите удалить эту запись?",
                "Предупреждение!", MessageBoxButton.OKCancel, MessageBoxImage.Warning)
                == MessageBoxResult.OK)
            {
                // DataRowView drv = (DataRowView)dataGrid1.SelectedItem;
                for (int i = 0; i < drvs.Length; i++)
                {
                    int a = (int)drvs[i].Row.ItemArray[0];
                    ServiceConnection.Channel.Delete(table1, (uint)a);
                }
                if (tabControl1.SelectedIndex == 0)
                {
                    int totalpages = pageproducts.TotalPages;
                    pageproducts.TotalItems = ServiceConnection.Channel.GetRowsCount("product");
                    if (pageproducts.PageNumber > pageproducts.TotalPages && pageproducts.TotalPages > 0)
                    {
                        pageproducts.PageNumber = pageproducts.TotalPages;
                    }
                    if (totalpages > pageproducts.TotalPages && pageproducts.TotalPages < PageInfo.TotalBtns)
                    {
                        BtnsInitialize(ref btnsproduct, ref pageproducts, ref spanel1);
                    }
                    pageproducts.BtnPagination(ref btnsproduct);
                    products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber - 1) * pageproducts.PageSize,
                    pageproducts.PageSize, objects);
                    if (products != null)
                        dataGrid1.ItemsSource = products.Data.DefaultView;
                    else
                        dataGrid1.ItemsSource = null;
                    dataGrid1.SelectedIndex = -1;
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    int totalpages = pageproductshop.TotalPages;
                    pageproductshop.TotalItems = ServiceConnection.Channel.GetRowsCount("productshop"); 
                    if (pageproductshop.PageNumber > pageproductshop.TotalPages && pageproductshop.TotalPages > 0)
                    {
                        pageproductshop.PageNumber = pageproductshop.TotalPages;
                    }
                    if (totalpages > pageproductshop.TotalPages && pageproductshop.TotalPages < PageInfo.TotalBtns)
                    {
                        BtnsInitialize(ref btnsproductshop, ref pageproductshop, ref spanel2);
                    }
                    pageproductshop.BtnPagination(ref btnsproductshop);
                    productshop = ServiceConnection.Channel.GetData("productshop", (pageproductshop.PageNumber - 1) * pageproductshop.PageSize,
                    pageproductshop.PageSize, objects);
                    if (productshop != null)
                        dataGrid2.ItemsSource = productshop.Data.DefaultView;
                    else
                        dataGrid2.ItemsSource = null;
                    dataGrid2.SelectedIndex = -1;
                }
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (dataGrid1.SelectedIndex == -1)
                    VisBtn(false, ref btns1);
                else
                {
                    VisBtn(true, ref btns1);
                    IList list = dataGrid1.SelectedItems;
                    drvs = new DataRowView[list.Count];
                    list.CopyTo(drvs, 0);
           //         drv = (DataRowView)dataGrid1.SelectedItem;
                }
                VisBtn(false, ref btns2);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                if (dataGrid2.SelectedIndex == -1)
                    VisBtn(false, ref btns2);
                else
                {
                    VisBtn(true, ref btns2);
                    IList list = dataGrid2.SelectedItems;
                    drvs = new DataRowView[list.Count];
                    list.CopyTo(drvs, 0);
                  //  drv = (DataRowView)dataGrid2.SelectedItem;
                }
                VisBtn(false, ref btns1);
            }
        }
        DataRowView[] drvs;
        
        void VisBtn(bool b, ref List<Button> btns)
        {
            if (b)
            {
                foreach (Button btn in btns)
                {
                    btn.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach (Button btn in btns)
                {
                    btn.Visibility = Visibility.Collapsed;
                }
            }
            foreach (Button btn in btns)
            {
                btn.IsEnabled = b;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            DateTime fdate, wdate;
            DateTime.TryParse(datefrom.Text, out fdate);
            DateTime.TryParse(datewhere.Text, out wdate);
            int fprice, wprice, famount, wamount;
            int.TryParse(txtPriceFrom.Text, out fprice);
            int.TryParse(txtPriceWhere.Text, out wprice);
            int.TryParse(txtAmountFrom.Text, out famount);
            int.TryParse(txtAmountWhere.Text, out wamount);
            if (txtPriceWhere.Text == "")
            {
                wprice = int.MaxValue;
            }
            if (txtAmountWhere.Text == "")
            {
                wamount = int.MaxValue;
            }
            if (datewhere.Text == "")
            {
                wdate = DateTime.MaxValue;
            }
            List<object> obj = new List<object>();
            obj.Add(txtName.Text);
            obj.Add(comboType.Text);
            obj.Add(comboUnit.Text);
            obj.Add(fdate);
            obj.Add(wdate);
            obj.Add(fprice);
            obj.Add(wprice);
            obj.Add(famount);
            obj.Add(wamount);
            obj.Add(comboProvider.Text);
            objects = obj.ToArray();
            if (comboStock.SelectedIndex == 0)
            {
                WareHouseData whd = ServiceConnection.Channel.GetData("product", 0, 10, objects);
                if (whd != null)
                    dataGrid1.ItemsSource = whd.Data.DefaultView;
                else
                {
                    dataGrid1.ItemsSource = null;
                    MessageBox.Show("Ничего не найдено");
                }
                tabControl1.SelectedIndex = 0;
            }
            else if (comboStock.SelectedIndex == 1)
            {
                WareHouseData whd = ServiceConnection.Channel.GetData("productshop", 0, 10, objects);
                if (whd != null)
                    dataGrid2.ItemsSource = whd.Data.DefaultView;
                else
                {
                    dataGrid2.ItemsSource = null;
                    MessageBox.Show("Ничего не найдено");
                }
                tabControl1.SelectedIndex = 1;
            }
            
        }
        object[] objects;
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            WriteOffWindow wow = new WriteOffWindow(drvs, table2, table1);
            wow.ShowDialog();
            int totalpages = pageproducts.TotalPages;
           
            pageproducts.TotalItems = ServiceConnection.Channel.GetRowsCount("product");
            if (pageproducts.PageNumber > pageproducts.TotalPages && pageproducts.TotalPages > 0)
            {
                pageproducts.PageNumber = pageproducts.TotalPages;
            }
            if ((totalpages < PageInfo.TotalBtns && pageproducts.TotalPages > totalpages)
                || (totalpages > pageproducts.TotalPages && pageproducts.TotalPages < PageInfo.TotalBtns))
            {
                BtnsInitialize(ref btnsproduct, ref pageproducts, ref spanel1);
            }
            pageproducts.BtnPagination(ref btnsproduct);
            products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber - 1) * pageproducts.PageSize,
               pageproducts.PageSize);
            if (products != null)
                dataGrid1.ItemsSource = products.Data.DefaultView;
            else
                dataGrid1.ItemsSource = null;
            dataGrid1.SelectedIndex = -1;
            totalpages = pageproductshop.TotalPages;
           
            pageproductshop.TotalItems = ServiceConnection.Channel.GetRowsCount("productshop");
            if (pageproductshop.PageNumber > pageproductshop.TotalPages && pageproductshop.TotalPages > 0)
            {
                pageproductshop.PageNumber = pageproductshop.TotalPages;
            }
            if ((totalpages < PageInfo.TotalBtns && pageproductshop.TotalPages > totalpages)
                || (totalpages > pageproductshop.TotalPages && pageproductshop.TotalPages < PageInfo.TotalBtns))
            {
                BtnsInitialize(ref btnsproductshop, ref pageproductshop, ref spanel2);
            }
            pageproductshop.BtnPagination(ref btnsproductshop);
            productshop = ServiceConnection.Channel.GetData("productshop", (pageproductshop.PageNumber-1)* pageproductshop.PageSize,
                pageproductshop.PageSize);
            if (productshop != null)
                dataGrid2.ItemsSource = productshop.Data.DefaultView;
            else
                dataGrid2.ItemsSource = null;
            dataGrid2.SelectedIndex = -1;
        }

        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно выйти?",
                "Предупреждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                LoginWindow lw = new LoginWindow();
                if (ServiceConnection.Channel != null)
                {
                    ServiceConnection.Channel.ClientDisconnect();
                    ServiceConnection.Channel = null;
                }
                lw.Show();
                Close();
            }
        }
        string table1, table2;

        private void BtnPageEnter_Click(object sender, RoutedEventArgs e)
        {
            PageSizeWindow psw = new PageSizeWindow();

            if (psw.ShowDialog() == true)
            {
                int pagesize = (int)psw.Tag;
                pageproducts.PageSize = pagesize;
                pageproductshop.PageSize = pagesize;
                pageproducts.PageNumber = 1;
                pageproductshop.PageNumber = 1;
                products = ServiceConnection.Channel.GetData("product", (pageproducts.PageNumber - 1) * pageproducts.PageSize,
                pageproducts.PageSize);
                if (products != null)
                    dataGrid1.ItemsSource = products.Data.DefaultView;
                else
                    dataGrid1.ItemsSource = null;

                productshop = ServiceConnection.Channel.GetData("productshop",
                    (pageproductshop.PageNumber - 1) * pageproductshop.PageSize,
                    pageproductshop.PageSize);

                if (productshop != null)
                    dataGrid2.ItemsSource = productshop.Data.DefaultView;
                else
                    dataGrid2.ItemsSource = null;
                pageproducts.BtnPagination(ref btnsproduct);
                pageproductshop.BtnPagination(ref btnsproductshop);
                BtnsInitialize(ref btnsproduct, ref pageproducts, ref spanel1);
                BtnsInitialize(ref btnsproductshop, ref pageproductshop, ref spanel2);
                if (btnsproduct.Length != 0)
                    btnsproduct[0].IsEnabled = false;
                if (btnsproductshop.Length != 0)
                    btnsproductshop[0].IsEnabled = false;
            }
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            UserWindow user = new UserWindow();
            user.ShowDialog();
            comboProvider.Items.Clear();
            WareHouseData prov = ServiceConnection.Channel.GetData("clients", 0, 10);
            if (prov != null)
            {
                comboProvider.Items.Add("");
                foreach (DataRowView drv in prov.Data.DefaultView)
                    comboProvider.Items.Add(drv);
            }
            if (user.Tag != null)
                comboProvider.Text = user.Tag.ToString();
        }

        private void BtnProv_Click(object sender, RoutedEventArgs e)
        {
            ProviderWindow1 pw1 = new ProviderWindow1();
            pw1.ShowDialog();
            comboProvider.Items.Clear();
            WareHouseData prov = ServiceConnection.Channel.GetData("provider", 0, 10);
            if (prov != null)
            {
                comboProvider.Items.Add("");
                foreach (DataRowView drv in prov.Data.DefaultView)
                    comboProvider.Items.Add(drv);
            }
            if (pw1.Tag != null)
                comboProvider.Text = pw1.Tag.ToString();
        }

        private void BtnType_Click(object sender, RoutedEventArgs e)
        {
            TOUWindow tow = new TOUWindow("type");
            tow.ShowDialog();
            comboType.Items.Clear();
            WareHouseData type = ServiceConnection.Channel.GetData("type", 0, 10);
            if (type != null)
            {
                comboType.Items.Add("");
                foreach (DataRowView drv in type.Data.DefaultView)
                    comboType.Items.Add(drv);
            }
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
                comboUnit.Items.Add("");
                foreach (DataRowView drv in unit.Data.DefaultView)
                    comboUnit.Items.Add(drv);
            }
            if (tow.Tag != null)
                comboUnit.Text = tow.Tag.ToString();
        }

        private void BtnSelling_Click(object sender, RoutedEventArgs e)
        {
            SellingWindow sw = new SellingWindow();
            sw.ShowDialog();
        }

        private void BtnWriteOff_Click(object sender, RoutedEventArgs e)
        {
            IList list = dataGrid2.SelectedItems;
            DataRowView[] drvs = new DataRowView[list.Count];
            list.CopyTo(drvs, 0);
            WriteOffWindow wow = new WriteOffWindow(drvs, "selling", "productshop");
            if (wow.ShowDialog() == true)
            {
                productshop = ServiceConnection.Channel.GetData("productshop", 0, 10);
                if (productshop != null)
                    dataGrid2.ItemsSource = productshop.Data.DefaultView;
                else
                    dataGrid2.ItemsSource = null;
            }
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                table1 = "product";
                table2 = "productshop";
                dataGrid2.SelectedIndex = -1;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                table2 = "product";
                table1 = "productshop";
                dataGrid1.SelectedIndex = -1;
            }
        }

        void BtnsInitialize(ref Button[] btns, ref PageInfo pageinfo, ref StackPanel spanel)
        {
            spanel.Children.RemoveRange(0, btns.Length);
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
                spanel.Children.Add(btns[i]);
            }
        }
    }
}
