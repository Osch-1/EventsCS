using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class JsonPropertyValueGetter : IGetSubstring
    {
        public string GetPropertyValue(string inputLine, string startingFlag, string endingFlag)
        {
            int PropertyValueStartIndex = inputLine.IndexOf(startingFlag) + startingFlag.Length;//начало названия события
            int PropertyValueEndIndex = inputLine.IndexOf(endingFlag, PropertyValueStartIndex);
            int PropertValueLength = PropertyValueEndIndex - PropertyValueStartIndex;

            if ((PropertyValueStartIndex >= 0) & (PropertyValueEndIndex >= 0))
                return inputLine.Substring(PropertyValueStartIndex, PropertValueLength);
            else
                return "";
        }

        public string GetPropertyValue(string inputLine, string startingFlag)
        {
            int PropertyValueStartIndex = inputLine.IndexOf(startingFlag) + startingFlag.Length;//начало названия события                        

            if (PropertyValueStartIndex >= 0)
                return inputLine.Substring(PropertyValueStartIndex);
            else
                return "";
        }
    }
}
