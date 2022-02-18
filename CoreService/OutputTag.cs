using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoreService
{
    [DataContract]
    public abstract class OutputTag : Tag
    {
        [DataMember]
        public float InitialValue { get; set; }
        [DataMember]
        public float Value { get; set; }
    }
}