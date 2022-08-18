$(document).ready(function () {
    $.getJSON(window.location.origin + '/config.json').then(function (data) {

        getUserName(data.apiUrl);

        $('#logout').click(function () {
            localStorage.removeItem("token");
            window.location.href = window.location.origin + "/Authentication/Login";
        })

    });
});

function getUserName(apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Authentication/GetUserNameInSystem",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $('.username').text(data);
        },
        error: function (xhr) {
            var error = xhr.responseJSON;
            if (error != undefined && error.error_description != null && error.error_description != "") {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'danger',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-warning me-5',    // Icon class
                    message: error.error_description
                });
            }
        }
    });
}