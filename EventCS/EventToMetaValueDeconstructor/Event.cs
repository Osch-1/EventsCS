﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class Event
    {
        public string CreationDate { get; set; }        
        public string EventKey { get; set; }
        public List<JsonProperty> JsonPropertiesMetaValue { get; set; }
        public Event()
        {
            this.CreationDate = "";
            this.EventKey = "";
            this.JsonPropertiesMetaValue = new List<JsonProperty>();
        }
        public Event(string key)
        {
            this.CreationDate = "";
            this.EventKey = key;
            this.JsonPropertiesMetaValue = new List<JsonProperty>();
        }
        public Event(string key, List<JsonProperty> listOfProperties)
        {
            this.CreationDate = "";
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
            return "EventKey:\n" + "  " + this.EventKey + "\n" + "JsonPropertiesMetaValue:\n" + properties;
        }
    }
}