using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Hosting;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace CoreService
{
    public static class TagProcessing
    {
        private const string SYSTEM_CONFIG_FILE = "systemConfig.xml";
        private const string ALARM_LOG_FILE = "alarmLog.txt";

        public delegate void ChangeInputTagDelegate(string input, double value);
        public static event ChangeInputTagDelegate inputChanged;

        public delegate void AlarmRaisedDelegate(Alarm alarm);
        public static event AlarmRaisedDelegate alarmRaised;

        public static readonly Dictionary<string, InputDriver> drivers = new Dictionary<string, InputDriver>();

        public static Dictionary<string, Thread> inputTagThreads = new Dictionary<string, Thread>();

        public static Dictionary<string, InputTag> inputTags = new Dictionary<string, InputTag>();
        public static Dictionary<string, OutputTag> outputTags = new Dictionary<string, OutputTag>();
        public static Dictionary<string, double> outputValues = new Dictionary<string, double>();
        public static Dictionary<string, Alarm> alarms = new Dictionary<string, Alarm>();

        public static readonly object locker = new object();

        static TagProcessing()
        {
            LoadSystemConfig();
            drivers.Add("SimulationDriver", new SimulationDriver());
            drivers.Add("RealTimeDriver", new RealTimeDriver());
            Simulate();
        }

        public static void Simulate()
        {
            foreach (Tag t in inputTags.Values.ToList())
            {
                if(t is InputTag)
                {
                    RunInputSimulationThread((InputTag)t);
                }
            }
        }

        private static void RunInputSimulationThread(InputTag t)
        {
            if (inputTagThreads.Count <= 4)
            {
                Thread th = new Thread(new ParameterizedThreadStart(SimulateInput));
            }
        }

        private static void SimulateInput(Object obj)
        {
            InputTag iTag = (InputTag)obj;
            double value;
            while(true)
            {
                if (iTag.ScanActive)
                {
                    lock (locker)
                    {
                        value = Math.Abs(drivers[iTag.Driver].ReturnValue(iTag.IOAddress));
                    }
                    if (iTag is DigitalInput)
                    {
                        value = GetDitigalTagValue(value);
                    }
                    else if (iTag is AnalogInput)
                    {
                        AnalogInput t = (AnalogInput)iTag;
                        value = GetAnalogTagValue(t.LowLimit, t.HighLimit, value);

                        // Check for alarm
                        CheckAlarm(t, value);
                    }

                    SaveNewTagValue(new TagValue(iTag.TagName, DateTime.Now, value, iTag.GetType().Name));
                    inputChanged?.Invoke(iTag.TagName, value);
                }
                Thread.Sleep(iTag.ScanTime * 1000);
            }
        }

        private static double GetAnalogTagValue(double lowLim, double highLim, double input)
        {
            double retVal = input < lowLim ? lowLim : input;
            retVal = input > highLim ? highLim : input;

            return retVal;
        }

        private static double GetDitigalTagValue(double input)
        {
            return input < 50 ? 0 : 1;
        }

        private static void SaveNewTagValue(TagValue tv)
        {
            using (var db = new TagContext())
            {
                tv.Id = db.TagValues.Count();
                db.TagValues.Add(tv);
                db.SaveChanges();

            }
        }

        private static void CheckAlarm(AnalogInput tag, double value)
        {
            using (var db = new TagContext())
            {
                foreach (Alarm a in tag.Alarms)
                {
                    if(tag.TagName == a.TagName)
                    {
                        if ((a.Type == AlarmType.LOW && value <= tag.LowLimit) || (a.Type == AlarmType.HIGH && value >= tag.HighLimit))
                        {
                            AlarmLog aLog = new AlarmLog(a, DateTime.Now, value);
                            //InvokeAlarm(a);
                            db.Alarms.Add(aLog);
                            //(alarmVal);
                        }
                    }
                    
                }
                db.SaveChanges();
            }
        }

        public static bool AddTag(Tag newTag)
        {
            using(var tagContext = new TagContext())
            {
                if (!DoesTagExist(newTag.TagName))
                {
                    tagContext.Tags.Add(newTag);
                    tagContext.SaveChanges();
                    return true;
                }
            }
            return false;

        }

        public static bool RemoveTag(string tagName)
        {
            if (DoesTagExist(tagName))
            {
                using (var tagContext = new TagContext())
                {
                    tagContext.Tags.Remove(tagContext.Tags.Find(tagName));
                    tagContext.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        public static bool SetScan(string tagName, bool scan)
        {
            if (DoesTagExist(tagName))
            {
                using (var tagContext = new TagContext())
                {
                    InputTag iTag = (InputTag)tagContext.Tags.Find(tagName);
                    iTag.ScanActive = scan;
                    tagContext.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        public static float? GetOutputValue(string tagName)
        {
            if (DoesTagExist(tagName))
            {
                using (var tagContext = new TagContext())
                {
                    OutputTag oTag = (OutputTag)tagContext.Tags.Find(tagName);

                    if (oTag != null)
                        return oTag.Value;

                }
            }
            return null;
        }

        public static bool ChangeOutputValue(string tagName, float newOutputValue)
        {
            if (DoesTagExist(tagName))
            {
                try
                {
                    using (var tagContext = new TagContext())
                    {
                        OutputTag t = (OutputTag)tagContext.Tags.Find(tagName);
                        t.Value = newOutputValue;
                        tagContext.SaveChanges();
                    }
                    return true;

                }
                catch (System.InvalidCastException e)
                {
                    return false;
                }

            }
            return false;
        }

        internal static bool DoesTagExist(string tagName)
        {
            using (var tagContext = new TagContext())
            {
                foreach (Tag t in tagContext.Tags)
                {
                    if (t.TagName.Equals(tagName))
                        return true;
                }
            }
            return false;
        }

        private static List<AnalogOutput> GetAnalogOutputs()
        {
            List<AnalogOutput> analogOutputList = new List<AnalogOutput>();

            foreach (OutputTag o in outputTags.Values)
            {
                if (o.GetType() == typeof(AnalogOutput))
                {
                    analogOutputList.Add((AnalogOutput)o);
                }
            }
            return analogOutputList;
        }

        private static List<DigitalOutput> GetDigitalOutputs()
        {
            List<DigitalOutput> digitalOutputList = new List<DigitalOutput>();

            foreach (OutputTag o in outputTags.Values)
            {
                if (o.GetType() == typeof(DigitalOutput))
                {
                    digitalOutputList.Add((DigitalOutput)o);
                }
            }
            return digitalOutputList;
        }

        private static List<DigitalInput> GetDigitalInputs()
        {
            List<DigitalInput> digitalInputList = new List<DigitalInput>();

            foreach (InputTag o in inputTags.Values)
            {
                if (o.GetType() == typeof(DigitalInput))
                {
                    digitalInputList.Add((DigitalInput)o);
                }
            }
            return digitalInputList;
        }

        private static List<AnalogInput> GetAnalogInputs()
        {
            List<AnalogInput> analogInputList = new List<AnalogInput>();

            foreach (InputTag o in inputTags.Values)
            {
                if (o.GetType() == typeof(AnalogInput))
                {
                    analogInputList.Add((AnalogInput)o);
                }
            }
            return analogInputList;
        }

        private static List<Tag> GetAllTags()
        {
            List<Tag> tags = new List<Tag>(inputTags.Values.ToList());
            tags.AddRange(outputTags.Values.ToList());

            return tags;
        }



        public static void SaveSystemConfig()
        {
                using (var writer = new StreamWriter(SYSTEM_CONFIG_FILE))
                {
                    var serializer = new XmlSerializer(typeof(List<Tag>));

                    serializer.Serialize(writer, GetAllTags());
                    Console.WriteLine("Serialization finished");
                }
        }

        public static void WriteAlarmLog()
        {

        }

        public static void LoadSystemConfig()
        {
            if (!File.Exists(SYSTEM_CONFIG_FILE))
            {
                Console.WriteLine("System config file doesn't exist!");
            }
            else
            {
                using (var reader = new StreamReader(SYSTEM_CONFIG_FILE))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Tag>));
                    var tagsList = (List<Tag>)serializer.Deserialize(reader);
                    if (tagsList != null)
                    {
                        lock(locker)
                        {
                            foreach(var t in tagsList)
                            {
                                if(t.GetType() == typeof(InputTag))
                                {
                                    inputTags.Add(t.TagName, (InputTag)t);
                                } else if(t.GetType() == typeof(OutputTag))
                                {
                                    outputTags.Add(t.TagName, (OutputTag)t);
                                }
                            }
                        }            
                    }
                }
            }
        }
    }
}