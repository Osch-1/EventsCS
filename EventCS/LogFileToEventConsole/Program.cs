using EventToMetaValueDeconstructor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogFileToEventConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Event> ListOfEventsFromLogFile = new List<Event>();
            JsonEventParser JsonEventParser = new JsonEventParser();
            string ReadedLine;

            StreamReader LogFile = new StreamReader(@"jsons/publish-integration-events-2020-03-10.log");

            while ((ReadedLine = LogFile.ReadLine()) != null)
            {
                ListOfEventsFromLogFile.Add(JsonEventParser.Deserialize(ReadedLine));
            }            
            foreach (Event JsonEvent in ListOfEventsFromLogFile)
            {
                Console.WriteLine(JsonEvent);
            }
        }
    }
}
