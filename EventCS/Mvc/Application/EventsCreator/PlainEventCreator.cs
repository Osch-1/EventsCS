﻿using EventToMetaValueDeconstructor;
using Mvc.Application.JsonCreator;
using Mvc.dto;
using System;
using System.Linq;

namespace Mvc.Application
{
    public class PlainEventCreator : IEventCreator
    {
        public string SerializeEvent( EventInfo eventInfo, Event @eventToCreate, String idProperty )
        {
            string json = "";

            for ( int i = 0; i < eventInfo.EnteredPropertiesValues.Count(); i++ )
            {
                JsonProperty property = @eventToCreate.JsonPropertiesMetaValue[ i ];
                String propertyValue = eventInfo.EnteredPropertiesValues[ i ];

                if ( String.IsNullOrEmpty( propertyValue ) )
                    json += $"\"{property.PropertyName}\": null, ";
                else
                    if ( property.PropertyType == PropertyType.String || property.PropertyType == PropertyType.DateTime )
                    json += $"\"{property.PropertyName}\": \"{propertyValue}\", ";
                else
                    json += $"\"{property.PropertyName}\": {propertyValue}, ";
            }

            String creationTime = DateTime.UtcNow.ToString( "O" );
            String dataProperty = $"\"CreationDate\":\"{creationTime}\"";

            json += $" {idProperty}, {dataProperty}}}";
            json = json.Insert( 0, "{" );

            return json;
        }
    }
}
