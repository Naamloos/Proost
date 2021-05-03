using FeestSpel.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Text.Json;

namespace FeestSpel.Api
{
    [Route("api")]
    public class GameController : Controller
    {
        private GameManager manager;

        public GameController(GameManager manager)
        {
            this.manager = manager;
        }

        // GET: GameController
        public string Index()
        {
            return "FeestSpel API";
        }

        // POST /api/update
        // Remember this should require the session's room key to match the room's key. else just ignore or something idk
        [HttpPost]
        [Route("update")]
        public void PostUpdate()
        {

        }

        // GET /api/ws
        [HttpGet]
        [Route("ws")]
        public async Task GetUpdateAsync()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                // keeping it simple I guess
                var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();

                while (ws.State != System.Net.WebSockets.WebSocketState.Closed && ws.State != System.Net.WebSockets.WebSocketState.CloseReceived)
                {
                    try
                    {
                        if (ws.State == WebSocketState.Open)
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new GameUpdate() { Action = "text", Context = new Random().Next().ToString() }));
                            await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    catch (Exception) { } // shhhhh

                    await Task.Delay(1000);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
            }
        }
    }
}
