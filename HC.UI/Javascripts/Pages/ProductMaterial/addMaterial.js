
$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var productId = searchParam.get("productId");
    var productName = searchParam.get("productName");

    $('#productName').val(productName);

    Codebase.helpers("jq-validation"), jQuery("#addProductMaterialForm").validate({
        submitHandler: function () {
            if ($("#addProductMaterialForm").valid()) {

                var productName = $("#productName").val().trim();
                var materialQuantity = $("#materialQuantity").val().trim();
                var materialId = $("#materialSelectList").val().trim() != "" ? $("#materialSelectList").val().trim() : null;
                var optionElement = $("#materialSelectList").select2("data")[0]?.element;
                var materialName = $(optionElement).data("name");

                var productMaterial = {
                    grant_type: "password",
                    productId: productId,
                    materialId: materialId,
                    materialQuantity: materialQuantity,
                    materialName: materialName,
                    productName: productName
                }

                addTempDataProductMaterial(productMaterial);

            }
        },
        rules: {
            'materialQuantity': {
                required: true,
                min: 1,
                number: true
            },
            'materialSelectList': {
                required: true
            }
        },
        messages: {
            'materialQuantity': {
                required: 'Malzeme Miktarı alanını doldurunuz!',
                min: "Malzeme Miktarı 0 dan büyük olmalıdır!",
                number: "Lütfen sayısal bir değer giriniz!"
            },
            "materialSelectList": 'Lütfen malzeme seçiniz!'
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getMaterialList(data.apiUrl);

        $('#saveProductMaterialList').click(function () {
            addProductMaterialList(data.apiUrl);
        });

    });


    addTempDataProductMaterial();

});

//Malzeme Lİstesi
function getMaterialList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Material/ActivesMaterialList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#materialSelectList").empty();
            var materialSelectListOption = '<option></option>'

            $.each(data, function (key, value) {

                materialSelectListOption += '<option data-name="' + value.MaterialName + '" value="' + value.Id + '">' + value.MaterialName + '</option>';
            });

            $("#materialSelectList").append(materialSelectListOption);
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

// TempDatadan Ürün Ekleme
function addTempDataProductMaterial(productMaterialData) {

    // Local Storage Ekleme
    var productMaterialList = JSON.parse(localStorage.getItem("materialList"));

    if (productMaterialList != null && productMaterialList.length > 0) {
        if (productMaterialData != null) {
            productMaterialList.push(productMaterialData);
        }
    } else {
        if (productMaterialData != null) {
            productMaterialList = [];
            productMaterialList.push(productMaterialData);
        }

    }

    localStorage.setItem("materialList", JSON.stringify(productMaterialList));


    // DataTable Ekleme
    $('#tempProductMaterialTableBody').empty();

    $.each(productMaterialList, function (key, value) {
        var item = '<tr><td class="text-center">' + value.productName + '</td><td class="d-none d-sm-table-cell">' + value.materialName + '</td><td class="fw-semibold">' + value.materialQuantity + '</td><td class="text-center"><button type="button" class="btn btn-sm btn-danger" data-bs-toggle="tooltip" onclick="questionModal(' + value.materialId + ')" title="Sil"><i class="fa fa-trash"></i></button></td></tr>';

        $('#tempProductMaterialTableBody').append(item);
    });

    initDataTable();
    tooltip();

    $("#materialQuantity").val("");
    $("#materialSelectList").val("").trigger("change");
}

//Ürün ve Malzeme İlişkilendirme
function addProductMaterialList(apiUrl) {
    var token = localStorage.getItem("token");
    var productMaterialList = JSON.parse(localStorage.getItem("materialList"));

    if (productMaterialList != null && productMaterialList.length > 0) {

        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: apiUrl + "ProductMaterial/AddProductMaterial",
            type: 'POST',
            headers: { "Authorization": "bearer " + token },
            data: JSON.stringify(productMaterialList),
            success: function (data) {
                if (data != undefined && data != null && data.Id > 0) {
                    Codebase.helpers('jq-notify', {
                        align: 'right',             // 'right', 'left', 'center'
                        from: 'top',                // 'top', 'bottom'
                        type: 'success',               // 'info', 'success', 'warning', 'danger'
                        icon: 'fa fa-check me-5',    // Icon class
                        message: "Kayıt İşlemi Başarılı :)",
                    });

                    localStorage.removeItem("materialList");
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
        retrieve: true,
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

            var productMaterialList = JSON.parse(localStorage.getItem("materialList"));

            if (productMaterialList != null && productMaterialList.length > 0) {
                const indexOfObject = productMaterialList.findIndex(object => {
                    return object.productId == id;
                });

                productMaterialList.splice(indexOfObject, 1);

                localStorage.setItem("materialList", JSON.stringify(productMaterialList));
                addTempDataProductMaterial();
            }
        }
    });
}