using EventToMetaValueDeconstructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.ViewModels
{
    public class EventsListViewModel
    {
        public IEnumerable<Event> AllEvents { get; set; }
    }
}
