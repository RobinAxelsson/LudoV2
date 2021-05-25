var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
connection.start();

    function login() {
        var accountName = document.getElementById("txt_account").value;
        var password = document.getElementById("txt_password").value;
        connection.invoke("SendLoginData", accountName, password);
    }
connection.on("LoginResult", function (result, message) {
    console.log(result + " " + message);
});