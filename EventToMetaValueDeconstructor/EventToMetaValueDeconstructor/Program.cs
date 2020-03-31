using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;

namespace EventToMetaValueDeconstructor
{    
    class Program//TODO make it collect all Event from ../jsons/.log file
    {
         public static void Main(string[] args)
         {
            List<JSONEvent> ListOfEventsFromLogFile = new List<JSONEvent>();
            StringToJSONEventDeserializer ToJSONEventDeserializer = new StringToJSONEventDeserializer();
            string ReadedLine;

            StreamReader LogFile = new StreamReader(@"jsons/publish-integration-events-2020-03-10.log");
            

            while ((ReadedLine = LogFile.ReadLine()) != null)
            {
                ListOfEventsFromLogFile.Add(ToJSONEventDeserializer.Deserialize(ReadedLine));
            }            
            
            foreach (JSONEvent Event in ListOfEventsFromLogFile)
            {
                Console.WriteLine(Event);
            }
        }
    }
}
