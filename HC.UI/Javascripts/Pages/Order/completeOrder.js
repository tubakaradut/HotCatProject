$(document).ready(function () {

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getTableList(data.apiUrl);
        getPaymentTypeList(data.apiUrl);

        $('#completeOrder').click(function () {
            var tableId = $("#tableSelectList").val();
            var discount = $("#discount").val();
            var paymentType = $("#paymentType").val();

            var completeOrderData = {
                cafeTableId: tableId,
                discount: discount,
                paymentTypeId: paymentType
            }

            completeOrder(completeOrderData, data.apiUrl);

        });
    });
});

// Masaların Listenmesi
function getTableList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "CafeTable/ActivesCafeTableList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != null) {

                // Masa TableLocation bilgisine göre apiden sıralı geliyor. Fakat burada dropdown içerisinde farklı sırada görünmemesi için id bazlı sıralama yapılıyor.
                data.sort(function (a, b) {
                    var a1 = a.Id, b1 = b.Id;
                    if (a1 == b1) return 0;
                    return a1 > b1 ? 1 : -1;
                });

                $("#tableSelectList").empty();

                var selectedId = data[0].Id;

                var tableSelectListOption = '<option></option>'

                $.each(data, function (key, value) {
                    if (value.TableStatus == 2) {
                        tableSelectListOption += '<option disabled value="' + value.Id + '">' + value.TableName + " ( " + value.TableStatusDisplayName + ' )</option>';
                    } else {
                        tableSelectListOption += '<option value="' + value.Id + '">' + value.TableName + " ( " + value.TableStatusDisplayName + ' )</option>';
                    }
                });

                getOrderByTableId(apiUrl, selectedId)

                $("#tableSelectList").append(tableSelectListOption);
                $("#tableSelectList").val(selectedId);

                $("#tableSelectList").on("change", function (e) {
                    var tableId = $("#tableSelectList").val();

                    getOrderByTableId(apiUrl, tableId)
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

// Ödeme Tiplerini Listeleme
function getPaymentTypeList(apiUrl) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "Enum/PaymentTypeList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {

            var paymentTypeOption = '';

            $.each(data, function (key, value) {
                paymentTypeOption += '<option value="' + value.Id + '">' + value.DisplayName + '</option>';
            });

            $("#paymentType").append(paymentTypeOption);
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

// Masa ID sine göre Sipariş Getirme
function getOrderByTableId(apiUrl, tableId) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Order/GetOrderByTableId/" + tableId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {

            $("#orderDetailTableBody").empty();

            if (data != null && data.OrderDetails != null && data.OrderDetails.length > 0) {

                $('#completeOrderBtn').css("display", "inline-block");

                $.each(data.OrderDetails, function (key, value) {
                    var item = '<tr><td class="text-center">' + value.Id + '</td><td>' + data.OrderName + '</td><td>' + value.Product.ProductName + '</td><td>' + value.Quantity + ' Adet</td><td>' + value.Product.UnitPrice + ' ₺</td><td>' + value.TotalPrice + ' ₺</td></tr>';

                    $("#orderDetailTableBody").append(item);
                });
            } else {
                $('#completeOrderBtn').css("display", "none");
            }

            initDataTable();
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

// Sipariş Kapatma
function completeOrder(completeOrderData, apiUrl) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Order/CompleteOrder",
        type: 'POST',
        headers: { "Authorization": "bearer " + token },
        data: completeOrderData,
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'success',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-check me-5',    // Icon class
                    message: "Hesap Kapatma İşlemi Başarılı 😊",
                });

                setTimeout(function () {
                    openReceipt(data);
                }, 2000);

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

// Fiş Açma
function openReceipt(data) {
    if (data != undefined && data != null) {
        window.location.href = "/Receipt/Index?Id=" + data.Id;
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