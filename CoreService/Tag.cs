using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.

namespace CoreService
{
    [DataContract]
    public abstract class  Tag
    {
        [Key]
        String tagName { get; set; }
        String description { get; set; }
        String IOAddress { get; set; }
    }
}
