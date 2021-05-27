"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
connection.start();

var diceRoll = 0;
var receivedPawns = [];

function rollDice() {
    //Html-element = diceRoll;

    if (receivedPawns.length === 0) {
        let pawns = [];
        //send pawns
    }
}

function receiveOption(playerOption) {

    receivedPawns = playerOption.PawnsToMove;
    diceRoll = playerOption.DiceRoll;
    let canTwo = playerOption.canTakeOutTwo;

    //rollDiceButton.onclick = display dice;

    for (var i = 0; i < pawns.Length; i++) {
        let id = '#' + current[i].X + ',' + current[i].Y;
        let gameSquare = select(id);
        gameSquare.onclick = selectPawnEvent(gameSquare);
    }
    if (canTwo === true) {
        //twoButton.onclick = takeOutTwoEvent();
    }
}

function noPlayMode() {
    //rolldicebutton.onclick = disable
    //twobutton.onclick = disable
    //activeSquares.onclick = disable
}

function takeOutTwoEvent() {
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
    //send
    noPlayMode();
}


function selectPawnEvent(gameSquare) {

    let colorInt = getColorEnum(gameSquare);
    let id = gameSquare.id;
    let X = id.split(',')[0];
    let Y = id.split(',')[1];
    let pawn = {
        "Color": colorInt,
        "X": X,
        "Y": Y
    }
    let pawns = [];
    pawns[0] = pawn;
    //send pawnArray
    noPlayMode();
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
function parseId(pawn) {
    return '#X' + pawn.X + 'Y' + pawn.Y;
}

var boardPawns = [];
//Incoming correct pawns from server
function RefreshPawns(inPawns) {
    let current = boardPawns;
    console.log(inPawns);

    //Removes pawns that moved
    for (var i = 0; i < current.length; i++) {

        let oldPawn = current[i];
        let isIncluded = isIncludedInArray(inPawns, oldPawn);
        if (isIncluded === false) {
            let id = parseId(oldPawn);
            let gameSquare = select(id);
            gameSquare.innerHTML = "";
        }
    }
    //Add pawns that moved
    
    for (var i = 0; i < inPawns.length; i++) {

        let pawn = inPawns[i];
        let isIncluded = isIncludedInArray(current, pawn);
        if (isIncluded === false) {
            let id = parseId(pawn);
            let gameSquare = select(id);
            let color = inPawns[i].Color;
            let pImg = createPawnImg(color);
            gameSquare.appendChild(pImg);
        }
    }

    boardPawns = inPawns;
}
function isIncludedInArray(pawnArray, pawnCompare) {

    for (var i = 0; i < pawnArray.length; i++) {
        let pawnJson = JSON.stringify(pawnArray[i]);
        let pawnCompareJson = JSON.stringify(pawnCompare);
        let isEqual = pawnJson == pawnCompareJson;
        if (isEqual) {
            return true;
        }
    }
    return false;
}
function createPawnImg(color) {

    let pawnImage = document.createElement("img");
    pawnImage.classList.add("pawn-image");
    console.log(getColorClass(color));
    pawnImage.classList.add(getColorClass(color));
    pawnImage.src = getSvgPath(color);
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

function getSvgPath(color) {
    console.log(color);
    //Blue
    if (color === 0) {
        return "images/Pawn.svg";
    }
    //Red
    if (color === 1) {
        return "images/Pawn.svg";
    }
    //Yellow
    if (color === 2) {
        return "images/Pawn.svg";
    }
    //Green
    if (color === 3) {
        return "images/Pawn.svg";
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
/*
 Incoming pawn array
 [
    {
        "Color": 0,
        "X": 4,
        "Y": 0
    },
    {
        "Color": 0,
        "X": 6,
        "Y": 0
    }
]

  [
    {
        Color: 0,
        X: 4,
        Y: 0
    },
    {
        Color: 0,
        X: 6,
        Y: 0
    }
]
{
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