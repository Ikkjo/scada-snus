using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Report Manager Menu");
            Console.WriteLine("    1. Dobavi po vremenskom opsegu");
            Console.WriteLine("    2. Dobavi po prioritetu");
            Console.WriteLine("    3. Dobavi po vremenu otpreme");
            Console.WriteLine("    4. Dobavi najskoriji AnalogInput tag");
            Console.WriteLine("    5. Dobavi najskoriji DigitalInput tag");
            Console.WriteLine("    6. Dobavi po imenu");
            Console.WriteLine("x - Exit");
            Console.Write("\r\nSelect an option: ");
        }
    }
}
