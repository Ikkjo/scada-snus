using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CoreService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRealTimeUnitService" in both code and config file together.
    [ServiceContract]
    public interface IRealTimeUnitService
    {
        [OperationContract]
        bool AddRTU(string message, byte[] signature);
        [OperationContract]
        void SendValue(string message, byte[] signature);
    }
}
