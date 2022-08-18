$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var type = searchParam.get("type");

    Codebase.helpers("jq-validation"), jQuery("#addEmployeeForm").validate({
        submitHandler: function () {
            if ($("#addEmployeeForm").valid()) {

                var firstName = $("#firstName").val().trim();
                var lastName = $("#lastName").val().trim();
                var address = $("#address").val().trim();
                var phoneNumber = $("#phoneNumber").val().trim();
                var email = $("#email").val().trim();
                var departmentId = $("#employeeDepartment").val().trim();

                var employeeData = {
                    grant_type: "password",
                    FirstName: firstName,
                    LastName: lastName,
                    Address: address,
                    PhoneNumber: phoneNumber,
                    Email: email,
                    DepartmentId: departmentId,
                }

                $('#appUserCreateModal').modal('show');


                $('#addAppUser').unbind('click');
                $('#addEmployee').unbind('click');

                $('#addAppUser').click(function () {

                    var appUserRoles = $('#appUserRoles').val();
                    employeeData.AppUserRoles = appUserRoles;
                    employeeData.IsCreateAppUser = true;

                    $.getJSON(window.location.origin + '/config.json').then(function (data) {
                        addEmployee(employeeData, data.apiUrl);
                    });
                });

                $('#addEmployee').click(function () {

                    employeeData.IsCreateAppUser = false;

                    $.getJSON(window.location.origin + '/config.json').then(function (data) {
                        addEmployee(employeeData, data.apiUrl);
                    });
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
            'email': {
                required: true,
                minlength: 3
            },
            'employeeDepartment': {
                required: true
            }
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
            'email': {
                required: 'Email alanını doldurunuz!',
                minlength: 'Email alanı 2 karakterden büyük olmalıdır!'
            },
            'employeeDepartment': 'Lütfen Departman Seçiniz!'
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getEmployeeList(data.apiUrl, type);
        getDepartmentList(data.apiUrl);
        getRoleList(data.apiUrl);
    });
});

//Çalışanları Listeleme
function getEmployeeList(apiUrl, type) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Employee/ActivesEmployeeList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#employeeTableBody").empty();

            $.each(data, function (key, value) {

                // kullanıcı ile ilişkilendirme yapılmak istenirse bu buton eklenip gerekli kodlar yazılacak
                //<button type="button" onclick="addAppUser(' + value.Id + ',\'' + value.FirstName + '\')" class="btn btn-sm btn-alt-info addAppUser me-1" data-bs-toggle="tooltip" title="Kullanıcı ile İlişkilendir"><i class="fa fa-eye"></i></button></i></button>


                var item = '<tr><td class="text-center">' + value.Id + '</td><td class="fw-semibold">' + value.FirstName + '</td><td class="d-none d-sm-table-cell">' + value.LastName + '</td><td class="d-none d-sm-table-cell">' + value.Address + '</td><td class="d-none d-sm-table-cell">' + value.PhoneNumber + '</td><td class="d-none d-sm-table-cell">' + value.Email + '</td><td class="d-none d-sm-table-cell">' + value.Department?.DepartmentName + '</td><td class="text-center process"><button type="button" onclick="updateEmployee(' + value.Id + ')" class="btn btn-sm btn-alt-warning me-1" data-bs-toggle="tooltip" title="Düzenle"><i class="fa fa-pencil"></i></button><button type="button" onclick="questionModal(' + value.Id + ')" class="btn btn-sm btn-danger" data-bs-toggle="tooltip" title="Sil"><i class="fa fa-trash"></i></button></td></tr>';

                $("#employeeTableBody").append(item);
            });

            initDataTable();
            tooltip();

            if (type == 'report') {
                $('#process, .process').css('display', 'none');
            } else {
                $('#process, .process').css('display', 'table-cell');

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

// Tüm Rollerin Listelenmesi
function getRoleList(apiUrl) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "Enum/RoleList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#appUserRoles").empty();

            var appUserRolesOption = '<option></option>';

            $.each(data, function (key, value) {
                appUserRolesOption += '<option value="' + value.Id + '">' + value.DisplayName + '</option>';
            });

            $("#appUserRoles").append(appUserRolesOption);

            $('#appUserRoles').select2({
                width: '100%',
                dropdownParent: $("#appUserCreateModal")
            })
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

//Çalışan Ekleme
function addEmployee(employeeData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Employee/AddEmployee",
        type: 'POST',
        headers: { "Authorization": "bearer " + token },
        data: employeeData,
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'success',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-check me-5',    // Icon class
                    message: "Kayıt İşlemi Başarılı :)",
                });
            }
            setTimeout(function () {
                window.location.reload();
            }, 1000);
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

