using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KindredGamer.Website.Models;
using KindredGamer.Game;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Actors.Client;
using KindredGamer.User.Interfaces;
using Microsoft.ServiceFabric.Actors;

namespace KindredGamer.API.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        

        public GamesController()
        {
            _gameService = ServiceProxy.Create<IGameService>(
                new Uri("fabric:/KindredGamer/Game/GameService"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<IEnumerable<ApiGame>> Get()
        {
            IEnumerable<KindredGamer.Game.Game> allGames = await _gameService.GetAllGames();

            return allGames.Select(p => new ApiGame
            {
                Name = p.Title,
                Description = "Amazing Game",
                MaxPlayers = p.MaxPlayersSupported
                
            });
        }

        private void PopulateUsers(int userCount)
        {
            for (int i = 0; i < userCount; i++)
            {
                var id = Guid.NewGuid().ToString();
                var actorProxy = ActorProxy.Create<IUser>(new ActorId(id), new Uri("fabric:/KindredGamer/UserActorService"));
                actorProxy.InitalizeData(id, string.Format("Name{0}", i), string.Format("GamerTag{0}", i));
            }            
        }
    }
}
