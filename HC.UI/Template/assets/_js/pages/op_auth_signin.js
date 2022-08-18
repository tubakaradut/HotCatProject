/*!
 * codebase - v5.1.0
 * @author pixelcave - https://pixelcave.com
 * Copyright (c) 2022
 */
Codebase.onLoad(class {
    static initValidationSignIn() {
        Codebase.helpers("jq-validation"), jQuery(".js-validation-signin").validate({
            submitHandler: function (form) {
                if ($("#loginForm").valid())
                    $("#loginForm").submit();
                return false; // prevent normal form posting
            },
            rules: {
                "login-username": {
                    required: !0,
                    minlength: 3
                },
                "login-password": {
                    required: !0,
                    minlength: 5
                }
            },
            messages: {
                "login-username": {
                    required: "Lütfen Kullanıcı Adı alanını doldurunuz!",
                    minlength: "Kullanıcı Adı alanı 3 karakterden fazla olmalıdır!"
                },
                "login-password": {
                    required: "Lütfen Şifre alanını doldurunuz!",
                    minlength: "Şifre alanı 5 karakterden fazla olmalıdır!"
                }
            }
        })
    }
    static init() {
        this.initValidationSignIn()
    }
}.init());