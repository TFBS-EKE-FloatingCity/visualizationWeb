//-------------Declare global Vars----------------------
var globals = {
    wsData: {},
    cubeRotationZ: 0.00,
    cubeRotationX: 0.00,
    heightA: 0.00,
    heightB: 0.00,
    heightC: 0.00,
    heightHY: 0.00,
    cityDataHeadID: 0,
    simulationID: 0,
    simulationStartTime: "0001-01-01T00:00:00",
    simulationEndTime: "0001-01-01T00:00:00",
    EnergyConsumptionComparisonVal: 0
};

const settings = {
    updateRate: 10000,
    heightFactor: 1
}
//-------------------end-------------------------------

function connect() {
    var host = 'ws://localhost:8109/Connection';
    var socket = new WebSocket(host);

    socket.onmessage = function (e) {
        console.log(e.data);
        globals.wsData = JSON.parse(e.data);    // has to be parsed?!
        if (typeof globals.wsData.State == 'undefined') {
            //Citydata.json
            heightData = updateModelRotation(globals.wsData.USonicOuter1, globals.wsData.USonicOuter2, globals.wsData.USonicOuter3, settings.heightFactor);
            globals.heightA = heightData.heightA;
            globals.heightB = heightData.heightB;
            globals.heightC = heightData.heightC;
            globals.heightHY = heightData.heightHY;

        } else {
            //CityDataHead.json
            globals.cityDataHeadID = globals.wsData.CityDataHeadID;
            globals.simulationID = globals.wsData.SimulationID;
            globals.simulationStartTime = globals.wsData.StartTime;
            globals.simulationEndTime = globals.wsData.EndTime;
        }
    };

    socket.onclose = function (e) {
        setTimeout(function () {
            connect();
        }, settings.updateRate);
    };
}
connect();

$(function () {
    var currentProgress = 0;
    var interval = setInterval(function () {
        var now = new Date();
        var endTime = new Date(globals.simulationEndTime);
        var startTime = new Date(globals.simulationStartTime);

        var diff = endTime.getTime() - startTime.getTime();
        var diffNow = endTime.getTime() - now.getTime();

        var actualTime = Math.round((100.00 - ((diffNow / diff) * 100))).toFixed(2);
        currentProgress = actualTime;

        $("#simulationTimeProgressBar")
            .css("width", actualTime + "%")
            .attr("aria-valuenow", currentProgress)
            .text(currentProgress + "% Complete");
        if (currentProgress >= 100)
            clearInterval(interval);
    }, 1000);
});

//For the SimulationProgressBar
$(function () {
    $.ajax({
        url: '/Dashboard/GetSimulationTitle',
        type: 'GET',
        success: function (data) {
            if (data === "") {
                return;
            }
            //Bug Fix
            var header = document.getElementById('SimulationNameH2');
            if (header != null) {
                header.innerHTML = data;
            }                     
        }
    });
})


//// TESTDATA
//testData = 
//{
//    "UUID": 3,
//    "CityDataHeadID": 1,
//	"USonicInner1": 399,
//	"USonicOuter1": 334,
//	"Pump1": -39,
//	"USonicInner2": 163,
//	"USonicOuter2": 309,
//	"Pump2": -81,
//	"USonicInner3": 214,
//	"USonicOuter3": 170,
//	"Pump3": -61,
//	"CreatedAt": "2021-01-04T14:38:15.618",
//	"MesurementTime": "2021-01-04T13:38:15.589",
//	"SimulationID": 2,
//	"WindMax": 0,
//	"WindCurrent": 0,
//	"SunMax": 0,
//	"SunCurrent": 0,
//	"ConsumptionMax": 0,
//	"ConsumptionCurrent": 0,
//	"SimulationActive": false,
//	"Simulationtime": null,
//	"TimeFactor": null
//};

//globals.wsData = testData;

//setInterval(function () {
//    globals.wsData.Pump1 = randomNumber(-100, 100);
//    globals.wsData.Pump2 = randomNumber(-100, 100);
//    globals.wsData.Pump3 = randomNumber(-100, 100);
//    globals.wsData.WindCurrent = randomNumber(0, 50);
//    globals.wsData.SunCurrent = randomNumber(0, 50);
//    globals.wsData.ConsumptionCurrent = randomNumber(0, 50);
//    globals.wsData.USonicOuter1 = randomNumber(150, 400);
//    globals.wsData.USonicOuter2 = randomNumber(150, 400);
//    globals.wsData.USonicOuter3 = randomNumber(150, 400);

//    heightData = updateModelRotation(globals.wsData.USonicOuter1, globals.wsData.USonicOuter2, globals.wsData.USonicOuter3);

//    globals.heightA = heightData.heightA;
//    globals.heightB = heightData.heightB;
//    globals.heightC = heightData.heightC;
//    globals.heightHY = heightData.heightHY;


//}, settings.updateRate);

//function randomNumber(min, max) {
//    if (min > max) {
//        let temp = max;
//        max = min;
//        min = temp;
//    }

//    if (min <= 0) {
//        return Math.floor(Math.random() * (max + Math.abs(min) + 1)) + min;
//    } else {
//        return Math.floor(Math.random() * (max - min + 1)) + min;
//    }
//}
//// END TEST

function updateModelRotation(USonicOuter1, USonicOuter2, USonicOuter3, heightFactorValue) {
    //Convert Sensordata from mm in m
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
        // height a and b are even
        cubeRotationZ = +0.0;
    }

    if (USonicOuter1 === USonicOuter3) {
        // height a and c are even
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

    //oldHeight = newHeight;

    return heightData;
}