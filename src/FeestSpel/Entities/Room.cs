using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public int MissionsPassed = 0;

        public bool finished = false;

        private List<WebsocketConnection> connectionsInRoom { get; set; } = new List<WebsocketConnection>();

        private List<ActiveSubMission> activeSubMissions { get; set; } = new List<ActiveSubMission>();

        private Mission lastMission { get; set; } = null;

        private SubMission lastSubMission { get; set; } = null;

        public Room(string roomcode, string hostkey, GameSettings settings, GamePack pack, IPAddress CreatedAt)
        {
            this.RoomCode = roomcode;
            this.HostKey = hostkey;
            this.Settings = settings;
            this.pack = pack;
            this.CreatedAt = CreatedAt;
            var miss = pack.BuildNewMissionString(settings, null);
            this.lastMission = miss.Item2;
            this.CurrentText = new AsyncObserver<string>(miss.Item1);
        }

        public async Task NextMission()
        {
            if (finished)
                return;

            MissionsPassed++;
            if (MissionsPassed > Settings.MissionCount)
            {
                finished = true;
                await CurrentText.SetValueAsync("Het spel is over! Bedankt voor het spelen.");
                await Task.Delay(5000);
                await KillAsync();
                return;
            }
            LastHostRequest = DateTime.Now;

            await nextAsync();
        }

        private async Task nextAsync()
        {
            var rng = new Random();

            var missionstring = "Er ging iets goed mis. Hier staat geen opdracht.";

            foreach (var sub in activeSubMissions)
            {
                sub.Duration--;
            }

            var subm = activeSubMissions.FirstOrDefault(x => x.Duration < 1);
            if (subm != null)
            {
                // deactivate submission
                missionstring = string.Format(subm.SubMission.Deactivation, subm.Players.ToArray());
                activeSubMissions.Remove(subm);
            }
            else if (rng.Next(0, 8) == 1 && (Settings.MissionCount - MissionsPassed > 1))
            {
                // activate new sub mission
                var selectedSubMission = pack.GetNewSubMission(lastSubMission);
                this.lastSubMission = selectedSubMission;
                var maxSelection = Settings.Players.Count() - (selectedSubMission.SubjectCount - 1);

                // using ToList to get a NEW list without shuffling the original list.
                var players = Settings.Players.ToList().OrderBy(x => rng.Next());
                // Get correct amount of random players, and shuffle selection
                var subjects = Settings.Players.GetRange(rng.Next(0, maxSelection), selectedSubMission.SubjectCount).OrderBy(x => rng.Next()).ToList();

                var active = new ActiveSubMission(subjects, selectedSubMission, rng.Next(1, Settings.MissionCount - MissionsPassed));
                activeSubMissions.Add(active);

                missionstring = string.Format(selectedSubMission.Activation, subjects.ToArray());
            }
            else
            {
                // regular mission
                var miss = pack.BuildNewMissionString(Settings, lastMission);
                missionstring = miss.Item1;
                lastMission = miss.Item2;
            }

            await CurrentText.SetValueAsync(missionstring);
        }

        public void AddConnection(WebsocketConnection connection)
        {
            this.connectionsInRoom.Add(connection);
        }
        public void RemoveConnection(WebsocketConnection connection)
        {
            if (this.connectionsInRoom.Contains(connection))
                this.connectionsInRoom.Remove(connection);
        }

        public async Task KillAsync()
        {
            foreach (var connection in connectionsInRoom)
            {
                await connection.UpdateAsync("Game gesloten door host.");
                await connection.DisconnectAsync();
                connection.cts.Cancel();
            }

            connectionsInRoom.Clear();
        }

        const string bag = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static string GenerateCode()
        {
            string code = "";
            for (int i = 0; i < 8; i++)
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
