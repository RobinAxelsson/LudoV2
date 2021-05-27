"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
connection.start();

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


connection.on("GameRoomAdded", function(){
    console.log("Added game room!");
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
connection.on("TokenValidated", function(result, gameId) {
    console.log("Token result: " + result);
    if(result) {
        AddNewGameRoom(gameId);
    }
    else {
       RedirectToLogin(gameId);
    }
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
    connection.invoke("AddGameRoom", gameId, getCookie("token"));
}
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}