using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseDLL;
using System.ServiceModel;

namespace WarehouseApp
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class ServiceCallBack : IProductCallBack
    {
        public delegate void DBUpdate(string table);
        public static event DBUpdate BaseUpdate;
        public void UpdateData(string table)
        {
            if (BaseUpdate != null)
                BaseUpdate(table);
        }
    }
}
