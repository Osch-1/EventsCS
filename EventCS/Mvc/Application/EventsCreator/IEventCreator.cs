using EventToMetaValueDeconstructor;
using Mvc.dto;

namespace Mvc.Application.JsonCreator
{
    public interface IEventCreator
    {
        string SerializeEvent( EventInfo jsonInfo, Event @eventToCreate, string EventId );
    }
}
