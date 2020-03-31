using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    class StringValueTypeDeterminator
    {
        private bool IsUInt(string InputLine)
        {
            int NumberValue;
            return int.TryParse(InputLine, out NumberValue);
        }

        private bool IsDate(string InputLine)
        {
            string[] DateFormats = {"dd.MM.yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss.SSSS"};

            DateTime DateValue;

            if (DateTime.TryParseExact(InputLine, DateFormats, new CultureInfo("en-US"), DateTimeStyles.None, out DateValue))
                return true;
            
            return false;
        }
        
        public string Get(string InputLine)
        {
            if (IsUInt(InputLine))
                return "UInt";
            else if (IsDate(InputLine))
                return "Date";
              return "String";
        }
    }
}
