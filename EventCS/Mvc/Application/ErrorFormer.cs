using Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Application
{
    public class ErrorFormer
    {
        public ErrorViewModel Form(string message)
        {
            return new ErrorViewModel { ErrorMessage = message };
        }
    }
}
