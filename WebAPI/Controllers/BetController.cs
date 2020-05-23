using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.DataBase;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BetController : ControllerBase
    {
        private readonly IHubContext<BetHub> _hubContext;

        public BetController(IHubContext<BetHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("placebet")]
        public IActionResult PlaceBet(Bet bet)
        {
            if (!BetsHandler.ManageBet(bet))
                return BadRequest();

            _hubContext.Clients.All.SendAsync("UpdatedJackpot", BetsHandler.Jackpot.ToString());

            return Ok();
        }

        [HttpPost("closebets")]
        public IActionResult CloseBets()
        {
            if (!BetsHandler.IsCloseable)
                return BadRequest();

            BetsHandler.CloseBets();

            _hubContext.Clients.All.SendAsync("CloseBets");

            var result = BetsHandler.PayBets();

            if (result == null)
                return BadRequest();

            _hubContext.Clients.All.SendAsync("ExtractionResult", $"{BetsHandler.ExtractedNumbersString()}. Jolly: {BetsHandler.ExtractedJolly}");


            foreach(var winner in result)
            {
                string connectionId = ExistingUsers.RegisteredUsers.Single(x => x.username == winner.username).connectionId;

                _hubContext.Clients.Client(connectionId).SendAsync("NotifyWinning", $"Hai vinto {winner.winning} euro");
            }


            return Ok();
        }
    }
}