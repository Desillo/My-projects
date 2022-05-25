using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Data;

namespace WarehouseDLL
{
    [ServiceContract(CallbackContract = typeof(IProductCallBack))]
    public interface IProductService
    {
        [OperationContract]
        WareHouseData GetData(string table, int start, int limit, params object[] properties); 

        [OperationContract(IsOneWay = true)]
        void ClientDisconnect();

        [OperationContract(IsOneWay = true)]
        void Add(string table, params object[] properties);

        [OperationContract(IsOneWay = true)]
        void Update(string table, params object[] properties);

        [OperationContract(IsOneWay = true)]
        void Delete(string table, uint id);

        [OperationContract]
        bool LoginClient(string log, string pass, out string position);

        [OperationContract]
        int GetRowsCount(string table, params object[] properties);
    }

    [ServiceContract]
    public interface IProductCallBack
    {
        [OperationContract(IsOneWay = true)]
        void UpdateData(string table);
    }

    [DataContract]
    public class WareHouseData
    {
        [DataMember]
        public DataTable Data { get; set; }
    }

}
