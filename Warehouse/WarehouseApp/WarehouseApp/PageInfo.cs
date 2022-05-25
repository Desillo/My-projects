using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WarehouseApp
{
    public class PageInfo
    {
        
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
        public static int TotalBtns = 5;
        public void BtnPagination(ref Button[] btns)
        {
            if (btns.Length < TotalBtns)
            {
                for (int i = 0; i < btns.Length; i++)
                {
                    if (i == PageNumber - 1)
                    {
                        btns[i].IsEnabled = false;
                    }
                    else
                    {
                        btns[i].IsEnabled = true;
                    }
                }
                return;
            }
            int x = ((int)btns[btns.Length - 1].Tag - (int)btns[0].Tag) / 2;
            int a = PageNumber + x - TotalPages;
            int b = PageNumber - x - 1;
            if (a > 0)
            {
                x += a;
            }
            else if (b < 0)
            {
                x += b;
            }
            for (int i = 0; i < btns.Length; i++)
            {
                if (i == x)
                {
                    btns[i].IsEnabled = false;
                }
                else
                {
                    btns[i].IsEnabled = true;
                }
                btns[i].Tag = PageNumber + i - x;
                btns[i].Content = btns[i].Tag;
            }
        }
    }
}
