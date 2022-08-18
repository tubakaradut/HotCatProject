$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var companyId = searchParam.get("id");

    Codebase.helpers("jq-validation"), jQuery("#updateCompanyForm").validate({
        submitHandler: function (form) {

            if ($("#updateCompanyForm").valid()) {
                var companyName = $("#companyName").val().trim();
                var address = $("#address").val().trim();
                var taxNo = $("#taxNo").val().trim();
                var phoneNumber = $("#phoneNumber").val().trim();


                var companyData = {
                    grant_type: "password",
                    id: companyId,
                    companyName: companyName,
                    address: address,
                    taxNo: taxNo,
                    phoneNumber: phoneNumber
                }
                $.getJSON(window.location.origin + '/config.json').then(function (data) {
                    updateCompany(companyData, data.apiUrl);
                });
            }
        },
        rules: {
            'companyName': {
                required: true,
                minlength: 3
            },
            'address': {
                minlength: 5
            },
            'phoneNumber': {
                required: true,
            }
        },
        messages: {
            'companyName': {
                required: 'Şirket Adı alanını doldurunuz!',
                minlength: 'Şirket Adı alanı 3 karakterden büyük olmalıdır!'
            },
            'address': {
                minlength: 'Açıklama alanı 5 karakterden büyük olmalıdır!'
            },
            'phoneNumber': {
                required: 'Şirket Telefon Numara alanını doldurunuz!',
            }
        }
    });
    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getCompany(data.apiUrl, companyId);
    })

});


//Şirketleri Listeleme
function getCompany(apiUrl, companyId) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Company/BringCompany/" + companyId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                $("#companyName").val(data.CompanyName);
                $("#address").val(data.Address);
                $("#taxNo").val(data.TaxNo);
                $("#phoneNumber").val(data.PhoneNumber);
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
//Şirket Güncelleme
function updateCompany(apiUrl, companyData) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Company/UpdateCompany",
        type: 'PUT',
        headers: { "Authorization": "bearer " + token },
        data: companyData,
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