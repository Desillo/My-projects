using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WarehouseDLL;

namespace WarehouseService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(Client));
            WSDualHttpBinding binding = new WSDualHttpBinding();
            binding.MaxReceivedMessageSize = 999999999;
            binding.SendTimeout = TimeSpan.FromSeconds(10);
            host.AddServiceEndpoint(typeof(IProductService),
                binding,
                "http://localhost:6000/WareHouse");
            host.Open();
            
            string cmd = "";
            Console.WriteLine("сервис поднят");
            while (cmd != "exit")
            {
                cmd = Console.ReadLine();
                if (cmd == "connecttodb")
                {
                    string ip, db, pass, log;
                    Console.Write("DB: ");
                    db = Console.ReadLine();
                    Console.Write("IP: ");
                    ip = Console.ReadLine();
                    Console.Write("Login: ");
                    log = Console.ReadLine();
                    Console.Write("Password: ");
                    pass = Console.ReadLine();
                    SQL.MysqlInit(new SQLOptions()
                    {
                        IP = ip,
                        DB = db,
                        Login = log,
                        Password = pass
                    });
                }
                else if (cmd == "/list")
                {
                    if (Client.GetAllNicks().Length != 0)
                    {
                        foreach (string s in Client.GetAllNicks())
                        {
                            Console.WriteLine(s);
                        }
                    }
                    else
                        Console.WriteLine("no clients!");
                }
            }
            
            Console.ReadLine();
            host.Close();
        }
    }
}
