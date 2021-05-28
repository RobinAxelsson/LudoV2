var Translations =
    [
        "Game_H1Title",
        "Game_CreateGameButton",
        "Game_AddPlayerButton",
        "Game_AddAiButton",
        "Game_StartGameButton",
        "Game_CouldNotFindTitle",
        "Game_InviteEnterEmail",
        "Game_PlayerJoinedSuffix"
        
        //"Enter the email of whom you wish to invite"
    ];
console.log(document.cookie);
/*
    document.getElementById("h1Title").innerHTML = Translations[0];
    document.getElementById("btn_createGame").innerHTML = Translations[1];
    document.getElementById("btn_addplayer").innerHTML = Translations[2];
    document.getElementById("btn_addai").innerHTML = Translations[3];
    document.getElementById("btn_startGame").innerHTML = Translations[4];
*/
"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
connection.start();
var GlobalPlayerName = "";
var mainLoopId = setInterval(function(){
    if(connection.connectionStarted) {
        BeginTranslation();
        CheckUrl();
        clearInterval(mainLoopId);
    }
}, 40);
var gameId = null;
function BeginTranslation() {
    let language = "";
    if(getCookie("lang") == null) {
        language = document.getElementById("DefaultLanguage").innerHTML;
    }
    else {
        language = getCookie("lang");
    }
    console.log(language);
    connection.invoke("RequestTranslation", Translations, language);
}
connection.on("TranslationDelivery", function(translationArray) {
    Translations = translationArray;
    Translate();
});

function Translate() {
    document.getElementById("h1Title").innerHTML = Translations[0];
    document.getElementById("btn_createGame").innerHTML = Translations[1];
    document.getElementById("btn_addplayer").innerHTML = Translations[2];
    document.getElementById("btn_addai").innerHTML = Translations[3];
    document.getElementById("btn_startGame").innerHTML = Translations[4];
    
}
function CheckUrl() {
    if(window.location.href.includes("gameId")) {
        console.log("has gameId in URL");
        gameId = window.location.href.split("=")[1]; //Format string to only parse gameId from URL
        console.log("Game ID: " + gameId);
        ValidateToken();
    }
    else if(window.location.href.includes("newGame")) {
        NewGame();
    }
}


connection.on("GameRoomAdded", function(returnGameId){
    gameId = returnGameId;
    console.log("GameRoomAdded: Game ID has been set to " + gameId);
    document.getElementById("welcomeContainer").style.display = 'none';
    document.getElementById("controlBox").style.display = 'unset';
    document.getElementById("playingField").style.display = 'unset';
    SendJoinNotification(false);
});
connection.on("PlayerNameReceived", function(playerName) {
    if(gameId == null) {
        GlobalPlayerName = playerName;
        console.log("Name received. gameId null Adding game room");
        AddNewGameRoom(null); 
    }
 else {
     console.log("Name received. gameId has value of: " + gameId + " checking if room exists");
        GlobalPlayerName = playerName;
        connection.invoke("RoomExists", gameId);
    }
});

//Press button
 function NewGame() {
     if(window.location.href.includes("gameId")) {
         window.location.replace("https://" + window.location.host + "/Ludo?newGame");
     }
    ValidateToken();
}
//Validate token
 function ValidateToken() {
        connection.invoke("ValidateToken", getCookie("token"));
}
//Token validation result
connection.on("TokenValidated", function(result) {
    console.log("Token result: " + result);
    if(result) {
        console.log("Result was true. Gameid: " + gameId)
        if(gameId != null) {
            console.log("Getting playername");
            connection.invoke("GetPlayerName");
        }
        else {
            console.log("Retrieving PlayerName");
            connection.invoke("GetPlayerName");
       
        }
    }
    else {
       RedirectToLogin(gameId);
    }
});
 connection.on("RoomExists", function(result) {
     if(result) {
         console.log("Invoking JoinRoom")
         //(string connectionId, string gameId, string playerName, string token)
         connection.invoke("JoinRoom", connection.connectionId, gameId, GlobalPlayerName, getCookie("token"));
     }
     else {
         document.getElementById("h1Title").innerHTML = Translations[5];
         console.log("Room was not found")
     }
 });
 connection.on("JoinedRoom", function(result, message) {
    console.log("Result: " + result);
    console.log("Message: " + message);
     document.getElementById("welcomeContainer").style.display = 'none';
     document.getElementById("controlBox").style.display = 'unset';
     document.getElementById("playingField").style.display = 'unset';
     document.getElementById("btn_addai").disabled = true;
     document.getElementById("btn_addplayer").disabled = true;
     document.getElementById("btn_startGame").disabled = true;
     SendJoinNotification(true);
     
 });
 function RedirectToLogin(gameId) {
     let lastUrl = "";
     if(window.location.href.includes("gameId")) {
         lastUrl = window.location.href;
     }
     else {
         lastUrl = window.location.href + "?gameId=" + gameId;
     }
     let now = new Date();
     let time = now.getTime();
     let expireTime = time + 1000*10800; //3 hours
     now.setTime(expireTime);
     document.cookie = "lastUrl=" + lastUrl + ";expiry=" + now.toUTCString() + ";sameSite=Lax;path=/";
     window.location.replace("https://" + window.location.host + "/Login");
 }
 //Add new gameroom
