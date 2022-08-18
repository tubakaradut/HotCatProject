$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var productId = searchParam.get("id");

    Codebase.helpers("jq-validation"), jQuery("#updateProductForm").validate({
        submitHandler: function (form) {
            if ($("#updateProductForm").valid()) {
                var productName = $("#productName").val().trim();
                var productDescription = $("#productDescription").val().trim();
                var productUnitPrice = $("#productUnitPrice").val().trim();
                var productCategory = $("#productCategory").val().trim() != "" ? $("#productCategory").val().trim() : null;

                var productData = {
                    grant_type: "password",
                    id: productId,
                    productName: productName,
                    description: productDescription,
                    unitPrice: productUnitPrice,
                    categoryId: productCategory
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    updateProduct(productData, data.apiUrl);
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
            }
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getCategoryList(data.apiUrl, productId);
    });
});


// Kategorilerin Listelenmesi
function getCategoryList(apiUrl, productId) {

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
            getProduct(productId, apiUrl);
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

// Ürün Getirme
function getProduct(productId, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Product/BringProduct/" + productId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                $("#productName").val(data.ProductName);
                $("#productDescription").val(data.Description);
                $("#productUnitPrice").val(data.UnitPrice);
                $("#productCategory").val(data.CategoryId);
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

//Ürün Güncelleme
function updateProduct(productData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Product/UpdateProduct",
        type: 'PUT',
        headers: { "Authorization": "bearer " + token },
        data: productData,
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