using EventToMetaValueDeconstructor;
using System.Collections.Generic;

namespace Mvc.Application.EventsHandler
{
    public interface IEventsManager
    {        
        List<Event> Parse( string eventsToAdd );
        void Add( List<Event> parsedEvents );
    }
}
