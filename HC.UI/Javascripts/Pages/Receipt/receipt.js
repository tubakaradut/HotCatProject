$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var receiptId = searchParam.get("Id");

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getReceipt(data.apiUrl, receiptId);
    });
});


// Fiş Getirme
function getReceipt(apiUrl, receiptId) {
    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Receipt/BringReceipt/" + receiptId,
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            if (data != undefined && data != null && data.Id > 0) {
                $('#receiptNumber').text(data.ReceiptNumber);
                $('#companyName').text(data.Company.CompanyName);
                $('#address').text("Adres: " + data.Company.Address);
                $('#phoneNumber').text("Telefon: " + data.Company.PhoneNumber);
                $('#taxNo').text("Vergi No: " + data.Company.TaxNo);


                // Sipariş Detay
                var orderLine = '';
                var subTotal = 0;
                $.each(data.Order.OrderDetails, function (key, value) {
                    orderLine += '<tr><td class="text-center">' + value.Id + '</td ><td><p class="fw-semibold mb-1">' + value.Product.ProductName + '</p><div class="text-muted">' + value.Product.Description + '</div></td><td class="text-center"><span class="badge rounded-pill bg-primary">' + value.Quantity + '</span></td><td class="text-end">' + value.Product.UnitPrice + ' ₺</td><td class="text-end">' + value.TotalPrice + ' ₺</td></tr>';

                    subTotal += value.TotalPrice;
                });

                $('#orders').append(orderLine);

                var subTotalLine = '<tr><td colspan = "4" class="fw-semibold text-end"> Ara Toplam</td><td class="text-end">' + subTotal + ' ₺</td></tr>';

                $('#orders').append(subTotalLine);

                var discount = '<tr><td colspan="4" class="fw-semibold text-end"> İndirim Oranı</td><td class="text-end">% ' + data.Discount + '</td></tr>';

                $('#orders').append(discount);

                var totalPrice = '<tr class="table-warning"><td colspan="4" class="fw-bold text-end">TOPLAM FİYAT</td><td class="fw-bold text-end">' + data.TotalPrice + ' ₺</td></tr>';

                $('#orders').append(totalPrice);

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