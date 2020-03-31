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
        public JSONEvent(string Key, string Name)
        {
            this.EventKey = Key;
            this.EventName = Name;
            this.EventPropertyMetaValue = new List<EventProperty>();
        }
        public JSONEvent(string Key, string Name, List<EventProperty> ListOfProperties)
        {
            this.EventKey = Key;
            this.EventName = Name;
            this.EventPropertyMetaValue = ListOfProperties;
        }
        
        
        public override string ToString()
        {
            string Properties = "";
            foreach (EventProperty Property in this.EventPropertyMetaValue)
            {
                Properties += "  " + Property + "\n\n";
            }
            return "EventKey:\n" + "  " + this.EventKey + "\n" + "EventName:\n" + "  " + this.EventName + "\n" + "EventPropertyMetaValue:\n" + Properties;
        }
    }
}
