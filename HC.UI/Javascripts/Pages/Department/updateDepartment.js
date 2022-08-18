$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var departmentId = searchParam.get("id");

    Codebase.helpers("jq-validation"), jQuery("#updateDepartmentForm").validate({
        submitHandler: function (form) {
            if ($("#updateDepartmentForm").valid()) {
                var departmentName = $("#departmentName").val().trim();
              
                var departmentData = {
                    grant_type: "password",
                    id: departmentId,
                    departmentName: departmentName,
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    updateDepartment(departmentData, data.apiUrl);
                });
            }
        },
        rules: {
            'departmentName': {
                required: true,
                minlength: 3
            }
        },
        messages: {
            'departmentName': {
                required: 'Departman Adı alanını doldurunuz!',
                minlength: 'Departman Adı alanı 3 karakterden büyük olmalıdır!'
            }
        }
    })

});


//Departman Getirme
function getDepartment(departmentId, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Department/BringDepartment/" + departmentId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                $("#departmentName").val(data.DepartmentName);
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


//Departman Güncelleme
function updateDepartment(departmentData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Department/UpdateDepartment",
        type: 'PUT',
        headers: { "Authorization": "bearer " + token },
        data: departmentData,
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