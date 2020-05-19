﻿using EventToMetaValueDeconstructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.ViewModels
{
    public class CreationPageViewModel
    {
        public string EventKey { get; set; }
        public List<JsonProperty> JsonPropertiesMetaValue { get; set; }
        public string CreatedJson { get; set; }
        public string EventId { get; set; }
        public List<String> EnteredData { get; set; }
    }
}