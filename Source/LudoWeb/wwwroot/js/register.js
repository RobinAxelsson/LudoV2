var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
connection.start();
var sel = document.getElementById('selectbox');
    function register() {
        var accountName = document.getElementById("txt_account").value;
        var email = document.getElementById("txt_email").value;
        var password = document.getElementById("txt_password").value;
        var preferredLanguage = sel.options[sel.selectedIndex].text;
        connection.invoke("SendRegistrationData", accountName, email, password, preferredLanguage);
    }
connection.on("RegistrationResult", function (result, message) {
    console.log(result + " " + message);
});