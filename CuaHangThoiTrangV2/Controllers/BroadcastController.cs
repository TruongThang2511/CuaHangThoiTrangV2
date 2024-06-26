﻿using CuaHangThoiTrangV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CuaHangThoiTrangV2.Controllers
{
    [ApiController]
    [Route("api/broadcast")]
    public class BroadcastController : Controller
    {
        
        private readonly IHubContext<ChatHub> _hub;

        public BroadcastController(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        [HttpGet]
        public async Task Get(string user, string message)
        {
            await _hub.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
