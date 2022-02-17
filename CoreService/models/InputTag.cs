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
        InputDriver Driver { get; set; }
        [DataMember]
        int ScanTime { get; set; }
        [DataMember]
        bool ScanActive { get; set; }
    }
}