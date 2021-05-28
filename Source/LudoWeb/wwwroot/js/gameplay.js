"use strict";


var optionPawns = [];
var diceRoll = null;
var boardPawns = [];
var canTakeOutTwo = false;
var selectedSquare = null;
var selectedPawn = null;

select("#btn_rollDice").disabled = true;
select("#btn_takeOutTwo").disabled = true;
select("#btn_moveSelected").disabled = true;

function resetOnSend() {
    canTakeOutTwo = false;
    if (selectedSquare !== null) {
        selectedSquare.style.borderColor = "black";
        selectedSquare.style.borderWidth = "1px";
    }
    selectedSquare = null;
    selectedPawn = null;
    console.log("Resetting optionPawns!!");
    optionPawns = [];
    diceRoll = null;
    select("#btn_rollDice").disabled = true;
    select("#btn_takeOutTwo").disabled = true;
    select("#btn_moveSelected").disabled = true;
}


//SignalR input to client
connection.on("ReceiveOption", function (returnPawnsToMove, returnCanTakeOutTwo, returnDiceRoll) {
    console.log("Received from networkmanager:")
    console.log("ReceiveOptionValues: " + returnCanTakeOutTwo.toString() + returnPawnsToMove.toString());
    optionPawns = returnPawnsToMove;
    console.log("optionPawns was set to: " + optionPawns);
    diceRoll = returnDiceRoll;
    canTakeOutTwo = returnCanTakeOutTwo;
    select("#btn_rollDice").disabled = false;
});
//SignalR update pawns
connection.on("UpdatePawns",
    function (inPawns) {

        //remove all
        for (var i = 0; i < boardPawns.length; i++) {
            let pImg = select("#pawn" + boardPawns[i].id);
            pImg.parentNode.removeChild(pImg);
        }

        //add all
        for (var i = 0; i < inPawns.length; i++) {

            let gameSquareId = parseGameSquareId(inPawns[i]);
            let gameSquare = select(gameSquareId);
            let color = inPawns[i].color;
            let pImg = createPawnImg(color);
            pImg.id = "pawn" + inPawns[i].id;
            let imgPawn = inPawns[i];
            pImg.onclick = function () { selectedPawn = imgPawn; }
            gameSquare.appendChild(pImg);
        }
        boardPawns = inPawns;
    });
