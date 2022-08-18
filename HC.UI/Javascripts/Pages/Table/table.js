$(document).ready(function () {

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getTableLocationList(data.apiUrl);
    });
});

// Masa Lokasyonlarını Lİsteleme
function getTableLocationList(apiUrl) {

    var token = localStorage.getItem("token");
    $.ajax({
        url: apiUrl + "Enum/TableLocationList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#tableLocations").empty();
            $("#tableLocationsContent").empty();

            var tableLocationHeaderItem = '';
            var tableLocationContentItem = '';
            $.each(data, function (key, value) {

                if (value.Id == 1) {
                    tableLocationHeaderItem += '<li class="nav-item"><button class="nav-link active" id="btabs-alt-static-' + value.Name + '-tab" data-bs-toggle="tab" data-bs-target="#btabs-alt-static-' + value.Name + '" role="tab" aria-controls="btabs-alt-static-' + value.Name + '" aria-selected="true"> ' + value.DisplayName + '</button ></li>';

                    tableLocationContentItem += '<div class="tab-pane active" id="btabs-alt-static-' + value.Name + '" role="tabpanel" aria-labelledby="btabs-alt-static-' + value.Name + '-tab"><div class="row" id="tableLocationContent' + value.Name + '"></div></div>';


                } else {
                    tableLocationHeaderItem += '<li class="nav-item"><button class="nav-link" id="btabs-alt-static-' + value.Name + '-tab" data-bs-toggle="tab" data-bs-target="#btabs-alt-static-' + value.Name + '" role="tab" aria-controls="btabs-alt-static-' + value.Name + '" aria-selected="true"> ' + value.DisplayName + '</button ></li>';

                    tableLocationContentItem += '<div class="tab-pane" id="btabs-alt-static-' + value.Name + '" role="tabpanel" aria-labelledby="btabs-alt-static-' + value.Name + '-tab"><div class="row" id="tableLocationContent' + value.Name + '"></div></div >';
                }
            });

            tableLocationHeaderItem += '<li class="nav-item ms-auto"><div class="block-options ps-3 pe-2" ><button type="button" class="btn-block-option" data-toggle="block-option" onclick="getTableList(\'' + apiUrl + '\')" data-bs-toggle="tooltip" title="Yenile" data-action="state_toggle" data-action-mode="demo"><i class="si si-refresh"></i></button></div></li>';

            $("#tableLocations").append(tableLocationHeaderItem);
            $("#tableLocationsContent").append(tableLocationContentItem);

            tooltip();
            getTableList(apiUrl);
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

// Masaları Listeleme
function getTableList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "CafeTable/ActivesCafeTableList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            var tableBackgroundColor = 'bg-gray-dark'
            var tableLocationName = '';
            $.each(data, function (key, value) {
                var tableHref = 'javascript:void(0)';
                if (tableLocationName !== value.TableLocationName) {
                    tableLocationName = value.TableLocationName;
                    $("#tableLocationContent" + value.TableLocationName).empty();
                }


                switch (value.TableStatus) {
                    case 1:
                        tableHref = 'javascript:orderRedirect(' + value.Id + ')';
                        tableBackgroundColor = 'bg-earth';
                        break;
                    case 2:
                        tableBackgroundColor = 'bg-warning';
                        break;
                    case 3:
                        tableHref = 'javascript:orderRedirect(' + value.Id + ')';
                        tableBackgroundColor = 'bg-pulse';
                        break;
                }

                var table = '<div class="col-md-6 col-xl-3"><a class="block block-rounded shadow-none ' + tableBackgroundColor + '" href="' + tableHref + '" ><div class="block-content block-content-full text-center ribbon ribbon-left ribbon-modern ribbon-light"><div class="ribbon-box">' + value.TableStatusDisplayName + '</div><img src="../../Content/Images/table.png"/><p class="fs-lg fw-semibold text-white mb-0">' + value.TableName + '</p><p class="fs-sm fw-semibold text-white-75 mb-0">' + value.Capacity + ' Kişilik</p></div></a></div>';


                $("#tableLocationContent" + value.TableLocationName).append(table);
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

// Siparişe Yönlendirme
function orderRedirect(tableId) {
    if (tableId != undefined && tableId != null) {
        window.location.href = "/Order/Index?tableId=" + tableId;
    }
}

// tooltip
function tooltip() {
    let e = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]:not(.js-bs-tooltip-enabled), .js-bs-tooltip:not(.js-bs-tooltip-enabled)'));
    window.helperBsTooltips = e.map((e => (e.classList.add("js-bs-tooltip-enabled"), new bootstrap.Tooltip(e, {
        container: e.dataset.bsContainer || "#page-container",
        animation: !(!e.dataset.bsAnimation || "true" != e.dataset.bsAnimation.toLowerCase())
    }))))
}

