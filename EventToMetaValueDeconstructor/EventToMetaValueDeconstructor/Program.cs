using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace EventToMetaValueDeconstructor
{    
    class Program//TODO make it collect all Event from ../jsons/.log file
    {
        public static void Main(string[] args)
        {
            List<JSONEvent> ListOfJsonsFromLogFile = new List<JSONEvent>();

            PropertyValueGetter ValueGetter = new PropertyValueGetter();

            string InputLine= "2020-03-10 00:00:33.1312|44|[Publish] {0e70ced1-0557-4fce-872a-1358cc3afd0c} key:ET_TravelLine.PriceOptimizer.IntegrationTLTransit.Events.IntegrationEvents.WebPms:WebPmsRoomsInventoryDelivered, json:{ \"PropertyId\":1111,\"RoomInventoryList\":[{\"RoomTypeId\":24755,\"Physical\":10,\"OutOfInventory\":1,\"DateInventories\":null},{\"RoomTypeId\":24756,\"Physical\":1,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":10704,\"Physical\":3,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":17708,\"Physical\":3,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":9346,\"Physical\":4,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":9328,\"Physical\":6,\"OutOfInventory\":1,\"DateInventories\":null},{\"RoomTypeId\":9438,\"Physical\":2,\"OutOfInventory\":1,\"DateInventories\":null},{\"RoomTypeId\":21509,\"Physical\":1,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":17349,\"Physical\":3,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":308836,\"Physical\":1,\"OutOfInventory\":1,\"DateInventories\":null}],\"Id\":\"0e70ced1-0557-4fce-872a-1358cc3afd0c\",\"CreationDate\":\"2020-03-09T21:00:33.1312183Z\",\"CorrelationId\":null}";
            string EventKey = ValueGetter.GetPropertyValue(InputLine, "{", "}");            
            string EventName = ValueGetter.GetPropertyValue(InputLine, "key:", ",");            
            string JsonString = ValueGetter.GetPropertyValue(InputLine, "json:");

            if ((EventKey == "") | (EventName == "") | (JsonString == ""))
                return;

            JSONEvent JsonEvent= new JSONEvent(EventKey, EventName);            
            JObject JObjectFromString = JObject.Parse(JsonString);
            
            foreach (JProperty Property in JObjectFromString.Properties())
            {
                string PropertyType = "String";
                
                if (Property.Value is JArray)
                {
                    PropertyType = "List<Object>";
                }

                string MetaInformation = Property.Name + "-" + PropertyType;
                JsonEvent.EventPropertyMetaValue.Add(MetaInformation);
            }
            ListOfJsonsFromLogFile.Add(JsonEvent);
            Console.WriteLine(ListOfJsonsFromLogFile[0]);
        }
    }
}
