using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCS
{
    class Program
    {
        static void Main(string[] args)
        {
            IStorage list = new StorageList();

            list.PutInStorage("2020-03-10 00:00:33.1312|44|[Publish] {0e70ced1-0557-4fce-872a-1358cc3afd0c} key:ET_TravelLine.PriceOptimizer.IntegrationTLTransit.Events.IntegrationEvents.WebPms:WebPmsRoomsInventoryDelivered, json:{\"PropertyId\":1111,\"RoomInventoryList\":[{\"RoomTypeId\":24755,\"Physical\":10,\"OutOfInventory\":1,\"DateInventories\":null},{\"RoomTypeId\":24756,\"Physical\":1,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":10704,\"Physical\":3,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":17708,\"Physical\":3,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":9346,\"Physical\":4,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":9328,\"Physical\":6,\"OutOfInventory\":1,\"DateInventories\":null},{\"RoomTypeId\":9438,\"Physical\":2,\"OutOfInventory\":1,\"DateInventories\":null},{\"RoomTypeId\":21509,\"Physical\":1,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":17349,\"Physical\":3,\"OutOfInventory\":0,\"DateInventories\":null},{\"RoomTypeId\":308836,\"Physical\":1,\"OutOfInventory\":1,\"DateInventories\":null}],\"Id\":\"0e70ced1 - 0557 - 4fce - 872a - 1358cc3afd0c\",\"CreationDate\":\"2020 - 03 - 09T21: 00:33.1312183Z\",\"CorrelationId\":null}");
            Console.WriteLine(list.GetElementByIndex(0));
            Console.ReadKey();
        }
    }
}
