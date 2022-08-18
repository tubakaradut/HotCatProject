
$(document).ready(function () {
    $.getJSON(window.location.origin + '/config.json').then(function (data) {
        getSidebarMenus(data.apiUrl);
    });
});

function getSidebarMenus(apiUrl) {

    var token = localStorage.getItem("token");

    $.ajax({
        url: apiUrl + "Page/PageList",
        type: 'GET',
        headers: { "Authorization": "bearer " + token },
        success: function (data) {
            $("#sidebarMenu").empty();
            $.each(data, function (key, value) {
                var menuItem = '<li class="nav-main-item"><a class="nav-main-link" href="' + value.Path +'"><span class="nav-main-link-name">' + value.Name + '</span></a></li>';

                if (value.IsExistSubMenu)
                {
                    createSubMenu(data, value);
                }
                else
                {
                    if (value.ParentId == null) {
                        $("#sidebarMenu").append(menuItem);
                    }
                }
            });

            handleNavbarItemClick();
        },
        error: function (xhr, ajaxOptions, throwError) {
            var error = xhr.responseJSON;
            console.log(error.error_description);
        }
    });
}

function createSubMenu(pages, value) {
    var menuItem = '<li class="nav-main-item"><a class="nav-main-link nav-main-link-submenu" data-toggle="submenu" aria-haspopup="true" aria-expanded="false" href="#"><span class="nav-main-link-name">' + value.Name + '</span></a><ul class="nav-main-submenu">';
    $.each(pages, function (key, data) {
        if (data.ParentId == value.Id) {
            var subMenuItem = '<li class="nav-main-item"><a class="nav-main-link" href="' + data.Path + '"><span class="nav-main-link-name">' + data.Name + '</span></a></li>';

            menuItem += subMenuItem;
        }
    });

    menuItem += '</ul></li>';

    $("#sidebarMenu").append(menuItem);
}

//Bu kodlar Template içerisinde dinamik menü isimleri dom a eklemeden önce calıstıgı için itemların altındaki submenülerin acılmasını saglamak için aşagıdaki kodlar templateden alınıp burada yazılır ki submenüler acılır sekilde olsun diye.
function handleNavbarItemClick() {
    let e = document.querySelectorAll('[data-toggle="submenu"]');
    e && e.forEach((e => {
        e.addEventListener("click", (t => {
            t.preventDefault();
            let i = e.closest(".nav-main");
            if (!((window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth) > 991 && i.classList.contains("nav-main-horizontal") && i.classList.contains("nav-main-hover"))) {
                let t = e.closest("li");
                t.classList.contains("open") ? (t.classList.remove("open"), e.setAttribute("aria-expanded", "false")) : (Array.from(e.closest("ul").children).forEach((e => {
                    e.classList.remove("open")
                })), t.classList.add("open"), e.setAttribute("aria-expanded", "true"))
            }
            return !1
        }))
    }))
}