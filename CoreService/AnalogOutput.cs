﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CoreService
{
    [DataContract]
    class AnalogOutput : OutputTag
    {
        [DataMember]
        public float LowLimit { get; set; }
        [DataMember]
        public float HighLimit { get; set; }
        [DataMember]
        public string Units { get; set; }

        public AnalogOutput() { }
    }
}
