using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace KindredGamer.Game
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class GameService : StatefulService, IGameService
    {
        private IGameRepository _repo;
        public GameService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddGame(Game game)
        {
            await _repo.AddGame(game);
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await _repo.GetAllGames();
        }

        public async Task<Game> GetGame(string gameId)
        {
            return await _repo.GetGame(gameId);
        }


        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
         {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
         };

        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repo = new ServiceFabricGameRepository(this.StateManager);

            var game1 = new Game
            {
                Id = "destiny2",
                Title = "Destiny 2",
                MaxPlayersSupported = 8
            };
            var game2 = new Game
            {
                Id = "battlefield1",
                Title = "Battle Field I",
                MaxPlayersSupported = 32
            };
            var game3 = new Game
            {
                Id = "mineCraft",
                Title = "MineCraft",
                MaxPlayersSupported = 4
            };
            await _repo.AddGame(game1);
            await _repo.AddGame(game2);
            await _repo.AddGame(game3);

            //IEnumerable<Game> all = await _repo.GetAllGames();
        }


    }
}
