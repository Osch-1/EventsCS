using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class Event
    {
        public string CreationDate { get; set; }
        public string EventKey { get; set; }
        public string EventName { get; set; }
        public List<JsonProperty> EventPropertyMetaValue { get; set; }
        public Event()
        {
            this.EventPropertyMetaValue = new List<JsonProperty>();
        }
        public Event(string key, string name)
        {
            this.EventKey = key;
            this.EventName = name;
            this.EventPropertyMetaValue = new List<JsonProperty>();
        }
        public Event(string key, string name, List<JsonProperty> listOfProperties)
        {
            this.EventKey = key;
            this.EventName = name;
            this.EventPropertyMetaValue = listOfProperties;
        }


        public override string ToString()
        {
            string properties = "";
            foreach (JsonProperty property in this.EventPropertyMetaValue)
            {
                properties += "  " + property + "\n\n";
            }
            return "EventKey:\n" + "  " + this.EventKey + "\n" + "EventName:\n" + "  " + this.EventName + "\n" + "EventPropertyMetaValue:\n" + properties;
        }
    }
}