//Button events (SignalR output)
function rollDice() {
    select("#btn_rollDice").disabled = true;
    console.log("You pressed roll dice button.");
    if (diceRoll != null) {
        console.log(diceRoll);
        SendRolledMessage();
    }
    if (optionPawns == null || optionPawns.length === 0) {
        console.log("You will pass.");
        resetOnSend();
        let sendPawns = [];
        sendPawnArray(sendPawns);
        return;
    }
    if (canTakeOutTwo === true) {
        select("#btn_takeOutTwo").disabled = false;
    }
    select("#btn_moveSelected").disabled = false;
}
function SendRolledMessage() {
    connection.invoke("SendRolledMessage", GlobalPlayerName, " rolled ", diceRoll, gameId).catch(function (err) {
        return console.error(err.toString());
    });
}
connection.on("ReceiveRolledMessage", function (playerName, rollMessage, diceRoll) {
    let listitem = document.createElement("li");
    listitem.textContent = playerName + rollMessage + diceRoll;
    document.getElementById("messagesList").appendChild(listitem);
});
function sendPawnSelection() {
    console.log("You pressed send pawn button.");
    console.log("About to send: " + selectedPawn);
    if (selectedPawn !== null) {
        let sendPawns = [];
        sendPawns[0] = selectedPawn;
        sendPawnArray(sendPawns);
        resetOnSend();
    }
}
function parseGameSquareId(pawn) {
    return '#x' + pawn.x + 'y' + pawn.y;
}
function sendTakeOutTwoSelection() {
    console.log("You pressed take out two button.");
    if (canTakeOutTwo === true) {
        let pawns = [];
        let pawn1 = {
            "id": 999,
            "color": 0,
            "x": 0,
            "y": 0
        }
        let pawn2 = {
            "id": 998,
            "color": 0,
            "x": 0,
            "y": 0
        }
        pawns[0] = pawn1;
        pawns[1] = pawn2;
        let sendPawns = [];
        sendPawns[0] = pawn1;
        sendPawns[1] = pawn2;
        console.log("Sending pawn array");
        sendPawnArray(sendPawns);
        resetOnSend();
    }
    canTakeOutTwo = false;
}
//Send SignalR
function sendPawnArray(pawns) {
    console.log("about to invoke ReceivePawns from javascript")
    console.log("this is the pawns" + pawns);
    connection.invoke("ReceivePawns", pawns).catch(function (err) {
        return console.error(err.toString());
    });
}
//onclick squares
function selectSquareAndPawn(square) {

    if (selectedSquare !== null) {
        selectedSquare.style.borderColor = "black";
        selectedSquare.style.borderWidth = "1px";
        selectedSquare = null;
    }
    if (selectedPawn !== null) {
        selectedPawn = null;
    }
    if (optionPawns.length === 0) {
        return;
    }
    if (square.hasChildNodes() === false) {
        return;
    }
    let optionPawn = validatePawnFromSquare(square);
    if (optionPawn !== null) {
        selectedSquare = square;
        selectedPawn = optionPawn;
        console.log("you selected: " + selectedPawn);
        selectedSquare.style.borderColor = "cyan";
        selectedSquare.style.borderWidth = "4px";
        selectedSquare = square;
    }
}
function validatePawnFromSquare(square) {
    let clickedPawn = getClickedPawn(square);
    if (pawnIsOption(clickedPawn)) {
        return clickedPawn;
    }
    return null;
}
function pawnIsOption(pawn) {

    for (var i = 0; i < optionPawns.length; i++) {
        if (pawn.id === optionPawns[i].id) {
            return true;
        }
    }
}
function getClickedPawn(square) {
    let imgId = square.firstElementChild.id;
    let pawnId = imgId.split("n")[1];
    for (var i = 0; i < boardPawns.length; i++) {
        if (boardPawns[i].id.toString() === pawnId) {
            return boardPawns[i];
        }
    }
}
//if statements
//Helper functions
function createPawnImg(color) {

    let pawnImage = document.createElement("img");
    pawnImage.classList.add("pawn-image");
    pawnImage.classList.add(getColorClass(color));
    pawnImage.src = getPawnImagePath(color);
    return pawnImage;
}
function getColorClass(color) {
    //Blue
    if (color === 0) {
        return "blue-pawn";
    }
    //Red
    if (color === 1) {
        return "red-pawn";
    }
    //Yellow
    if (color === 2) {
        return "yellow-pawn";
    }
    //Green
    if (color === 3) {
        return "green-pawn";
    }
}
function getPawnImagePath(color) {

    //Blue
    if (color === 0) {
        return "images/Pawns/blue_64.png";
    }
    //Red
    if (color === 1) {
        return "images/Pawns/red_64.png";
    }
    //Yellow
    if (color === 2) {
        return "images/Pawns/yellow_64.png";
    }
    //Green
    if (color === 3) {
        return "images/Pawns/green_64.png";
    }
}
function getColorEnum(gameSquare) {
    let pawnImage = gameSquare.firstChild;

    if (pawnImage.classList.contains("blue-pawn")) {
        return 0;
    }
    if (pawnImage.classList.contains("red-pawn")) {
        return 1;
    }
    if (pawnImage.classList.contains("yellow-pawn")) {
        return 2;
    }
    if (pawnImage.classList.contains("green-pawn")) {
        return 3;
    }
}

function select(element) {
    if (document.querySelector(element) !== null) {
        return document.querySelector(element);
    } else {
        console.error('select(element): ' + element + ' is not found!');
        return null;
    }
}