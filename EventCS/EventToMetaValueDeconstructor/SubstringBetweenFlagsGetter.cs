using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class SubstringBetweenFlagsGetter: IGetSubstring
    {        
        public string Get(string InputLine, string PropertyName, string EndingFlag)
        {

            if ((InputLine.IndexOf(PropertyName) < 0) | (PropertyName == "") | (EndingFlag == ""))
                return "NoParam";

            int PropertyValueStartIndex = InputLine.IndexOf(PropertyName) + PropertyName.Length;//начало названия события                                        
            int PropertyValueEndIndex = InputLine.IndexOf(EndingFlag, PropertyValueStartIndex);
            int PropertValueLength = PropertyValueEndIndex - PropertyValueStartIndex;

            if ((PropertyValueStartIndex >= 0) & (PropertyValueEndIndex >= 0))
                return InputLine.Substring(PropertyValueStartIndex, PropertValueLength);
            else
                return "NoParam";
        }

        public string Get(string InputLine, string PropertyName)
        {
            if ((InputLine.IndexOf(PropertyName) < 0) | (PropertyName == ""))
                return "NoParam";

            int PropertyValueStartIndex = InputLine.IndexOf(PropertyName) + PropertyName.Length;//начало названия события

            return InputLine.Substring(PropertyValueStartIndex);
        }
    }
}
