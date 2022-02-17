using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreService
{
    public enum AlarmPriority
    {
        HI,
        MED,
        LO
    }
    public class Alarm
    {
        public string alarmMessage { get; set; }
        public AlarmPriority priority { get; set; }
        public bool processed { get; set; }
    }
}