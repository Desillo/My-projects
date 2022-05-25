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

namespace WarehouseApp
{
    /// <summary>
    /// Логика взаимодействия для PageSizeWindow.xaml
    /// </summary>
    public partial class PageSizeWindow : Window
    {
        public PageSizeWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int pagesize;
            int.TryParse(txtPageSize.Text, out pagesize);
            Tag = pagesize;
            DialogResult = true;
            Close();
        }
    }
}
