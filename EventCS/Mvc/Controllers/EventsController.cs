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
        public ViewResult CreationPage([FromQuery(Name = "eventKey")] string eventKey)
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

                CreationPageViewModel creationPageViewModel = new CreationPageViewModel
                {
                    EventKey = @event.EventKey,
                    JsonPropertiesMetaValue = @event.JsonPropertiesMetaValue
                };

                return View("CreationPage", creationPageViewModel);
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
        public ViewResult CreateEvent(JsonInfo jsonInfo)
        {
            try
            {
                if (String.IsNullOrEmpty(jsonInfo.EventKey))
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel
                    {
                        ErrorMessage = "Parameter eventKey can't be null or empty"
                    };
                    return View("Error", errorViewModel);//error page
                }

                Event @eventToCreate = _eventRepository.GetEvent(jsonInfo.EventKey);

                if (String.IsNullOrEmpty(@eventToCreate.EventKey))
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel
                    {
                        ErrorMessage = $"No such event \"{jsonInfo.EventKey}\" was found"
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

                CreationPageViewModel creationPageViewModel = new CreationPageViewModel
                {
                    EventKey = eventToCreate.EventKey,
                    JsonPropertiesMetaValue = eventToCreate.JsonPropertiesMetaValue,
                    CreatedJson = json,
                    EventId = guid.ToString(),
                    EnteredData = jsonInfo.Properties
                };

                return View("CreationPage", creationPageViewModel);
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
        [HttpPost, DisableRequestSizeLimit]
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

                var Events = _eventRepository.GetAllEvents();
                EventsListViewModel eventsViewModel = new EventsListViewModel
                {
                    AllEvents = Events
                };                

                return View("EventsList", eventsViewModel);                
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
        private string GetCurrentUtcDate()
        {
            DateTime date = DateTime.UtcNow;
            String currentDate = date.ToString("O");
            return currentDate;
        }
    }    
}