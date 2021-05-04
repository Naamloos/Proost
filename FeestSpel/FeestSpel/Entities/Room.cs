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

        public AsyncObserver<string> CurrentText = new AsyncObserver<string>("initial value");

        public DateTime LastHostRequest = DateTime.Now;

        public IPAddress CreatedAt;

        public GameSettings Settings { get; set; }

        public Room(string roomcode, string hostkey, GameSettings settings, GamePack pack, IPAddress CreatedAt)
        {
            this.RoomCode = roomcode;
            this.HostKey = hostkey;
            this.Settings = settings;
            this.pack = pack;
            this.CreatedAt = CreatedAt;
        }

        public async Task NextMission()
        {
            await CurrentText.SetValueAsync(pack.BuildNewMissionString(Settings));
        }
    }

    public class AsyncObserver<T>
    {
        public delegate Task OnUpdate(T value);

        private List<OnUpdate> updaters;

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
