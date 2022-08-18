
const authFilter = new AuthFilter();


function AuthFilter() {

    $("body").css("display", "none");

    var token = localStorage.getItem("token");

    if (token == null) {
        window.location.href = window.location.origin + "/Authentication/Login";
    } else {
        $("body").css("display", "block");

    }
}

