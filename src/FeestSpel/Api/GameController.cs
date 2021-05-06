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


        /* DEPRECATED */
        //// POST /api/update
        //// Remember this should require the session's room key to match the room's key. else just ignore or something idk
        //[HttpPost]
        //[Route("update")]
        //public async Task PostUpdate()
        //{
        //    string roomcode = HttpContext.Session.GetStringValue("roomcode");
        //    var room = manager.GetRoomByCode(roomcode);
        //    string key = HttpContext.Session.GetStringValue("hostkey");
        //    if (room != null && manager.CheckHost(roomcode, key))
        //    {
        //        // host sent a room update. now update.
        //        await room.NextMission();
        //    }
        //    else
        //    {
        //        HttpContext.Response.StatusCode = 500;
        //    }
        //}

        // GET /api/ws
        [HttpGet]
        [Route("ws")]
        public async Task GetUpdateAsync()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                // keeping it simple I guess
                var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var roomcode = HttpContext.Session.GetStringValue("roomcode");
                var roomkey = HttpContext.Session.GetStringValue("hostkey");

                WebsocketConnection connection = new WebsocketConnection(ws, manager);
                await connection.StartSession(roomcode, roomkey);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token);
                    }
                    catch (Exception)
                    {
                        // just offload a close request, silently catch if fails. no worries
                    }
                });
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
            }
        }
    }
}
