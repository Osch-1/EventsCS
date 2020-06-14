using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    //интерфейс для классов, выделяющих подстроки
    interface IGetSubstring
    {
        string Get( string StringToLookIn, string StartingFlag, string EndingFlag );
        string Get( string StringToLookIn, string StartingFlag );
    }
}
