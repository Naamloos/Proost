using FeestSpel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FeestSpel
{
    public class GameManager : IDisposable
    {
        private List<GamePack> packs;
        private List<Room> rooms;
        private CancellationTokenSource cts;

        public GameManager()
        {
            cts = new CancellationTokenSource();
        }

        public void Preload()
        {

        }

        public void Start()
        {
            _ = Task.Run(async () => await loop());
        }

        public string DependencyTest() => "dooot";

        public Room GetRoomByCode(string code)
        {
            if(rooms.Any(x => x.RoomCode == code))
            {
                return rooms.First(x => x.RoomCode == code);
            }

            return null;
        }

        public bool CheckHost(string roomcode, string hostkey)
        {
            return rooms.First(x => x.RoomCode == roomcode).HostKey == hostkey;
        }

        private async Task loop()
        {
            while(!cts.IsCancellationRequested)
            {
                foreach(var r in rooms)
                {
                    if(DateTime.Now.Subtract(r.LastHostRequest).TotalMinutes > 10)
                    {

                    }
                }

                await Task.Delay(500);
            }
            // do cleanup
        }

        public void Dispose()
        {
            cts.Cancel();
        }

    }
}
