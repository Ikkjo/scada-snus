using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Trending
{
    class TrendingCallback : TrendingServiceReference.ITrendingServiceCallback
    {
        public void OnInputValueChanged(string tagName, double value)
        {

            Console.WriteLine($"Tag {tagName} value updated: {value}");
        }
    }


    class Program
    {
        private static TrendingServiceReference.TrendingServiceClient client = new TrendingServiceReference.TrendingServiceClient(new InstanceContext(new TrendingCallback()));
        static void Main(string[] args)
        {
            Console.WriteLine("Trending client started (press any key to exit)...");
            client.Init();
            Console.ReadKey();
        }
    }
}
