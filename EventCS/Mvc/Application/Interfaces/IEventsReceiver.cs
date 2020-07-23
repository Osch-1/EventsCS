using EventToMetaValueDeconstructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Application.Interfaces
{
    public interface IEventsReceiver
    {
        void Bind();
        void Receive();
    }
}
