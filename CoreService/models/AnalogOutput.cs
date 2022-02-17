using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService
{
    class AnalogOutput : OutputTag
    {
        public float LowLimit { get; set; }
        public float HighLimit { get; set; }
        public string Units { get; set; }

        public AnalogOutput() { }
    }
}
