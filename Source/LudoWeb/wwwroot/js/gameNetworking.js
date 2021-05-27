"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
connection.start();
var GlobalPlayerName = "";
var mainLoopId = setInterval(function(){
    if(connection.connectionStarted) {
        CheckUrl();
        clearInterval(mainLoopId);
    }
}, 40);
var gameId = null;
function CheckUrl() {
    if(window.location.href.includes("gameId")) {
        console.log("has gameId in URL");
        gameId = window.location.href.split("=")[1]; //Format string to only parse gameId from URL
        console.log("Game ID: " + gameId);
        ValidateToken();
    }
}


connection.on("GameRoomAdded", function(returnGameId, playerName){
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
    ValidateToken();
}
//Validate token
 function ValidateToken() {
        connection.invoke("ValidateToken", getCookie("token"));
}
//Token validation result
connection.on("TokenValidated", function(result, returnGameId) {
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
         console.log("Room was not found")
     }
 });
 connection.on("JoinedRoom", function(result, message) {
    console.log("Result: " + result);
    console.log("Message: " + message);
     document.getElementById("welcomeContainer").style.display = 'none';
     document.getElementById("controlBox").style.display = 'unset';
     document.getElementById("playingField").style.display = 'unset';
     SendJoinNotification(true);
     
 });
 function RedirectToLogin(gameId) {
     let lastUrl = window.location.href + "?gameId=" + gameId;
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
    var person = prompt("Enter the email of whom you wish to invite", "Harry Potter");
    if (person != null) {
        //string recipient, string gameId, string gameUrl, string token
        let gameUrl = "";
        if(window.location.href.includes("gameId"))
            gameUrl = window.location.href;
        else
            gameUrl = window.location.href + "?gameId=" + gameId;
        connection.invoke("InvitePlayer", person, gameId, gameUrl, getCookie("token"));
    }
    connection.on("PlayerInvited", function() {
       console.log("Player invited!"); 
    });
}
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
       document.getElementById("messagePlayer" + clientArray.toString()).innerHTML = playerName + " has joined";
        GetRoomMembers();
});
 connection.on("RetrieveJoinNotifications", function(playerNames, playerIndex) {
     for (let i = 0; i <playerIndex.length; i++) {
         document.getElementById("messagePlayer" + playerIndex[i].toString()).innerHTML = playerNames[i] + " has joined";
     }
 });
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
 