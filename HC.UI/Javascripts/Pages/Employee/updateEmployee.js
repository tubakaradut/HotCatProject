$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var employeeId = searchParam.get("id");

    Codebase.helpers("jq-validation"), jQuery("#updateEmployeeForm").validate({
        submitHandler: function (form) {
            if ($("#updateEmployeeForm").valid()) {

                var firstName = $("#firstName").val().trim();
                var lastName = $("#lastName").val().trim();
                var address = $("#address").val().trim();
                var phoneNumber = $("#phoneNumber").val().trim();
                var email = $("#email").val().trim();
                var departmentId = $("#employeeDepartment").val().trim() != null ? $("#employeeDepartment").val().trim() : "-";

                var employeeData = {
                    grant_type: "password",
                    Id: employeeId,
                    FirstName: firstName,
                    LastName: lastName,
                    Address: address,
                    PhoneNumber: phoneNumber,
                    Email: email,
                    DepartmentId: departmentId,
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    updateEmployee(employeeData, data.apiUrl);
                });
            }
        },
        rules: {
            'firstName': {
                required: true,
                minlength: 3
            },
            'lastName': {
                required: true,
                minlength: 3
            },
            'address': {
                required: true,
                minlength: 3
            },
            'email': {
                required: true,
                minlength: 3
            },
            'phoneNumber': {
                required: true,
                minlength: 3
            },

        },
        messages: {
            'firstName': {
                required: 'İsim alanını doldurunuz!',
                minlength: 'İsim alanı 2 karakterden büyük olmalıdır!'
            },
            'lastName': {
                required: 'Soyİsim alanı doldurunuz!',
                minlength: 'İsim alanı 1 karakterden büyük olmalıdır!'
            },
            'address': {
                required: 'Adres alanı doldurunuz!',
                minlength: 'İsim alanı 2 karakterden büyük olmalıdır!'
            },
            'phoneNumber': {
                required: 'Telefon alanını doldurunuz!',
                minlength: 'Telefon alanı 7 karakterden büyük olmalıdır!'
            },
            'email': {
                required: 'Email alanını doldurunuz!',
                minlength: 'Email alanı 2 karakterden büyük olmalıdır!'
            }
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getDepartmentList(data.apiUrl, employeeId);
    });
});

//Departmanları Listeleme
function getDepartmentList(apiUrl, employeeId) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Department/ActivesDepartmentList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#employeeDepartment").empty();

            var employeeDepartmentOptions = '<option value="">-</option>'

            $.each(data, function (key, value) {

                employeeDepartmentOptions += '<option value="' + value.Id + '">' + value.DepartmentName + '</option>';

            });

            $("#employeeDepartment").append(employeeDepartmentOptions);
            getEmployee(employeeId, apiUrl);

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

//Çalışan Getirme
function getEmployee(employeeId, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Employee/BringEmployee/" + employeeId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                $("#firstName").val(data.FirstName);
                $("#lastName").val(data.LastName);
                $("#address").val(data.Address);
                $("#phoneNumber").val(data.PhoneNumber);
                $("#email").val(data.Email);
                $("#employeeDepartment").val(data.DepartmentId);
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

//Çalışan Güncelleme
function updateEmployee(employeeData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Employee/UpdateEmployee",
        type: 'PUT',
        headers: { "Authorization": "bearer " + token },
        data: employeeData,
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