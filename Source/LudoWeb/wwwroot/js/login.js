var Translations = 
    [
        "Login_IncorrectCredentialsLabel"  
    ];
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
connection.start();
var mainLoopId = setInterval(function(){
 if(connection.connectionStarted) {
     cookieLoad();
     getTranslations(Translations);
     clearInterval(mainLoopId);
 }
}, 40);
//Send cookie request
    async function cookieLoad() {
        if(getCookie("token") != null) {
            var loginCookie = getCookie("token");
            await connection.invoke("SendCookie", loginCookie);
        }
        else {
            document.getElementById("overlay").style.display = 'none';
        }
    }
//Cookie result
connection.on("CookieResult", function (result, message) {
    if(result === true) {
        redirect();
    }
    else {
        document.getElementById("overlay").style.display = 'none';
    }
});
    //Jump to game page
function redirect() {
    if(getCookie("lastUrl") != null) {
        window.location.replace(getCookie("lastUrl"));
    }
    else {
        var host = window.location.host;
        window.location.replace("https://" + host + "/GroupChat");
    }

}
//Send translation request
async function getTranslations(propertyNames) {
    var RegionCode = document.getElementById("RegionCode").innerHTML;
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
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}