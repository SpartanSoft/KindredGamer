using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KindredGamer.Website.Models;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using KindredGamer.Game;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors;
using KindredGamer.User.Interfaces;

namespace KindredGamer.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HomePageModel model = new HomePageModel();
            model.Games = GetGames();

            PopulateUsers(100);

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
        private List<ApiGame> GetGames()
        {
            IGameService _gameService = ServiceProxy.Create<IGameService>(
                        new Uri("fabric:/KindredGamer/GameService"),
                        new ServicePartitionKey(0));


            IEnumerable<KindredGamer.Game.Game> allGames = _gameService.GetAllGames().Result;
            return allGames.Select(p => new ApiGame
            {
                Name = p.Title,
                Description = "Amazing Game",
                MaxPlayers = p.MaxPlayersSupported

            }).ToList();
        }
    }
}
