using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using KindredGamer.User.Interfaces;
using System.Runtime.Serialization;
using KindredGamer.Session.Interfaces;

namespace KindredGamer.User
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
    internal class User : Actor, IUser
    {

        /// <summary>
        /// Initializes a new instance of User
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public User(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
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
        
        Task IUser.InitalizeData(string id, string name, string gamerTag)
        {
            var data = new UserData { Id = id, Name = name, GamerTag = gamerTag };
            return StateManager.SetStateAsync<UserData>("UserData", data);
        }

        Task IUser.JoinSession(string sessionid)
        {
            var data = StateManager.GetStateAsync<UserData>("UserData").Result;
            data.CurrentSession = sessionid;
            var sessionProxy = ActorProxy.Create<ISession>(new ActorId(sessionid), new Uri("fabric:/KindredGamer/SessionActorService"));
            sessionProxy.AddPlayer(data.Id);
            return StateManager.SetStateAsync<UserData>("UserData", data);
        }

        Task IUser.LeaveSession(string sessionid)
        {
            var data = StateManager.GetStateAsync<UserData>("UserData").Result;
            data.CurrentSession = string.Empty;
            return StateManager.SetStateAsync<UserData>("UserData", data);
        }
    }
}
