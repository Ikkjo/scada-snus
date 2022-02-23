using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoreService
{

    public enum AlarmType
    {
        HIGH,
        LOW
    }
    [DataContract]
    public class Alarm
    {
        [DataMember]
        public string AlarmMessage { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public bool Processed { get; set; }
        [DataMember]
        public AlarmType Type { get; set; }
        [DataMember]
        public string TagName { get; set; }

        public Alarm() { }

        public override string ToString()
        {
            return $"ALARM||Type: {Type}| Priority: {Priority}| Tag: {TagName}";
        }
    }


}