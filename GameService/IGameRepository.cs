using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindredGamer.Game
{
    interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllGames();

        Task AddGame(Game game);

        Task<Game> GetGame(string gameId);
    }
}