# Documentation
This is an backend-frontend project that is based on the Ludo Board Game and is used to for practicing Http-Requests, EF, ASP.NET web framework and SignalR.

# Architecture depencency diagrams
## We have spent a lot of time discussing the project architecture and made a big shift last 5 days when removing the API-project and replacing all API-calls with SignalR.
![image](https://user-images.githubusercontent.com/63591629/120038766-0dde3280-c004-11eb-9ae9-acc219e9d768.png)
## A big focus was to keep the libraries (LudoGame, LudoTranslation) independent of the main projects.
![image](https://user-images.githubusercontent.com/63591629/120039244-dae86e80-c004-11eb-8ce8-41c00366694f.png)

# Dependency Injection
Stand alone container injection of LudoGame in [LudoProvider.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoGame/GameEngine/Configuration/LudoProvider.cs).
This design creates an additional dependency injection system on top of ASP.NET's. The system adds a new container of game services for every created instance of the LudoProvider. This makes it possible to have multiple gamerooms with different ludo games at the same time.
```csharp
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
```
This class provide the needed game services for each game and is applied in [GameRoom.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoWeb/GameClasses/GameRoom.cs)
```csharp
        public GameRoom(AbstractFactory gameFactory, IGameNetworkManager manager, string gameId, LudoNetworkFactory factory)
        {
            var provider = gameFactory.BuildLudoProvider();
            _gamePlay = provider.GetGameService<IGamePlay>();
            _gameEvent = provider.GetGameService<IGameEvent>();
            _boardCollection = provider.GetGameService<IBoardCollection>();
```
