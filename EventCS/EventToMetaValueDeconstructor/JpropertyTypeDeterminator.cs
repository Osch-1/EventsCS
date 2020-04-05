using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EventToMetaValueDeconstructor
{
    public class JpropertyTypeDeterminator
    {
        private bool IsUInt(string InputLine)
        {
            int NumberValue;
            return int.TryParse(InputLine, out NumberValue);
        }

        private bool IsDate(string InputLine)
        {
            string[] DateFormats = { "dd.MM.yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss.ffff" };

            DateTime DateValue;

            if (DateTime.TryParseExact(InputLine, DateFormats, new CultureInfo("en-US"), DateTimeStyles.None, out DateValue))
                return true;

            return false;
        }

        public JsonPropertyType Get(JProperty Jproperty)
        {
            if (IsUInt(Jproperty.Value.ToString()))
                return JsonPropertyType.Int;
            else if (IsDate(Jproperty.Value.ToString()))
                return JsonPropertyType.DateTime;
            else if (Jproperty.Value is JArray)
                return JsonPropertyType.List;            
            return JsonPropertyType.String;
        }
    }
}
