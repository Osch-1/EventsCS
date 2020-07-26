using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    //метаинформация о событии
    public class Event
    {        
        public string EventKey { get; set; }
        public List<JsonProperty> JsonPropertiesMetaValue { get; set; }
        public DateTime CreationDate { get; set; }

        public Event()
        {
            CreationDate = DateTime.Now;
            EventKey = "";
            JsonPropertiesMetaValue = new List<JsonProperty>();
        }

        public Event( string key, List<JsonProperty> listOfProperties, DateTime creationDate )
        {
            CreationDate = creationDate;
            EventKey = key;
            JsonPropertiesMetaValue = listOfProperties;
        }

        public override string ToString()
        {
            string properties = "";
            foreach ( JsonProperty property in this.JsonPropertiesMetaValue )
            {
                properties += "  " + property + "\n\n";
            }
            return "EventKey:\n" + "  " + EventKey + "\n" + "CreationDate:\n" + "  " + CreationDate + "\n" + "JsonPropertiesMetaValue:\n" + properties;
        }
    }
}
