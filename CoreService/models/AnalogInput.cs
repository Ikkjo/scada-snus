using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CoreService
{
    [DataContract]
    class AnalogInput: InputTag
    {
        [DataMember]
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set; }
        [DataMember]
        public string Units { get; set; }
        [DataMember]
        public virtual List<Alarm> Alarms { get; set; }
        
        public AnalogInput() { }
    }
}
