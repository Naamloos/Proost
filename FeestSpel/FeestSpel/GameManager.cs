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

        private async Task loop()
        {
            while(!cts.IsCancellationRequested)
            {
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
