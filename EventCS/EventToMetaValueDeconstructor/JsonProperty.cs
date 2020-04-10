﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class JsonProperty
    {
        public string PropertyName { get; set; }
        public PropertyType PropertyType { get; set; }
        public string SampleValue { get; }

        public JsonProperty(string Name, PropertyType Type, string defaultValue)
        {
            this.PropertyName = Name;
            this.PropertyType = Type;
            this.SampleValue = defaultValue;
        }

        public override string ToString()
        {
            return "Property name: " + this.PropertyName + "\n" + "  Property type: " + this.PropertyType + "\n" + "  Sample value: " + "\n  " + this.SampleValue;
        }
    }
}
