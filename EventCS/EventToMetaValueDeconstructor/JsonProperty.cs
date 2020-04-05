using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class JsonProperty
    {
        public string PropertyName { get; set; }
        public JsonPropertyType PropertyType { get; set; }
        public string DefaultValue { get; }

        public JsonProperty(string Name, JsonPropertyType Type, string DefaultValue)
        {
            this.PropertyName = Name;
            this.PropertyType = Type;
            this.DefaultValue = DefaultValue;
        }

        public override string ToString()
        {
            return "Name: " + this.PropertyName + "\n" + "  PropertyType: " + this.PropertyType + "\n" + "  DefaultValue: " + "\n  " + this.DefaultValue;
        }
    }
}
