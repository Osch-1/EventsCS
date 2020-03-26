using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCS
{
    class ConsoleReader : Reader
    {
        public string readData()
        {
            string readedData = Console.ReadLine();

            return readedData;
        }
    }
}
