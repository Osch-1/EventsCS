using EventToMetaValueDeconstructor;
using Mvc.Application.EventsHandler;
using Mvc.Application.Interfaces;
using System;
using System.Collections.Generic;

namespace Mvc.Application
{
    public class LogEventsManager : IEventsManager
    {
        private readonly IEventRepository _eventRepository;
        const int MaxAllowedJsonLength = 1000;
        public LogEventsManager( IEventRepository eventRepository )
        {
            _eventRepository = eventRepository;
        }
        public List<Event> Parse( string eventsToAdd )
        {
            List<Event> parsedEvents = new List<Event>();

            //substring getter
            SubstringBetweenFlagsGetter substringGetter = new SubstringBetweenFlagsGetter();
            //deserializer
            JsonEventParser jsonEventParser = new JsonEventParser();
            while ( true )
            {
                string eventFromLogKey = substringGetter.Get( eventsToAdd, "key:", "," );
                string jsonFromLog = substringGetter.Get( eventsToAdd, "json:", "\n" );
                if ( ( String.IsNullOrEmpty( eventFromLogKey ) ) || ( String.IsNullOrEmpty( jsonFromLog ) ) )
                {
                    break;
                }

                int startingJsonIndex = eventsToAdd.IndexOf("json:");
                int endingJsonIndex = startingJsonIndex + jsonFromLog.Length;

                eventsToAdd = eventsToAdd.Remove( 0, endingJsonIndex );

                bool isTooBigEvent = jsonFromLog.Length > MaxAllowedJsonLength;

                if ( !isTooBigEvent )
                {
                    Event parsedEvent = jsonEventParser.Parse( eventFromLogKey, jsonFromLog );
                    parsedEvents.Add( parsedEvent );
                }
            }
            return parsedEvents;
        }
        public void Add( List<Event> events)
        {
            foreach ( Event parsedEvent in events )
            {
                string eventFromLogKey = parsedEvent.EventKey;

                Event existingEvent = _eventRepository.GetEvent( eventFromLogKey );

                if ( existingEvent == null )
                {
                    _eventRepository.Add( parsedEvent );
                }
                else
                {
                    DateTime newEventCreationDate = parsedEvent.CreationDate;
                    DateTime eventCreationDate = existingEvent.CreationDate;
                    
                    bool isParsedNewer = newEventCreationDate.CompareTo(eventCreationDate) > 0;

                    if (isParsedNewer)
                    {
                        _eventRepository.Update(parsedEvent);
                    }
                }
            }
        }        
    }
}
