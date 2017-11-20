using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KindredGamer.Game
{

    public class ServiceFabricGameRepository : IGameRepository
    {
        private IReliableStateManager _stateManager;
        private object cancellationToken;

        public ServiceFabricGameRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }
        public async Task<IEnumerable<Game>> GetAllGames()
        {
            var games = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Game>>("Games");
            var result = new List<Game>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allGames = await games.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allGames.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<string, Game> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }

        public async Task AddGame(Game game)
        {
            var games = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Game>>("Games");

            using (var tx = _stateManager.CreateTransaction())
            {
                await games.AddOrUpdateAsync(tx, game.Id, game, (id, value) => game);

                await tx.CommitAsync();
            }
        }

        public async Task<Game> GetGame(string gameId)
        {
            var games = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Game>>("Games");

            using (var tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<Game> game = await games.TryGetValueAsync(tx, gameId);

                return game.HasValue ? game.Value : null;
            }
        }
    }
}