//Çalışan Silme
function deleteEmployee(id, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Employee/DeleteEmployee/" + id,
        type: 'DELETE',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null) {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'success',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-check me-5',    // Icon class
                    message: "Silme İşlemi Başarılı :)",
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

//Çalışan Güncelleme
function updateEmployee(id) {

    if (id != undefined && id != null) {
        window.location.href = "/Employee/Update?id=" + id;
    }
}

//Departmanları Listeleme
function getDepartmentList(apiUrl) {

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


// Ajax işleminden sonra datatable oluşması için ve sıralama, arama gibi özelliklerin doğru çalışabilmesi için template içerisindeki kendi js dosyasından alındı.
function initDataTable() {
    jQuery.extend(jQuery.fn.dataTable.ext.classes, {
        sWrapper: "dataTables_wrapper dt-bootstrap5",
        sFilterInput: "form-control",
        sLengthSelect: "form-select"
    }), jQuery.extend(!0, jQuery.fn.dataTable.defaults, {
        language: {
            infoFiltered: "",
            zeroRecords: "Hiç kayıt eşleşmedi!",
            infoEmpty: "0 girdiden 0 ile 0 arası gösteriliyor.",
            emptyTable: "Tabloda veri yok",
            lengthMenu: "_MENU_",
            search: "_INPUT_",
            searchPlaceholder: "Ara",
            info: "Sayfa <strong>_PAGE_</strong> / <strong>_PAGES_</strong>",
            paginate: {
                first: '<i class="fa fa-angle-double-left"></i>',
                previous: '<i class="fa fa-angle-left"></i>',
                next: '<i class="fa fa-angle-right"></i>',
                last: '<i class="fa fa-angle-double-right"></i>'
            }
        }
    }), jQuery.extend(!0, jQuery.fn.DataTable.Buttons.defaults, {
        dom: {
            button: {
                className: "btn btn-sm btn-primary"
            }
        }
    }), jQuery(".js-dataTable-full-pagination").DataTable({
        pagingType: "full_numbers",
        pageLength: 20,
        lengthMenu: [
            [20, 40, 60],
            [20, 40, 60]
        ],
        autoWidth: !1
    })
}

// Düzenleme ve silme butonlarının üzerinde tooltip çıkması için ajax işleminden sonra butonlar ekranda oluşunca çalışması için template içerisindeki kendi js dosyasından alındı.
function tooltip() {
    let e = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]:not(.js-bs-tooltip-enabled), .js-bs-tooltip:not(.js-bs-tooltip-enabled)'));
    window.helperBsTooltips = e.map((e => (e.classList.add("js-bs-tooltip-enabled"), new bootstrap.Tooltip(e, {
        container: e.dataset.bsContainer || "#page-container",
        animation: !(!e.dataset.bsAnimation || "true" != e.dataset.bsAnimation.toLowerCase())
    }))))
}

// Sil  butonuna basıldıktan sonra emin misiniz? diye sorulması için ve sonrasında evet butonuna basıldıktan sonra silme methodunun çağırılması için template içerisinde kendi js dosyasından alındı.
function questionModal(id) {
    let toast = Swal.mixin({
        buttonsStyling: false,
        target: '#page-container',
        customClass: {
            confirmButton: 'btn btn-success m-1',
            cancelButton: 'btn btn-danger m-1',
            input: 'form-control'
        }
    });

    toast.fire({
        title: 'Silmek istediğinize emin misiniz?',
        icon: 'warning',
        showCancelButton: true,
        customClass: {
            confirmButton: 'btn btn-danger m-1',
            cancelButton: 'btn btn-secondary m-1'
        },
        confirmButtonText: 'Evet!',
        cancelButtonText: 'Hayır',
        html: false,
        preConfirm: e => {
            return new Promise(resolve => {
                setTimeout(() => {
                    resolve();
                }, 50);
            });
        }
    }).then(result => {
        if (result.value) {
            $.getJSON(window.location.origin + '/config.json').then(function (data) {
                deleteEmployee(id, data.apiUrl);
            });
        }
    });
}