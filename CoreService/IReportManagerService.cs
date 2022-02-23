using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CoreService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportManagerService" in both code and config file together.
    [ServiceContract]
    public interface IReportManagerService
    {
        [OperationContract]
        List<AlarmLog> GetAlarmsByDate(DateTime dateFrom, DateTime dateTo, string sortBy, bool descending = false);
        [OperationContract]
        List<AlarmLog> GetAlarmsByPriority(int priority, bool descending = false);
        [OperationContract]
        List<TagValue> GetTagValuesByDate(DateTime dateFrom, DateTime dateTo, bool descending = false);
        [OperationContract]
        List<TagValue> GetMostRecentAIValues(bool descending = false);
        [OperationContract]
        List<TagValue> GetMostRecentDIValues(bool descending = false);
        [OperationContract]
        List<TagValue> GetAllTagValuesByID(string id, bool descending = false);
    }
}
