using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WarehouseDLL;

namespace WarehouseService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Client : SQL, IProductService, IDisposable
    {
        static List<Client> array = new List<Client>();
        string login;
        string password;
        IProductCallBack callback;

        public Client()
        {
            array.Add(this);
            callback = OperationContext.Current.
                        GetCallbackChannel<IProductCallBack>();
            login = "";
            //Console.WriteLine(login + " подключился");
        }

        ~Client()
        {
            array.Remove(this);
        }

        public void Add(string table, params object[] properties)
        {
            if (properties.Length != GetColumnNames(table).Length - 1)
                return;
            string strcol = "";
            string strprop = "";
            string[] columns = GetColumnNames(table);
            for (int i = 1; i < columns.Length; i++)
            {
                strcol += columns[i];
                strprop += "@" + columns[i];
                if (i < columns.Length - 1)
                {
                    strcol += ",";
                    strprop += ",";
                }
            }
            string sql = string.Format("INSERT into {0} ({1}) VALUES ({2})", table, strcol, strprop);
            MySqlCommand mc = new MySqlCommand(sql, connection);
            string[] str = strprop.Split(new char[] { ',' });

            for (int i = 0; i < properties.Length; i++)
            {
                mc.Parameters.AddWithValue(str[i], properties[i]);
            }

            if (MysqlConnect())
            {
                mc.ExecuteNonQuery();
                connection.Close();
                mc.Dispose();

            }
            foreach (Client C in array)
                if (!C.Equals(this))
                    C.callback.UpdateData(table);
            Console.WriteLine("Пользователь '{0}' добавил новый элемент в таблицу '{1}'", login, table);
        }

        public void Update(string table, params object[] properties)
        {
            if (properties.Length != GetColumnNames(table).Length)
                return;
            string strcol = "";
            
            string[] columns = GetColumnNames(table);
            string strprop = "@" + columns[0] + ",";
            for (int i = 1; i < columns.Length; i++)
            {
                strcol += string.Format("{0} = @{0}", columns[i]);
                strprop += "@" + columns[i];
                if (i < columns.Length - 1)
                {
                    strcol += ", ";
                    strprop += ",";
                }
            }
            strcol += string.Format(" WHERE {0} = @{0}", columns[0]);
            string sql = string.Format("UPDATE {0} SET {1}", table, strcol);
            MySqlCommand mc = new MySqlCommand(sql, connection);
            string[] str = strprop.Split(new char[] { ',' });
            for (int i = 0; i < properties.Length; i++)
            {
                mc.Parameters.AddWithValue(str[i], properties[i]);
            }

            if (MysqlConnect())
            {
                mc.ExecuteNonQuery();
                connection.Close();
                mc.Dispose();

            }
            foreach (Client C in array)
                if (!C.Equals(this))
                    C.callback.UpdateData(table);
            Console.WriteLine("Пользователь '{0}' изменил элемент в таблице '{1}'", login, table);
        }

        public void Delete(string table,uint id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE id = {1}", table, id);
            MySqlCommand mc = new MySqlCommand(sql, connection);
            if (MysqlConnect())
            {
                mc.ExecuteNonQuery();
                connection.Close();
                mc.Dispose();
            }
            foreach (Client C in array)
                if (!C.Equals(this))
                    C.callback.UpdateData(table);
            Console.WriteLine("Пользователь '{0}' удалил элемент из таблицы '{1}'", login, table);
        }


        public bool LoginClient(string log, string pass, out string position)
        {
            position = "";
            foreach (string s in GetAllNicks())
            {
                if (log == s) return false;
            }
            bool b = false;
            string sql = string.Format("SELECT * FROM clients WHERE login = '{0}' AND password = '{1}'", log, pass);
            if (MysqlConnect())
            {
                MySqlCommand mc = new MySqlCommand(sql, connection);
                MySqlDataReader dr = mc.ExecuteReader();
                if (dr.HasRows)
                {
                    b = true;
                    login = log;
                    password = pass;
                    dr.Read();
                    position = dr.GetString("position");
                    Console.WriteLine(login + " подключился");
                }
                CloseConnect();
            }
            return b;

        }
        static string[] GetColumnNames(string table)
        {
            List<string> array = new List<string>();
            string sql = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", table);
            if (MysqlConnect())
            {
                MySqlCommand mc = new MySqlCommand(sql, connection);
                MySqlDataReader dr = mc.ExecuteReader();
                while (dr.Read())
                {
                    array.Add((string)dr[0]);
                }

                CloseConnect();
            }

            return array.ToArray();
        }

        internal static string[] GetAllNicks()
        {
            List<string> nicks = new List<string>();
            foreach (Client cl in array)
            {
                nicks.Add(cl.login);
            }
            return nicks.ToArray();
        }

        public void ClientDisconnect()
        {
            Dispose();
        }

        public void Dispose()
        {
            array.Remove(this);
            if(login!="")
                Console.WriteLine(login + " отключился");
            
        }
        //SELECT * FROM table WHERE prop1 AND prop2 AND prop3 AND prop4 LIMIT start, limit
        public WareHouseData GetData(string table, int start, int limit, params object[] properties)
        {
            
            
            string strcol = "";
            List<MySqlParameter> msparray = new List<MySqlParameter>();
            if (properties != null && properties.Length!=0)
            {
                //if (properties.Length != columns.Length-1)
                //    return null;
                string[] columns = GetColumnNames(table);
                strcol += " WHERE ";
                int j = 0;
                for (int i = 0; i < properties.Length; i++)
                {
                    j++;
                    if (properties[i].GetType() == typeof(string))
                    {
                        if ((string)properties[i] == "")
                            continue;
                        strcol += string.Format("{0} RLIKE '{1}'", columns[j], properties[i]);
                    }
                    else if (properties[i].GetType() == typeof(int) && properties[i + 1].GetType() == typeof(int))
                    {
                        strcol += string.Format("({0} >= {1} AND {0} <= {2})", columns[j], properties[i], properties[i + 1]);
                        i++;
                    }
                    else if (properties[i].GetType() == typeof(DateTime) && properties[i + 1].GetType() == typeof(DateTime))
                    {
                        strcol += string.Format("{0} BETWEEN STR_TO_DATE(@{0}1, '%Y-%m-%d %H:%i:%s')" +
                            " AND STR_TO_DATE(@{0}2, '%Y-%m-%d %H:%i:%s')", columns[j]);
                        msparray.Add(new MySqlParameter("@" + columns[j] + "1", properties[i]));
                        msparray.Add(new MySqlParameter("@" + columns[j] + "2", properties[i+1]));
                        i++;
                    }
                    else
                    {
                        strcol += string.Format("{0} = @{0}", columns[j]);
                        msparray.Add(new MySqlParameter("@" + columns[j], properties[i]));
                    }
                    if (i < properties.Length - 1)
                    {
                        strcol += " AND ";
                    }
                    
                }
                if (strcol == " WHERE ")
                    strcol = "";
                else if (strcol.EndsWith(" AND "))
                    strcol = strcol.Remove(strcol.LastIndexOf(" AND "));

            }
            DataTable dt = new DataTable();
            WareHouseData whd = new WareHouseData();
            string sql = string.Format("SELECT * FROM {0}{1} LIMIT {2},{3}", table, strcol, start, limit);
            if (MysqlConnect())
            {
                MySqlCommand mc = new MySqlCommand(sql, connection);
                foreach (MySqlParameter msp in msparray)
                {
                    mc.Parameters.Add(msp);
                }
                MySqlDataReader dr = mc.ExecuteReader();
                if (dr.HasRows)
                {
                    dt.Load(dr);
                }
                else
                {
                    CloseConnect();
                    return null;
                }
                whd.Data = dt;
                CloseConnect();
            }

            return whd;
        }

        public int GetRowsCount(string table, params object[] properties)
        {
            

            string strcol = "";
            List<MySqlParameter> msparray = new List<MySqlParameter>();
            if (properties.Length != 0)
            {
                //if (properties.Length != columns.Length-1)
                //    return null;
                string[] columns = GetColumnNames(table);
                strcol += " WHERE ";
                int j = 0;
                for (int i = 0; i < properties.Length; i++)
                {
                    j++;
                    if (properties[i].GetType() == typeof(string))
                    {
                        if ((string)properties[i] == "")
                            continue;
                        strcol += string.Format("{0} RLIKE '{1}'", columns[j], properties[i]);
                    }
                    else if (properties[i].GetType() == typeof(int) && properties[i + 1].GetType() == typeof(int))
                    {
                        strcol += string.Format("({0} >= {1} AND {0} <= {2})", columns[j], properties[i], properties[i + 1]);
                        i++;
                    }
                    else if (properties[i].GetType() == typeof(DateTime) && properties[i + 1].GetType() == typeof(DateTime))
                    {
                        strcol += string.Format("({0} >= @{1}1 AND {0} <= @{2}2)", columns[j], properties[i], properties[i + 1]);
                        msparray.Add(new MySqlParameter("@" + columns[j] + "1", properties[i]));
                        msparray.Add(new MySqlParameter("@" + columns[j] + "2", properties[i+1]));
                        i++;
                    }
                    else
                    {
                        strcol += string.Format("{0} = @{0}", columns[j]);
                        msparray.Add(new MySqlParameter("@" + columns[j], properties[i]));
                    }
                    if (i < properties.Length - 1)
                    {
                        strcol += " AND ";
                    }

                }
                if (strcol == " WHERE ")
                    strcol = "";
                else if (strcol.EndsWith(" AND "))
                    strcol = strcol.Remove(strcol.LastIndexOf(" AND "));

            }

            string sql = string.Format("SELECT Count(*) FROM {0} {1}", table,strcol);
            long count = 0;
            if (MysqlConnect())
            {
                MySqlCommand mc = new MySqlCommand(sql, connection);
                foreach (MySqlParameter msp in msparray)
                {
                    mc.Parameters.Add(msp);
                }
                count = (long)mc.ExecuteScalar();
                CloseConnect();
            }

            return (int)count;
        }

    }
}
