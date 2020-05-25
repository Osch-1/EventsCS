using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EventToMetaValueDeconstructor
{
    public class JsonEventParser//парсит 1 событие в Event (1x Event => Event object)
    {
        private readonly JpropertyTypeDeterminator jpropertyTypeDeterminator = new JpropertyTypeDeterminator();
        private readonly List<String> propertiesToPass = new List<String>() { "CreationDate", "CorrelationId", "Id" };

        public Event Parse(string eventKey, string Json)
        {
            if (Json == "")
                return new Event();
            DateTime CreationDate = DateTime.Now;

            JObject jObjectFromString = JObject.Parse(Json);
            List<JsonProperty> listOfProperties = new List<JsonProperty>();

            foreach (JProperty Property in jObjectFromString.Properties())
            {
                if (!(propertiesToPass.Contains(Property.Name)))
                {
                    PropertyType propertyType = jpropertyTypeDeterminator.Get(Property.Value.ToString());
                    string propertyValue = Property.Value.ToString();
                    propertyValue = Regex.Replace(propertyValue, @"[ \r\n\t]", "");
                    listOfProperties.Add(new JsonProperty(Property.Name, propertyType, propertyValue));
                }
                if (Property.Name == "CreationDate")
                    CreationDate = Convert.ToDateTime(Property.Value);
            }
            return new Event(eventKey, listOfProperties, CreationDate);
        }
    }
}
