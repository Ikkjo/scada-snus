using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoreService
{
    [DataContract]
    public abstract class InputTag: Tag
    {
        [DataMember]
        public string Driver { get; set; }
        [DataMember]
        public int ScanTime { get; set; }
        [DataMember]
        public bool ScanActive { get; set; }
    }
}