using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class Event//объект, в который десериализируется событие
    {        
        public string EventKey { get; set; }
        public List<JsonProperty> JsonPropertiesMetaValue { get; set; }
        public DateTime CreationDate { get; set; }
        public Event()
        {
            this.CreationDate = DateTime.Now;
            this.EventKey = "";
            this.JsonPropertiesMetaValue = new List<JsonProperty>();
        }
        public Event(string key)
        {
            this.CreationDate = DateTime.Now;
            this.EventKey = key;
            this.JsonPropertiesMetaValue = new List<JsonProperty>();
        }
        public Event(string key, List<JsonProperty> listOfProperties)
        {
            this.CreationDate = DateTime.Now;
            this.EventKey = key;
            this.JsonPropertiesMetaValue = listOfProperties;
        }

        public Event(string key, List<JsonProperty> listOfProperties, DateTime creationDate)
        {
            this.CreationDate = creationDate;
            this.EventKey = key;
            this.JsonPropertiesMetaValue = listOfProperties;
        }

        public override string ToString()
        {
            string properties = "";
            foreach (JsonProperty property in this.JsonPropertiesMetaValue)
            {
                properties += "  " + property + "\n\n";
            }
            return "EventKey:\n" + "  " + this.EventKey + "\n" + "CreationDate:\n" + "  " + this.CreationDate + "\n" + "JsonPropertiesMetaValue:\n" + properties;
        }
    }
}
