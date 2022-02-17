using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CoreService
{
    // Ovo je samo primer proste implementacije simulacionog driver-a sa vrednostima signala koje se krecu od 0 do 100
    // Ukoliko u sistemu postoji i RealTime driver, preporuka je da se koristi nasledjivanje ili implementacija interfejsa, zarad uniformnog pristupa ovim driver-ima
    [DataContract]
    [KnownType(typeof(InputTag))]
    public class SimulationDriver: InputDriver
    {

            [DataMember]
            private static readonly List<string> addresses = new List<string> { "S", "C", "R" };

            public double ReturnValue(string address)
            {
                switch (address)
                {
                    case "S":
                        return Sine();
                    case "C":
                        return Cosine();
                    case "R":
                        return Ramp();
                    default:
                        return -1000;
                }
            }

            private static double Sine()
            {
                return 100 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
            }

            private static double Cosine()
            {
                return 100 * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI);
            }

            private static double Ramp()
            {
                return 100 * DateTime.Now.Second / 60;
            }
    }
}