using FeestSpel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FeestSpel
{
    public class WebsocketConnection
    {
        private WebSocket websocket;
        private GameManager manager;
        private CancellationTokenSource cts;

        public WebsocketConnection(WebSocket websocket, GameManager manager)
        {
            this.websocket = websocket;
            this.manager = manager;
            this.cts = new CancellationTokenSource();
        }

        public async Task StartSession(string roomCode)
        {
            // register session to updater
            var room = manager.GetRoomByCode(roomCode);

            if (room is null) // no such room kill connection
                return;

            room.CurrentText.Register(update);

            await update(room.CurrentText.GetValue());

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[2];
                    var returnBuffer = new ArraySegment<byte>(buffer);
                    await websocket.ReceiveAsync(returnBuffer, cts.Token);

                    // This will just throw when connection dies
                    // hacky, but works.
                }
                catch (Exception ex)
                {
                    break;
                } // shhhhh
            }

            cts.Cancel();
            room.CurrentText.Unregister(update);
        }

        private async Task update(string newvalue)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
                new GameUpdate()
                {
                    Action = "text",
                    Context = newvalue
                }));

            await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
        }
    }
}
