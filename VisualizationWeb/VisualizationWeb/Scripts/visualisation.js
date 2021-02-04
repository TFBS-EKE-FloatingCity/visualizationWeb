//-------------Declare global Vars----------------------

//Fix to not overwrite the ID with 0
var headID;
var globals = {
    wsData: {},
    cubeRotationZ: 0.00,
    cubeRotationX: 0.00,
    heightA: 0.00,
    heightB: 0.00,
    heightC: 0.00,
    heightHY: 0.00,
    cityDataHeadID: headID,
    simulationID: 0,
    simulationStartTime: "0001-01-01T00:00:00",
    simulationEndTime: "0001-01-01T00:00:00",
    EnergyConsumptionComparisonVal: 0,
    MaxWind: 0,
    MaxSun: 0,
    MaxConsumption: 0
};

const settings = {
    updateRate: 10000,
    heightFactor: 1,
    standardHeight: 250 // 250mm
}

var currentHeight = settings.standardHeight;
var lastHeight = settings.standardHeight;

//-------------------end-------------------------------

function connect() {
    //Test to get the IP for the Connectionstring!
    //var host;
    //var socket;
    //$.ajax({
    //    type: "POST",
    //    url: "/Dashboard/GetID", // the URL of the controller action method
    //    data: null, // optional data
    //    success: function (result) {
    //        // do something with result
    //        if (result == '0.0.0.1') {
    //            host = 'ws://localhost:8109/Connection';
    //            socket = new WebSocket(host);
    //        } else {
    //            host = 'ws://'+result+':8109/Connection';
    //            socket = new WebSocket(host);
    //        }
    //    },
    //    error: function (req, status, error) {
    //        host = 'ws://localhost:8109/Connection';
    //        socket = new WebSocket(host);
    //    }
    //});

    var host = 'ws://localhost:8109/Connection';
    var socket = new WebSocket(host);
    socket.onmessage = function (e) {
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
            headID = globals.wsData.CityDataHeadID;
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


// Progressbar
$(function () {
    var currentProgress = 0;
    var interval = setInterval(function () {
        var now = new Date();
        var endTime = new Date(globals.simulationEndTime);
        var startTime = new Date(globals.simulationStartTime);

        var diff = endTime.getTime() - startTime.getTime();
        var diffNow = endTime.getTime() - now.getTime();

        //var actualTime = Math.round((100.00 - ((diffNow / diff) * 100))).toFixed(2);
        var actualTime = Math.round(100.00 - (diffNow / diff) * 100).toFixed(2);
        currentProgress = actualTime;

        $("#simulationTimeProgressBar")
            .css("width", actualTime + "%")
            .attr("aria-valuenow", currentProgress)
            .text(Math.trunc(currentProgress) + "% Complete");
        if (currentProgress >= 100)
            clearInterval(interval);
    }, 1000); // 1000
});

// Simulation title
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
});

//Get Values of Settings-Table
$(function () {

    $.ajax({
        url: '/API/Dashboard/GetMaxValues',
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            data = JSON.parse(data)
            data.forEach(function (value) {
                globals.MaxSun = value.SunMax;
                globals.MaxWind = value.WindMax;
                globals.MaxConsumption = value.ConsumptionMax;
            })
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
});

function updateModelRotation(USonicOuter1, USonicOuter2, USonicOuter3, heightFactorValue) {
    // USonicOuter1 = 0;
    // USonicOuter2 = 0; // lowest value possible will lead to heightHY = 0
    // USonicOuter3 = 0;

    // USonicOuter1 = 300;
    // USonicOuter2 = 300; // highest value possible will lead to heightHY = 300
    // USonicOuter3 = 300;

    //Convert Sensordata from mm in m
    if (heightFactorValue > 0) {
        USonicOuter1 *= heightFactorValue;
        USonicOuter2 *= heightFactorValue;
        USonicOuter3 *= heightFactorValue;
    }

    var radiant;

    var cubeLengths = {
        width: 4000,
        height: .5,
        depth: 4000,
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

    globals.cubeRotationX = cubeRotationX;
    globals.cubeRotationZ = cubeRotationZ;

    // set height variables
    lastHeight = currentHeight;
    currentHeight = heightData.heightHY;

    return heightData;
}