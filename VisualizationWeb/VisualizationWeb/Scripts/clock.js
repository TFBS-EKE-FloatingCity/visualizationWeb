$(document).ready(initClock)

function initClock() {
    console.log("Init Clock")
    getSimTimes()
    calculateTime();
}

function calculateTime() {
    setTimeout(calculateTime, 1000);
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
            console.log(errorThrown);
        }
    });
}