using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventToMetaValueDeconstructor;
using Microsoft.AspNetCore.Mvc;
using Mvc.Data.Repositories;
using Mvc.ViewModels;

namespace Mvc.Controllers
{    
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;
        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }        
        public ViewResult GetAllEvents()
        {
            ViewBag.Title = "События";
            EventsListViewModel eventsViewModel = new EventsListViewModel();
            var Events = _eventRepository.GetAllEvents();
            eventsViewModel.AllEvents = Events;
            return View(eventsViewModel);
        }        

        [HttpPost("{eventKey}")]
        public ViewResult GetEvent(string eventKey)
        {
            if (eventKey is null)
            {
                return View("");
            }
            EventViewModel eventViewModel = new EventViewModel();
            var Event = _eventRepository.GetEvent(eventKey);
            return View(eventViewModel);
        }
    }
}