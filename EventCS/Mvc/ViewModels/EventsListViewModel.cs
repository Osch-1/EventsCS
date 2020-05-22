using EventToMetaValueDeconstructor;
using System.Collections.Generic;

namespace Mvc.ViewModels
{
    public class EventsListViewModel
    {
        public IEnumerable<Event> AllEvents { get; set; }
    }
}
