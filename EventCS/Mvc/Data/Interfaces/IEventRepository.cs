using EventToMetaValueDeconstructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc
{
    public interface IEventRepository
    {
        List<Event> GetAllEvents();
        Event GetEvent(string eventKey);
        void Create(Event eventToCreate);
        void Update(Event eventToUpdate);
        void Delete(string eventtKey);        
    }
}
