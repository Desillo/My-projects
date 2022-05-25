using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WarehouseDLL;

namespace WarehouseApp
{
    static class ServiceConnection
    {
        public static IProductService Channel;

        public static void Initialize()
        {
            WSDualHttpBinding binding = new WSDualHttpBinding();
            binding.SendTimeout = TimeSpan.FromSeconds(10);
            Channel = new DuplexChannelFactory<IProductService>(
               new ServiceCallBack(),
               binding,
               "http://localhost:6000/WareHouse").CreateChannel();

        }

    }
}
