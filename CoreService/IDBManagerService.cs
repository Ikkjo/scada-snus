using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CoreService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDBManagerService
    {
        [OperationContract]
        bool ChangeOutputValue(string tagName, float newOutputValue);
        [OperationContract]
        float? GetOutputValue(string tagName);
        [OperationContract]
        bool SetScan(string tagName, bool scan);
        [OperationContract]
        bool AddTag(Tag newTag);
        [OperationContract]
        bool RemoveTag(string tagName);
        [OperationContract]
        bool RegisterUser(string username, string password);
        [OperationContract]
        string LogIn(string username, string password);
        [OperationContract]
        bool LogOut(string token);

    }


    //// Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
