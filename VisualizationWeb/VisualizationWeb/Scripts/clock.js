$(document).ready(initClock)

function initClock() {
    getSimTimes()
    calculateTime()

    //checkSimulationStatus()
}

function calculateTime() {
    if (window.SimTimes != null) {
        const START_TIME = new Date(window.SimTimes.StartTime)
        const END_TIME = new Date(window.SimTimes.EndTime)

        const REAL_START_TIME = new Date(window.SimTimes.RealStartTime)
        const ACTUAL_TIME = new Date()

        const FACTOR = window.SimTimes.SimFactor

        var milliseconds = Math.abs(ACTUAL_TIME - REAL_START_TIME)
        var multipliedMilliseconds = milliseconds * FACTOR

        var simulationMilliseconds = START_TIME.getTime() + multipliedMilliseconds

        var simulationTime = new Date(simulationMilliseconds)

        var h = simulationTime.getHours() 
        var m = simulationTime.getMinutes()
        var s = simulationTime.getSeconds()

        h = (h < 10) ? "0" + h : h
        m = (m < 10) ? "0" + m : m
        s = (s < 10) ? "0" + s : s

        var time = h + ":" + m + ":" + s
    }

    $('.sim-time h2').html(time)

    window.calculateInterval = setTimeout(calculateTime, 100)
}

function getSimTimes() {
    $.ajax({
        url: '/API/Simulation/GetSimulationTimes',
        dataType: 'json',
        type: 'get',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            window.SimTimes = JSON.parse(data)
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown)
        }
    });
}

//function checkSimulationStatus() {
//    $.ajax({
//        url: '/API/Simulation/CheckSimulationStatus',
//        dataType: 'json',
//        type: 'post',
//        contentType: 'application/json',
//        processData: false,
//        success: function (data, textStatus, jQxhr) {
//            if (JSON.parse(data)) {
//                getSimTimes()
//                calculateTime()
//            } else {
//                clearInterval(window.calculateInterval)
//                $('.sim-time h2').html("00:00:00")
//            }
//        },
//        error: function (jqXhr, textStatus, errorThrown) {
//            console.log(errorThrown)
//        }
//    });

//    setTimeout(checkSimulationStatus, 1000)
//}