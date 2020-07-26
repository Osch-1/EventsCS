using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EventToMetaValueDeconstructor
{
    //парсит 1 событие в Event (1x Event => Event object)
    public class JsonEventParser
    {
        private readonly JpropertyTypeDeterminator jpropertyTypeDeterminator = new JpropertyTypeDeterminator();
        private readonly List<String> propertiesToPass = new List<String>() { "CreationDate", "CorrelationId", "Id" };

        public Event Parse( string eventKey, string json )
        {
            if ( ( json == "" ) || ( !( IsJson(json) ) ) )
                return new Event();

            DateTime creationDate = DateTime.Now;

            JObject jObjectFromString = JObject.Parse( json );
            List<JsonProperty> listOfProperties = new List<JsonProperty>();

            foreach ( JProperty property in jObjectFromString.Properties() )
            {
                if ( !( propertiesToPass.Contains( property.Name ) ) )
                {
                    PropertyType propertyType = jpropertyTypeDeterminator.Get( property.Value.ToString() );
                    string propertyValue = property.Value.ToString();
                    propertyValue = Regex.Replace( propertyValue, @"[ \r\n\t]", "" );
                    listOfProperties.Add( new JsonProperty( property.Name, propertyType, propertyValue ) );
                }
                if ( property.Name == "CreationDate" )
                    creationDate = Convert.ToDateTime( property.Value );
            }
            return new Event( eventKey, listOfProperties, creationDate );
        }

        private bool IsJson( string providedString )
        {
            try
            {
                JObject.Parse(providedString);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
    }
}
