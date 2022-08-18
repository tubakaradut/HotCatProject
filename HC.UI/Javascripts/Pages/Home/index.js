$(document).ready(function () {
    $.getJSON(window.location.origin + '/config.json').then(function (data) {

        getAuthorizeMainPage(data.apiUrl);

    });
});

//Sistemdeki Kullanıcının Yetkisine Bağlı Görebileceği Anasayfayı Getirme
function getAuthorizeMainPage(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Page/GetMainPage",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != null) {
                var url = window.location.origin + data.Path;

                window.location.href = url;
            }
        },
        error: function (xhr, ajaxOptions, throwError) {
            var error = xhr.responseJSON;
            console.log(error.error_description);
        }
    });
}