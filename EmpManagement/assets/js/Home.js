
function onPageLoad() {
    $(document).on("click", "#formLogin", function () {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        var username = $("#username").val();
        var password = $("#password").val();
        if (username.length <= 0 && password.length <= 0) {
            $('#username').focus();
            $("#validateUser").html("Please enter your Email");
        } else if (password.length <= 0) {
            if (!$("#validateUser").is(':empty')) {
                var result = re.test(username);
                if (!result) {
                    $('#username').focus();
                } else {
                    $("#validateUser").html("");
                    $('#password').focus();
                    $("#validatePassword").html("Please enter a password");
                }
            } else {
                $('#password').focus();
                $("#validatePassword").html("Please enter a password");
            }
        } else {
            if (password.length < 6) {
                $('#password').focus();
                
            } else {
                $("#validatePassword").html("");
                var result = re.test(username);
                if (!result) {
                    $('#username').focus();
                    $("#validateUser").html("Email is not valid");
                } else {
                    $("#validateUser").html("");
                    $("#formId")[0].submit();
                }
            }

        }
    });
}

$(function () {
    onPageLoad();
});
