

var isAuthorized = false;
$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var type = searchParam.get("type");

    Codebase.helpers("jq-validation"), jQuery("#addProductForm").validate({
        submitHandler: function () {
            if ($("#addProductForm").valid()) {

                var productName = $("#productName").val().trim();
                var productDescription = $("#productDescription").val().trim();
                var productUnitPrice = $("#productUnitPrice").val().trim();
                var productCategory = $("#productCategory").val().trim() != "" ? $("#productCategory").val().trim() : null;

                var productData = {
                    grant_type: "password",
                    productName: productName,
                    description: productDescription,
                    unitPrice: productUnitPrice,
                    categoryId: productCategory
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    addProduct(productData, data.apiUrl);
                });
            }
        },
        rules: {
            'productName': {
                required: true,
                minlength: 3
            },
            'productDescription': {
                minlength: 5
            },
            'productUnitPrice': {
                required: true,
                min: 1,
                number: true
            },
            'productCategory': {
                required: true
            }
        },
        messages: {
            'productName': {
                required: 'Ürün Adı alanını doldurunuz!',
                minlength: 'Ürün Adı alanı 3 karakterden büyük olmalıdır!'
            },
            'productDescription': {
                minlength: 'Açıklama alanı 5 karakterden büyük olmalıdır!'
            },
            'productUnitPrice': {
                required: 'Ürün Fiyatı doldurunuz!',
                min: "Fiyat 0 dan büyük olmalıdır!",
                number: "Lütfen sayısal bir değer giriniz!"
            },
            "productCategory": 'Lütfen kategori seçiniz!'
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getUserRoles(data);
        getProductList(data.apiUrl, type);
        getCategoryList(data.apiUrl);
        getMaterialList(data.apiUrl);


    });
});

//Kullanıcı Rollerini Getirme
function getUserRoles(config) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: config.apiUrl + "Authentication/GetUserRolesInSystem",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $.each(data, function (key, value) {
                if (config.userRole.Manager == value || config.userRole.Admin == value) {
                    isAuthorized = true;
                }
            });
        },
        error: function (xhr) {

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

//Ürünleri Listeleme
function getProductList(apiUrl, type) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Product/ActivesProductList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#productTableBody").empty();

            $.each(data, function (key, value) {

                var item = '<tr><td class="text-center">' + value.Id + '</td><td class="fw-semibold">' + value.ProductName + '</td><td class="d-none d-sm-table-cell">' + value.Description + '</td><td class="d-none d-sm-table-cell">' + value.UnitPrice + ' ₺</td><td class="text-center">' + value.Category.CategoryName + '</td><td class="text-center">' + value.ProductQuantity + '</td><td class="text-center process"><button type="button" onclick="addProductMaterial(' + value.Id + ',\'' + value.ProductName + '\')" class="btn btn-sm btn-alt-info addProductMaterial me-1" data-bs-toggle="tooltip" title="Malzemeler ile İlişkilendir"><i class="fa fa-link"></i></button><button id="requestProduct-' + value.Id + '" type="button" onclick="requestProduct(' + value.Id + ',\'' + apiUrl + '\')" class="btn btn-sm btn-alt-primary requestProduct me-1" data-bs-toggle="tooltip" title="Ürün Talep Et"><i class="fa fa-hand-point-up"></i></button><button type="button" data-bs-toggle="modal" data-bs-target="#materialList" id="product-' + value.Id + '" class="btn btn-sm btn-alt-success materialList me-1" data-bs-toggle="tooltip" title="Malzeme Listesi"><i class="fa fa-list"></i></button><button type="button" onclick="updateProduct(' + value.Id + ')" class="btn btn-sm btn-alt-warning updateProduct me-1" data-bs-toggle="tooltip" title="Düzenle"><i class="fa fa-pencil"></i></button><button type="button" onclick="questionModal(' + value.Id + ')" class="btn btn-sm btn-danger deleteProduct" data-bs-toggle="tooltip" title="Sil"><i class="fa fa-trash"></i></button></td></tr> ';

                $("#productTableBody").append(item);

                $('#product-' + value.Id).click(function () {
                    $('#requestMaterial').attr("data-productId", value.Id);
                });

                if (isAuthorized) {
                    $('.updateProduct, .deleteProduct, .materialList, .addProductMaterial').css('display', 'inline-block');

                    $('#requestProduct-' + value.Id).css('display', 'none');
                }
                else {
                    $('.updateProduct, .deleteProduct, .materialList, .addProductMaterial').css('display', 'none');

                    $('#requestProduct-' + value.Id).css('display', 'inline-block');


                    if (value.ProductQuantity <= 3) {
                        $('#requestProduct-' + value.Id).css('display', 'inline-block');
                    } else {
                        $('#requestProduct-' + value.Id).css('display', 'none');
                    }
                }


            });


            initDataTable();
            tooltip();

            if (isAuthorized && type == 'report') {
                $('#process, .process').css('display', 'none');
            } else if (!isAuthorized && type == 'report') {
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

// Kategorilerin Listelenmesi
function getCategoryList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Category/ActivesCategoryList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#productCategory").empty();

            var productCategoryOption = '<option value="">-</option>'

            $.each(data, function (key, value) {

                productCategoryOption += '<option value="' + value.Id + '">' + value.CategoryName + '</option>';

            });

            $("#productCategory").append(productCategoryOption);

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

//Ürün Ekleme
function addProduct(productData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Product/AddProduct",
        type: 'POST',
        headers: { "Authorization": "bearer " + token },
        data: productData,
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

//Ürün Silme
function deleteProduct(id, apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Product/DeleteProduct/" + id,
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

// Ürün Güncellleme
function updateProduct(id) {

    if (id != undefined && id != null) {
        window.location.href = "/Product/Update?id=" + id;
    }
}

//Malzeme Listleme
function getMaterialList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Material/ActivesMaterialList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#materialTableBody").empty();
            $.each(data, function (key, value) {

                var stockStatus = value.CurrentStock <= value.MinStock ? "Az" : "Normal";
                var stockStatusColor = value.CurrentStock <= value.MinStock ? "bg-danger" : "bg-success";

                var item = '<tr><td>' + value.MaterialName + '</td><td>' + value.UnitTypeName + '</td><td>' + moment(value.ExpirationDate, 'YYYY-MM-DD').format('DD-MM-YYYY') + '</td><td>' + value.MinStock + '</td><td>' + value.CurrentStock + '</td><td>' + value.MaxStock + '</td><td class="d-none d-sm-table-cell">                    <span class="badge ' + stockStatusColor + '"> ' + stockStatus + '</span></td></tr>';

                $("#materialTableBody").append(item);
            });

            $('#requestMaterial').click(function () {
                var productId = $(this).data("productid");
                requestMaterial(productId, apiUrl);
            });
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

// Ürüne Göre Malzeme İsteme
function requestMaterial(productId, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Material/RequestMaterial?ProductId=" + productId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null) {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'success',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-check me-5',    // Icon class
                    message: data,
                });

                $('#materialList').modal("hide");
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

// Ürün İsteme
function requestProduct(id, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Product/RequestProduct/" + id,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null) {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'success',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-check me-5',    // Icon class
                    message: data,
                });
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

// Ürüne Göre Malzeme Ekleme
function addProductMaterial(id, productName) {
    if (id != undefined && id != null) {
        window.location.href = "/ProductMaterial/AddMaterial?productId=" + id + "&productName=" + productName;
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
                deleteProduct(id, data.apiUrl);
            });
        }
    });
}