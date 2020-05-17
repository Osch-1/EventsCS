using EventToMetaValueDeconstructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.ViewModels
{
    public class CreateEventViewModel
    {
        public string EventKey { get; set; }
        public List<JsonProperty> JsonPropertiesMetaValue { get; set; }
    }
}
