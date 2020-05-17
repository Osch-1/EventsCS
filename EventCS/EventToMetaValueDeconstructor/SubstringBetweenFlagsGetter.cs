using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class SubstringBetweenFlagsGetter: IGetSubstring
    {        
        public string Get(string inputLine, string propertyName, string endingFlag)
        {
            if (String.IsNullOrEmpty(inputLine))
                return "";
            if ((inputLine.IndexOf(propertyName) < 0) | (propertyName == "") | (endingFlag == ""))
                return "";

            int PropertyValueStartIndex = inputLine.IndexOf(propertyName) + propertyName.Length;//начало названия события                                        
            int PropertyValueEndIndex = inputLine.IndexOf(endingFlag, PropertyValueStartIndex);
            int PropertValueLength = PropertyValueEndIndex - PropertyValueStartIndex;

            if ((PropertyValueStartIndex >= 0) & (PropertyValueEndIndex >= 0))
                return inputLine.Substring(PropertyValueStartIndex, PropertValueLength);
            else
                return "";
        }

        public string Get(string inputLine, string propertyName)
        {
            if ((inputLine.IndexOf(propertyName) < 0) | (propertyName == ""))
                return "";

            int PropertyValueStartIndex = inputLine.IndexOf(propertyName) + propertyName.Length;//начало названия события

            return inputLine.Substring(PropertyValueStartIndex);
        }
    }
}
