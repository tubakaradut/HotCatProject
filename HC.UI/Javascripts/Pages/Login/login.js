$(document).ready(function () {

    Codebase.helpers("jq-validation"), jQuery(".js-validation-signin").validate({
        submitHandler: function (form) {
            if ($("#loginForm").valid()) {
                var username = $("#login-username").val().trim();
                var password = $("#login-password").val().trim();

                var loginData = {
                    grant_type: "password",
                    username: username,
                    password: password
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    login(loginData, data.apiUrl);
                });
            }
        },
        rules: {
            "login-username": {
                required: !0,
                minlength: 3
            },
            "login-password": {
                required: !0,
                minlength: 5
            }
        },
        messages: {
            "login-username": {
                required: "Lütfen Kullanıcı Adı alanını doldurunuz!",
                minlength: "Kullanıcı Adı alanı 3 karakterden fazla olmalıdır!"
            },
            "login-password": {
                required: "Lütfen Şifre alanını doldurunuz!",
                minlength: "Şifre alanı 5 karakterden fazla olmalıdır!"
            }
        }
    })
});


function login(loginData, apiUrl) {

    localStorage.removeItem("token");

    $.ajax({
        url: apiUrl + "Authentication/Login",
        type: 'POST',
        data: loginData,
        success: function (data) {
            localStorage.setItem("token", data.access_token);
            window.location.href = window.location.origin + "/Home/Index";
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
