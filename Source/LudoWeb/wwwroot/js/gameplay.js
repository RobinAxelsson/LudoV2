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

    for (var i = 0; i < receivedPawns.Length; i++) {
        let id = parseGameSquareId(receivedPawns[i]);
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

//TODO change id split, trim.
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
function parseGameSquareId(pawn) {
    return '#X' + pawn.X + 'Y' + pawn.Y;
}

var boardPawns = [];
//Incoming correct pawns from server
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

    for (var i = 0; i < inPawns.length; i++) {

        let newPawn = inPawns[i];
        
        if (hasPawnMoved(currentPawns, newPawn) === true) {
            let gameSquareId = parseGameSquareId(newPawn);
            let gameSquare = select(gameSquareId);
            let color = inPawns[i].Color;
            let pImg = createPawnImg(color);
            pImg.id = "pawn" + newPawn.Id;
            gameSquare.appendChild(pImg);
        }
    }

    boardPawns = inPawns;
}
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

var str = '<a href="http://www.com">item to replace</a>'; //it can be anything
var Obj = document.getElementById('TargetObject'); //any element to be fully replaced
if (Obj.outerHTML) { //if outerHTML is supported
    Obj.outerHTML = str; ///it's simple replacement of whole element with contents of str var
}
else { //if outerHTML is not supported, there is a weird but crossbrowsered trick
    var tmpObj = document.createElement("div");
    tmpObj.innerHTML = '<!--THIS DATA SHOULD BE REPLACED-->';
    ObjParent = Obj.parentNode; //Okey, element should be parented
    ObjParent.replaceChild(tmpObj, Obj); //here we placing our temporary data instead of our target, so we can find it then and replace it into whatever we want to replace to
    ObjParent.innerHTML = ObjParent.innerHTML.replace('<div><!--THIS DATA SHOULD BE REPLACED--></div>', str);
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