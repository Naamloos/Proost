using FeestSpel.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
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
            this.rooms = new List<Room>();
            this.packs = new List<GamePack>();
            string currentdir = Directory.GetCurrentDirectory();
            string packpath = Path.Combine(currentdir, "packs");

            // we can just preload here, no worries lol
            if (!Directory.Exists(packpath))
            {
                Directory.CreateDirectory(packpath);
                var path = Path.Combine(packpath, "default.json");
                File.Create(path).Close();
                File.WriteAllText(path, JsonSerializer.Serialize(new GamePack(), typeof(GamePack), new JsonSerializerOptions() { WriteIndented = true }));
            }

            var packs = Directory.GetFiles(packpath).Where(x => x.EndsWith(".json"));

            foreach (var pack in packs)
            {
                if(!File.Exists(pack))
                {
                    continue;
                }
                var loadedPack = (GamePack)JsonSerializer.Deserialize(File.ReadAllText(pack), typeof(GamePack));
                if (loadedPack.Missions.Count > 0 && loadedPack.SubMissions.Count > 0)
                {
                    this.packs.Add(loadedPack);
                }
            }
        }

        public List<GamePack> GetPacks()
        {
            return packs;
        }

        public int GetRoomCount()
        {
            return this.rooms.Count(x => !x.finished);
        }

        public int GetClientCount()
        {
            return this.rooms.Select(x => x.GetConnectionCount()).Sum();
        }

        public void Start()
        {
            _ = Task.Run(async () => await loop());
        }

        public string DependencyTest() => "dooot";

        public Room GetRoomByCode(string code)
        {
            return rooms.FirstOrDefault(x => x.RoomCode == code);
        }

        public void RegisterRoom(Room room)
        {
            this.rooms.Add(room);
        }

        public void UnregisterRoom(Room room)
        {
            this.rooms.Remove(room);
        }

        public GamePack GetPack(string title)
        {
            return packs.FirstOrDefault(x => x.Title == title);
        }

        public int GetAmountOfRoomsOnIp(IPAddress ip)
        {
            return rooms.Count(x => x.CreatedAt == ip);
        }

        public bool CheckHost(string roomcode, string hostkey)
        {
            return rooms.FirstOrDefault(x => x.RoomCode == roomcode)?.HostKey == hostkey;
        }

        private async Task loop()
        {
            while (!cts.IsCancellationRequested)
            {
                List<Room> dead = new List<Room>();
                foreach (var r in rooms)
                {
                    if (DateTime.Now.Subtract(r.LastHostRequest).TotalMinutes > 30)
                    {
                        await r.KillAsync();
                        dead.Add(r);
                    }
                    else if (r.finished)
                    {
                        dead.Add(r);
                    }
                }
                rooms.RemoveAll(x => dead.Contains(x));

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
