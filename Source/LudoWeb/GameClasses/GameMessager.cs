using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Interfaces;
using LudoWeb.GameInterfaces;

namespace LudoWeb.GameClasses
{
    //TranslationEngine

    public class GameMessager : IGameMessager
    {
        private IGameEvent _gameEvent;
        private IGameNetworkManager _networkManager;
        private IGameRoom _room;
        public GameMessager(IGameEvent gameEvent, IGameNetworkManager manager, IGameRoom room)
        {
            _gameEvent = gameEvent;
            _networkManager = manager;
            _room = room;
        }

        public void Bind()
        {
            _gameEvent.OnAllTeamPawnsOutEvent += AllPawnsOut;
            _gameEvent.OnGoalEvent += OnGoal;
            _gameEvent.OnBounceEvent += OnBounce;
        }

        private void OnBounce(GameEnum.TeamColor color)
        {
            SendMessage($"Team {color} bounced, ouch!");
        }
        private void OnGoal(GameEnum.TeamColor color, int left)
        {
            SendMessage($"Player {color} scored!");
        }
        private void AllPawnsOut(GameEnum.TeamColor color)
        {
            SendMessage($"Player {color} has no birds in the nest!");
        }

        private void SendMessage(string message)
        {
            _networkManager.SendGameMessage(message, _room.GameId);
        }
    }
}
