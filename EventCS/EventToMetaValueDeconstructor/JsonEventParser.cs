using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class JsonEventParser
    {        
        private JpropertyTypeDeterminator jpropertyTypeDeterminator = new JpropertyTypeDeterminator();

        public Event Parse(string eventKey, string Json)
        {
            if (Json == "")
                return new Event();
            string CreationDate = "";

            JObject jObjectFromString = JObject.Parse(Json);            
            List<JsonProperty> listOfProperties = new List<JsonProperty>();

            foreach (JProperty Property in jObjectFromString.Properties())
            {
                PropertyType propertyType = jpropertyTypeDeterminator.Get(Property.Value.ToString());
                string propertyValue = Property.Value.ToString();
                listOfProperties.Add(new JsonProperty(Property.Name, propertyType, propertyValue));
                if (Property.Name == "CreationDate")
                    CreationDate = Property.Value.ToString();
            }
            return new Event(eventKey, listOfProperties, CreationDate);
        }
    }
}
