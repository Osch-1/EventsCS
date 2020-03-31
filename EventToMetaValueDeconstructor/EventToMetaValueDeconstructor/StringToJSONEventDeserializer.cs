using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class StringToJSONEventDeserializer
    {
        private IGetSubstring PropertyValueGetter = new SubstringBetweenFlagsGetter();
        private StringValueTypeDeterminator StringValueTypeDeterminator = new StringValueTypeDeterminator();

        public JSONEvent Deserialize(string InputLine)
        {                            
            string EventKey = PropertyValueGetter.Get(InputLine, "{", "}");
            string EventName = PropertyValueGetter.Get(InputLine, "key", ",");
            JSONEvent JsonEvent = new JSONEvent(EventKey, EventName);

            string JsonString = PropertyValueGetter.Get(InputLine, "json");

            if (JsonString != "")
            {
                JObject JObjectFromString = JObject.Parse(JsonString);

                foreach (JProperty Property in JObjectFromString.Properties())
                {
                    string PropertyType;
                    string PropertyValue = Property.Value.ToString();

                    if (Property.Value is JArray)
                        PropertyType = "List<Object>";
                    else
                        PropertyType = StringValueTypeDeterminator.Get(PropertyValue).ToString();

                    JsonEvent.EventPropertyMetaValue.Add(new EventProperty(Property.Name, PropertyType, PropertyValue));
                }
            }
            else
                JsonEvent.EventPropertyMetaValue.Add(new EventProperty("NoName", "NoType", "NoDefaultValue"));

            return JsonEvent;
        }
    }
}
