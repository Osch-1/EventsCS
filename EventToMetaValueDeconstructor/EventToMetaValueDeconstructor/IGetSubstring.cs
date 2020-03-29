using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    interface IGetSubstring
    {
        string GetPropertyValue(string StringToLookIn, string StartingFlag, string EndingFlag);
        string GetPropertyValue(string StringToLookIn, string StartingFlag);
    }
}
