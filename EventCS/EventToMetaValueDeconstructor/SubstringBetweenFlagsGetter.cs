using System;
using System.Collections.Generic;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    //реализация интерфейса igetsubstring флагами
    public class SubstringBetweenFlagsGetter: IGetSubstring
    {        
        public string Get( string inputLine, string propertyName, string endingFlag )
        {
            if ( String.IsNullOrEmpty( inputLine ) )
                return "";
            if ( ( propertyName == "" ) || ( endingFlag == "" ) || ( inputLine.IndexOf( propertyName ) < 0 ) )
                return "";

            //начало названия события                                        
            int PropertyValueStartIndex = inputLine.IndexOf( propertyName ) + propertyName.Length;
            int PropertyValueEndIndex = inputLine.IndexOf( endingFlag, PropertyValueStartIndex );
            int PropertValueLength = PropertyValueEndIndex - PropertyValueStartIndex;

            if ( ( PropertyValueStartIndex >= 0 ) && ( PropertyValueEndIndex >= 0 ) )
                return inputLine.Substring( PropertyValueStartIndex, PropertValueLength );
            else
                return null;
        }

        public string Get( string inputLine, string propertyName )
        {
            if ( ( inputLine.IndexOf( propertyName ) < 0 ) || ( propertyName == "" ) )
                return null;

            //начало названия события
            int PropertyValueStartIndex = inputLine.IndexOf( propertyName ) + propertyName.Length;

            return inputLine.Substring( PropertyValueStartIndex );
        }
    }
}
