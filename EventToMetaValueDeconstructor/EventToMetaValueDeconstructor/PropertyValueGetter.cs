using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class PropertyValueGetter : IGetSubstring
    {
        public string GetPropertyValue(string InputLine, string PropertyName, string EndingFlag)
        {
            int PropertyValueStartIndex = InputLine.IndexOf(PropertyName) + PropertyName.Length;//начало названия события
            int PropertyValueEndIndex = InputLine.IndexOf(EndingFlag, PropertyValueStartIndex);
            int PropertValueLength = PropertyValueEndIndex - PropertyValueStartIndex;

            if ((PropertyValueStartIndex >= 0) & (PropertyValueEndIndex >= 0))
                return InputLine.Substring(PropertyValueStartIndex, PropertValueLength);
            else
                return "";
        }

        public string GetPropertyValue(string InputLine, string PropertyName)
        {
            int PropertyValueStartIndex = InputLine.IndexOf(PropertyName) + PropertyName.Length;//начало названия события                        

            if (PropertyValueStartIndex >= 0)
                return InputLine.Substring(PropertyValueStartIndex);
            else
                return "";
        }
    }
}
