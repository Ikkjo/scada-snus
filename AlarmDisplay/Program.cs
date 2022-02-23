using AlarmDisplay.AlarmDisplayServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class AlarmDisplayCallback : AlarmDisplayServiceReference.IAlarmDisplayServiceCallback
    {
        public void OnNewAlarmLog(Alarm alarm)
        {
            Console.WriteLine("===============================================");
            Console.WriteLine("==================== Alarm ====================");
            Console.WriteLine("===============================================");
            for (int i = 0; i < alarm.Priority; i++)
            {
                Console.WriteLine(alarm.ToString());
            }
        }
    }


    class Program
    {
        private static AlarmDisplayServiceReference.AlarmDisplayServiceClient client = new AlarmDisplayServiceReference.AlarmDisplayServiceClient(new InstanceContext(new AlarmDisplayCallback()));
        static void Main(string[] args)
        {
            Console.WriteLine("Alarm Display client started (press any key to exit)...");
            client.Init();
            Console.ReadKey();
        }
    }
}
