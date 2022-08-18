$(document).ready(function () {

    var searchParam = new URLSearchParams(window.location.search);
    var tableId = searchParam.get("tableId");

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getTableList(tableId, data.apiUrl);
        getOrderLocalStorage(tableId);
    });
});

// Masaları Listeleme
function getTableList(tableId, apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "CafeTable/ActivesCafeTableList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {

            // Masa TableLocation bilgisine göre apiden sıralı geliyor. Fakat burada dropdown içerisinde farklı sırada görünmemesi için id bazlı sıralama yapılıyor.
            data.sort(function (a, b) {
                var a1 = a.Id, b1 = b.Id;
                if (a1 == b1) return 0;
                return a1 > b1 ? 1 : -1;
            });

            $("#tableSelectList").empty();
            $('#orderTitleDescription').empty();
            var tableSelectListOption = '<option></option>'
            $.each(data, function (key, value) {
                if (tableId == value.Id && value.TableStatus == 3) {
                    $('#orderTitleDescription').text('Bu masadan daha önce sipariş alınmıştır.');
                }

                if (value.TableStatus == 2) {
                    tableSelectListOption += '<option disabled="true"  tablestatus="' + value.TableStatus + '" value="' + value.Id + '">' + value.TableName + " ( " + value.TableStatusDisplayName + ' )</option>';
                } else {
                    tableSelectListOption += '<option tablestatus="' + value.TableStatus + '" value="' + value.Id + '">' + value.TableName + '</option>';
                }
            });


            $("#tableSelectList").append(tableSelectListOption);
            $("#tableSelectList").val(tableId);

            $("#tableSelectList").on("change", function (e) {
                var tableId = $("#tableSelectList").val();

                var optionElement = $(this).select2("data")[0]?.element;
                var tableStatus = $(optionElement).attr("tablestatus");

                if (tableStatus == 3) {
                    $('#orderTitleDescription').text('Bu masadan daha önce sipariş alınmıştır.');
                } else {
                    $('#orderTitleDescription').empty();
                }
                getOrderLocalStorage(tableId);
            });

            getCategoryList(apiUrl);
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

//Kategorileri Listeleme
function getCategoryList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Category/ActivesCategoryList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#categoryContainer").addClass("block-mode-loading");
            $("#category").empty();
            $("#categoryContent").empty();

            $.each(data, function (key, value) {

                if (value.Id == 1) {
                    var categoryHeaderItem = '<li class="nav-item"><button class="nav-link active" id="btabs-alt-static-' + value.Id + '-tab" data-bs-toggle="tab" data-bs-target="#btabs-alt-static-' + value.Id + '" role="tab" aria-controls="btabs-alt-static-' + value.Id + '" aria-selected="true"> ' + value.CategoryName + '</button ></li>';

                    var categoryContentItem = '<div class="tab-pane active" id="btabs-alt-static-' + value.Id + '" role="tabpanel" aria-labelledby="btabs-alt-static-' + value.Id + '-tab"><div class="row" id="categoryContent' + value.Id + '"></div></div>';
                }
                else {
                    var categoryHeaderItem = '<li class="nav-item"><button class="nav-link" id="btabs-alt-static-' + value.Id + '-tab" data-bs-toggle="tab" data-bs-target="#btabs-alt-static-' + value.Id + '" role="tab" aria-controls="btabs-alt-static-' + value.Id + '" aria-selected="true"> ' + value.CategoryName + '</button ></li>';

                    var categoryContentItem = '<div class="tab-pane" id="btabs-alt-static-' + value.Id + '" role="tabpanel" aria-labelledby="btabs-alt-static-' + value.Id + '-tab"><div class="row" id="categoryContent' + value.Id + '"></div></div >';
                }

                $("#category").append(categoryHeaderItem);
                $("#categoryContent").append(categoryContentItem);


                $.each(value.Products, function (productKey, productValue) {


                    var isDisabled = productValue.ProductQuantity > 0 ? '' : 'disabled';
                    var product = '<div class="col-md-6 col-xl-3"><a class="block block-rounded block-fx-shadow text-center" href="javascript:void(0)"><div class="block-content block-content-full block-content-sm bg-corporate-dark"><div class="fw-semibold text-white">' + productValue.ProductName + '</div></div><div class="block-content block-content-full bg-image ribbon ribbon-left ribbon-bookmark ribbon-success" style="background-image: url(\'' + 'http://cdn.onlinewebfonts.com/svg/img_546302.png' + '\');height:150px"><div class="ribbon-box">' + productValue.UnitPrice + ' ₺</div></div><div class="block-content block-content-full block-content-sm bg-gray-light"><div class="fs-sm text-white d-sm-flex justify-content-sm-between align-items-center"><button type="button" class="btn btn-sm btn-outline-success me-1" ' + isDisabled + ' onClick="addProductToOrderLocalStorage(' + productValue.Id + ',\'' + productValue.ProductName + '\')" data-bs-toggle="tooltip" data-bs-placement="top" title="Siparişe Ekle"><i class="fa fa-fw fa-plus" ></i></button><div class="fs-sm text-muted">Stok: ' + productValue.ProductQuantity + '</div><button ' + isDisabled + ' type="button" class="btn btn-sm btn-outline-danger me-1" data-bs-toggle="tooltip" data-bs-placement="top" onClick="removeProductToOrderLocalStorage(' + productValue.Id + ',' + false + ')" title="Siparişten Çıkar"><i class="fa fa-fw fa-times"></i></button></div></div><div class="block-content block-content-full h-120"><div class="fs-sm text-muted">' + productValue.Description + '</div></div></a></div>';

                    $("#categoryContent" + value.Id).append(product);
                });

            });

            var categoryHeaderItem = '<li class="nav-item ms-auto"><div class="block-options ps-3 pe-2" ><button type="button" class="btn-block-option" data-toggle="block-option" onclick="getCategoryList(\'' + apiUrl + '\')" data-bs-toggle="tooltip" title="Yenile" data-action="state_toggle" data-action-mode="demo"><i class="si si-refresh"></i></button></div></li>';

            $("#category").append(categoryHeaderItem);

            $("#categoryContainer").removeClass("block-mode-loading");

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

// Local Storageden Sipariş Edilecek Ürünü Ekleme
function addProductToOrderLocalStorage(productId, productName) {
    var tableId = $("#tableSelectList").val();
    var orderLocalStorage = JSON.parse(localStorage.getItem("tableOrder" + tableId));

    var order = {
        products: [],
        tableId: null
    }

    if (orderLocalStorage != null) {

        var existProduct = orderLocalStorage.products?.find(x => x.productId == productId);
        if (existProduct != undefined && existProduct != null) {
            existProduct.quantity += 1;
        } else {
            orderLocalStorage.products.push({ productId: productId, productName: productName, quantity: 1 });
        }

        localStorage.setItem("tableOrder" + tableId, JSON.stringify(orderLocalStorage));
    } else {
        order.tableId = tableId
        order.products.push({ productId: productId, productName: productName, quantity: 1 });
        localStorage.setItem("tableOrder" + tableId, JSON.stringify(order));
    }

    getOrderLocalStorage(tableId);
}

// Local Storageden Sipariş Edilecek Ürünü Silme
function removeProductToOrderLocalStorage(productId, isAllQuantity) {
    var tableId = $("#tableSelectList").val();
    var orderLocalStorage = JSON.parse(localStorage.getItem("tableOrder" + tableId));

    if (orderLocalStorage != null) {

        var existProduct = orderLocalStorage.products?.find(x => x.productId == productId);
        if (existProduct != undefined && existProduct != null) {
            if (isAllQuantity) {
                var index = orderLocalStorage.products?.indexOf(existProduct)
                orderLocalStorage.products?.splice(index, 1);
            } else {
                if (existProduct.quantity > 1) {
                    existProduct.quantity -= 1;
                } else {
                    var index = orderLocalStorage.products?.indexOf(existProduct)
                    orderLocalStorage.products?.splice(index, 1);
                }
            }
        }

        localStorage.setItem("tableOrder" + tableId, JSON.stringify(orderLocalStorage));
    }

    getOrderLocalStorage(tableId);
}

// Local Storageden Sipariş Getirme
function getOrderLocalStorage(tableId) {

    var order = JSON.parse(localStorage.getItem('tableOrder' + tableId));

    if (order != undefined && order != null && order.products.length > 0) {
        $('#orderContainer').removeClass('d-none');
        var numberOfProducts = 0;
        var productsToOrder = '';
        $.each(order.products, function (key, value) {
            numberOfProducts += value.quantity;
            productsToOrder += '<li><a class="text-dark d-flex py-2" href="javascript:void(0)"><div class="flex-shrink-0 me-2 ms-3"></div><div class="flex-grow-1 pe-2"><p class="fw-medium mb-1">' + value.productName + '</p><div class="text-muted">' + value.quantity + ' Adet</div></div><button type="button" onClick="removeProductToOrderLocalStorage(' + value.productId + ',' + true + ')"" class="btn btn-alt-danger me-2 mb-2"><i class="fa fa-trash"></i></button></a></li>';
        });



        $('#orderInfoText').text(numberOfProducts + ' Ürün');

        $('#productsToOrder').empty();
        $('#productsToOrder').append(productsToOrder);

        $('#saveOrder').unbind('click');
        $('#saveOrder').click(function () {
            saveOrder(tableId);
        });
    } else {
        $('#orderContainer').addClass('d-none');
    }
}

//Siparişi Kaydetme
function saveOrder(tableId) {

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        addOrder(tableId, data.apiUrl);
    });
}

