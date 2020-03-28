using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    interface IGetSubstring
    {
        string GetPropertyValue(string stringToLookIn, string startingFlag, string endingFlag);
    }
}
