using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class EventProperty
    {
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        string DefaultValue { get; }

        public EventProperty(string Name, string Type, string DefaultValue)
        {
            this.PropertyName = Name;
            this.PropertyType= Type;
            this.DefaultValue = DefaultValue;
        }

        public override string ToString()
        {
            return "Name: " + this.PropertyName + "\n" + "  PropertyType: " + this.PropertyType + "\n" + "  DefaultValue: " + "\n  " + this.DefaultValue;
        }
    }
}
