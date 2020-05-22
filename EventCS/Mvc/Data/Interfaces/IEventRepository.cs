﻿using EventToMetaValueDeconstructor;
using System.Collections.Generic;

namespace Mvc
{
    public interface IEventRepository
    {
        List<Event> GetAllEvents();
        Event GetEvent(string eventKey);
        void Add(Event eventToCreate);
        void Update(Event eventToUpdate);
        void Delete(string eventtKey);
    }
}
