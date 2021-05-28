![Logo](/Documentation/LudoBanner_NoShadow.png)

# PGBSNH20 Group 5 Ludo Web
## An online Ludo game based on SignalR


# Game Logic
All the game logic happens on the server.
## Server to Client data:
- textmessages - to all clients
- a big [pawn.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoGame/GameEngine/Classes/Pawn.cs) array with all pawns - to all clients
- A small pawn array and bool which represents if a player can take out two pawns (6 and more then one in base), and the pawns that can be choosen. - to one player-client
## Playing client to server:
- a pawn array with one or two pawns. This represents what pawns the client player choose.
## One player full turn
![image](https://user-images.githubusercontent.com/63591629/120044458-2fdcb280-c00e-11eb-907d-2468e1eb94c2.png)

# Key Classes

# Architecture depencency diagrams
## We have spent a lot of time discussing the project architecture and made a big shift last 5 days when removing the API-project and replacing all API-calls with SignalR.
![image](https://user-images.githubusercontent.com/63591629/120038766-0dde3280-c004-11eb-9ae9-acc219e9d768.png)
## A big focus was to keep the libraries (LudoGame, LudoTranslation) independent of the main projects.
![image](https://user-images.githubusercontent.com/63591629/120039244-dae86e80-c004-11eb-8ce8-41c00366694f.png)

# Dependency Injection
A good way to clear up dependencies is a good design for instantiating the Library classes. This is made through stand alone container injection of LudoGame in [LudoProvider.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoGame/GameEngine/Configuration/LudoProvider.cs).
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
```csharp
public class GamePlay : IGamePlay
{
        public async Task Start(args)
        {
                 foreach (var player in _orderedPlayers)
                {
                /*

                rolling dice

                check board for valid options to provide the player


                waits for IGamePlayer response on player.ChoosePlay


                validates the response


                if valid


                        update the board in memory


                        send updated pawns through SignalR

                        */
}
```
