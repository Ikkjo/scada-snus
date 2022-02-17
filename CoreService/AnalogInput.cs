using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService
{
    class AnalogInput: InputTag
    {
        public double lowLimit { get; set; }
        public double highLimit { get; set; }
        public string units { get; set; }
        public List<Alarm> alarms { get; set; }
    }
}
