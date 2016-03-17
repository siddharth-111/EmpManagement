$(document).ready(function () {

    //validation rules
    $.validator.addMethod("dateLowerThanToday", function (value, element) {
        var myDate = value;
        return Date.parse(myDate) < new Date();
    }, "Joining date must be lesser than today's date");

    $.validator.addMethod("dateofBirth", function (value, element) {
        var myDate = value;
        var limitDate = new Date();
        limitDate.setFullYear(2000);
        return Date.parse(myDate) < limitDate;
    }, "Date of birth cannot lie beyond the year 2000");

    $("#createForm").validate({
        //set this to false if you don't what to set focus on the first invalid input

        errorPlacement: function (error, element) {
            error.appendTo(".errors");
        },
        rules: {
            "Email": {
                required: true,
                email: true
            },
            "Password": {
                minlength: 6,
                required: true
            },
            "ConfirmPassword": {
                required: true,
                equalTo: "#Password"
            },
            "Contact": {
                minlength: 10,
                number: true
            }
        },
        messages: {
            "Email": {
                required: "Email is required",
                email: "Not a valid email address"
            },
            "Password": {
                minlength: "Password should contain a minimum of 6 characters",
                required: "Password is mandatory"
            },
            "ConfirmPassword": {
                required: "Confirm password is required",
                equalTo: "Passwords should match"
            },
            "Contact": {
                minlength: "Mobile number should contain more than 10 characters",
                number: "Mobile number should only contain digits"
            }
        },

        showErrors: function (errorMap, errorList) {
            $("createForm").find("input").each(function () {
                $(this).removeClass("error");
            });
            $(".errors").html("");
            if (errorList.length) {
                $(errorList[0]['element']).addClass("error");
                var elem = $(errorList[0]['element']).next();
                elem.html(errorList[0]['message']);
            }
        }
    });

});