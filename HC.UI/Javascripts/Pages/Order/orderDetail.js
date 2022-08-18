$(document).ready(function () {
    var searchParam = new URLSearchParams(window.location.search);
    var tableId = searchParam.get("tableId");

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getTableList(data.apiUrl, tableId);
    });
});

//Masaları Listeleme
function getTableList(apiUrl, tableId) {

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
                var selectedId = tableId != null ? tableId : data[0].Id;
                var tableSelectListOption = '<option></option>'
                $.each(data, function (key, value) {
                    if (value.TableStatus == 2) {
                        tableSelectListOption += '<option disabled="true" value="' + value.Id + '">' + value.TableName + " ( " + value.TableStatusDisplayName + ' )</option>';
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

// Masa Id sine göre Sipariş Getirme
function getOrderByTableId(apiUrl, tableId) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Order/GetOrderByTableId/" + tableId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#orderDetailTableBody").empty();

            if (data != null && data.OrderDetails != null) {

                $.each(data.OrderDetails, function (key, value) {
                    var item = '<tr><td class="text-center">' + value.Id + '</td><td>' + data.OrderName + '</td><td>' + value.Product.ProductName + '</td><td>' + value.Quantity + ' Adet</td><td>' + value.Product.UnitPrice + ' ₺</td><td>' + value.TotalPrice + ' ₺</td><td class="text-center"><button type="button" onclick="updateOrderDetail(' + value.Id + ')" class="btn btn-sm btn-alt-warning" data-bs-toggle="tooltip" title="Siparişi Değiştir"><i class="fa fa-pencil"></i></button><button type="button" onclick="questionModal(' + value.Id + ',' + tableId + ')" class="btn btn-sm btn-danger" data-bs-toggle="tooltip" title="Siparişi İptal Et"><i class="fa fa-trash"></i></button></td></tr>';

                    $("#orderDetailTableBody").append(item);
                });

            }

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

//Sipariş Detay Silme
function deleteOrderDetail(id, tableId, apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "OrderDetail/DeleteOrderDetail/" + id,
        type: 'DELETE',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null) {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'success',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-check me-5',    // Icon class
                    message: "İptal İşlemi Başarılı :)",
                });
            }
            setTimeout(function () {
                window.location.href = "/Order/Detail?tableId=" + tableId
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
function questionModal(id, tableId) {
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
        title: 'Siparişi iptal etmek istediğinize emin misiniz?',
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
                deleteOrderDetail(id, tableId, data.apiUrl);
            });

        }
    });
}