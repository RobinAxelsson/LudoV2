﻿var Translations = 
    [
        "Login_IncorrectCredentialsLabel"  
    ];
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
connection.start();
var mainLoopId = setInterval(function(){
    console.log("incrementing");
 if(connection.connectionStarted) {
     console.log("started!");
     cookieLoad();
     getTranslations(Translations);
     clearInterval(mainLoopId);
 }
}, 40);
    async function cookieLoad() {
        var loginCookie = document.cookie;
        await connection.invoke("SendCookie", loginCookie);
    }
//Send translation request
async function getTranslations(propertyNames) {
    var RegionCode = document.getElementById("RegionCode").innerHTML;
    console.log(RegionCode);
    await connection.invoke("RequestTranslation", propertyNames, RegionCode);
}

//Receive translations
connection.on("TranslationDelivery", function (properties) {
    Translations = properties;
});
    async function login() {
        var accountName = document.getElementById("txt_account").value;
        var password = document.getElementById("txt_password").value;
        await connection.invoke("SendLoginData", accountName, password);
    }
    
 connection.on("LoginResult", function (result, message) {
    if(result === false) {
        document.getElementById("err_input").style.display = 'unset';
        document.getElementById("err_input").innerHTML = Translations[0];
    }
    else {
        document.cookie = message;
        redirect();
    }
});
    connection.on("CookieResult", function (result, message) {
      
   if(result === true) {
      redirect();
   }
   else {
       document.getElementById("overlay").style.display = 'none';
   }

});
function redirect() {
    var host = window.location.host;
    console.log("https://" + host + "/GroupChat");
    window.location.replace("https://" + host + "/GroupChat");
}
document.getElementById("txt_password").addEventListener("keyup", function(event) {
    if (event.key.toLowerCase() === "enter") {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
        document.getElementById("btn_login").click();
    }
});
document.getElementById("txt_account").addEventListener("keyup", function(event) {
    if (event.key.toLowerCase() === "enter") {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
        document.getElementById("btn_login").click();
    }
});