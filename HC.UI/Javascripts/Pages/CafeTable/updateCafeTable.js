$(document).ready(function () {
    var searchParam = new URLSearchParams(window.location.search);
    var cafeTableId = searchParam.get("id");

    Codebase.helpers("jq-validation"), jQuery("#updateCafeTableForm").validate({
        submitHandler: function (form) {
            if ($("#updateCafeTableForm").valid()) {

                var tableName = $("#cafeTableName").val().trim();
                var capacity = $("#capacity").val().trim();
                var tableLocation = $("#tableLocation").val().trim() != "" ? $("#tableLocation").val().trim() : null;
                var tableStatus = $("#tableStatus").val().trim() != "" ? $("#tableStatus").val().trim() : null;


                var cafeTableData = {
                    grant_type: "password",
                    id: cafeTableId,
                    tableName: tableName,
                    capacity: capacity,
                    TableLocation: tableLocation,
                    TableStatus: tableStatus
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    updateCafeTable(cafeTableData, data.apiUrl);
                });
            }
        },
        rules: {
            'tableName': {
                required: true,
                minlength: 1
            },
            'capacity': {
                required: true,
                min: 1,
                number: true
            }
        },
        messages: {
            'tableName': {
                required: 'Masa Adı alanını doldurunuz!',
                minlength: 'Masa Adı alanı 1 karakterden büyük olmalıdır!'
            },
            'capacity': {
                required: 'Masa Kapasitesini doldurunuz!',
                min: "Kapasite 0 dan büyük olmalıdır!",
                number: "Lütfen sayısal bir değer giriniz!"
            }
        }
    });

    $.getJSON(window.location.origin + '/config.json').then(function (data) {

        getTableLocationList(data.apiUrl, cafeTableId);
        getTableStatusList(data.apiUrl, cafeTableId);
    })

});


//Masa Lokasyonlarını Lİsteleme
function getTableLocationList(apiUrl, cafeTableId) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "Enum/TableLocationList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#tableLocation").empty();
            var selectedId = data[0].Id;
            var tableLocationOption = '<option value="">-</option>';

            $.each(data, function (key, value) {
                tableLocationOption += '<option value="' + value.Id + '">' + value.DisplayName + '</option>';
            });

            $("#tableLocation").append(tableLocationOption);
            $("#tableLocation").val(selectedId);
            getCafeTable(cafeTableId, apiUrl);
        },
        error: function (xhr, ajaxOptions, throwError) {

            if (xhr.status != null && xhr.status == 401) {
                window.location.href = window.location.origin + "/Error/AuthenticationError";
            }
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

//Masa Durumlarını Listelemesi
function getTableStatusList(apiUrl, cafeTableId) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "Enum/TableStatusList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#tableStatus").empty();

            var selectedId = data[0].Id;
            var tableStatusOption = '<option value="">-</option>';

            $.each(data, function (key, value) {
                tableStatusOption += '<option value="' + value.Id + '">' + value.DisplayName + '</option>';
            });

            $("#tableStatus").append(tableStatusOption);
            $("#tableStatus").val(selectedId);

            getCafeTable(cafeTableId, apiUrl);
        },

        error: function (xhr, ajaxOptions, throwError) {

            if (xhr.status != null && xhr.status == 401) {
                window.location.href = window.location.origin + "/Error/AuthenticationError";
            }
         
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

//Masa Getirme 
function getCafeTable(cafeTableId, apiUrl) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "CafeTable/BringCafeTable/" + cafeTableId,
        type: 'GET',
        headers: { "Authorization": "bearer" + token },
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {

                $("#cafeTableName").val(data.TableName);
                $("#capacity").val(data.Capacity);
                $("#tableLocation").prop('checked', data.TableLocationDisplayName);
                $("#tableStatus").prop('checked', data.TableStatusDisplayName);
            }
        },
        error: function (xhr, ajaxOptions, throwError) {
            if (xhr.status != null && xhr.status == 401) {
                window.location.href = window.location.origin + "/Error/AuthenticationError";
            }
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


//Masa Güncelleme
function updateCafeTable(cafeTableData, apiUrl) {
    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "CafeTable/UpdateCafeTable",
        type: 'PUT',
        headers: { "Authorization": "bearer " + token },
        data: cafeTableData,
        success: function (data) {
            if (data == 1) {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'success',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-check me-5',    // Icon class
                    message: "Güncelleme İşlemi Başarılı :)",
                });
            }
            setTimeout(function () {
                window.location.reload();
            }, 2000);
        },
        error: function (xhr, ajaxOptions, throwError) {

            if (xhr.status != null && xhr.status == 401) {
                window.location.href = window.location.origin + "/Error/AuthenticationError";
            }
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