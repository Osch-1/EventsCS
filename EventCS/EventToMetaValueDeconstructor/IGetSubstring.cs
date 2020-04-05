using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    interface IGetSubstring
    {
        string Get(string StringToLookIn, string StartingFlag, string EndingFlag);
        string Get(string StringToLookIn, string StartingFlag);
    }
}
