using EventToMetaValueDeconstructor;
using Mvc.Application.EventsHandler;
using System;

namespace Mvc.Application
{
    public class LogEventsHandler : IEventsHandler
    {
        private readonly IEventRepository _eventRepository;
        public LogEventsHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public void Handle(string eventsToAdd)//puts entered events from log file in db
        {
            SubstringBetweenFlagsGetter substringGetter = new SubstringBetweenFlagsGetter();//substring getter
            JsonEventParser jsonEventParser = new JsonEventParser();//deserializer                        
            while (true)
            {
                string eventFromLogKey = substringGetter.Get(eventsToAdd, "key:", ",");
                string jsonFromLog = substringGetter.Get(eventsToAdd, "json:", "\n");
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
                        _eventRepository.Add(newEvent);
                    }
                    else
                    {
                        DateTime newEventCreationDate = newEvent.CreationDate;
                        DateTime eventCreationDate = comparableEvent.CreationDate;

                        int isEventNewer = newEventCreationDate.CompareTo(eventCreationDate);

                        if (isEventNewer > 0)
                            _eventRepository.Update(newEvent);
                    }
                }
            }
        }
    }
}
