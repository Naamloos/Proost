using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FeestSpel.Entities
{
    public class Room
    {
        public string RoomCode;

        /// <summary>
        /// Key stored in hosts' session.
        /// </summary>
        public string HostKey;

        public GamePack pack;

        public AsyncObserver<string> CurrentText;

        public DateTime LastHostRequest = DateTime.Now;

        public IPAddress CreatedAt;

        public GameSettings Settings { get; set; }

        private List<WebsocketConnection> connectionsInRoom { get; set; } = new List<WebsocketConnection>();

        public Room(string roomcode, string hostkey, GameSettings settings, GamePack pack, IPAddress CreatedAt)
        {
            this.RoomCode = roomcode;
            this.HostKey = hostkey;
            this.Settings = settings;
            this.pack = pack;
            this.CreatedAt = CreatedAt;
            this.CurrentText = new AsyncObserver<string>(pack.BuildNewMissionString(settings));
        }

        public async Task NextMission()
        {
            LastHostRequest = DateTime.Now;
            await CurrentText.SetValueAsync(pack.BuildNewMissionString(Settings));
        }

        public void AddConnection(WebsocketConnection connection)
        {
            this.connectionsInRoom.Add(connection);
        }

        public async Task KillAsync()
        {
            foreach(var connection in connectionsInRoom)
            {
                connection.cts.Cancel();
            }

            connectionsInRoom.Clear();
        }

        const string bag = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static string GenerateCode()
        {
            string code = "";
            for(int i = 0; i < 8; i++)
            {
                code += bag[new Random().Next(0, bag.Length - 1)];
            }

            return code;
        }
    }

    public class AsyncObserver<T>
    {
        public delegate Task OnUpdate(T value);

        private List<OnUpdate> updaters = new List<OnUpdate>();

        private T value;

        public AsyncObserver(T initialValue)
        {
            this.value = initialValue;
        }

        public void Register(OnUpdate method)
        {
            if (!updaters.Contains(method))
                updaters.Add(method);
        }

        public void Unregister(OnUpdate method)
        {
            if (updaters.Contains(method))
                updaters.Remove(method);
        }

        public async Task SetValueAsync(T value)
        {
            this.value = value;

            foreach (var m in updaters)
            {
                await m.Invoke(this.value);
            }
        }

        public T GetValue()
        {
            return this.value;
        }
    }
}
