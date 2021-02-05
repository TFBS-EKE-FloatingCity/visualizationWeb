//-------------Declare global Vars----------------------

//Fix to not overwrite the ID with 0

var globals = {
    wsData: {},
    cubeRotationZ: 0.00,
    cubeRotationX: 0.00,
    heightA: 0.00,
    heightB: 0.00,
    heightC: 0.00,
    heightHY: 0.00,
    cityDataHeadID: null,
    simulationID: 0,
    simulationStartTime: "0001-01-01T00:00:00",
    simulationEndTime: "0001-01-01T00:00:00",
    EnergyConsumptionComparisonVal: 0,
    MaxWind: 0,
    MaxSun: 0,
    MaxConsumption: 0,
    AnimationStop: false
};

const settings = {
    updateRate: 2000,
    heightFactor: 1,
    standardHeight: 250 // 250mm
}

var currentHeight = settings.standardHeight;
var lastHeight = settings.standardHeight;

var host;

//-------------------end-------------------------------


//Get the IP for the websocket Connectionstring!
function getIP() {
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetIPFromSettings",
        success: function (result) {
            if (result == "") {
                host = 'ws://localhost:8109/Connection';
            } else {
                host = result;
            }

            connect();
        },
        error: function (req, status, error) {
            console.log("Connection ERROR")
            host = 'ws://localhost:8109/Connection';

            connect();
        }
    });
}

function connect() {
    var socket = new WebSocket(host);
    socket.onmessage = function (e) {
        globals.wsData = JSON.parse(e.data);    // has to be parsed?!
        console.log(JSON.stringify(globals.wsData));
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

//First get IP to generate a Connectionstring
getIP();


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


// fill PowerChart and  CityHeight Chart with history from DB
$(function () {
    $.ajax({
        url: '/API/Dashboard/GetCurrentCityDataHeadID',
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            globals.cityDataHeadID = data;
            GetSimulationHistory();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            globals.cityDataHeadID = null;
        }
    });

    function GetSimulationHistory() {
        if (globals.cityDataHeadID != null) {
            $.ajax({
                url: '/API/Dashboard/GetSimulationHistory/' + globals.cityDataHeadID,
                dataType: 'json',
                type: 'GET',
                contentType: 'application/json',
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    var chartData = []
                    data = JSON.parse(data)
                    data.forEach(function (value) {
                        //HeightChart
                        heightData = updateModelRotation(value.USonicOuter1, value.USonicOuter2, value.USonicOuter3, settings.heightFactor);
                        setCityHeight(new Date(value.Simulationtime).getTime(), heightData.heightHY);
                        //PowerChart
                        setSimData(new Date(value.Simulationtime).getTime(), value.WindCurrent, value.SunCurrent, value.ConsumptionCurrent)
                    })
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }
    }
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


//fill EnergyConsumptionComparison chart with history from DB
setInterval(function () {
    if (globals.cityDataHeadID != null) {
        $.ajax({
            url: '/API/Dashboard/GetRecentlyGeneratedEnergy/' + globals.cityDataHeadID,
            dataType: 'json',
            type: 'GET',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                var energy = 0;
                var consumption = 0;
                var chartData = [];
                data = JSON.parse(data)
                data.forEach(function (value) {

                    //Die Energie in MW/h errechnen
                    energy += Math.round(globals.MaxWind * (value.WindCurrent / 100));
                    energy += Math.round(globals.MaxSun * (value.SunCurrent / 100));
                    consumption += Math.round(globals.MaxConsumption * (value.ConsumptionCurrent / 100));
                })
                globals.EnergyConsumptionComparisonVal = energy - consumption;
                chartData[0] = energy;
                chartData[1] = consumption;
                setEnergyConsumptionData(chartData);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
}, settings.updateRate)


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
        width: 500,
        height: .5,
        depth: 500,
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


//Reset CityHeight Chart
function resetCityHeight() {
    heightDataConfig.data.datasets[0].data = [];
    globals.AnimationStop = false;
}

function stopCityHeight() {
    window.cityHeight.stop();
    globals.AnimationStop = true;

}

function startCityHeight() {
    window.cityHeight.render();
    globals.AnimationStop = false;
}