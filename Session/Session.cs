using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using KindredGamer.Session.Interfaces;

namespace KindredGamer.Session
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Session : Actor, ISession
    {
        /// <summary>
        /// Initializes a new instance of Session
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Session(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public Task AddPlayer(string userId)
        {
            var data = StateManager.GetStateAsync<SessionData>("SessionData").Result;
            data.CurrentPlayers.Add(userId);
            return StateManager.SetStateAsync<SessionData>("SessionData", data);
        }

        public Task<List<string>> GetPlayers()
        {
            throw new NotImplementedException();
        }

        public Task<SessionData> GetSessionData()
        {
            throw new NotImplementedException();
        }

        public Task InitalizeData(string id, string title, string gameId, string ownerId, DateTime startDateTime, int totalDesiredPlayers, List<string> currentPlayers, DateTime? EndDateTime)
        {
            var data = new SessionData { Id = id, Title=title, TotalDesiredPlayers= totalDesiredPlayers,
                GameId = gameId,StartDateTime = startDateTime
            };
            return StateManager.SetStateAsync<SessionData>("SessionData", data);
        }

        public Task RemovePlayer(string userId)
        {
            var data = StateManager.GetStateAsync<SessionData>("SessionData").Result;
            data.CurrentPlayers.Remove(userId);
            return StateManager.SetStateAsync<SessionData>("SessionData", data);
        }



        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("count", 0);
        }

    
    }
}
