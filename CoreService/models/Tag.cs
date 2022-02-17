using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CoreService
{
    [DataContract]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(AnalogOutput))]
    [XmlInclude(typeof(DigitalInput))]
    [XmlInclude(typeof(DigitalOutput))]
    [KnownType(typeof(AnalogInput))]
    [KnownType(typeof(AnalogOutput))]
    [KnownType(typeof(DigitalInput))]
    [KnownType(typeof(DigitalOutput))]
    public abstract class  Tag
    {

        [Key]
        [DataMember]
        String TagName { get; set; }
        [DataMember]
        String Description { get; set; }
        [DataMember]
        String IOAddress { get; set; }
    }
}
