﻿using EventToMetaValueDeconstructor;
using Microsoft.Data.SqlClient;
using Mvc.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Mvc.Data.Repositories
{
    public class SQLEventRepository : IEventRepository
    {
        private readonly string _connectionString;
        public SQLEventRepository( string connectionString )
        {
            _connectionString = connectionString;
        }

        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();
            using SqlConnection connection = new SqlConnection( _connectionString );
            connection.Open();
            using SqlCommand command = connection.CreateCommand();

            command.Connection = connection;
            command.CommandText =
                @"SELECT
                    [EventKey],                            
                    [CreationDate]                            
                    FROM Events";


            using SqlDataReader reader = command.ExecuteReader();
            while ( reader.Read() )
            {
                string readedKey = Convert.ToString( reader[ "EventKey" ] ).Replace( " ", "" );
                DateTime readedDate = Convert.ToDateTime( reader[ "CreationDate" ] );

                var readedEvent = new Event( readedKey, GetJsonProperties( readedKey ), readedDate );

                events.Add( readedEvent );
            }

            connection.Close();

            return events;
        }

        public Event GetEvent( string eventKey )
        {
            Event readedEvent = new Event();
            using SqlConnection connection = new SqlConnection( _connectionString );
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.Parameters.Add( "@eventKey", SqlDbType.VarChar ).Value = eventKey;
            command.CommandText = @"SELECT [EventKey], [CreationDate]
                                    FROM [Events]
                                    WHERE [EventKey] = @eventKey";

            using SqlDataReader reader = command.ExecuteReader();
            if ( reader.Read() )
            {
                string readedKey = Convert.ToString( reader[ "EventKey" ] ).Replace( " ", "" );
                DateTime readedDate = Convert.ToDateTime( reader[ "CreationDate" ] );
                readedEvent = new Event( readedKey, GetJsonProperties( readedKey ), readedDate );
            }
            else
                readedEvent = null;

            connection.Close();

            return readedEvent;
        }
        public void Add(Event eventToCreate)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.Parameters.Add( "@eventKey", SqlDbType.NVarChar ).Value = eventToCreate.EventKey;
            command.Parameters.Add( "@creationDate", SqlDbType.DateTime ).Value = eventToCreate.CreationDate;
            command.CommandText =
                @"INSERT INTO [Events] ([EventKey], [CreationDate])
                  VALUES (@eventKey, @creationDate)";
            command.ExecuteNonQuery();
            foreach ( JsonProperty property in eventToCreate.JsonPropertiesMetaValue )
                PutJsonProperty( property, eventToCreate.EventKey );

            connection.Close();
        }
        //TODO: make it more productive with merge and join
        public void Update(Event eventToUpdate)
        {
            Delete( eventToUpdate.EventKey );
            Add( eventToUpdate );
        }

        public void Delete(string eventKey)
        {
            if ( String.IsNullOrEmpty( eventKey ) )
                return;

            using SqlConnection connection = new SqlConnection( _connectionString );
            connection.Open();
            using SqlCommand command = connection.CreateCommand();

            command.Parameters.Add( "@eventKey", SqlDbType.NVarChar ).Value = eventKey;
            command.CommandText =
                @"DELETE FROM [EventPropertiesMetaValue]
                  WHERE [EventKey] = @eventKey
                  DELETE FROM [Events]
                  WHERE [EventKey] = @eventKey";
            command.ExecuteNonQuery();

            connection.Close();
        }
        private List<JsonProperty> GetJsonProperties(string eventKey)
        {
            List<JsonProperty> properties = new List<JsonProperty>();
            using SqlConnection connection = new SqlConnection( _connectionString );
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.Parameters.Add( "@eventKey", SqlDbType.NVarChar ).Value = eventKey;
            command.CommandText =
                 @"SELECT
                    [PropertyName],
                    [ValueType],
                    [SampleValue]
                    FROM EventPropertiesMetaValue
                    WHERE EventKey = @eventKey";

            using SqlDataReader reader = command.ExecuteReader();
            while ( reader.Read() )
            {
                var readedType = ( reader[ "ValueType" ] ) switch
                {
                    "String" => PropertyType.String,
                    "Number" => PropertyType.Number,
                    "DateTime" => PropertyType.DateTime,
                    "List" => PropertyType.List,
                    "Object" => PropertyType.Object,
                    _ => PropertyType.String
                };

                var readedProperty = new JsonProperty
                {
                    PropertyName = Convert.ToString( reader[ "PropertyName" ] ),
                    PropertyType = readedType,
                    SampleValue = Convert.ToString( reader[ "SampleValue" ] )
                };
                properties.Add( readedProperty );
            }

            connection.Close();

            return properties;
        }
        private void PutJsonProperty( JsonProperty property, string eventKey )
        {
            if ( String.IsNullOrEmpty( eventKey ) )
                return;
            using SqlConnection connection = new SqlConnection( _connectionString );
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.Parameters.Add( "@eventKey", SqlDbType.NVarChar ).Value = eventKey;
            command.Parameters.Add( "@propertyName", SqlDbType.NVarChar ).Value = property.PropertyName;
            command.Parameters.Add( "@propertyType", SqlDbType.NVarChar ).Value = property.PropertyType;
            command.Parameters.Add( "@sampleValue", SqlDbType.NVarChar ).Value = property.SampleValue;
            command.CommandText =
                @"INSERT INTO [EventPropertiesMetaValue] ([PropertyName], [EventKey], [ValueType], [SampleValue])
                  VALUES (@propertyName, @eventKey, @propertyType, @sampleValue)";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
