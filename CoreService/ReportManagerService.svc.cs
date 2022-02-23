using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CoreService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReportManagerService.svc or ReportManagerService.svc.cs at the Solution Explorer and start debugging.
    public class ReportManagerService : IReportManagerService
    {
        public List<AlarmLog> GetAlarmsByDate(DateTime dateFrom, DateTime dateTo, string sortBy, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public List<AlarmLog> GetAlarmsByPriority(int priority, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public List<TagValue> GetAllTagValuesByID(string id, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public List<TagValue> GetMostRecentAIValues(bool descending = false)
        {
            throw new NotImplementedException();
        }

        public List<TagValue> GetMostRecentDIValues(bool descending = false)
        {
            throw new NotImplementedException();
        }

        public List<TagValue> GetTagValuesByDate(DateTime dateFrom, DateTime dateTo, bool descending = false)
        {
            throw new NotImplementedException();
        }
    }
}
