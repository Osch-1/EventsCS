using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class JSONEvent
    {
        public string CreationDate { get; set; }
        public string EventKey { get; set; }
        public string EventName { get; set; }
        public List<EventProperty> EventPropertyMetaValue { get; set; }
        public JSONEvent()
        {
            this.EventPropertyMetaValue = new List<EventProperty>();
        }
        public JSONEvent(string key, string name)
        {
            this.EventKey = key;
            this.EventName = name;
            this.EventPropertyMetaValue = new List<EventProperty>();
        }
        public JSONEvent(string key, string name, List<EventProperty> listOfProperties)
        {
            this.EventKey = key;
            this.EventName = name;
            this.EventPropertyMetaValue = listOfProperties;
        }
        
        
        public override string ToString()
        {
            string properties = "";
            foreach (EventProperty property in this.EventPropertyMetaValue)
            {
                properties += "  " + property + "\n\n";
            }
            return "EventKey:\n" + "  " + this.EventKey + "\n" + "EventName:\n" + "  " + this.EventName + "\n" + "EventPropertyMetaValue:\n" + properties;
        }
    }
}
