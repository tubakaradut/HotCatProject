$(document).ready(function () {

    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getPageList(data.apiUrl);
    });
});

//Sayfaları Listeleme
function getPageList(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Page/ActivesPageList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#pageTableBody").empty();

            $.each(data, function (key, value) {

                var isExistSubMenu = value.IsExistSubMenu ? "Evet" : "Hayır";
                var IsMainPage = value.IsMainPage ? "Evet" : "Hayır";

                var item = '<tr><td class="text-center">' + value.Id + '</td><td class="fw-semibold">' + value.Name + '</td><td class="d-none d-sm-table-cell">' + value.Path + '</td><td class="d-none d-sm-table-cell">' + isExistSubMenu + '</td><td class="d-none d-sm-table-cell">' + IsMainPage + '</td><td class="d-none d-sm-table-cell">' + value.Roles + '</td><td class="text-center"><button type="button" onclick="pageRole(' + value.Id + ',\'' + value.Name + '\')" class="btn btn-sm btn-alt-info pageRol me-1" data-bs-toggle="tooltip" title="Rol ile İlişkilendir"><i class="fa fa-circle-user"></i></button></td></tr>';


                $("#pageTableBody").append(item);
            });

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

//Sayfaya Göre Rolleri Getirme
function pageRole(id, pageName) {
    if (id != undefined && id != null) {
        window.location.href = "/Page/PageRole?pageId=" + id + "&pageName=" + pageName;
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