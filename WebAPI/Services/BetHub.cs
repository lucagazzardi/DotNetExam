using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataBase;

namespace WebAPI.Services
{
    public class BetHub : Hub
    {
        public async Task AskJackpot(string username)
        {
            ExistingUsers.RegisteredUsers.Single(x => x.username == username).connectionId = Context.ConnectionId;
            await Clients.Caller.SendAsync("SendJackpot", BetsHandler.Jackpot.ToString());
        }
    }
}
