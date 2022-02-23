using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreService
{
    public interface InputDriver
    {
        double ReturnValue(string address);
    }
}