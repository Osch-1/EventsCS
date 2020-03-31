using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class SubstringBetweenFlagsGetter : IGetSubstring
    {
        private readonly int DelimiterLength = 1;

        public string Get(string InputLine, string PropertyName, string EndingFlag)
        {            

            if (InputLine.IndexOf(PropertyName) < 0)
                return "";

            int PropertyValueStartIndex = InputLine.IndexOf(PropertyName) + PropertyName.Length + DelimiterLength;//начало названия события                                        
            int PropertyValueEndIndex = InputLine.IndexOf(EndingFlag, PropertyValueStartIndex);
            int PropertValueLength = PropertyValueEndIndex - PropertyValueStartIndex;

            if ((PropertyValueStartIndex >= 0) & (PropertyValueEndIndex >= 0))
                return InputLine.Substring(PropertyValueStartIndex, PropertValueLength);
            else
                return "";
        }

        public string Get(string InputLine, string PropertyName)
        {
            if (InputLine.IndexOf(PropertyName) < 0)
                return "";

            int PropertyValueStartIndex = InputLine.IndexOf(PropertyName) + PropertyName.Length + DelimiterLength;//начало названия события

            return InputLine.Substring(PropertyValueStartIndex);
        }
    }
}
