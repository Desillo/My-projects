using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarehouseService
{
    public class SQLOptions
    {
        public string IP { get; set; }
        public string DB { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class SQL
    {
        static protected MySqlConnection connection;
        static protected SQLOptions options;

        static internal void MysqlInit(SQLOptions options)
        {
            SQL.options = options;
            MySqlConnectionStringBuilder sb =
                new MySqlConnectionStringBuilder();
            sb.Server = options.IP;
            sb.Database = options.DB;
            sb.UserID = options.Login;
            sb.Password = options.Password;
            sb.CharacterSet = "utf8";
            connection = new MySqlConnection(sb.GetConnectionString(true));
        }
        static protected bool MysqlConnect()
        {
            if (connection == null)
            {
                MysqlInit(new SQLOptions
                {
                    DB = "warehousebase",
                    IP = "localhost",
                    Login = "root",
                    Password = "admin"
                });
            }
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        static protected void CloseConnect()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}
