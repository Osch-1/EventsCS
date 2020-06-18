using EventToMetaValueDeconstructor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mvc.Application.EventsHandler;
using Mvc.Application.JsonCreator;
using Mvc.dto;
using Mvc.ViewModels;
using System;

namespace Mvc.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCreator _jsonCreator;
        private readonly IEventsManager _eventsHandler;
        public EventsController( IEventRepository eventRepository, IEventCreator jsonCreator, IEventsManager eventsHandler )
        {
            _eventRepository = eventRepository;
            _jsonCreator = jsonCreator;
            _eventsHandler = eventsHandler;
        }

        [HttpGet]
        //возврашает страницу с таблицей
        public ViewResult EventsList()
        {
            
            var Events = _eventRepository.GetAllEvents();
            EventsListViewModel eventsViewModel = new EventsListViewModel
            {
                AllEvents = Events
            };
            return View( eventsViewModel );            
        }

        [HttpGet]
        //возвращает страницу создания
        public ViewResult CreationPage( [FromQuery(Name = "eventKey")] string eventKey )
        {
            if ( String.IsNullOrEmpty( eventKey ) )
            {
                //переход на страницу с ошибкой
                return CreateErrorView( "Parameter eventKey can't be null or empty" );
            }
            
            var @event = _eventRepository.GetEvent( eventKey );

            if ( @event == null )
            {
                //переход на страницу с ошибкой
                return CreateErrorView( $"No such event \"{eventKey}\" was found" );
            }

            CreationPageViewModel creationPageViewModel = new CreationPageViewModel
            {
                EventKey = @event.EventKey,
                JsonPropertiesMetaValue = @event.JsonPropertiesMetaValue
            };

            return View( "CreationPage", creationPageViewModel );            
        }

        [HttpPost]
        //страница создания с уже сгенерированным событием
        public ViewResult CreateEvent( EventInfo eventInfo )
        {
            
            if ( String.IsNullOrEmpty( eventInfo.EventKey ) )
            {
                //переход на страницу с ошибкой
                return CreateErrorView( "Parameter eventKey can't be null or empty" );
            }

            Event eventToCreate = _eventRepository.GetEvent( eventInfo.EventKey );

            if ( String.IsNullOrEmpty( @eventToCreate.EventKey ) )
            {
                //переход на страницу с ошибкой
                return CreateErrorView( $"No such event \"{eventInfo.EventKey}\" was found" );
            }

            Guid guid = Guid.NewGuid();
            String idProperty = $"\"Id\":\"{guid}\"";

            String formedJson = _jsonCreator.SerializeEvent( eventInfo, eventToCreate, idProperty );

            CreationPageViewModel creationPageViewModel = new CreationPageViewModel
            {
                EventKey = eventToCreate.EventKey,
                JsonPropertiesMetaValue = eventToCreate.JsonPropertiesMetaValue,
                CreatedJson = formedJson,
                EventId = guid.ToString(),
                EnteredPropertiesValues = eventInfo.EnteredPropertiesValues
            };


            return View( "CreationPage", creationPageViewModel );            
        }

        [HttpGet]
        //страница добавления событий из логов
        public ViewResult AddEvents()
        {
            return View( "AddEvents" );
        }

        [HttpPost, DisableRequestSizeLimit]
        //обрабатываем полученные из логов событий
        public ViewResult AddProvidedEvents( string eventsToAdd )
        {                        
            if ( !String.IsNullOrEmpty( eventsToAdd ) )
            {
                _eventsHandler.Add( _eventsHandler.Parse( eventsToAdd ) );
            }
            var Events = _eventRepository.GetAllEvents();

            EventsListViewModel eventsViewModel = new EventsListViewModel
            {
                AllEvents = Events
            };
            return View( "EventsList", eventsViewModel );                        
        }

        [AllowAnonymous]
        public ViewResult Error()
        {
            return CreateErrorView("");
        }

        //возвращает страницу с ошибкой
        private ViewResult CreateErrorView( string message )
        {
            ErrorViewModel errorViewModel = new ErrorViewModel
            {
                ErrorMessage = message
            };

            return View( "Error", errorViewModel );
        }
    }
}
