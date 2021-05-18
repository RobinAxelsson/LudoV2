# Database.txt
Account\
Contains information about player\
Example: \
AccountName, Password(Hashed), Wins, Losses, 


PlayerQueue\
Contains information about accounts currently on queue for a game.\
Example:\
AccountID, WinRatio, QueueStartTime\
* When 4 players with equal win ratio are in queue it will combine them into a new game and removing them
  from the queue table.\
  Priority are based on QueueStartTime


GameLobby\
Contains information about a game currently being created as part of a custom game
Example\
GameID, AccountID, IsAI, Color

* When the game is started, \
  the row will be truncated from this table and an active game is created in the Game database instead



General notes:\
Research additional tables.

#Endpoints.txt

GetAccount(Account ID)\
Returns information about an account from the database

GetAccounts(Search parameter)
Returns all accounts based on the search parameter\
Keeping string null will return all accounts, but possibility to use IE: *a to filter all accounts on A.

GetPlayerWinRatio(Account ID)\
Get the player win/losses and calculate the ratio in percent.

GetPlayersInBracket(int WinRatePercent)
Returns all players within a specified win-rate bracket.

GetPlayersInQueue\
Returns all players currently in queue

SendInvite(string email)
Sends a game invite to the player\
--This might actually put the player in a game lobby waiting for another player to join
  Leaving the lobby will render the game impossible to join and thus removing it from the list.

Investigation:
* Discuss possible endpoints regarding game logic. 
  It feels more natural to have them in raw code inside the game, 
  but to fulfill project requirements we might need to implement it as endpoints.


