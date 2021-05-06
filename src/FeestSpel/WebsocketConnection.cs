using FeestSpel.Entities;
using System;
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
        public CancellationTokenSource cts;

        public WebsocketConnection(WebSocket websocket, GameManager manager)
        {
            this.websocket = websocket;
            this.manager = manager;
            this.cts = new CancellationTokenSource();
        }

        public async Task StartSession(string roomCode, string hostKey)
        {
            // register session to updater
            var room = manager.GetRoomByCode(roomCode);

            await updateAsync("Loading...");

            if (room is null) // no such room kill connection
            {
                await DisconnectAsync();
                cts.Cancel();
                return;
            }

            room.AddConnection(this);

            room.CurrentText.Register(updateAsync);

            await updateAsync(room.CurrentText.GetValue());

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[2];
                    var returnBuffer = new ArraySegment<byte>(buffer);
                    await websocket.ReceiveAsync(returnBuffer, cts.Token);
                    if (Encoding.UTF8.GetString(returnBuffer.Array) == "++")
                    {
                        if(room.HostKey == hostKey)
                            await room.NextMission();
                    }
                }
                catch (Exception ex)
                {
                    break;
                } // shhhhh
            }

            cts.Cancel();
            try
            {
                await DisconnectAsync();
            }
            catch (Exception) { }

            room.CurrentText.Unregister(updateAsync);
            room.RemoveConnection(this);
        }

        public async Task DisconnectAsync()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
                new GameUpdate()
                {
                    Action = "redirect",
                    Context = "/"
                }));

            await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
        }

        private async Task updateAsync(string newvalue)
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
