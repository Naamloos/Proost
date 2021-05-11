using FeestSpel.Entities;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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

            await UpdateAsync("Loading...");

            if (room is null) // no such room kill connection
            {
                await DisconnectAsync();
                cts.Cancel();
                return;
            }

            room.AddConnection(this);

            room.CurrentText.Register(UpdateAsync);

            await UpdateAsync(room.CurrentText.GetValue());

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[2];
                    var returnBuffer = new ArraySegment<byte>(buffer);
                    await websocket.ReceiveAsync(returnBuffer, cts.Token);

                    var cmd = Encoding.UTF8.GetString(returnBuffer.Array);

                    if (room.HostKey == hostKey)
                        switch (cmd)
                        {
                            case "++":
                                // next page command
                                await room.NextMission();
                                break;
                            case "xx":
                                // exit command
                                await room.KillAsync();
                                break;
                            default:
                                break;
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

            room.CurrentText.Unregister(UpdateAsync);
            room.RemoveConnection(this);
        }

        public async Task DisconnectAsync()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
                new GameUpdate()
                {
                    Action = "redirect",
                    Context = HttpUtility.HtmlEncode("/invalidate")
                }));

            await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
        }

        public async Task UpdateAsync(string newvalue)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
                new GameUpdate()
                {
                    Action = "text",
                    Context = HttpUtility.HtmlEncode(newvalue)
                }));

            await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
        }
    }
}
