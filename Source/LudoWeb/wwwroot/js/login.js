var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
connection.start();
var mainLoopId = setInterval(function(){
    console.log("incrementing");
 if(connection.connectionStarted) {
     console.log("started!");
     cookieLoad();
     clearInterval(mainLoopId);
 }
}, 40);
    async function cookieLoad() {
        var loginCookie = document.cookie;
        await connection.invoke("SendCookie", loginCookie);
    }
    async function login() {
        if (connection.connectionStarted) {
            console.log("yea!");
        }
        var accountName = document.getElementById("txt_account").value;
        var password = document.getElementById("txt_password").value;
        await connection.invoke("SendLoginData", accountName, password);
    }
    
 connection.on("LoginResult", function (result, message) {
    if(result === false) {
        document.getElementById("err_input").style.display = 'unset';
        document.getElementById("err_input").innerHTML = "Incorrect account name or password";
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