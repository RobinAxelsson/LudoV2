using System;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LudoGame.GameEngine.Configuration
{
    public class LudoContainerBuilder
    {
        public IServiceProvider Build()
        {
            var container = new ServiceCollection();

            container
                .AddScoped<IBoardCollection, IBoardCollection>()
                .AddScoped<IGameAction, GameAction>()
                .AddScoped<IGamePlay, GamePlay>()
                .AddScoped<IGameEvent, GameEvent>()
                .AddScoped<IFunctionRegistrar, FunctionRegistrar>()
                .AddScoped<IOptionsValidator, OptionsValidator>()
                .AddSingleton<IFunctionRegistrar, FunctionRegistrar>()
                .AddSingleton<IBoardOrm, BoardOrm>();
                

            return container.BuildServiceProvider();
        }
    }
}
