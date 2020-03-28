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

            return inputLine.Substring(PropertyValueStartIndex, PropertValueLength);
        }

        public string GetPropertyValue(string inputLine, string startingFlag)
        {
            int PropertyValueStartIndex = inputLine.IndexOf(startingFlag) + startingFlag.Length;//начало названия события                        

            return inputLine.Substring(PropertyValueStartIndex);
        }
    }
}
