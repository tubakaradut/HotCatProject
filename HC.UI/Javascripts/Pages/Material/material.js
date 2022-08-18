
$(document).ready(function () {

    Codebase.helpers("jq-validation"), jQuery("#addMaterialForm").validate({
        submitHandler: function () {
            if ($("#addMaterialForm").valid()) {

                var materialName = $("#materialName").val().trim();
                var unitTypeId = $("#unitTypeId").val().trim();
                var expirationDate = $("#expirationDate").val().trim();
                var currentStock = $("#currentStock").val().trim();
                var minStock = $("#minStock").val().trim();
                var maxStock = $("#maxStock").val().trim();

                var materialData = {
                    grant_type: "password",
                    materialName: materialName,
                    unitType: unitTypeId,
                    expirationDate: moment(expirationDate, 'DD-MM-YYYY').format('YYYY-MM-DD'),
                    currentStock: currentStock,
                    minStock: minStock,
                    maxStock: maxStock
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    addMaterial(materialData, data.apiUrl);
                });
            }
        },
        rules: {
            'materialName': {
                required: true,
                minlength: 3
            },
            'unitTypeId': {
                required: true
            },
            'expirationDate': {
                required: true,
            },
            'currentStock': {
                required: true,
                min: 1,
                number: true
            },
            'minStock': {
                required: true,
                min: 1,
                number: true
            },
            'maxStock': {
                required: true,
                min: 1,
                number: true
            }

        },
        messages: {
            'materialName': {
                required: 'Malzeme Adı alanını doldurunuz!',
                minlength: 'Malzeme Adı alanı 3 karakterden büyük olmalıdır!'
            },
            'unitTypeId': 'Lütfen Birim Tipi seçiniz!',
            'expirationDate': {
                required: 'Lütfen Son Kullanma Tarihi seçiniz!',
            },
            'currentStock': {
                required: 'Mevcut Stok Miktarı alanını doldurunuz!',
                min: "Mevcut Stok Miktarı 0 dan büyük olmalıdır!",
                number: "Lütfen sayısal bir değer giriniz!"
            },
            'minStock': {
                required: 'Minimum Stok Miktarı alanını doldurunuz!',
                min: "Minimum Stok Miktarı 0 dan büyük olmalıdır!",
                number: "Lütfen sayısal bir değer giriniz!"
            },
            'maxStock': {
                required: 'Maksimum Stok Miktarı alanını doldurunuz!',
                min: "Maksimum Stok Miktarı 0 dan büyük olmalıdır!",
                number: "Lütfen sayısal bir değer giriniz!"
            },
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getUnitTypeList(data.apiUrl);
        getMaterialList(data.apiUrl);
    });

    //son kullanma tarihi seçmek için takvim kütüphanesi
    $('#expirationDate').flatpickr({
        dateFormat: "d-m-Y",
        locale: "tr"
    });
});

//Malzemeleri Listeleme
function getMaterialList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Material/ActivesMaterialList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#materialTableBody").empty();
            $.each(data, function (key, value) {
                var item = '<tr><td class="text-center">' + value.Id + '</td><td>' + value.MaterialName + '</td><td>' + value.UnitTypeName + '</td><td>' + moment(value.ExpirationDate, 'YYYY-MM-DD').format('DD-MM-YYYY') + '</td><td>' + value.MinStock + '</td><td>' + value.CurrentStock + '</td><td>' + value.MaxStock + '</td><td class="text-center"><button type="button" onclick="addProductMaterial(' + value.Id + ',\'' + value.MaterialName + '\')" class="btn btn-sm btn-alt-info me-1" data-bs-toggle="tooltip" title="Ürünler ile İlişkilendir"><i class="fa fa-link"></i></button><button type="button" onclick="updateMaterial(' + value.Id + ')" class="btn btn-sm btn-alt-warning me-1" data-bs-toggle="tooltip" title="Düzenle"><i class="fa fa-pencil"></i></button><button type="button" onclick="questionModal(' + value.Id + ')" class="btn btn-sm btn-danger me-1" data-bs-toggle="tooltip" title="Sil"><i class="fa fa-trash"></i></button></td></tr>';

                $("#materialTableBody").append(item);
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

// Malzeme Tiplerini Listeleme
function getUnitTypeList(apiUrl) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "Enum/UnitTypeList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#unitTypeId").empty();

            var unitTypeOption = ''

            $.each(data, function (key, value) {
                unitTypeOption += '<option value="' + value.Id + '">' + value.DisplayName + '</option>';
            });

            $("#unitTypeId").append(unitTypeOption);
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

//Malzeme Ekleme 
function addMaterial(materialData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Material/AddMaterial",
        type: 'POST',
        headers: { "Authorization": "bearer " + token },
        data: materialData,
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

//Malzeme Silme
function deleteMaterial(id, apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Material/DeleteMaterial/" + id,
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

// Malzemeye Ürün Ekleme
function addProductMaterial(id, materialName) {
    if (id != undefined && id != null) {
        window.location.href = "/ProductMaterial/AddProduct?materialId=" + id + "&materialName=" + materialName;
    }
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
                deleteMaterial(id, data.apiUrl);
            });

        }
    });
}