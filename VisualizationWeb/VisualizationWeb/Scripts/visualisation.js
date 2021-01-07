var updateRate, wsData, cubeRotationZ, cubeRotationX, heightA, heightB, heightC, heightHY;

updateRate = 1000;

function connect() {
    var host = 'ws://localhost:8109/Connection';
    var socket = new WebSocket(host);

    socket.onmessage = function (e) {
        console.log(e.data);
        wsData = JSON.parse(e.data);    // has to be parsed?!
        updateModelRotation();
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
	"CityDataID": 3,
	"USonicInner1": 399,
	"USonicOuter1": 334,
	"Pump1": -39,
	"USonicInner2": 163,
	"USonicOuter2": 309,
	"Pump2": -81,
	"USonicInner3": 214,
	"USonicOuter3": 170,
	"Pump3": -61,
	"CreatedAt": "2021-01-04T14:38:15.6189975+01:00",
	"MesurementTime": "2021-01-04T13:38:15.589Z",
	"SimulationID": null,
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

    updateModelRotation();
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

function updateModelRotation() {
    var radiant;

    var cubeLengths = {
        width: 2,
        height: .5,
        depth: 2,
    };

    if (wsData.USonicOuter1 === wsData.USonicOuter2) {
        // wsData a and b are even
        cubeRotationZ = +0.0;
    }

    if (wsData.USonicOuter1 === wsData.USonicOuter3) {
        // wsData a and c are even
        cubeRotationX = +0.0;
    }

    if (wsData.USonicOuter2 > wsData.USonicOuter1) {
        // B > A
        radiant = (-1) * Math.atan((wsData.USonicOuter2 - wsData.USonicOuter1) / cubeLengths.width);
        cubeRotationZ = radiant;
    }

    if (wsData.USonicOuter1 > wsData.USonicOuter2) {
        // A > B
        radiant = Math.atan((wsData.USonicOuter1 - wsData.USonicOuter2) / cubeLengths.width);
        cubeRotationZ = radiant;
    }

    if (wsData.USonicOuter1 > wsData.USonicOuter3) {
        // A > C
        radiant = (-1) * Math.atan((wsData.USonicOuter1 - wsData.USonicOuter3) / cubeLengths.width);
        cubeRotationX = radiant;
    }

    if (wsData.USonicOuter3 > wsData.USonicOuter1) {
        // C > A
        radiant = Math.atan((wsData.USonicOuter3 - wsData.USonicOuter1) / cubeLengths.width);
        cubeRotationX = radiant;
    }

    heightA = wsData.USonicOuter1 * Math.cos(Math.abs(cubeRotationZ));
    heightB = wsData.USonicOuter2 * Math.cos(Math.abs(cubeRotationZ));
    heightC = wsData.USonicOuter3 * Math.cos(Math.abs(cubeRotationX));

    heightHY = Math.round((((heightA + heightB + heightC) / 3) + Number.EPSILON) * 100) / 100 ;
}


