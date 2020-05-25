using EventToMetaValueDeconstructor;
using Mvc.dto;

namespace Mvc.Application.JsonCreator
{
    public interface IJsonCreator
    {
        string Create(JsonInfo jsonInfo, Event @eventToCreate, string idProperty);
    }
}
