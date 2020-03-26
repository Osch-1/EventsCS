using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCS
{
    interface IStorage
    { 
        void PutInStorage(string data);
        List<string> GetStorage();
        string GetElementByIndex(int Index);
        void PrintStorage();
    }
}
