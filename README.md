![image](https://user-images.githubusercontent.com/63591629/120205672-84f51000-c22a-11eb-9017-f352cc1a798f.png)

# PGBSNH20 Group 5 Ludo Web

Ludo Web project is a .NET online Ludo Board Game Platform. It was made between 10th of may and 30th of may 2021, during our studies to .NET Developer at Teknik Högskolan.

Albin Alm -> [GitHub](https://github.com/albinalm)

Robin Axelsson -> [GitHub](https://github.com/robinaxelsson)

---
Table of content
- [PGBSNH20 Group 5 Ludo Web](#pgbsnh20-group-5-ludo-web)
  - [Features](#features)
  - [Technologies](#technologies)
  - [Architecture](#architecture)
  - [Project: Ludo Game & Ludo Web](#project-ludo-game--ludo-web)
    - [Server Side Game Loop](#server-side-game-loop)
    - [A full loop across the network with SignalR](#a-full-loop-across-the-network-with-signalr)
    - [Custom Dependency Injection of Game](#custom-dependency-injection-of-game)
  - [Project: LudoDataAccess; EmailClient](#project-ludodataaccess-emailclient)
    - [Flow:](#flow)
  - [Project: LudoTranslation](#project-ludotranslation)
    - [Flow:](#flow-1)
    - [Why this way?](#why-this-way)

---
## Features

- Game rooms
- Multiplayer
- AI players
- Email invitations
- SQL database with users, gamedata
- Token and cookie authentication
- Server Side Logic

## Technologies

- ASP.NET
- Razor Pages
- C#
- SignalR
- SQL-Server
- Entity Framework
- Git version control
- GitHub
- JavaScript
- Visual Studio
- SMTP client

---

## Architecture

With SOLID principles in mind we keep the libraries (LudoGame, LudoTranslation) as libraries - independent of the main projects.

- LudoGame: is the logic and the factories for the game objects.
- LudoDataAccess: Manages the data layer with users and game data.
- LudoTranslation: Manages the language translations in email and web interface.
- LudoWeb: Manages the frontend (Razor pages and JavaScript), the networking of the game and the game rooms (and more).

![image](https://user-images.githubusercontent.com/63591629/120039244-dae86e80-c004-11eb-8ce8-41c00366694f.png)

---
## Project: Ludo Game & Ludo Web

All the game logic is calculated on the server (to eliminate cheating).

### Server Side Game Loop

```csharp

    foreach (var player in _orderedPlayers)
    {
    /*
    rolling dice
    check board for valid options to provide the player
    waits for player response
    validates the response
    if valid:
        update the board in memory
        send updated pawns through SignalR
    */
    }
}
```

### A full loop across the network with SignalR

This image describes the loop with both a human player connected over the network, but also our AI (called Stefan).

![image](https://user-images.githubusercontent.com/63591629/120044458-2fdcb280-c00e-11eb-907d-2468e1eb94c2.png)

Links to classes in the diagram.

- [GameNetworkManager.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoWeb/GameClasses/GameNetworkManager.cs)
- [NetworkPlayer.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoWeb/GameClasses/NetworkPlayer.cs)
- [GameRoom.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoWeb/GameClasses/GameRoom.cs)
- [GamePlay.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoGame/GameEngine/Classes/GamePlay.cs)

---

### Custom Dependency Injection of Game

A good way to clear up dependencies is a good design for instantiating the Library classes. This is made through stand alone container injection of LudoGame in [LudoProvider.cs](https://github.com/PGBSNH20/ludo-v2-group-g5_albin-robin/blob/main/Source/LudoGame/GameEngine/Configuration/LudoProvider.cs).
This design creates an additional dependency injection system separate from ASP.NETs. The system adds a new container of game services for every created instance of the LudoProvider. This makes it possible to have multiple game rooms with different Ludo games at the same time.

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

---

## Project: LudoDataAccess; EmailClient

Generates a HTML Email body and sends it to clients using SMTP-protocol.
### Flow:
<br>
First of all we instantiate a new SMTP client using Google Server, since our host email is using Gmail.
<br>
<br>

![bild](https://user-images.githubusercontent.com/70197523/120022388-a36dc800-bfec-11eb-9681-1ffeb35c5d96.png)

<br>
We then wish to get a list of all the account objects connected to the recipients. This is because we want the e-mail to be formatted in the receivers preferred language using TranslationEngine.<br><br>

![bild](https://user-images.githubusercontent.com/70197523/120022562-dc0da180-bfec-11eb-89fb-f682671b22ba.png)

<br>
We are also sending the emails separately using a for-loop. This is of two reasons:
<ol>
  <li>We don't want to leak any data to the recipients. Like other receivers.</li>
  <li>We want the e-mail to be translated individually based on the recipients preferred language</li>
  </ol>
Also, notice the email body is set using the method GenerateBody(). This method basically imports our HTML-body and then alters it for the user using simple string.replace.
<br><br>

![bild](https://user-images.githubusercontent.com/70197523/120022760-25f68780-bfed-11eb-9bf4-c82388b3df2e.png)

---

## Project: LudoTranslation
Alters strings using reflection and source files
### Flow:
<br>

![bild](https://user-images.githubusercontent.com/70197523/120022007-19256400-bfec-11eb-8e0e-2bfc95136832.png)
<br>
<br>
First of all we have .lang files which contains lines of the respective translation and it's correspondent property name. The content is split using double equal signs.
<br>
Example: `Game_H1Title==Välkommen till Ludo!`
<br>

![bild](https://user-images.githubusercontent.com/70197523/120020039-7f5cb780-bfe9-11eb-85d3-92bcf1631f6f.png)
<br><br>
Now on the InitializeLanguage() method will firstly make a new instance of the class Dict that contains the properties.
Secondly, a StreamReader will reach each line in the .lang file and find a property with the same name as the left section in that new instance of Dict. If it finds such property it will set that value to it's correspondant left section. Then this process is repeated until the entire file has been read.<br><br>
![bild](https://user-images.githubusercontent.com/70197523/120020686-58eb4c00-bfea-11eb-91c9-5982bced1e3f.png)


As a final result you will get back the newly instanced Dict class and can access all the properties with it's values.<br>
Then just set the chosen content value to any property of Dict.<br><br>

### Why this way?
<br>
We chose to structure it this way since it is very simple to extend the translation afterwards.<br>
If we want to add a completely new string, the only thing we gotta do is to create that property in Dict.cs and then add that property and it's value to the language file.
<br>
If we instead wish to add another language we just need to name the file a 2-character ISO standard country name(SE, US, RU...) and then .lang.<br>
In that file just copy over the properties and set the desired values.<br><br>

**Example load:**
<br><br>
![bild](https://user-images.githubusercontent.com/70197523/120021278-242bc480-bfeb-11eb-96f1-60fadae121d9.png)
<br>
With reflection it becomes very simple.
