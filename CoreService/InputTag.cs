using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreService
{
    public abstract class InputTag: Tag
    {
        InputDriver driver { get; set; }
        int scanTime { get; set; }
        bool scanActive { get; set; }
    }
}