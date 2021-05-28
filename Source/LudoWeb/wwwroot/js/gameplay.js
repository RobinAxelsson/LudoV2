"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
connection.start();

//global variables
var diceRoll = null;
const boardPawns = [];
const optionPawns = [];
var canTakeOutTwo = null;
var selectedSquare = null;
var selectedPawn = null;

//SignalR input
function receiveOption(playerOption) {

    optionPawns = playerOption.PawnsToMove;
    diceRoll = playerOption.DiceRoll;
    canTakeOutTwo = playerOption.canTakeOutTwo;
} 
function RefreshPawns(inPawns) {
    let currentPawns = boardPawns;
    console.log(inPawns);

    //Removes pawns that moved
    for (var i = 0; i < currentPawns.length; i++) {
        let oldPawn = currentPawns[i];
        if (isPawnEradicated(inPawns, oldPawn) || hasPawnMoved(inPawns, oldPawn)) {
            let pImg = select("#pawn" + oldPawn.Id);
            pImg.parentNode.removeChild(pImg);
        }
    }

    //Add new pawns
    for (var i = 0; i < inPawns.length; i++) {

        let newPawn = inPawns[i];

        if (hasPawnMoved(currentPawns, newPawn) === true) {
            let gameSquareId = parseGameSquareId(newPawn);
            let gameSquare = select(gameSquareId);
            let color = inPawns[i].Color;
            let pImg = createPawnImg(color);
            pImg.id = "pawn" + newPawn.Id;
            pImg.onclick = function () { selectedPawn = newPawn; }
            pImg.onclick = function () { gameSquare }
            gameSquare.appendChild(pImg);
        }
    }

    boardPawns = inPawns;
}

//Button events (SignalR output)
function rollDice() {
    if (diceRoll !== null) {
        console.log(diceRoll);
    }
    if (receivedPawns.length === 0) {
        let pawns = [];
        //send empty pawn array
    }
}
function sendPawnSelection() {

}
function sendTakeOutTwoSelection() {
    if (canTakeOutTwo === true) {
        let pawns = [];
        let pawn1 = {
            "Color": 0,
            "X": 0,
            "Y": 0
        }
        let pawn2 = {
            "Color": 0,
            "X": 0,
            "Y": 0
        }
        pawns[0] = pawn1;
        pawns[1] = pawn2;
        //Send
    }
    canTakeOutTwo = null;
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
    let choosePawn = pawnsToChoose(square);
    if (choosePawn !== null) {
        selectedSquare = square;
        selectedPawn = choosePawn;
        selectedSquare.style.borderColor = "cyan";
        selectedSquare.style.borderWidth = "4px";
        selectedSquare = square;
    }
}
function pawnsToChoose(square) {
    let imgId = square.firstElementChild.id;
    let pawnId = imgId.split("n")[1];
    for (var i = 0; i < optionPawns.length; i++) {
        let choosePawn = optionPawns[i];
        let choosePawnId = choosePawn.Id.toString();
        if (pawnId === choosePawnId) {
            return choosePawn;
        }
    }
    return null;
}

//if statements
function hasPawnMoved(inPawns, oldPawn) {
    for (var i = 0; i < inPawns; i++) {
        let inPawn = inPawns[i];
        if (inPawn.Id === oldPawn.Id && inPawn.X === oldPawn.X && inPawn.Y === oldPawn.Y) {
            return false;
        }
    }
    return true;
}
function isPawnEradicated(inPawns, oldPawn) {

    for (var i = 0; i < inPawns; i++) {
        let inPawn = inPawns[i];
        if (inPawn.Id === oldPawn.Id) {
            return false;
        }
    }
    return true;
}
//Helper functions
function createPawnImg(color) {

    let pawnImage = document.createElement("img");
    pawnImage.classList.add("pawn-image");
    console.log(getColorClass(color));
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
    console.log(color);
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
function parseGameSquareId(pawn) {
    return '#X' + pawn.X + 'Y' + pawn.Y;
}
function select(element) {
    if (document.querySelector(element) !== null) {
        return document.querySelector(element);
    } else {
        console.error('select(element): ' + element + ' is not found!');
        return null;
    }
}
/*
    "PawnsToMove": [
            {
                "Color": 0,
                "X": 4,
                "Y": 0
            },
            {
                "Color": 0,
                "X": 5,
                "Y": 0
            }
        ],
        "CanTakeOutTwo": true,
        "DiceRoll": 4
}*/