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
        public ViewResult EventsList()
        {            
            var Events = _eventRepository.GetAllEvents();

            EventsListViewModel eventsViewModel = new EventsListViewModel
            {
                AllEvents = Events
            };

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

            var @event = _eventRepository.GetEvent(eventKey);

            CreateEventViewModel eventViewModel = new CreateEventViewModel
            {
                EventKey = @event.EventKey,
                JsonPropertiesMetaValue = @event.JsonPropertiesMetaValue
            };

            return View("CreateEvent", eventViewModel);
        }
        [HttpPost]
        public ViewResult CreateEvent(JsonInfo jsonInfo)
        {
            DateTime localDate = DateTime.Now;
            Guid guid = Guid.NewGuid();
            Property dataProperty = new Property("CreationDate", localDate.ToString(), "DateTime");
            Property idProperty = new Property("Id", guid.ToString(), "String");            
            jsonInfo.Properties.Add(idProperty);
            jsonInfo.Properties.Add(dataProperty);


            string json = CreateJson(jsonInfo);

            FormedJsonViewModel jsonViewModel = new FormedJsonViewModel
            {
                Json = json
            };

            return View("FormedJson", jsonViewModel);
        }
        private string CreateJson(JsonInfo jsonInfo)
        {
            string json = "";

            foreach (Property property in jsonInfo.Properties)
            {                
                if (property.Type == "String" || property.Type == "DateTime")
                    json += $"\"{property.Name}\": \"{property.Value}\", ";
                else
                    json += $"\"{property.Name}\": {property.Value}, ";
            }

            json = json.Remove(json.Length - 2);
            json = json.Insert(0, "{");
            json += "}";

            return json.ToString();
        }
    }
}