
$(document).ready(function () {

    Codebase.helpers("jq-validation"), jQuery("#addDepartmentForm").validate({
        submitHandler: function (form) {
            if ($("#addDepartmentForm").valid()) {

                var departmentName = $("#departmentName").val().trim();
                var departmentCompanyId = $("#departmentCompany").val().trim();

                var departmentData = {
                    grant_type: "password",
                    departmentName: departmentName,
                    CompanyId: departmentCompanyId,
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    addDepartment(departmentData, data.apiUrl);
                });
            }
        },
        rules: {
            'departmentName': {
                required: true,
                minlength: 3
            }, 'departmentCompany': {
                required: true
            }
        },
        messages: {
            'departmentName': {
                required: 'Departman Adı alanını doldurunuz!',
                minlength: 'Departman Adı alanı 3 karakterden büyük olmalıdır!'
            },
            'departmentCompany': 'Lütfen Şirket Adını Seçiniz!'
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getDepartmentList(data.apiUrl);
        getCompanyList(data.apiUrl);
    });
});



//Şirketleri Listeleme
function getCompanyList(apiUrl) {

    var token = localStorage.getItem("token");
    debugger
    $.ajax({
        url: apiUrl + "Company/ActivesCompanyList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#departmentCompany").empty();

            var departmentCompanyOptions = '<option></option>'

            $.each(data, function (key, value) {

                departmentCompanyOptions += '<option value="' + value.Id + '">' + value.CompanyName + '</option>';

            });

            $("#departmentCompany").append(departmentCompanyOptions);

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
//Departmanları Listeleme
function getDepartmentList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Department/ActivesDepartmentList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#departmentTableBody").empty();
            $.each(data, function (key, value) {

                var item = '<tr><td class="text-center">' + value.Id + '</td><td class="fw-semibold">' + value.DepartmentName + '</td><td class="fw-semibold">' + value.Company?.CompanyName + '</td><td class="text-center"><button type="button" onclick="updateDepartment(' + value.Id + ')" class="btn btn-sm btn-alt-warning" data-bs-toggle="tooltip" title="Düzenle"><i class="fa fa-pencil"></i></button><button type="button" onclick="questionModal(' + value.Id + ')" class="btn btn-sm btn-danger" data-bs-toggle="tooltip" title="Sil"><i class="fa fa-trash"></i></button></td></tr>';

                $("#departmentTableBody").append(item);
            });
            initDataTable();
            tooltip();
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

//Departman Ekleme
function addDepartment(departmentData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Department/AddDepartment",
        type: 'POST',
        headers: { "Authorization": "bearer " + token },
        data: departmentData,
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

//Departman Güncelleme
function updateDepartment(id) {

    if (id != undefined && id != null) {
        window.location.href = "/Department/Update?id=" + id;
    }
}

//Departman Silme
function deleteDepartment(id, apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Department/DeleteDepartment/" + id,
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
                deleteDepartment(id, data.apiUrl);
            });
        }
    });
}