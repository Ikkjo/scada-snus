using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoreService
{
    public class AlarmLog
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime TimeStamp {get; set; }
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public Alarm Alarm { get; set; }

        public AlarmLog() { }

        public AlarmLog(Alarm alarm, DateTime time, double val)
        {
            Alarm = alarm;
            TimeStamp = time;
            Value = val;
        }

        public override string ToString()
        {
            return $"Time: {TimeStamp}| Value: {Value}|| {Alarm}";
        }
    }
}