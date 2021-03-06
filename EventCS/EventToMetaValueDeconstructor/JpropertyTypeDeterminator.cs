﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace EventToMetaValueDeconstructor
{
    //распозглватель типа JSON поля
    public class JpropertyTypeDeterminator
    {
        private bool IsNumber( string inputLine )
        {            
            string Pattern = "^[-+]?[0-9]*[.,]?[0-9]*$";

            return Regex.IsMatch( inputLine, Pattern );
        }

        private bool IsDate( string inputLine )
        {
            if ( DateTime.TryParse( inputLine, out _ ) )
                return true;

            return false;
        }               

        private bool IsObject( string inputLine )
        {                                 
            if ( ( inputLine.StartsWith( "{" ) ) & ( inputLine.EndsWith( "}" ) ) )
                return true;
            return false;
        }

        private bool IsList(string inputLine)
        {
            if ( ( inputLine.StartsWith( "[" ) ) & ( inputLine.EndsWith( "]" ) ) )
                return true;
            return false;
        }

        public PropertyType Get(String inputLine)
        {            
            if (inputLine == "")
            {
                return PropertyType.String;
            }
            if (IsObject(inputLine))
            {
                return PropertyType.Object;
            }
            else if (IsNumber(inputLine))
            {
                return PropertyType.Number;
            }
            else if (IsDate(inputLine))
            {
                return PropertyType.DateTime;
            }
            else if (IsList(inputLine))
            {
                return PropertyType.List;
            }

            return PropertyType.String;
        }
    }
}
