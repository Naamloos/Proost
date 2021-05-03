using FeestSpel.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // GET /api/update
        [HttpGet]
        [Route("update")]
        public GameUpdate GetUpdate()
        {
            // requests a state update. Might not actually be different from the previous state.
            return new GameUpdate() { Text = manager.DependencyTest() };
        }
    }
}
