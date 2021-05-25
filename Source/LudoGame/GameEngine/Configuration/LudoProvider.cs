using System;
using System.Collections.Generic;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LudoGame.GameEngine.Configuration
{
    public class LudoProvider : ILudoProvider
    {
        private IServiceProvider _provider;
        public LudoProvider()
        {
            var container = new ServiceCollection();

            container
                .AddSingleton<IBoardCollection, BoardCollection>()
                .AddSingleton<IGameAction, GameAction>()
                .AddSingleton<IGamePlay, GamePlay>()
                .AddSingleton<IGameFunction, GameFunction>()
                .AddSingleton<IGameEvent, DefaultGameEvent>()
                .AddSingleton<IOptionsValidator, OptionsValidator>()
                .AddSingleton<IEqualityComparer<Pawn>, PawnComparer>()
                .AddSingleton<IGameFunction, GameFunction>()
                .AddSingleton<IBoardOrm, BoardOrm>();

            _provider = container.BuildServiceProvider();
        }

        public T GetGameService<T>()
        {
           return _provider.GetService<T>();
        }
    }
}
