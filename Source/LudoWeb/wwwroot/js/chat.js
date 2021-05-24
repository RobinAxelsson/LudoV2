"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start();

//Binding event button to Invoke server AddUser method
document.getElementById("userNameButton").addEventListener("click", function (event) {
    let user = document.getElementById("messageInput").value;
    connection.invoke("AddUser", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
//Server calls on user added
connection.on("ConfirmUser", function (username) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `You are added to the chat as: ${username}`;
});


//Binding event button to Invoke server SendMessage
document.getElementById("sendAllButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
//Server calls new message
connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user}: ${message}`;
});

//Binding createGroupButton to Invoke CreateGroup() on server
document.getElementById("createGroupButton").addEventListener("click", function (event) {
    var groupName = document.getElementById("messageInput").value;
    connection.invoke("CreateGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
//Server calls function to confirm user
connection.on("ConfirmGroup",
    function (groupName) {
        var li = document.createElement("li");
        document.getElementById("messagesList").appendChild(li);
        li.textContent = `Hey you created group: ${groupName}`;
    });

//Binding joinGroupButton to invoke JoinGroup() on server
document.getElementById("joinGroupButton").addEventListener("click", function (event) {
    var groupName = document.getElementById("messageInput").value;
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//server messages user to confirm join
connection.on("ConfirmJoin", function (groupName) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    document.getElementById("sendToMyGroupButton").value += " " + groupName;
    li.textContent = `You are now a part of group: ${groupName}!`;
});

//server messages the others in group that user joined
connection.on("NewJoin", function (user) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} joined our group!`;
});

//Binding sendToMyGroupButton to invoke SendMessageToMyGroup() on server
document.getElementById("sendToMyGroupButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToMyGroup", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
//server invokes messaging the group
connection.on("ReceiveGroupMessage", function (user, group, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user}(${group}): ${message}!`;
});