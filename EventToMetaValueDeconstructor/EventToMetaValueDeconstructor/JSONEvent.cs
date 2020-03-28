using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class JSONEvent
    {
        public string EventKey { get; set; }
        public string EventName { get; set; }
        public List<string> EventPropertyMetaValue { get; set; }
        public JSONEvent()
        {
            this.EventPropertyMetaValue = new List<String>();
        }
        public JSONEvent(string Key, string Name)
        {
            this.EventKey = Key;
            this.EventName = Name;
            this.EventPropertyMetaValue = new List<String>();
        }
        public JSONEvent(string Key, string Name, List<string> ListOfProperties)
        {
            this.EventKey = Key;
            this.EventName = Name;
            this.EventPropertyMetaValue = ListOfProperties;
        }
        
        
        public override string ToString()
        {
            string Properties = "";
            foreach (string Property in this.EventPropertyMetaValue)
            {
                Properties += "  " + Property + "\n\n";
            }
            return "EventKey:\n" + "  " +this.EventKey + "\n" + "EventName:\n" + "  " + this.EventName + "\n" + "EventPropertyMetaValue:\n" + Properties;
        }
    }
}
