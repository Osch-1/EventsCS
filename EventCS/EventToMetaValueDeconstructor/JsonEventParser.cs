using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class JsonEventParser
    {
        private IGetSubstring PropertyValueGetter = new SubstringBetweenFlagsGetter();
        private JpropertyTypeDeterminator JpropertyTypeDeterminator = new JpropertyTypeDeterminator();

        public Event Deserialize(string InputLine)
        {
            string EventKey = PropertyValueGetter.Get(InputLine, "{", "}");
            string EventName = PropertyValueGetter.Get(InputLine, "key:", ",");
            Event JsonEvent = new Event(EventKey, EventName);

            string JsonString = PropertyValueGetter.Get(InputLine, "json:");

            if (JsonString != "NoParam")
            {
                JObject JObjectFromString = JObject.Parse(JsonString);

                foreach (JProperty Property in JObjectFromString.Properties())
                {
                    JsonPropertyType PropertyType = JpropertyTypeDeterminator.Get(Property);
                    string PropertyValue = Property.Value.ToString();

                    JsonEvent.EventPropertyMetaValue.Add(new JsonProperty(Property.Name, PropertyType, PropertyValue));
                }
            }            

            return JsonEvent;
        }
    }
}
