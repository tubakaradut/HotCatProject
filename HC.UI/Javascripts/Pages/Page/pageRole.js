
$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var pageId = searchParam.get("pageId");
    var pageName = searchParam.get("pageName");

    $('#pageName').val(pageName);
    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getRoleList(data.apiUrl);
    });


    Codebase.helpers("jq-validation"), jQuery("#addPageRoleForm").validate({
        submitHandler: function () {
            if ($("#addPageRoleForm").valid()) {

                var roleList = $("#roleList").val();

                var pageRoleData = {
                    grant_type: "password",
                    pageId: pageId,
                    RoleList: roleList,
                }

                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    addPageRole(pageRoleData, data.apiUrl);
                });
            }
        },
        rules: {
            'roleList': {
                required: true
            }
        },
        messages: {
            "roleList": 'Lütfen Rol seçiniz!'
        }
    })
});


// Tüm Rollerin Listelenmesi
function getRoleList(apiUrl) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "Enum/RoleList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#roleList").empty();

            var roleListOption = '<option></option>';

            $.each(data, function (key, value) {
                roleListOption += '<option value="' + value.Id + '">' + value.DisplayName + '</option>';
            });

            $("#roleList").append(roleListOption);
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

//Sayfa Rol Ekleme
function addPageRole(pageRoleData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Page/AddPageRole",
        type: 'POST',
        headers: { "Authorization": "bearer " + token },
        data: pageRoleData,
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