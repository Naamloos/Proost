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
using InertiaCore;
using FeestSpel.Middleware;

namespace FeestSpel.Api
{
    [Route("/beta")]
    public class WebController : Controller
    {
        private GameManager gameManager;

        public WebController(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public async Task<IActionResult> Index()
        {
            return Inertia.Render("Home", new {
                activeGames = gameManager.GetRoomCount(),
                activeClients = gameManager.GetClientCount()
            });
        }

        [HttpGet("new")]
        public async Task<IActionResult> NewGame()
        {
            return Inertia.Render("CreateNew");
        }

        [HttpGet("join")]
        public async Task<IActionResult> JoinGame()
        {
            return Inertia.Render("JoinExisting");
        }

        [HttpGet("game")]
        public async Task<IActionResult> InGame()
        {
            return Inertia.Render("InGame");
        }

        [HttpGet("end")]
        public async Task<IActionResult> End()
        {
            return Inertia.Render("GameEnded");
        }
    }
}
