﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoreService
{
    public enum AlarmPriority
    {
        HI,
        MED,
        LO
    }
    [DataContract]
    public class Alarm
    {
        [DataMember]
        public string alarmMessage { get; set; }
        [DataMember]
        public AlarmPriority priority { get; set; }
        [DataMember]
        public bool processed { get; set; }
    }
}