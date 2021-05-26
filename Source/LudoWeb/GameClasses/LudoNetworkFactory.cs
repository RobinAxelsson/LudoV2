using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGame.GameEngine;
using LudoGame.GameEngine.AI;
using LudoGame.GameEngine.Configuration;
using LudoGame.GameEngine.Interfaces;
using LudoWeb.GameInterfaces;

namespace LudoWeb.GameClasses
{
    public class LudoNetworkFactory
    {
        public INetworkPlayer NetworkPlayer(IGameNetworkManager manager, GameEnum.TeamColor color, Client client,
            IGamePlayer stephan) => new NetworkPlayer(manager, color, client, stephan);

        public IGameMessager GameMessager(IGameEvent gameEvent, IGameNetworkManager manager, IGameRoom room) =>
            new GameMessager(gameEvent, manager, room);

        public IGamePlayer AIPlayer(GameEnum.TeamColor color, IBoardCollection boardCollection) => new Stephan(color, boardCollection);

        public IGameRoom GameRoom(AbstractFactory gameFactory, IGameNetworkManager manager, string gameId,
            LudoNetworkFactory factory)
            => new GameRoom(gameFactory, manager, gameId, factory);

    }
}
