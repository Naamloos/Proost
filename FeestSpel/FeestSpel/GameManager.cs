﻿using FeestSpel.Entities;
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

            // we can just preload here, no worries lol
            if (!Directory.Exists("packs"))
            {
                Directory.CreateDirectory("packs");
                var path = Path.Combine("packs", "default.json");
                File.Create(path).Close();
                File.WriteAllText(path, JsonSerializer.Serialize(new GamePack(), typeof(GamePack), new JsonSerializerOptions() { WriteIndented = true }));
            }

            var packs = Directory.GetFiles("packs").Where(x => x.EndsWith(".json"));

            foreach(var pack in packs)
            {
                this.packs.Add((GamePack)JsonSerializer.Deserialize(File.ReadAllText(pack), typeof(GamePack)));
            }
        }

        public List<GamePack> GetPacks()
        {
            return packs;
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
            return rooms.First(x => x.RoomCode == roomcode).HostKey == hostkey;
        }

        private async Task loop()
        {
            while(!cts.IsCancellationRequested)
            {
                foreach(var r in rooms)
                {
                    if(DateTime.Now.Subtract(r.LastHostRequest).TotalHours > 5)
                    {
                        await r.KillAsync();
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