function AddNewGameRoom(gameId) {
     if(gameId == null) {
       console.log("Adding null gameId");  
     }
     //string connectionId, string gameId, string playerName, string token)
    connection.invoke("AddGameRoom", connection.connectionId, gameId, GlobalPlayerName, getCookie("token"));
}
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}
function InvitePlayer() {
    var person = prompt(Translations[6]);
    if (person != null) {
        //string recipient, string gameId, string gameUrl, string token
        let gameUrl = "";
        if(window.location.href.includes("gameId"))
            gameUrl = window.location.href;
        else
            gameUrl = "https://" + window.location.host + "/Ludo/?gameId=" + gameId;
        connection.invoke("InvitePlayer", person, gameId, gameUrl, getCookie("token"));
    }
}
connection.on("PlayerInvited", function() {
    console.log("Player invited!");
});
function AddAi() {
    connection.invoke("AddAi", gameId); 
}
connection.on("AiAdded", function() {
    SendJoinNotification(true);
});
//Get notifications of members who are in the room
function GetRoomMembers() {
     //string playerName, string gameId
     connection.invoke("RetrieveJoinNotifications", GlobalPlayerName, gameId);
}
//Send notification that you just joined
function SendJoinNotification(hasJoined) {
         connection.invoke("JoinGameMessage", GlobalPlayerName, gameId);
}
//Add name to list
connection.on("JoinGameMessage", function(playerName, clientArray) {
       document.getElementById("messagePlayer" + clientArray.toString()).innerHTML = playerName + Translations[7];
        GetRoomMembers();
});
 connection.on("RetrieveJoinNotifications", function(playerNames, playerIndex, totalLength) {
     for (let i = 0; i < playerIndex.length; i++) {
         document.getElementById("messagePlayer" + playerIndex[i].toString()).innerHTML = playerNames[i] + Translations[7];
     }
     if(totalLength >= 4) {
         document.getElementById("btn_addai").disabled = true;
         document.getElementById("btn_addplayer").disabled = true;
         document.getElementById("btn_startGame").disabled = false;
     }
 });
 function StartGame() { 
     SendMessage("Starting game! Woho!");
     connection.invoke("StartGame", gameId);
 }
 connection.on("GameStarted", function() {
     document.getElementById("btn_addai").style.display = 'none';
     document.getElementById("btn_addplayer").style.display = 'none';
     document.getElementById("btn_startGame").style.display = 'none';
     document.getElementById("messageBox").style.display = 'none';
 });
 function SendMessage(str) {
     if(str != null) {
         connection.invoke("SendRoomMessage", GlobalPlayerName, str, gameId).catch(function (err) {
             return console.error(err.toString());
         });
         document.getElementById("txt_message").value = "";
     }
    else {
         var message = document.getElementById("txt_message").value;
         connection.invoke("SendRoomMessage", GlobalPlayerName, message, gameId).catch(function (err) {
             return console.error(err.toString());
         });
         document.getElementById("txt_message").value = "";
      
     }
 }
connection.on("ReceiveGameMessage", function (playerName, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${playerName}: ${message}`;
});
document.getElementById("txt_message").addEventListener("keyup", function(event) {
    if (event.key.toLowerCase() === "enter" && !isEmptyOrSpaces(document.getElementById("txt_message").value)) {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
       
        SendMessage();
    }
});
function isEmptyOrSpaces(str){
    return str === null || str.match(/^ *$/) !== null;
}
function debug_GetAllRooms() {
   connection.invoke("GetAllRooms");  
}
connection.on("AllRooms", function(roomArray) {
 console.log(roomArray);
});
 function debug_SendRoomMessage(str) {
     connection.invoke("SendRoomMessage", str, gameId);
 }
 connection.on("GameMessage", function(message){
     console.log(message);
 });
 