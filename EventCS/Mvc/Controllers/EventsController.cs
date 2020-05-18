using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventToMetaValueDeconstructor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mvc.Data.Repositories;
using Mvc.dto;
using Mvc.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Mvc.Controllers
{    
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;
        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }        
        [HttpGet]
        public ViewResult EventsList()
        {
            try
            {
                var Events = _eventRepository.GetAllEvents();
                EventsListViewModel eventsViewModel = new EventsListViewModel
                {
                    AllEvents = Events
                };

                return View(eventsViewModel);
            }
            catch (SqlException e)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
            return View("Error", errorViewModel);
            }            
        }
        [HttpGet]
        public ViewResult CreateEvent([FromQuery(Name = "eventKey")] string eventKey)
        {            
            if (String.IsNullOrEmpty(eventKey))
            {
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = "Parameter eventKey can't be null or empty"
                };
                return View("Error", errorViewModel);//error page
            }

            try
            {
                var @event = _eventRepository.GetEvent(eventKey);

                if (String.IsNullOrEmpty(@event.EventKey))
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel
                    {
                        ErrorMessage = $"No such event \"{eventKey}\" was found"
                    };
                    return View("Error", errorViewModel);//error page
                }

                CreateEventViewModel createEventViewModel = new CreateEventViewModel
                {
                    EventKey = @event.EventKey,
                    JsonPropertiesMetaValue = @event.JsonPropertiesMetaValue
                };

                return View("CreateEvent", createEventViewModel);
            }
            catch (SqlException e)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
                return View("Error", errorViewModel);
            }
        }
        [HttpPost]
        public ViewResult CreateEvent(JsonInfo jsonInfo, string eventKey)
        {
            try
            {
                Event @eventToCreate = _eventRepository.GetEvent(eventKey);

                if (String.IsNullOrEmpty(@eventToCreate.EventKey))
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel
                    {
                        ErrorMessage = $"No such event \"{eventKey}\" was found"
                    };
                    return View("Error", errorViewModel);//error page
                }

                Guid guid = Guid.NewGuid();
                String idProperty = $"\"Id\":\"{guid}\"";
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
                String dataProperty = $"\"Creation date\":\"{creationTime}\"";

                json += $" {idProperty}, {dataProperty}}}";
                json = json.Insert(0, "{");
                
                CreateEventViewModel createEventViewModel = new CreateEventViewModel
                {
                    EventKey = eventToCreate.EventKey,
                    JsonPropertiesMetaValue = eventToCreate.JsonPropertiesMetaValue,
                    CreatedJson = json,
                    EventId = guid.ToString()
                };

                return View("CreateEvent", createEventViewModel);
            }
            catch (SqlException e)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
                return View("Error", errorViewModel);
            }
        }        
        [HttpGet]
        public ViewResult AddEventsFromLog()
        {
            return View("AddEventsFromLog");
        }
        [HttpPost]
        public ViewResult AddEventsFromLog(String eventsToAdd)
        {
            try
            {
                SubstringBetweenFlagsGetter substringGetter = new SubstringBetweenFlagsGetter();                
                JsonEventParser jsonEventParser = new JsonEventParser();                

                while (true)
                {
                    string eventFromLogKey = substringGetter.Get(eventsToAdd, "key:", ",");
                    string jsonFromLog = substringGetter.Get(eventsToAdd, "json:", "\n");
                    if ((String.IsNullOrEmpty(eventFromLogKey)) || (String.IsNullOrEmpty(jsonFromLog)))                    
                        break;                    

                    int startingKeyIndex = eventsToAdd.IndexOf("key:");
                    int startingJsonIndex = eventsToAdd.IndexOf("json:");

                    eventsToAdd = eventsToAdd.Remove(startingJsonIndex, jsonFromLog.Length);
                    eventsToAdd = eventsToAdd.Remove(startingKeyIndex, eventFromLogKey.Length);

                    Event newEvent = jsonEventParser.Parse(eventFromLogKey, jsonFromLog);
                    Event eventFromDb = _eventRepository.GetEvent(eventFromLogKey);                    

                    if (eventFromDb.EventKey == "")
                    {
                        _eventRepository.Create(newEvent);
                    }
                    else
                    {
                        DateTime newEventCreationDate = DateTime.Parse(newEvent.CreationDate);
                        DateTime eventFromDbCreationDate = DateTime.Parse(eventFromDb.CreationDate);
                        if (newEventCreationDate > eventFromDbCreationDate)
                            _eventRepository.Update(newEvent);
                    }
                }

                var Events = _eventRepository.GetAllEvents();
                EventsListViewModel eventsViewModel = new EventsListViewModel
                {
                    AllEvents = Events
                };                

                return View("EventsList", eventsViewModel);                
            }
            catch (Exception e)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
                
                return View("Error", errorViewModel);
            }                                                  
        }
        private string GetCurrentUtcDate()
        {
            DateTime date = DateTime.UtcNow;
            String currentDate = date.ToString("O");
            return currentDate;
        }
    }    
}