using EventToMetaValueDeconstructor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mvc.Application;
using Mvc.dto;
using Mvc.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
                return CreateErrorView(e.Message);//redirect to error page with provided message
            }
        }
        [HttpGet]
        public ViewResult CreationPage([FromQuery(Name = "eventKey")] string eventKey)
        {
            if (String.IsNullOrEmpty(eventKey))
            {                
                return CreateErrorView("Parameter eventKey can't be null or empty");//redirect to error page with provided message
            }

            try
            {
                var @event = _eventRepository.GetEvent(eventKey);

                if (String.IsNullOrEmpty(@event.EventKey))
                {                                        
                    return CreateErrorView($"No such event \"{eventKey}\" was found");//redirect to error page with provided message
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
                return CreateErrorView(e.Message);//redirect to error page with provided message
            }
        }
        [HttpPost]
        public ViewResult CreateEvent(JsonInfo jsonInfo)
        {
            try
            {
                JsonCreator jsonCreator = new JsonCreator();

                if (String.IsNullOrEmpty(jsonInfo.EventKey))
                {                    
                    return CreateErrorView("Parameter eventKey can't be null or empty");//redirect to error page with provided message
                }

                Event eventToCreate = _eventRepository.GetEvent(jsonInfo.EventKey);

                if (String.IsNullOrEmpty(@eventToCreate.EventKey))
                {                    
                    return CreateErrorView($"No such event \"{jsonInfo.EventKey}\" was found");//redirect to error page with provided message
                }

                Guid guid = Guid.NewGuid();
                String idProperty = $"\"Id\":\"{guid}\"";

                String formedJson = jsonCreator.Create(jsonInfo, eventToCreate, idProperty);

                CreationPageViewModel creationPageViewModel = new CreationPageViewModel
                {
                    EventKey = eventToCreate.EventKey,
                    JsonPropertiesMetaValue = eventToCreate.JsonPropertiesMetaValue,
                    CreatedJson = formedJson,
                    EventId = guid.ToString(),
                    EnteredData = jsonInfo.Properties
                };

                return View("CreationPage", creationPageViewModel);
            }
            catch (SqlException e)
            {
                return CreateErrorView(e.Message);//redirect to error page with provided message
            }
        }
        [HttpGet]
        public ViewResult AddEvents()
        {
            return View("AddEvents");
        }
        [HttpPost, DisableRequestSizeLimit]
        public ViewResult AddProvidedEvents(string eventsToAdd)
        {
            try
            {
                EventsFromLogHandler eventsFromLogHandler = new EventsFromLogHandler(_eventRepository);
                if (!String.IsNullOrEmpty(eventsToAdd))
                {
                    eventsFromLogHandler.HandleAddedEvents(eventsToAdd);
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
                return CreateErrorView(e.Message);
            }
        }
        

        private ViewResult CreateErrorView(string message)//returns view with error message
        {
            ErrorViewModel errorViewModel = new ErrorViewModel
            {
                ErrorMessage = message 
            };

            return View("Error", errorViewModel);
        }
    }
}