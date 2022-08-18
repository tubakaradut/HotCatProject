$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var categoryId = searchParam.get("id");

    Codebase.helpers("jq-validation"), jQuery("#updateCategoryForm").validate({
        submitHandler: function () {

            if ($("#updateCategoryForm").valid()) {
                var categoryName = $("#categoryName").val().trim();
                var categoryDescription = $("#categoryDescription").val().trim();
                var categoryParentId = $("#categoryParentId").val().trim() != "" ? $("#categoryParentId").val().trim() : null;
                var isExistSubCategory = $("#isExistSubCategory").is(':checked');

                var categoryData = {
                    grant_type: "password",
                    id: categoryId,
                    categoryName: categoryName,
                    description: categoryDescription,
                    isExistSubCategory: isExistSubCategory,
                    parentId: categoryParentId
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    updateCategory(categoryData, data.apiUrl);
                });
            }
        },
        rules: {
            'categoryName': {
                required: true,
                minlength: 3
            },
            'categoryDescription': {
                minlength: 5
            }
        },
        messages: {
            'categoryName': {
                required: 'Kategori Adı alanını doldurunuz!',
                minlength: 'Kategori Adı alanı 3 karakterden büyük olmalıdır!'
            },
            'categoryDescription': {
                minlength: 'Açıklama alanı 3 karakterden büyük olmalıdır!'
            }
        }
    })

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getCategoryList(data.apiUrl, categoryId);
    });
});

//Kategorilerin Listelenemesi
function getCategoryList(apiUrl, categoryId) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Category/ActivesCategoryList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#categoryParentId").empty();

            var categorySelectListOption = '<option value="null">Yok</option>'

            $.each(data, function (key, value) {

                categorySelectListOption += '<option value="' + value.Id + '">' + value.CategoryName + '</option>';
            });

            $("#categoryParentId").append(categorySelectListOption);
            getCategory(categoryId, apiUrl);
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

//Kategori Getirme
function getCategory(categoryId, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Category/BringCategory/" + categoryId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                $("#categoryName").val(data.CategoryName);
                $("#categoryDescription").val(data.Description);
                $("#categoryParentId").val(data.ParentId);
                $("#isExistSubCategory").prop('checked', data.IsExistSubCategory);
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


//Kategori Güncelleme
function updateCategory(categoryData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Category/UpdateCategory",
        type: 'PUT',
        headers: { "Authorization": "bearer " + token },
        data: categoryData,
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