using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCS
{
    class StorageList : IStorage
    {
        List<string> DataList = new List<string>();

        public string GetElementByIndex(int Index)
        {
            return DataList[Index];
        }
        public List<string> GetStorage()
        {
            return DataList;
        }

        public void PutInStorage(string data)
        {
            DataList.Add(data);
        }

        public void PrintStorage()
        {
            if (!DataList.Any())
            {
                Console.WriteLine("Data list is empty");
            }
            else
            {
                foreach (string data in DataList)
                {
                    Console.WriteLine(data);
                }
            }
        }
    }
}
