using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EventToMetaValueDeconstructor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc.Data.Repositories;
using Mvc.dto;
using Mvc.ViewModels;
using Newtonsoft.Json;

namespace Mvc.Controllers
{    
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;
        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }        
        public ViewResult EventsList()
        {
            ViewBag.Title = "События";
            EventsListViewModel eventsViewModel = new EventsListViewModel();
            var Events = _eventRepository.GetAllEvents();
            eventsViewModel.AllEvents = Events;
            return View(eventsViewModel);
        }

        [HttpGet]
        public ViewResult CreateEvent([FromQuery(Name = "eventKey")] string eventKey)
        {            
            if (eventKey is null)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel();
                errorViewModel.ErrorMessage = "Parameter eventKey can't be null or empty";
                return View("Error", errorViewModel);//error page
            }
            CreateEventViewModel eventViewModel = new CreateEventViewModel();
            var @event = _eventRepository.GetEvent(eventKey);            
            eventViewModel.EventKey = @event.EventKey;            
            eventViewModel.JsonPropertiesMetaValue = @event.JsonPropertiesMetaValue;
            return View("CreateEvent", eventViewModel);
        }

        [HttpPost]
        public string CreateEvent(JsonInfo jsonInfo)
        {
            DateTime localDate = DateTime.Now;
            Property dataProperty = new Property("CreationDate", localDate.ToString(), "DateTime");
            jsonInfo.Properties.Add(dataProperty);

            return JsonConvert.SerializeObject(jsonInfo);
        }

        private string SerializeEvent(Event @event)
        {
            string serializedEvent="{}";

            return serializedEvent;
        }
    }
}