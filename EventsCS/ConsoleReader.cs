using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCS
{
    class ConsoleReader : IReader
    {
        public string ReadData()
        {
            string readedData = Console.ReadLine();

            return readedData;
        }
    }
}