function addOrder(tableId, apiUrl) {

    var token = localStorage.getItem("token");
    var order = JSON.parse(localStorage.getItem('tableOrder' + tableId));

    var orderData = {
        cafeTableId: tableId,
        orderDetails: []
    }
    if (order != undefined && order != null && order.products != undefined && order.products != null) {

        $.each(order.products, function (key, value) {
            orderData.orderDetails.push({ productId: value.productId, quantity: value.quantity });
        });
    }

    $.ajax({
        url: apiUrl + "Order/AddOrder",
        type: 'POST',
        headers: { "Authorization": "bearer " + token },
        data: orderData,
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

            localStorage.removeItem("tableOrder" + tableId);
            getOrderLocalStorage(tableId);

            setTimeout(function () {
                window.location.href = window.location.origin + "/Table/Index";
            }, 3000);
        },
        error: function (xhr, ajaxOptions, throwError) {

            if (xhr.status != null && xhr.status == 401) {
                window.location.href = window.location.origin + "/Error/AuthenticationError";
            }
            var error = xhr.responseJSON;
            if (error != undefined && error.error_description != undefined && error.error_description != null && error.error_description != "") {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'danger',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-warning me-5',    // Icon class
                    message: error.error_description
                });

            }
            if (error != undefined && error.Message != undefined && error.Message != null && error.Message != "") {
                Codebase.helpers('jq-notify', {
                    align: 'right',             // 'right', 'left', 'center'
                    from: 'top',                // 'top', 'bottom'
                    type: 'danger',               // 'info', 'success', 'warning', 'danger'
                    icon: 'fa fa-warning me-5',    // Icon class
                    message: error.Message
                });
            }
        }
    });
}

function tooltip() {
    let e = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]:not(.js-bs-tooltip-enabled), .js-bs-tooltip:not(.js-bs-tooltip-enabled)'));
    window.helperBsTooltips = e.map((e => (e.classList.add("js-bs-tooltip-enabled"), new bootstrap.Tooltip(e, {
        container: e.dataset.bsContainer || "#page-container",
        animation: !(!e.dataset.bsAnimation || "true" != e.dataset.bsAnimation.toLowerCase())
    }))))
}