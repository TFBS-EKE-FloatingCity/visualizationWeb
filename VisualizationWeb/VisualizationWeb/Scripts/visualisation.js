var updateRate, wsData, cubeRotationZ, cubeRotationX, heightA, heightB, heightC, heightHY, heightFactor;

//var heightData = {
//    heightA,
//    heightB,
//    heightC,
//    heightHY
//}

updateRate = 10000;
heightFactor = 1;

function connect() {
    var host = 'ws://localhost:8109/Connection';
    var socket = new WebSocket(host);

    socket.onmessage = function (e) {
        console.log(e.data);
        wsData = JSON.parse(e.data);    // has to be parsed?!

        heightData = updateModelRotation(wsData.USonicOuter1, wsData.USonicOuter2, wsData.USonicOuter3, heightFactor);

        heightA = heightData.heightA;
        heightB = heightData.heightB;
        heightC = heightData.heightC;
        heightHY = heightData.heightHY;
    };

    socket.onclose = function (e) {
        setTimeout(function () {
            connect();
        }, updateRate);
    };
}
//connect();

// TESTDATA
testData = 
{
	"UUID": 3,
	"USonicInner1": 399,
	"USonicOuter1": 334,
	"Pump1": -39,
	"USonicInner2": 163,
	"USonicOuter2": 309,
	"Pump2": -81,
	"USonicInner3": 214,
	"USonicOuter3": 170,
	"Pump3": -61,
	"CreatedAt": "2021-01-04T14:38:15.618",
	"MesurementTime": "2021-01-04T13:38:15.589",
	"SimulationID": 2,
	"WindMax": 0,
	"WindCurrent": 0,
	"SunMax": 0,
	"SunCurrent": 0,
	"ConsumptionMax": 0,
	"ConsumptionCurrent": 0,
	"SimulationActive": false,
	"Simulationtime": null,
	"TimeFactor": null
};

wsData = testData;

setInterval(function () {
    wsData.Pump1 = randomNumber(-100, 100);
    wsData.Pump2 = randomNumber(-100, 100);
    wsData.Pump3 = randomNumber(-100, 100);
    wsData.WindCurrent = randomNumber(0, 50);
    wsData.SunCurrent = randomNumber(0, 50);
    wsData.ConsumptionCurrent = randomNumber(0, 50);
    wsData.USonicOuter1 = randomNumber(150, 400);
    wsData.USonicOuter2 = randomNumber(150, 400);
    wsData.USonicOuter3 = randomNumber(150, 400);

    heightData = updateModelRotation(wsData.USonicOuter1, wsData.USonicOuter2, wsData.USonicOuter3);

    heightA = heightData.heightA;
    heightB = heightData.heightB;
    heightC = heightData.heightC;
    heightHY = heightData.heightHY;

}, updateRate);

function randomNumber(min, max) {
    if (min > max) {
        let temp = max;
        max = min;
        min = temp;
    }

    if (min <= 0) {
        return Math.floor(Math.random() * (max + Math.abs(min) + 1)) + min;
    } else {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }
}
// END TEST

function updateModelRotation(USonicOuter1, USonicOuter2, USonicOuter3, heightFactorValue) {
    if (heightFactorValue > 0) {
        USonicOuter1 *= heightFactorValue;
        USonicOuter2 *= heightFactorValue;
        USonicOuter3 *= heightFactorValue;
    }

    var radiant;

    var cubeLengths = {
        width: 2,
        height: .5,
        depth: 2,
    };

    if (USonicOuter1 === USonicOuter2) {
        // wsData a and b are even
        cubeRotationZ = +0.0;
    }

    if (USonicOuter1 === USonicOuter3) {
        // wsData a and c are even
        cubeRotationX = +0.0;
    }

    if (USonicOuter2 > USonicOuter1) {
        // B > A
        radiant = (-1) * Math.atan((USonicOuter2 - USonicOuter1) / cubeLengths.width);
        cubeRotationZ = radiant;
    }

    if (USonicOuter1 > USonicOuter2) {
        // A > B
        radiant = Math.atan((USonicOuter1 - USonicOuter2) / cubeLengths.width);
        cubeRotationZ = radiant;
    }

    if (USonicOuter1 > USonicOuter3) {
        // A > C
        radiant = (-1) * Math.atan((USonicOuter1 - USonicOuter3) / cubeLengths.width);
        cubeRotationX = radiant;
    }

    if (USonicOuter3 > USonicOuter1) {
        // C > A
        radiant = Math.atan((USonicOuter3 - USonicOuter1) / cubeLengths.width);
        cubeRotationX = radiant;
    }

    heightData = {
        heightA: USonicOuter1 * Math.cos(Math.abs(cubeRotationZ)),
        heightB: USonicOuter2 * Math.cos(Math.abs(cubeRotationZ)),
        heightC: USonicOuter3 * Math.cos(Math.abs(cubeRotationX)),
        heightHY: 0
    };

    heightData.heightHY = Math.round((((heightData.heightA + heightData.heightB + heightData.heightC) / 3) + Number.EPSILON) * 100) / 100;

    return heightData;
}