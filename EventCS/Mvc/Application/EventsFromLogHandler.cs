﻿using EventToMetaValueDeconstructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Application
{
    public class EventsFromLogHandler
    {
        private readonly IEventRepository _eventRepository;
        public EventsFromLogHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public void HandleAddedEvents(string eventsToAdd)//puts entered events from log file in db
        {
            SubstringBetweenFlagsGetter substringGetter = new SubstringBetweenFlagsGetter();//substring getter
            JsonEventParser jsonEventParser = new JsonEventParser();//deserializer                        
            while (true)
            {
                string jsonFromLog = "";
                string eventFromLogKey = substringGetter.Get(eventsToAdd, "key:", ",");
                jsonFromLog = substringGetter.Get(eventsToAdd, "json:", "\n");
                if ((String.IsNullOrEmpty(eventFromLogKey)) || (String.IsNullOrEmpty(jsonFromLog)))
                {
                    break;
                }

                int startingJsonIndex = eventsToAdd.IndexOf("json:");
                int endingJsonIndex = startingJsonIndex + jsonFromLog.Length;

                eventsToAdd = eventsToAdd.Remove(0, endingJsonIndex);

                if (!(jsonFromLog.Length > 1000))
                {
                    Event newEvent = jsonEventParser.Parse(eventFromLogKey, jsonFromLog);
                    Event comparableEvent = _eventRepository.GetEvent(eventFromLogKey);

                    if (comparableEvent == null)
                    {
                        _eventRepository.Create(newEvent);
                    }
                    else
                    {
                        DateTime newEventCreationDate = DateTime.Parse(newEvent.CreationDate);
                        DateTime eventCreationDate = DateTime.Parse(comparableEvent.CreationDate);
                        if (newEventCreationDate > eventCreationDate)
                            _eventRepository.Update(newEvent);
                    }
                }
            }
        }
    }
}
