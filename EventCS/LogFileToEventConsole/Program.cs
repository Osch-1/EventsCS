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
            SubstringBetweenFlagsGetter substringGetter = new SubstringBetweenFlagsGetter();
            List<Event> listOfEventsFromLogFile = new List<Event>();
            JsonEventParser jsonEventParser = new JsonEventParser();
            string readedLine;

            StreamReader logFile = new StreamReader(@"jsons/publish-integration-events-2020-03-10.log");

            while ((readedLine = logFile.ReadLine()) != null)
            {
                string eventFromLogKey = substringGetter.Get(readedLine, "key:", ",");
                string jsonFromLog = substringGetter.Get(readedLine, "json:");                
                listOfEventsFromLogFile.Add(jsonEventParser.Parse(eventFromLogKey, jsonFromLog));
            }           
            
            foreach (Event jsonEvent in listOfEventsFromLogFile)
            {
                Console.WriteLine(jsonEvent);
            }
        }
    }
}
