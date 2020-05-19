using EventToMetaValueDeconstructor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mvc.Application;
using Mvc.dto;
using Mvc.ViewModels;
using System;


namespace Mvc.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;

        private readonly ErrorFormer _errorFormer = new ErrorFormer();
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
                return View("Error", _errorFormer.Form(e.Message));
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
                return View("Error", _errorFormer.Form(e.Message));
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
                    ErrorViewModel errorViewModel = new ErrorViewModel
                    {
                        ErrorMessage = "Parameter eventKey can't be null or empty"
                    };
                    return View("Error", errorViewModel);//error page
                }

                Event eventToCreate = _eventRepository.GetEvent(jsonInfo.EventKey);

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
                return View("Error", _errorFormer.Form(e.Message));
            }
        }
        [HttpGet]
        public ViewResult AddEvents()
        {
            return View("AddEvents");
        }
        [HttpPost, DisableRequestSizeLimit]
        public ViewResult AddEventsFromLog(string eventsToAdd)
        {
            try
            {
                HandleAddedEvents(eventsToAdd);

                var Events = _eventRepository.GetAllEvents();

                EventsListViewModel eventsViewModel = new EventsListViewModel
                {
                    AllEvents = Events
                };

                return View("EventsList", eventsViewModel);
            }
            catch (SqlException e)
            {
                return View("Error", _errorFormer.Form(e.Message));
            }
        }
        private void HandleAddedEvents(string eventsToAdd)
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
        }
    }
}