using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoreService
{
    public class TagValue
    {
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
        public string Type { get; set; }

        public TagValue() { }
        public TagValue(string tagName, DateTime time, double value, string type)
        {
            TagName = tagName;
            Time = time;
            Value = value;
            Type = type;
        }

        public override string ToString()
        {
            return $"TagValue: TagName={TagName}| Time={Time}| Value={Value}|Type={Type}";
        }
    }
}