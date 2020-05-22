using EventToMetaValueDeconstructor;
using Mvc.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Application
{
    public class JsonCreator
    {
        public string Create(JsonInfo jsonInfo, Event @eventToCreate, String idProperty)
        {                        
            string json = "";

            for (int i = 0; i < jsonInfo.Properties.Count(); i++)
            {
                JsonProperty property = @eventToCreate.JsonPropertiesMetaValue[i];
                String propertyValue = jsonInfo.Properties[i];

                if (String.IsNullOrEmpty(propertyValue))
                    json += $"\"{property.PropertyName}\": null, ";
                else
                    if (property.PropertyType == PropertyType.String || property.PropertyType == PropertyType.DateTime)
                    json += $"\"{property.PropertyName}\": \"{propertyValue}\", ";
                else
                    json += $"\"{property.PropertyName}\": {propertyValue}, ";
            }

            String creationTime = GetCurrentUtcDate();
            String dataProperty = $"\"CreationDate\":\"{creationTime}\"";

            json += $" {idProperty}, {dataProperty}}}";
            json = json.Insert(0, "{");

            return json;
        }
        private string GetCurrentUtcDate()
        {
            DateTime date = DateTime.UtcNow;
            String currentDate = date.ToString("O");
            return currentDate;
        }
    }
}
