var Translations = 
    [
        "Register_EnterValidEmailLabel", 
        "Register_SelectLanguageLabel", 
        "Register_PasswordStrengthLengthLabel",
        "Register_PasswordStrengthUppercaseLabel",
        "Register_PasswordMatchLabel",
        "Register_PasswordMatchOkLabel",
        "Register_AccountNameLengthLabel",
        "Register_AccountNameSpecialsLabel"
    ];
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
connection.start();
var mainLoopId = setInterval(function(){
    console.log("incrementing");
    if(connection.connectionStarted) {
        console.log("started!");
        getTranslations(Translations);
        clearInterval(mainLoopId);
        //Begin translations
       
    }
}, 40);
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


document.addEventListener("DOMContentLoaded", function() {
    setColor("rgb(255, 106, 106)");
});
function setColor(str) {
    document.getElementById("err_accountname").style.color = str;
    document.getElementById("err_password_match").style.color = str;
    document.getElementById("err_password_strength").style.color = str;
    document.getElementById("err_invalid_email").style.color = str;
    document.getElementById("err_language_select").style.color = str;
}
var sel = document.getElementById('selectbox');
    function register() {
        var password = document.getElementById("txt_password").value;
        var retype = document.getElementById("txt_retypepassword").value;
        var accountName = document.getElementById("txt_account").value;
        var email = document.getElementById("txt_email").value;
        var preferredLanguage = sel.options[sel.selectedIndex].text;
        console.log(preferredLanguage);
        checkAccount();
        checkPasswords();
        if(!email.includes("@") || !email.split("@")[1].includes(".") || email.endsWith(".")) {
            document.getElementById("err_invalid_email").style.display = 'unset';
            document.getElementById("err_invalid_email").innerHTML = Translations[0];
        }
        if(preferredLanguage.includes(" ")) {
            document.getElementById("err_language_select").style.display = 'unset';
            document.getElementById("err_language_select").innerHTML = Translations[1];
        }
        
        if(password === retype && password.length >= 8 && email.includes("@") && email.split("@")[1].includes(".") && !preferredLanguage.includes(" ") && accountNameValid(accountName) && !isEmptyOrSpaces(accountName)) {
            connection.invoke("SendRegistrationData", accountName, email, password, preferredLanguage);
        }
    else {
        console.log("something is faulty");
        }
    }
    function checkPasswords() {
        var password = document.getElementById("txt_password").value;
        var retype = document.getElementById("txt_retypepassword").value;
        if(password.length < 8) {
            document.getElementById("err_password_strength").style.display = 'unset';
            document.getElementById("err_password_strength").innerHTML = Translations[2];
        }
        else if(!hasUppercase(password)) {
            document.getElementById("err_password_strength").style.display = 'unset';
            document.getElementById("err_password_strength").innerHTML = Translations[3];
        }
        else {
            document.getElementById("err_password_strength").style.display = 'none';
        }
        if(!isEmptyOrSpaces(retype)) {
            if(password !== retype) {
                document.getElementById("err_password_match").style.color = "rgb(255, 106, 106)";
                document.getElementById("err_password_match").style.display = 'unset';
                document.getElementById("err_password_match").innerHTML = Translations[4];
                
            }
            else {
                document.getElementById("err_password_match").style.display = 'unset';
                document.getElementById("err_password_match").style.color = "rgb(124, 252, 107)";
                document.getElementById("err_password_match").innerHTML = Translations[5];
            }
        }
    }
    function checkAccount() {
        var accountName = document.getElementById("txt_account").value;
        if(accountName.length <= 4) {
            document.getElementById("err_accountname").style.display = 'unset';
            document.getElementById("err_accountname").innerHTML = Translations[6];
        }
        else if(!accountNameValid(accountName)) {
            document.getElementById("err_accountname").style.display = 'unset';
            document.getElementById("err_accountname").innerHTML = Translations[7];
        }
        else {
            document.getElementById("err_accountname").style.display = 'none';
        }
    }
    function checkEmail() {
        var email = document.getElementById("txt_email").value;
        if(email.includes("@") && email.split("@")[1].includes(".") && !email.endsWith(".")) {
            document.getElementById("err_invalid_email").style.display = 'none';
        }
    }
    function languageSelected() {
        document.getElementById("err_language_select").style.display = 'none';
    }
function hasUppercase(str) {
    var result = false;
    for (i = 0; i <str.length; i++) {
        var letter = str[i];
        if(letter.match(/[a-z]/i)) {
            if(letter === letter.toUpperCase()) {
                result = true;
            }
        }
       
    }
    return result;
}
function accountNameValid(str) {
        return str === null || str.match(/^[a-z0-9]+$/i) !== null;
}



function isEmptyOrSpaces(str){
    return str === null || str.match(/^ *$/) !== null;
}
connection.on("RegistrationResult", function (result, message) {
    console.log(result + " " + message);
});
