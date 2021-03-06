﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace KindredGamer.Session.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ISession : IActor
    {
        Task InitalizeData(string id, string title, string gameId, string ownerId, DateTime startDateTime, 
            int TotalDesiredPlayers, List<string> CurrentPlayers, DateTime? EndDateTime);
        
        Task<SessionData> GetSessionData();

        Task AddPlayer(string userId);

        Task RemovePlayer(string userId);

        Task<List<string>> GetPlayers();
    }
}

