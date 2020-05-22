using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    interface IGetSubstring//интерфейс для классов, выделяющих подстроки
    {
        string Get(string StringToLookIn, string StartingFlag, string EndingFlag);
        string Get(string StringToLookIn, string StartingFlag);
    }
}
