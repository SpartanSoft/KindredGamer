using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace KindredGamer.Game
{
    public interface IGameService: IService
    {
        Task<IEnumerable<Game>> GetAllGames();
        Task AddGame(Game game);
        Task<Game> GetGame(string gameId);
    }
}
