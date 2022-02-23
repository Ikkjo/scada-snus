using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CoreService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITrendingService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(ITrendingCallback))]
    public interface ITrendingService
    {
        [OperationContract]
        void Init();
    }

    public interface ITrendingCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnInputValueChanged(string tagName, double value);
    }
}
