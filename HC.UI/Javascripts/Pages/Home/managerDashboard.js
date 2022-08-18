$(document).ready(function () {

    $('#productReport').click(function () {
        window.location.href = window.location.origin + "/Product/List?type=report";
    });

    $('#orderReport').click(function () {
        window.location.href = window.location.origin + "/Order/CompleteOrderReport";
    });


    $('#employeeReport').click(function () {
        window.location.href = window.location.origin + "/Employee/List?type=report";
    });
});